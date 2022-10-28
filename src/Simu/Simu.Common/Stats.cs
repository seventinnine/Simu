using System.Collections;
using System.Text.Json.Serialization;

namespace Simu.Common
{
    /// <summary>
    /// Singular Modifier of a <see cref="Stat"/>.
    /// Has a (per <see cref="ModifierList"/>) unique <see cref="ID"/> (for later removal from the calculation).
    /// If this modifier should only apply under certain conditions, <see cref="RequiredTags"/> specifies which <see cref="ModifierTag"/>s must currently be selected for the calculation.
    /// </summary>
    public class Modifier
    {
        public string ID { get; set; } = string.Empty;
        public List<ModifierTag> RequiredTags { get; set; } = new();
        public decimal Value { get; set; }
        public char Unit { get; set; }

        public override string ToString()
        {
            if (!RequiredTags.Any())
                return $"{ID}{Unit}";
            else
                return $"{Value}{Unit} ({ID} ({RequiredTags.Select(e => e.ToString()).Aggregate((l, r) => $"{l}, {r}")}))";
        }
    }

    /// <summary>
    /// Indexable <see cref="Modifier"/>-List (by <see cref="Modifier.ID"/>) with adapater function for adding/removing <see cref="Modifier"/>s.
    /// </summary>
    public class ModifierList : IEnumerable<Modifier>
    {
        private Dictionary<string, Modifier> modifiers { get; set; } = new();

        /// <summary>
        /// Adds <paramref name="modifier"/> to <see cref="ModifierList"/>.
        /// </summary>
        /// <param name="modifier"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Add(Modifier modifier)
        {
            if (modifiers.ContainsKey(modifier.ID)) throw new ArgumentException("Duplicate key not allowed.");

            modifiers[modifier.ID] = modifier;
        }

        /// <summary>
        /// Adds <paramref name="modifier"/> to <see cref="ModifierList"/>.
        /// </summary>
        /// <param name="modifier"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Add(string id, decimal value)
        {
            Add(new Modifier { ID = id, Value = value });
        }

        /// <summary>
        /// Adds <paramref name="modifier"/> to <see cref="ModifierList"/>.
        /// </summary>
        /// <param name="modifier"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Add(string id, decimal value, params ModifierTag[] tags)
        {
            Add(new Modifier { ID = id, Value = value, RequiredTags = tags.ToList()});
        }

        /// <summary>
        /// Removes <paramref name="modifier"/> from <see cref="ModifierList"/>.
        /// </summary>
        /// <param name="modifier"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Remove(Modifier modifier)
        {
            modifiers.Remove(modifier.ID);
        }

        /// <summary>
        /// Removes <paramref name="modifier"/> from <see cref="ModifierList"/>.
        /// </summary>
        /// <param name="modifierName"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Remove(string modifierName)
        {
            if (!modifiers.ContainsKey(modifierName)) throw new ArgumentException("Key does not exist.");

            modifiers.Remove(modifierName);
        }

        /// <summary>
        /// Calculates the big P (Product over all) of <see cref="Modifier"/>s in <see cref="ModifierList"/>.
        /// <paramref name="tags"/> are provided for conditional <see cref="Modifier"/>s.
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public decimal ProductForMatchingTags(ICollection<ModifierTag> tags)
        {
            decimal product = 1.0m;
            foreach (var pair in modifiers)
            {
                if (!pair.Value.RequiredTags.Any() || tags.Intersect(pair.Value.RequiredTags).Any())
                {
                    product *= (1.0m + pair.Value.Value / 100.0m);
                }
            }
            return product - 1.0m;
        }
        /// <summary>
        /// Calculates the big S (Sum of all) of <see cref="Modifier"/>s in <see cref="ModifierList"/>.
        /// <paramref name="tags"/> are provided for conditional <see cref="Modifier"/>s.
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public decimal SumForMatchingTags(ICollection<ModifierTag> tags)
        {
            decimal sum = 0.0m;
            foreach (var pair in modifiers)
            {
                if (!pair.Value.RequiredTags.Any() || tags.Intersect(pair.Value.RequiredTags).Any())
                {
                    sum += pair.Value.Value;
                }
            }
            return sum;
        }

        // : IEnumerable<Modifier>
        public IEnumerator<Modifier> GetEnumerator()
        {
            return modifiers.Values.GetEnumerator();
        }

