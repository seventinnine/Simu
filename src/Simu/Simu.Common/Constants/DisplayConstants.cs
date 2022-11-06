using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simu.Common.Constants
{
    public class DisplayConstants
    {
        public const string SYMBOL_HEALTH = "❤";
        public const string SYMBOL_DEFENSE = "❈";
        public const string SYMBOL_TRUEDEFENSE = "❂";
        public const string SYMBOL_STRENGTH = "❁";
        public const string SYMBOL_INTELLIGENCE = "✎";
        public const string SYMBOL_ATTACKSPEED = "⚔";
        public const string SYMBOL_FEROCITY = "⫽";
        public const string SYMBOL_ABILITYDAMAGE = "๑";
        public const string SYMBOL_SPEED = "✦";
        public const string SYMBOL_CRITCHANCE = "☣";
        public const string SYMBOL_CRITDAMAGE = "☠";
        public const string SYMBOL_DAMAGE = "❁";
        public const string SYMBOL_MAGICFIND = "✯";
        public const string SYMBOL_PETLUCK = "♣";
        public const string SYMBOL_SEACREATURECHANCE = "α";
        public const string SYMBOL_STAR = "✪";

        public static FormattedString FormattedStringHealth(decimal value)
        {
            return new() { ColorCodeHex = ColorCodeHex.RED, Content = $"{SYMBOL_HEALTH} Health: {value:N2}" };
        }
        public static FormattedString FormattedStringDefense(decimal value)
        {
            return new() { ColorCodeHex = ColorCodeHex.GREEN, Content = $"{SYMBOL_DEFENSE} Defense: {value:N2}" };
        }
        public static FormattedString FormattedStringTrueDefense(decimal value)
        {
            return new() { ColorCodeHex = ColorCodeHex.WHITE, Content = $"{SYMBOL_TRUEDEFENSE} True Defense: {value:N2}" };
        }
        public static FormattedString FormattedStringStrength(decimal value)
        {
            return new() { ColorCodeHex = ColorCodeHex.DARK_RED, Content = $"{SYMBOL_STRENGTH} Strength: {value:N2}" };
        }
        public static FormattedString FormattedStringIntelligence(decimal value)
        {
            return new() { ColorCodeHex = ColorCodeHex.AQUA, Content = $"{SYMBOL_INTELLIGENCE} Intelligence: {value:N2}" };
        }
        public static FormattedString FormattedStringAttackSpeed(decimal value)
        {
            return new() { ColorCodeHex = ColorCodeHex.YELLOW, Content = $"{SYMBOL_ATTACKSPEED} Attack Speed: {value:N2}" };
        }
        public static FormattedString FormattedStringAttackSpeedWithOvercap(decimal value, decimal uncapped, bool isOvercapped)
        {
            return new() { ColorCodeHex = ColorCodeHex.YELLOW, Content = isOvercapped ? $"{SYMBOL_ATTACKSPEED} Attack Speed: {value:N2} ({uncapped:N2})" : $"{SYMBOL_ATTACKSPEED} Attack Speed: {value:N2}" };
        }
        public static FormattedString FormattedStringFerocity(decimal value)
        {
            return new() { ColorCodeHex = ColorCodeHex.RED, Content = $" {SYMBOL_FEROCITY} Ferocity: {value:N2}" };
        }
        public static FormattedString FormattedStringAbilityDamage(decimal value)
        {
            return new() { ColorCodeHex = ColorCodeHex.RED, Content = $"{SYMBOL_ABILITYDAMAGE} Ability Damage: {value:N2}" };
        }
        public static FormattedString FormattedStringSpeed(decimal value)
        {
            return new() { ColorCodeHex = ColorCodeHex.WHITE, Content = $"{SYMBOL_SPEED} Speed: {value:N2}" };
        }
        public static FormattedString FormattedStringSpeedWithOvercap(decimal value, decimal uncapped, bool isOvercapped)
        {
            return new() { ColorCodeHex = ColorCodeHex.WHITE, Content = isOvercapped ? $"{SYMBOL_SPEED} Speed: {value:N2} ({uncapped:N2})" : $"{SYMBOL_SPEED} Speed: {value:N2}" };
        }
        public static FormattedString FormattedStringCritChance(decimal value)
        {
            return new() { ColorCodeHex = ColorCodeHex.DARK_BLUE, Content = $"{SYMBOL_CRITCHANCE} Crit Chance: {value:N2}" };
        }
        public static FormattedString FormattedStringCritChanceWithOvercap(decimal value, decimal uncapped, bool isOvercapped)
        {
            return new() { ColorCodeHex = ColorCodeHex.DARK_BLUE, Content = isOvercapped ? $"{SYMBOL_CRITCHANCE} Crit Chance: {value:N2} ({uncapped:N2})" : $"{SYMBOL_CRITCHANCE} Crit Chance: {value:N2}" };
        }
        public static FormattedString FormattedStringCritDamage(decimal value)
        {
            return new() { ColorCodeHex = ColorCodeHex.DARK_BLUE, Content = $"{SYMBOL_CRITDAMAGE} Crit Damage: {value:N2}" };
        }
        public static FormattedString FormattedStringDamage(decimal value)
        {
            return new() { ColorCodeHex = ColorCodeHex.RED, Content = $"{SYMBOL_DAMAGE} Damage: {value:N2}" };
        }
        public static FormattedString FormattedStringDamagePercent(decimal value)
        {
            return new() { ColorCodeHex = ColorCodeHex.RED, Content = $"{SYMBOL_DAMAGE} Damage: {value:N2} %" };
        }
        public static FormattedString FormattedStringMagicFind(decimal value)
        {
            return new() { ColorCodeHex = ColorCodeHex.AQUA, Content = $"{SYMBOL_MAGICFIND} Magic Find: {value:N2}" };
        }
        public static FormattedString FormattedStringPetLuck(decimal value)
        {
            return new() { ColorCodeHex = ColorCodeHex.LIGHT_PURPLE, Content = $"{SYMBOL_PETLUCK} Pet Luck: {value:N2}" };
        }
        public static FormattedString FormattedStringSeaCreatureChance(decimal value)
        {
            return new() { ColorCodeHex = ColorCodeHex.DARK_AQUA, Content = $"{SYMBOL_SEACREATURECHANCE} Sea Creature Chance: {value:N2}" };
        }
        public static FormattedString FormattedStringSeaCreatureChanceWithOvercap(decimal value, decimal uncapped, bool isOvercapped)
        {
            return new() { ColorCodeHex = ColorCodeHex.DARK_AQUA, Content = isOvercapped ? $"{SYMBOL_SEACREATURECHANCE} SC Chance: {value:N2} ({uncapped:N2})" : $"{SYMBOL_SEACREATURECHANCE} SC Chance: {value:N2}" };
        }
        public static FormattedString FormattedStringStars(decimal value)
        {
            return new() { ColorCodeHex = ColorCodeHex.GOLD, Content = $"{SYMBOL_STAR} Gear Bonus: {value:N2} %" };
        }

    }
}
