using System.Collections;
using System.Text.Json.Serialization;

namespace Simu.Common
{
    public delegate void OnPropertyChanged();

    /// <summary>
    /// Compound of <see cref="ModifierList"/> with different semantics.
    /// Each <see cref="Stat"/> can have <see cref="BaseStats"/>, <see cref="UnscalableStats"/>, <see cref="AdditiveMultipliers"/> and <see cref="MultiplicativeMultipliers"/>.
    /// A <see cref="Stat"/> can have a Cap.
    /// </summary>
    public class Stat
    {
        private ModifierTag _lastUsedTags;
        private decimal _cachedValue;
        private decimal _cachedValueUncapped;
        private bool _cacheValid;

        public event Action<decimal, decimal>? OnValueChanged;

        /// <summary>
        /// Minimum value.
        /// </summary>
        public decimal MinValue { get; }

        /// <summary>
        /// Optional default cap.
        /// <see langword="null"/> means uncapped.
        /// </summary>
        public decimal? DefaultCap { get; }

        private decimal? _modifiedCap;
        /// <summary>
        /// Overrides <see cref="DefaultCap"/>.
        /// Invalidates cache.
        /// <see langword="null"/> means <see cref="DefaultCap"/> is used.
        /// </summary>
        public decimal? ModifiedCap { get => _modifiedCap; set => UpdateModifiedCap(value); }

        /// <summary>
        /// Selects the cap that should be used.
        /// <see langword="null"/> means that there is no cap.
        /// </summary>
        public decimal? CapToUse { get => ModifiedCap ?? DefaultCap ?? null; }

        /// <summary>
        /// Updates value of <see cref="_modifiedCap"/> and invalidates cache.
        /// </summary>
        /// <param name="value"></param>
        private void UpdateModifiedCap(decimal? value)
        {
            _modifiedCap = value;
            InvalidateCache();
        }

        public bool IsOvercapped()
        {
            return Total < TotalUncapped;
        }

        /// <summary>
        /// Can be scaled with <see cref="AdditiveMultipliers"/> and <see cref="MultiplicativeMultipliers"/>.
        /// </summary>
        public ModifierList BaseStats { get; private set; }
        /// <summary>
        /// Cannot be scaled by any means.
        /// Those are added to the final result at the end.
        /// </summary>
        public ModifierList UnscalableStats { get; private set; }
        /// <summary>
        /// Each <see cref="Modifier"/> is added together and the final value is used as another multiplier.
        /// </summary>
        public ModifierList AdditiveMultipliers { get; private set; }
        /// <summary>
        /// All <see cref="Modifier"/>s are factored together and the final value is used as another multiplier.
        /// </summary>
        public ModifierList MultiplicativeMultipliers { get; private set; }

        /// <summary>
        /// Storage for the calcuated total value.
        /// </summary>
        public decimal Total { get => _cachedValue; }

        /// <summary>
        /// Storage for the calculated total value, ignoring the cap.
        /// </summary>
        public decimal TotalUncapped { get => _cachedValueUncapped; }

        public Stat(decimal? defaultCap = null, decimal minValue = 0, bool isInverse = false)
        {
            BaseStats = new SumModifierList(this);
            UnscalableStats = new SumModifierList(this);
            AdditiveMultipliers = new SumModifierList(this);
            MultiplicativeMultipliers = new ProductModifierList(this, isInverse);
            this.DefaultCap = defaultCap;
            _cacheValid = false;
            MinValue = minValue;
        }

        public void InvalidateCache()
        {
            _cacheValid = false;
        }

        /// <summary>
        /// Creates a deep-clone.
        /// </summary>
        /// <returns></returns>
        public Stat Clone()
        {
            Stat cpy = new(DefaultCap);
            cpy.ModifiedCap = ModifiedCap;
            cpy.BaseStats = BaseStats.Clone(cpy);
            cpy.UnscalableStats = UnscalableStats.Clone(cpy);
            cpy.AdditiveMultipliers = AdditiveMultipliers.Clone(cpy);
            cpy.MultiplicativeMultipliers = MultiplicativeMultipliers.Clone(cpy);
            return cpy;
        }

        /// <summary>
        /// Calculates the total of all <see cref="ModifierList"/>s with respect to their semantics.
        /// The provided <paramref name="tags"/> are used by conditional <see cref="Modifier"/>s to determine if their values can be used.
        /// Result can be capped (<see cref="ModifiedCap"/> overrides <see cref="DefaultCap"/>).
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public decimal CalculateTotal(ModifierTag tags)
        {
            if (_cacheValid && _lastUsedTags.IsSubsetOf(tags)) return _cachedValue;

            Constants.ProfileConstants.calculations++;
            decimal sumBase = BaseStats.CalculateTotal(tags);
            decimal sumAdditive = 1.0m + AdditiveMultipliers.CalculateTotal(tags) / 100.0m;
            decimal productMultiplicative = MultiplicativeMultipliers.CalculateTotal(tags);
            decimal sumUnscalable = UnscalableStats.CalculateTotal(tags);

            decimal total = sumBase * sumAdditive * productMultiplicative + sumUnscalable;

            // handle modified cap
            decimal result = CapToUse is null ? total : Math.Max(Math.Min(total, CapToUse.Value), MinValue);

            // cache result
            _cachedValueUncapped = total;
            _cachedValue = result;
            _cacheValid = true;
            _lastUsedTags = tags;
            OnValueChanged?.Invoke(result, total);

            return result;
        }

    }

