using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private decimal _value;
        public decimal Value { get => _value; set => SetValue(value); }

        private void SetValue(decimal newValue)
        {
            if (_value != newValue)
            {
                _value = newValue;
                // invalidate cache of owning modifier
                RefToModifierList?.InvalidateCaches();
            }
        }

        public ModifierTag RequiredTags { get; set; }
        public decimal LocalMultiplier { get; set; } = 1.0m;
        public string Unit { get; set; } = string.Empty;

        /// <summary>
        /// Reference to owning <see cref="ModifierList"/>.
        /// </summary>
        public ModifierList? RefToModifierList { get; set; }
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
            return Value.ToString("N2");
        }

        public string ValueToString()
        {
            /*.Select(e => e.ToString()).Aggregate((l, r) => $"{l}, {r}")*/
            /*
            if (string.IsNullOrEmpty(Unit))
            {
                if (RequiredTags == ModifierTag.None)
                    return $"{Value:N2}";
                else
                    return $"{Value:N2} ({RequiredTags })";
            }
            else
            {
                if (RequiredTags == ModifierTag.None)
                    return $"{Value:N2} {Unit}";
                else
                    return $"{Value:N2} {Unit} ({RequiredTags})";
            }
            */
            return "";
        }

        public string RequiredTagsToString()
        {
            if (RequiredTags.IsEqualTo(ModifierTag.None))
            {
                return "";
            }
            else
            {
                return RequiredTags.ToAggregatedString();
            }
        }

        public Modifier Clone(ModifierList? refToModifierList = null)
        {
            Modifier cpy = new(ID, Value, RequiredTags, LocalMultiplier, Unit);
            cpy.RefToModifierList = refToModifierList;
            return cpy;
        }
    }
}
