using System.Collections;
using System.Text.Json.Serialization;

namespace Simu.Common
{
    /// <summary>
    /// Singular Modifier of a <see cref="Stat"/>.
    /// Has a (per <see cref="ModifierList"/>) unique <see cref="ID"/> (for later removal from the calculation).
    /// If this modifier should only apply under certain conditions, <see cref="RequiredTags"/> specifies which <see cref="ModifierTag"/>s must currently be selected for the calculation.
    /// </summary>
    public class Modifier : ICloneable
    {
        public string ID { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public ModifierTag RequiredTags { get; set; }
        public decimal LocalMultiplier { get; set; } = 1.0m;
        public string Unit { get; set; } = string.Empty;
        public Modifier()
        {

        }
        public Modifier(string iD, decimal value)
        {
            ID = iD;
            Value = value;
            RequiredTags = ModifierTag.None;
        }
        public Modifier(string iD, decimal value, ModifierTag requiredTags)
        {
            ID = iD;
            Value = value;
            RequiredTags = requiredTags;
        }
        public Modifier(string iD, decimal value, ModifierTag requiredTags, decimal localMultiplier, string unit)
        {
            ID = iD;
            Value = value;
            RequiredTags = requiredTags;
            LocalMultiplier = localMultiplier;
            Unit = unit;
        }

        public override string ToString()
        {
            if (RequiredTags != ModifierTag.None)
                return $"{ID} {Unit}";
            else
                return $"{Value} {Unit} ({ID} ({RequiredTags /*.Select(e => e.ToString()).Aggregate((l, r) => $"{l}, {r}")*/}))";
        }

        public object Clone()
        {
            return new Modifier(ID, Value, RequiredTags, LocalMultiplier, Unit);
        }
    }

    /// <summary>
    /// Indexable <see cref="Modifier"/>-List (by <see cref="Modifier.ID"/>) with adapater function for adding/removing <see cref="Modifier"/>s.
    /// </summary>
    public class ModifierList : IEnumerable<Modifier>
    {
        private ModifierTag _lastUsedTagsProduct;
        private decimal _cachedValueProduct;
        private bool _cacheValidProduct = false;
        private ModifierTag _lastUsedTagsSum;
        private decimal _cachedValueSum;
        private bool _cacheValidSum = false;

        public decimal ValueSum { get => _cachedValueSum; }
        public decimal ValueProduct { get => _cachedValueProduct; }

        public OnPropertyChanged? OnCacheInvalidated { get; set; }
        private Dictionary<string, Modifier> modifiers { get; set; } = new();

        /// <summary>
        /// Invalidates the cache and notifies its <see cref="ModifierList"/> of the cache being invalidated.
        /// </summary>
        private void InvalidateCaches()
        {
            _cacheValidProduct = false;
            _cacheValidSum = false;
            // invalidate parent caches
            OnCacheInvalidated?.Invoke();
        }

        /// <summary>
        /// Creates a deep-clone.
        /// </summary>
        /// <returns></returns>
        public ModifierList Clone()
        {
            ModifierList cpy = new();
            foreach (var entry in modifiers)
            {
                cpy.modifiers.Add(entry.Key, (Modifier)entry.Value.Clone());
            }
            return cpy;
        }

        /// <summary>
        /// Adds <paramref name="modifier"/> to <see cref="ModifierList"/>.
        /// </summary>
        /// <param name="modifier"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Add(Modifier modifier)
        {
            if (modifiers.ContainsKey(modifier.ID)) throw new ArgumentException("Duplicate key not allowed.");

            modifiers[modifier.ID] = modifier;
            InvalidateCaches();
        }

        /// <summary>
        /// Adds <paramref name="modifier"/> to <see cref="ModifierList"/>.
        /// </summary>
        /// <param name="modifier"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Add(string id, decimal value)
        {
            Add(new Modifier(id, value));
        }

        /// <summary>
        /// Adds <paramref name="modifier"/> to <see cref="ModifierList"/>.
        /// </summary>
        /// <param name="modifier"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Add(string id, decimal value, ModifierTag tags)
        {
            Add(new Modifier(id, value, tags));
        }

