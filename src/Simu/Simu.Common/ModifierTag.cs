using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simu.Common
{
    /// <summary>
    /// Tags for <see cref="Modifier"/>s.
    /// </summary>
    [Flags]
    public enum ModifierTag : int
    {
        None = 0,
        Base = 1,
        Armor = 2,
        Weapon = 4,
        Equipment = 8,
        Reforge = 16,
        Merged = 32,
        Skill = 64,
        Effect = 128,
        Slayer = 256,
        Pet = 512,
        Conditional = 1_024,
        Undead = 2_048,
        Cubism = 4_096,
        Arachnids = 8_192,
        Ender = 16_384,
        Blazes = 32_768,
        FirstStrike = 65_536,
        TripleStrike = 131_072,
        Flame = 262_144,
        Venomous = 524_288
    }

    public static class ModifierTagExtensions
    {
        public static string ToAggregatedString(this ModifierTag current)
        {
            return Enum.GetValues<ModifierTag>()
                .Where(t => t.IsSubsetOf(current) && !t.IsEqualTo(ModifierTag.None))
                .Select(t => t.ToString())
                .Aggregate((l, r) => $"{l}, {r}");
        }
        public static ModifierTag Add(this ModifierTag current, ModifierTag toAdd)
        {
            return current | toAdd;
        }
        public static ModifierTag Remove(this ModifierTag current, ModifierTag toRemove)
        {
            return current & ~toRemove;
        }

        public static bool IsSubsetOf(this ModifierTag current, ModifierTag other)
        {
            return (current & other) == current || current == ModifierTag.None;
        }

        public static bool IsEqualTo(this ModifierTag current, ModifierTag other)
        {
            return (current & other) == current && (current & other) == other;
        }

    }

}