        // : IEnumerable<Modifier>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    /// <summary>
    /// Compound of <see cref="ModifierList"/> with different semantics.
    /// Each <see cref="Stat"/> can have <see cref="BaseStats"/>, <see cref="UnscalableStats"/>, <see cref="AdditiveMultipliers"/> and <see cref="MultiplicativeMultipliers"/>.
    /// A <see cref="Stat"/> can have a Cap.
    /// </summary>
    public class Stat
    {
        /// <summary>
        /// Optional default cap.
        /// <see langword="null"/> means uncapped.
        /// </summary>
        public decimal? DefaultCap { get; set; }
        /// <summary>
        /// Overrides <see cref="DefaultCap"/>.
        /// <see langword="null"/> means <see cref="DefaultCap"/> is used.
        /// </summary>
        public decimal? ModifiedCap { get; set; }
        /// <summary>
        /// Can be scaled with <see cref="AdditiveMultipliers"/> and <see cref="MultiplicativeMultipliers"/>.
        /// </summary>
        public ModifierList BaseStats { get; } = new();
        /// <summary>
        /// Cannot be scaled by any means.
        /// Those are added to the final result at the end.
        /// </summary>
        public ModifierList UnscalableStats { get; } = new();
        /// <summary>
        /// Each <see cref="Modifier"/> is added together and the final value is used as another multiplier.
        /// </summary>
        public ModifierList AdditiveMultipliers { get; } = new();
        /// <summary>
        /// All <see cref="Modifier"/>s are factored together and the final value is used as another multiplier.
        /// </summary>
        public ModifierList MultiplicativeMultipliers { get; } = new();

        public Stat(decimal? defaultCap = null)
        {
            this.DefaultCap = defaultCap;
        }

        /// <summary>
        /// Copy all <see cref="Modifier"/>s from <paramref name="src"/> to <paramref name="dest"/>.
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <exception cref="ArgumentException"></exception>
        private void MergeModifiers(ModifierList dest, ModifierList src)
        {
            foreach (var item in src)
            {
                dest.Add(item);
            }
        }

        /// <summary>
        /// Adds the contents of the <see cref="ModifierList"/>s of <paramref name="other"/> to this.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Stat Combine(Stat other)
        {
            MergeModifiers(this.BaseStats, other.BaseStats);
            MergeModifiers(this.UnscalableStats, other.UnscalableStats);
            MergeModifiers(this.AdditiveMultipliers, other.AdditiveMultipliers);
            MergeModifiers(this.MultiplicativeMultipliers, other.MultiplicativeMultipliers);
            return this;
        }

        /// <summary>
        /// Calculates the total of all <see cref="ModifierList"/>s with respect to their semantics.
        /// The provided <paramref name="tags"/> are used by conditional <see cref="Modifier"/>s to determine if their values can be used.
        /// Result can be capped (<see cref="ModifiedCap"/> overrides <see cref="DefaultCap"/>).
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public decimal CalculateTotal(ICollection<ModifierTag> tags)
        {
            decimal sumBase = BaseStats.SumForMatchingTags(tags);
            decimal sumAdditive = AdditiveMultipliers.SumForMatchingTags(tags) / 100.0m;
            decimal productMultiplicative = MultiplicativeMultipliers.ProductForMatchingTags(tags);
            decimal sumUnscalable = UnscalableStats.SumForMatchingTags(tags);

            decimal? capToUse = ModifiedCap ?? DefaultCap ?? null;

            decimal total = (sumBase * (1.0m + sumAdditive) * (1.0m + productMultiplicative)) + sumUnscalable;

            if (capToUse is null) return total;
            else return Math.Min(total, capToUse.Value);
        }

    }

    /// <summary>
    /// Data class for storage of the character stats (+ selected tags for conditionals, + weapon specific stuff like int scaling).
    /// Compound of multiple <see cref="Stat"/>s.
    /// </summary>
    public class AllStats
    {
        public SortedSet<ModifierTag> ConditionalTags { get; } = new();

        public decimal IntelligenceScaleFactor { get; set; }

        #region Stats

        public Stat FlatAttackDamage { get; } = new();

        public Stat FlatAbilityDamage { get; } = new();