        /// <summary>
        /// Removes <paramref name="modifier"/> from <see cref="ModifierList"/>.
        /// </summary>
        /// <param name="modifier"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Remove(Modifier modifier)
        {
            Remove(modifier.ID);
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
            InvalidateCaches();
        }

        /// <summary>
        /// Replaces <see cref="Modifier"/> with <paramref name="modifier"/> and forces a cache invalidation.
        /// </summary>
        /// <param name="modifier"></param>
        /// <exception cref="ArgumentException"></exception>
        public void InvalidateCacheAndReplaceModifier(Modifier modifier)
        {
            if (!modifiers.ContainsKey(modifier.ID)) throw new ArgumentException("Key does not exist.");

            modifiers[modifier.ID] = modifier;
            InvalidateCaches();
        }

        /// <summary>
        /// Calculates the big P (Product over all) of <see cref="Modifier"/>s in <see cref="ModifierList"/>.
        /// <paramref name="tags"/> are provided for conditional <see cref="Modifier"/>s.
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public decimal ProductForMatchingTags(ModifierTag tags)
        {
            if (_cacheValidProduct && _lastUsedTagsProduct.IsEqualTo(tags)) return _cachedValueProduct;
            decimal product = 1.0m;
            foreach (var pair in modifiers)
            {
                if (pair.Value.RequiredTags.IsSubsetOf(tags))
                {
                    product *= 1.0m + (pair.Value.Value * pair.Value.LocalMultiplier) / 100.0m;
                }
            }
            _lastUsedTagsProduct = tags;
            _cacheValidProduct = true;
            _cachedValueProduct = product - 1.0m;
            return _cachedValueProduct;
        }

        /// <summary>
        /// Calculates the inverse product of <see cref="Modifier"/>s in <see cref="ModifierList"/>.
        /// <paramref name="tags"/> are provided for conditional <see cref="Modifier"/>s.
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public decimal InverseProductForMatchingTags(ModifierTag tags)
        {
            if (_cacheValidProduct && _lastUsedTagsProduct.IsEqualTo(tags)) return _cachedValueProduct;
            decimal product = 1.0m;
            foreach (var pair in modifiers)
            {
                if (pair.Value.RequiredTags.IsSubsetOf(tags))
                {
                    product *= 1.0m - (pair.Value.Value * pair.Value.LocalMultiplier) / 100.0m;
                }
            }
            _lastUsedTagsProduct = tags;
            _cacheValidProduct = true;
            _cachedValueProduct = 1.0m - product;
            return _cachedValueProduct;
        }

