using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simu.Common
{
    /// <summary>
    /// Indexable <see cref="Modifier"/>-List (by <see cref="Modifier.ID"/>) with adapater function for adding/removing <see cref="Modifier"/>s.
    /// </summary>
    public abstract class ModifierList : IEnumerable<Modifier>
    {
        protected ModifierTag _lastUsedTags;
        protected decimal _cachedValue;
        protected bool _cacheValid = false;

        public decimal Value { get => _cachedValue; }

        public Dictionary<string, Modifier> Modifiers { get; } = new();

        /// <summary>
        /// Reference to the owning <see cref="Stat"/>.
        /// </summary>
        public Stat? RefToStat { get; set; }

        public ModifierList(Stat? refToStat = null)
        {
            RefToStat = refToStat;
        }

        /// <summary>
        /// Invalidates the cache and notifies its <see cref="Stat"/> of the cache being invalidated.
        /// </summary>
        public void InvalidateCaches()
        {
            if (!_cacheValid) return;

            _cacheValid = false;
            // invalidate parent caches
            RefToStat?.InvalidateCache();
        }

        /// <summary>
        /// Creates a deep-clone.
        /// </summary>
        /// <returns></returns>
        public abstract ModifierList Clone(Stat? refToStat = null);

        /// <summary>
        /// Calculates the total of this <see cref="Stat"/> and caches the results.
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public decimal CalculateTotal(ModifierTag tags)
        {
            if (_cacheValid && _lastUsedTags.IsEqualTo(tags)) return _cachedValue;
            decimal result = CalculateTotalImpl(tags);
            _lastUsedTags = tags;
            _cacheValid = true;
            _cachedValue = result;
            return _cachedValue;
        }

        /// <summary>
        /// Implementation of the calculation.
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        protected abstract decimal CalculateTotalImpl(ModifierTag tags);

        /// <summary>
        /// Gets <see cref="Modifier"/> with ID <paramref name="modifierName"/>.
        /// </summary>
        /// <param name="modifierName"></param>
        /// <exception cref="ArgumentException"></exception>
        public Modifier Get(string modifierName)
        {
            if (Modifiers.ContainsKey(modifierName)) throw new ArgumentException("Duplicate key not allowed.");

            return Modifiers[modifierName];
        }

        /// <summary>
        /// Adds <paramref name="modifier"/> to <see cref="ModifierList"/>.
        /// </summary>
        /// <param name="modifier"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Add(Modifier modifier)
        {
            if (Modifiers.ContainsKey(modifier.ID)) throw new ArgumentException("Duplicate key not allowed.");

            Modifiers[modifier.ID] = modifier;
            modifier.RefToModifierList = this;
            InvalidateCaches();
        }

        /// <summary>
        /// Creates a new <see cref="Modifier"/> from <paramref name="id"/>, <paramref name="value"/> and <paramref name="tags"/> and add is to <see cref="ModifierList"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <param name="tags"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Add(string id, decimal value, ModifierTag tags = ModifierTag.None)
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
        /// Removes <see cref="Modifier"/> <paramref name="modifierName"/> from <see cref="ModifierList"/>.
        /// </summary>
        /// <param name="modifierName"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Remove(string modifierName)
        {
            if (!Modifiers.ContainsKey(modifierName)) throw new ArgumentException("Key does not exist.");

            Modifiers[modifierName].RefToModifierList = null;
            Modifiers.Remove(modifierName);
            InvalidateCaches();
        }

        // : IEnumerable<Modifier>
        public IEnumerator<Modifier> GetEnumerator()
        {
            return Modifiers.Values.GetEnumerator();
        }

        // : IEnumerable<Modifier>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
