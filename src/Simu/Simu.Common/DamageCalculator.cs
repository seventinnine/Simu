using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simu.Common
{

    public abstract class DamageBreakdown
    {
        public decimal HitsPerSecond { get; set; }
        public decimal DamagePerHit { get; set; }
        public decimal DamagePerSecond { get; set; }

        public abstract string GetBreakdownFormatted();
    }

    public abstract class AttackDamageBreakdown : DamageBreakdown
    {
        public decimal AdditionalHitsChance { get; set; }
    }

    public class MeleeDamageBreakdown : DamageBreakdown
    {
        public override string GetBreakdownFormatted()
        {
            throw new NotImplementedException();
        }
    }

    public class RangedDamageBreakdown : DamageBreakdown
    {
        public override string GetBreakdownFormatted()
        {
            throw new NotImplementedException();
        }
    }

    public class MagicDamageBreakdown : DamageBreakdown
    {
        public override string GetBreakdownFormatted()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Calulcates the damage based on provided <see cref="AllStats"/> and <
    /// </summary>
    public class DamageCalculator
    {
        /// <summary>
        /// Calculates the damage per hit based on provided <paramref name="stats"/> and <paramref name="mode"/>.
        /// </summary>
        /// <param name="stats"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static decimal CalculateAverageDamagePerHit(AllStats stats, AttackMode mode)
        {
            if (mode == AttackMode.Melee)
            {
                decimal baseDamage = stats.FlatAttackDamage.CalculateTotal(stats.ConditionalTags) + 5.0m;
                decimal strengthMultiplier = 1.0m + stats.Strength.CalculateTotal(stats.ConditionalTags) / 100.0m;
                decimal critChance = Math.Min(stats.CritChancePercent.CalculateTotal(stats.ConditionalTags) / 100.0m, 1.0m);
                decimal critDamageMuliplier = 1.0m + stats.CritDamagePercent.CalculateTotal(stats.ConditionalTags) / 100.0m;
                decimal increasedDamageMultiplier = 1.0m + stats.IncreasedDamageMeleePercent.CalculateTotal(stats.ConditionalTags);
                decimal moreDamageMultiplier = 1.0m + stats.MoreDamagePercent.CalculateTotal(stats.ConditionalTags);
                return baseDamage * strengthMultiplier * (critDamageMuliplier * critChance + (1.0m - critChance)) * increasedDamageMultiplier * moreDamageMultiplier; 
            }
            else if (mode == AttackMode.Ranged)
            {
                decimal baseDamage = stats.FlatAttackDamage.CalculateTotal(stats.ConditionalTags) + 5.0m;
                decimal strengthMultiplier = 1.0m + stats.Strength.CalculateTotal(stats.ConditionalTags) / 100.0m;
                decimal critChance = Math.Min(stats.CritChancePercent.CalculateTotal(stats.ConditionalTags) / 100.0m, 1.0m);
                decimal critDamageMuliplier = 1.0m + stats.CritDamagePercent.CalculateTotal(stats.ConditionalTags) / 100.0m;
                decimal increasedDamageMultiplier = 1.0m + stats.IncreasedDamageRangedPercent.CalculateTotal(stats.ConditionalTags);
                decimal moreDamageMultiplier = 1.0m + stats.MoreDamagePercent.CalculateTotal(stats.ConditionalTags);
                return baseDamage * strengthMultiplier * (critDamageMuliplier * critChance + (1.0m - critChance)) * increasedDamageMultiplier * moreDamageMultiplier;
            }
            else if (mode == AttackMode.Magic)
            {
                decimal baseDamage = stats.FlatAbilityDamage.CalculateTotal(stats.ConditionalTags) + 5.0m;
                decimal intelligenceMultiplier = (stats.Intelligence.CalculateTotal(stats.ConditionalTags) / 100.0m) * stats.IntelligenceScaleFactor;
                decimal abilityDamageMultiplier = 1.0m + stats.AbilityDamagePercent.CalculateTotal(stats.ConditionalTags) / 100.0m;
                decimal increasedDamageMultiplier = 1.0m + stats.IncreasedDamageRangedPercent.CalculateTotal(stats.ConditionalTags);
                decimal moreDamageMultiplier = 1.0m + stats.MoreDamagePercent.CalculateTotal(stats.ConditionalTags);
                return baseDamage * intelligenceMultiplier * abilityDamageMultiplier * increasedDamageMultiplier * moreDamageMultiplier;
            }
            else
            {
                return -1.0m;
            }
        }

        /// <summary>
        /// Calculates the damage per second based on provided <paramref name="stats"/> and <paramref name="mode"/>.
        /// </summary>
        /// <param name="stats"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static decimal CalculateDamagePerSecond(AllStats stats, AttackMode mode)
        {
            decimal damagePerHit = CalculateAverageDamagePerHit(stats, mode);
            return damagePerHit; // TODO
        }
    }
}