        public Stat IncreasedDamageMeleePercent { get; } = new();

        public Stat IncreasedDamageRangedPercent { get; } = new();

        public Stat IncreasedDamageMagicPercent { get; } = new();

        public Stat MoreDamagePercent { get; } = new();

        public Stat Health { get; } = new();

        public Stat Defense { get; } = new();

        public Stat TrueDefense { get; } = new();

        public Stat Speed { get; } = new(400.0m);

        public Stat Strength { get; } = new();

        public Stat Intelligence { get; } = new();

        public Stat CritChancePercent { get; } = new(100.0m);

        public Stat CritDamagePercent { get; } = new();

        public Stat AttackSpeedPercent { get; } = new(100.0m);

        public Stat Ferocity { get; } = new();

        public Stat AbilityDamagePercent { get; } = new();

        public Stat MagicFind { get; } = new();

        public Stat PetLuck { get; } = new();

        public Stat SeaCreatureChance { get; } = new(100.0m);

        #endregion

        /// <summary>
        /// Add all stats of <paramref name="other"/> to this.
        /// Merge stats
        /// </summary>
        /// <param name="other"></param>
        public void MergeWith(AllStats other)
        {
            IntelligenceScaleFactor = other.IntelligenceScaleFactor;
            FlatAttackDamage.Combine(other.FlatAttackDamage);
            FlatAbilityDamage.Combine(other.FlatAbilityDamage);
            IncreasedDamageMeleePercent.Combine(other.IncreasedDamageMeleePercent);
            IncreasedDamageRangedPercent.Combine(other.IncreasedDamageRangedPercent);
            IncreasedDamageMagicPercent.Combine(other.IncreasedDamageMagicPercent);
            MoreDamagePercent.Combine(other.MoreDamagePercent);
            Health.Combine(other.Health);
            Defense.Combine(other.Defense);
            TrueDefense.Combine(other.TrueDefense);
            Speed.Combine(other.Speed);
            Strength.Combine(other.Strength);
            Intelligence.Combine(other.Intelligence);
            CritChancePercent.Combine(other.CritChancePercent);
            CritDamagePercent.Combine(other.CritDamagePercent);
            AttackSpeedPercent.Combine(other.AttackSpeedPercent);
            Ferocity.Combine(other.Ferocity);
            AbilityDamagePercent.Combine(other.AbilityDamagePercent);
            MagicFind.Combine(other.MagicFind);
            PetLuck.Combine(other.PetLuck);
            SeaCreatureChance.Combine(other.SeaCreatureChance);

            other.ConditionalTags.ToList().ForEach(t => ConditionalTags.Add(t));
        }

        /// <summary>
        /// Deep-clones this.
        /// </summary>
        /// <returns></returns>
        public AllStats Copy()
        {
            AllStats copy = new();
            copy.IntelligenceScaleFactor = IntelligenceScaleFactor;
            copy.FlatAttackDamage.Combine(FlatAttackDamage);
            copy.FlatAbilityDamage.Combine(FlatAbilityDamage);
            copy.IncreasedDamageMeleePercent.Combine(IncreasedDamageMeleePercent);
            copy.IncreasedDamageRangedPercent.Combine(IncreasedDamageRangedPercent);
            copy.IncreasedDamageMagicPercent.Combine(IncreasedDamageMagicPercent);
            copy.MoreDamagePercent.Combine(MoreDamagePercent);
            copy.Health.Combine(Health);
            copy.Defense.Combine(Defense);
            copy.TrueDefense.Combine(TrueDefense);
            copy.Speed.Combine(Speed);
            copy.Strength.Combine(Strength);
            copy.Intelligence.Combine(Intelligence);
            copy.CritChancePercent.Combine(CritChancePercent);
            copy.CritDamagePercent.Combine(CritDamagePercent);
            copy.AttackSpeedPercent.Combine(AttackSpeedPercent);
            copy.Ferocity.Combine(Ferocity);
            copy.AbilityDamagePercent.Combine(AbilityDamagePercent);
            copy.MagicFind.Combine(MagicFind);
            copy.PetLuck.Combine(PetLuck);
            copy.SeaCreatureChance.Combine(SeaCreatureChance);

            ConditionalTags.ToList().ForEach(t => copy.ConditionalTags.Add(t));

            return copy;
        }

    }

}