        /// <summary>
        /// Calculates the big S (Sum of all) of <see cref="Modifier"/>s in <see cref="ModifierList"/>.
        /// <paramref name="tags"/> are provided for conditional <see cref="Modifier"/>s.
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public decimal SumForMatchingTags(ModifierTag tags)
        {
            if (_cacheValidSum && _lastUsedTagsSum.IsEqualTo(tags)) return _cachedValueSum;
            decimal sum = 0.0m;
            foreach (var pair in modifiers)
            {
                if (pair.Value.RequiredTags.IsSubsetOf(tags))
                {
                    sum += pair.Value.Value * pair.Value.LocalMultiplier;
                }
            }
            _lastUsedTagsSum = tags;
            _cacheValidSum = true;
            _cachedValueSum = sum;
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
        private bool _isMultiplierOnly;

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
        /// Updates value of <see cref="_modifiedCap"/> and invalidates cache.
        /// </summary>
        /// <param name="value"></param>
        private void UpdateModifiedCap(decimal? value)
        {
            _modifiedCap = value;
            InvalidateCache();
        }

        /// <summary>
        /// Can be scaled with <see cref="AdditiveMultipliers"/> and <see cref="MultiplicativeMultipliers"/>.
        /// </summary>
        public ModifierList BaseStats { get; private set; } = new();
        /// <summary>
        /// Cannot be scaled by any means.
        /// Those are added to the final result at the end.
        /// </summary>
        public ModifierList UnscalableStats { get; private set; } = new();
        /// <summary>
        /// Each <see cref="Modifier"/> is added together and the final value is used as another multiplier.
        /// </summary>
        public ModifierList AdditiveMultipliers { get; private set; } = new();
        /// <summary>
        /// All <see cref="Modifier"/>s are factored together and the final value is used as another multiplier.
        /// </summary>
        public ModifierList MultiplicativeMultipliers { get; private set; } = new();

        /// <summary>
        /// Storage for the calcuated total value.
        /// </summary>
        public decimal Total { get => _cachedValue; }

        /// <summary>
        /// Storage for the calculated total value, ignoring the cap.
        /// </summary>
        public decimal TotalUncapped { get => _cachedValueUncapped; }

        public Stat(decimal? defaultCap = null, decimal minValue = 0)
        {
            BaseStats = new();
            BaseStats.OnCacheInvalidated += InvalidateCache;
            UnscalableStats = new();
            UnscalableStats.OnCacheInvalidated += InvalidateCache;
            AdditiveMultipliers = new();
            AdditiveMultipliers.OnCacheInvalidated += InvalidateCache;
            MultiplicativeMultipliers = new();
            MultiplicativeMultipliers.OnCacheInvalidated += InvalidateCache;
            this.DefaultCap = defaultCap;
            _cacheValid = false;
            MinValue = minValue;
            _isMultiplierOnly = false;
        }

        public Stat(bool isMultiplier) : this(null, 0)
        {
            _isMultiplierOnly = isMultiplier;
        }

        private void InvalidateCache()
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
            cpy.BaseStats = BaseStats.Clone();
            cpy.UnscalableStats = UnscalableStats.Clone();
            cpy.AdditiveMultipliers = AdditiveMultipliers.Clone();
            cpy.MultiplicativeMultipliers = MultiplicativeMultipliers.Clone();
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

            decimal sumBase = BaseStats.SumForMatchingTags(tags);
            decimal sumAdditive = AdditiveMultipliers.SumForMatchingTags(tags) / 100.0m;
            decimal productMultiplicative = MultiplicativeMultipliers.ProductForMatchingTags(tags);
            decimal sumUnscalable = UnscalableStats.SumForMatchingTags(tags);

            decimal total = !_isMultiplierOnly
                ? sumBase * (1.0m + sumAdditive) * (1.0m + productMultiplicative) + sumUnscalable
                : (1.0m + sumAdditive) * (1.0m + productMultiplicative);

            // handle modified cap
            decimal? capToUse = ModifiedCap ?? DefaultCap ?? null;

            decimal result = capToUse is null ? total : Math.Max(Math.Min(total, capToUse.Value), MinValue);

            // cache result
            _cachedValueUncapped = total;
            _cachedValue = result;
            _cacheValid = true;
            _lastUsedTags = tags;

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

        public decimal IntelligenceScaleFactor { get; set; }

        public decimal AbilityCooldown { get; set; } = 1.0m;

        public Modifier DungeoneeringDungeonizedMultiplier { get; set; } = new();

        #region Stats

        public Stat FlatAttackDamage { get; private set; } = new();

        public Stat FlatAbilityDamage { get; private set;  } = new();

        //TODO: change multipliers to just modifierlists?
        
        public ModifierList IncreasedDamageMeleePercent { get; private set; } = new();

        public ModifierList IncreasedDamageRangedPercent { get; private set;  } = new();

        public ModifierList IncreasedDamageMagicPercent { get; private set;  } = new();

        public ModifierList MoreDamagePercent { get; private set;  } = new();

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
        public ModifierList DamageReductionPercent { get; private set; } = new();

        #endregion

        public Stats()
        {
            
        }

        public void AddBaseStats()
        {
            FlatAttackDamage.BaseStats.Add("Base", 5.0m);
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
            copy.DungeoneeringDungeonizedMultiplier = (Modifier)DungeoneeringDungeonizedMultiplier.Clone();
            copy.FlatAttackDamage = FlatAttackDamage.Clone();
            copy.FlatAbilityDamage = FlatAbilityDamage.Clone();
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