    /// <summary>
    /// Data class for storage of the character stats (+ selected tags for conditionals, + weapon specific stuff like int scaling).
    /// Compound of multiple <see cref="Stat"/>s.
    /// </summary>
    public class Stats
    {

        public ModifierTag ConditionalTags { get; private set; } = ModifierTag.None;

        //TODO: move to weapon
        public decimal IntelligenceScaleFactor { get; set; }

        //TODO: move to weapon
        public decimal AbilityCooldown { get; set; } = 1.0m;

        public Modifier DungeoneeringDungeonizedMultiplier { get; set; } = new();

        #region Stats
                
        public ModifierList IncreasedDamageMeleePercent { get; private set; }

        public ModifierList IncreasedDamageRangedPercent { get; private set;  }

        public ModifierList IncreasedDamageMagicPercent { get; private set;  }

        public ModifierList MoreDamagePercent { get; private set;  }

        public Stat Health { get; private set; } = new();

        public decimal Mana { get => Intelligence.Total + 100; }

        public Stat Defense { get; private set;  } = new();

        public Stat TrueDefense { get; private set;  } = new();

        public Stat Speed { get; private set;  } = new(400.0m);

        public Stat Strength { get; private set;  } = new();

        public Stat Intelligence { get; private set;  } = new();

        public Stat CritChancePercent { get; private set;  } = new(100.0m);

        public Stat CritDamagePercent { get; private set;  } = new();

        public Stat AttackSpeedPercent { get; private set;  } = new(100.0m);

        public Stat Ferocity { get; private set;  } = new();

        public Stat AbilityDamagePercent { get; private set;  } = new();

        public Stat MagicFind { get; private set;  } = new();

        public Stat PetLuck { get; private set;  } = new();

        public Stat SeaCreatureChance { get; private set; } = new(100.0m);
        public ModifierList DamageReductionPercent { get; private set; }

        #endregion

        public Stats()
        {
            IncreasedDamageMeleePercent = new SumModifierList();
            IncreasedDamageRangedPercent = new SumModifierList();
            IncreasedDamageMagicPercent = new SumModifierList();
            MoreDamagePercent = new ProductModifierList(null, true);
            DamageReductionPercent = new ProductModifierList();
        }

        public void AddBaseStats()
        {
            //FlatAttackDamage.BaseStats.Add("Base", 5.0m);
            Health.BaseStats.Add("Base", 100.0m);
            Speed.BaseStats.Add("Base", 100.0m);
            CritChancePercent.BaseStats.Add("Base", 30.0m);
            CritDamagePercent.BaseStats.Add("Base", 50.0m);
            SeaCreatureChance.BaseStats.Add("Base", 20.0m);
        }

        public void AddConditionalTag(ModifierTag tag)
        {
            ConditionalTags = ConditionalTags.Add(tag);
        }

        public void RemoveConditionalTag(ModifierTag tag)
        {
            ConditionalTags = ConditionalTags.Remove(tag);
        }


        /// <summary>
        /// Creates a deep-clone.
        /// </summary>
        /// <returns></returns>
        public Stats Clone()
        {
            Stats copy = new();
            copy.ConditionalTags = ConditionalTags;
            copy.IntelligenceScaleFactor = IntelligenceScaleFactor;
            copy.AbilityCooldown = AbilityCooldown;
            copy.DungeoneeringDungeonizedMultiplier = DungeoneeringDungeonizedMultiplier.Clone();
            //copy.FlatAttackDamage = FlatAttackDamage.Clone();
            //copy.FlatAbilityDamage = FlatAbilityDamage.Clone();
            copy.IncreasedDamageMeleePercent = IncreasedDamageMeleePercent.Clone();
            copy.IncreasedDamageRangedPercent = IncreasedDamageRangedPercent.Clone();
            copy.IncreasedDamageMagicPercent = IncreasedDamageMagicPercent.Clone();
            copy.MoreDamagePercent = MoreDamagePercent.Clone();
            copy.Health = Health.Clone();
            copy.Defense = Defense.Clone();
            copy.TrueDefense = TrueDefense.Clone();
            copy.Speed = Speed.Clone();
            copy.Strength = Strength.Clone();
            copy.Intelligence = Intelligence.Clone();
            copy.CritChancePercent = CritChancePercent.Clone();
            copy.CritDamagePercent = CritDamagePercent.Clone();
            copy.AttackSpeedPercent = AttackSpeedPercent.Clone();
            copy.Ferocity = Ferocity.Clone();
            copy.AbilityDamagePercent = AbilityDamagePercent.Clone();
            copy.MagicFind = MagicFind.Clone();
            copy.PetLuck = PetLuck.Clone();
            copy.SeaCreatureChance = SeaCreatureChance.Clone();
            copy.DamageReductionPercent = DamageReductionPercent.Clone();
            return copy;
        }
        
    }

}