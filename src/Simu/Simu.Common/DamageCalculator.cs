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
    /// Calulcates the values for all <see cref="Stats"/>
    /// </summary>
    public class StatsCalculator
    {
        public Stats AllStats { get; set; }
        public AttackMode AttackMode { get; set; }

        public StatsCalculator(Stats allStats, AttackMode attackMode)
        {
            AllStats = allStats;
            AttackMode = attackMode;
        }

        public void RecalculateAllStats()
        {
            //TODO: move to Stats
            AllStats.FlatAttackDamage.CalculateTotal(AllStats.ConditionalTags);
            AllStats.FlatAbilityDamage.CalculateTotal(AllStats.ConditionalTags);
            AllStats.IncreasedDamageMeleePercent.ProductForMatchingTags(AllStats.ConditionalTags);
            AllStats.IncreasedDamageRangedPercent.ProductForMatchingTags(AllStats.ConditionalTags);
            AllStats.IncreasedDamageMagicPercent.ProductForMatchingTags(AllStats.ConditionalTags);
            AllStats.MoreDamagePercent.ProductForMatchingTags(AllStats.ConditionalTags);
            AllStats.Health.CalculateTotal(AllStats.ConditionalTags);
            AllStats.Defense.CalculateTotal(AllStats.ConditionalTags);
            AllStats.TrueDefense.CalculateTotal(AllStats.ConditionalTags);
            AllStats.Speed.CalculateTotal(AllStats.ConditionalTags);
            AllStats.Strength.CalculateTotal(AllStats.ConditionalTags);
            AllStats.Intelligence.CalculateTotal(AllStats.ConditionalTags);
            AllStats.CritChancePercent.CalculateTotal(AllStats.ConditionalTags);
            AllStats.CritDamagePercent.CalculateTotal(AllStats.ConditionalTags);
            AllStats.AttackSpeedPercent.CalculateTotal(AllStats.ConditionalTags);
            AllStats.Ferocity.CalculateTotal(AllStats.ConditionalTags);
            AllStats.AbilityDamagePercent.CalculateTotal(AllStats.ConditionalTags);
            AllStats.MagicFind.CalculateTotal(AllStats.ConditionalTags);
            AllStats.PetLuck.CalculateTotal(AllStats.ConditionalTags);
            AllStats.SeaCreatureChance.CalculateTotal(AllStats.ConditionalTags);
            AllStats.DamageReductionPercent.InverseProductForMatchingTags(AllStats.ConditionalTags);
        }

        /// <summary>
        /// Calculates the damage per hit based on <see cref="AllStats"/> and <see cref="AttackMode"/>.
        /// </summary>
        /// <returns></returns>
        public decimal CalculateAverageDamagePerHit()
        {
            if (AttackMode == AttackMode.Melee)
            {
                decimal baseDamage = AllStats.FlatAttackDamage.CalculateTotal(AllStats.ConditionalTags);
                decimal strengthMultiplier = 1.0m + AllStats.Strength.CalculateTotal(AllStats.ConditionalTags) / 100.0m;
                decimal critChance = Math.Min(AllStats.CritChancePercent.CalculateTotal(AllStats.ConditionalTags) / 100.0m, 1.0m);
                decimal critDamageMuliplier = 1.0m + AllStats.CritDamagePercent.CalculateTotal(AllStats.ConditionalTags) / 100.0m;
                decimal increasedDamageMultiplier = 1.0m + AllStats.IncreasedDamageMeleePercent.ProductForMatchingTags(AllStats.ConditionalTags);
                decimal moreDamageMultiplier = 1.0m + AllStats.MoreDamagePercent.ProductForMatchingTags(AllStats.ConditionalTags);
                //TODO: fabled

                return baseDamage * strengthMultiplier * (critDamageMuliplier * critChance + (1.0m - critChance)) * increasedDamageMultiplier * moreDamageMultiplier; 
            }
            else if (AttackMode == AttackMode.Ranged)
            {
                decimal baseDamage = AllStats.FlatAttackDamage.CalculateTotal(AllStats.ConditionalTags) + 5.0m;
                decimal strengthMultiplier = 1.0m + AllStats.Strength.CalculateTotal(AllStats.ConditionalTags) / 100.0m;
                decimal critChance = Math.Min(AllStats.CritChancePercent.CalculateTotal(AllStats.ConditionalTags) / 100.0m, 1.0m);
                decimal critDamageMuliplier = 1.0m + AllStats.CritDamagePercent.CalculateTotal(AllStats.ConditionalTags) / 100.0m;
                decimal increasedDamageMultiplier = 1.0m + AllStats.IncreasedDamageRangedPercent.ProductForMatchingTags(AllStats.ConditionalTags);
                decimal moreDamageMultiplier = 1.0m + AllStats.MoreDamagePercent.ProductForMatchingTags(AllStats.ConditionalTags);
                return baseDamage * strengthMultiplier * (critDamageMuliplier * critChance + (1.0m - critChance)) * increasedDamageMultiplier * moreDamageMultiplier;
            }
            else if (AttackMode == AttackMode.Magic)
            {
                decimal baseDamage = AllStats.FlatAbilityDamage.CalculateTotal(AllStats.ConditionalTags) + 5.0m;
                decimal intelligenceMultiplier = (AllStats.Intelligence.CalculateTotal(AllStats.ConditionalTags) / 100.0m) * AllStats.IntelligenceScaleFactor;
                decimal abilityDamageMultiplier = 1.0m + AllStats.AbilityDamagePercent.CalculateTotal(AllStats.ConditionalTags) / 100.0m;
                decimal increasedDamageMultiplier = 1.0m + AllStats.IncreasedDamageMagicPercent.ProductForMatchingTags(AllStats.ConditionalTags);
                decimal moreDamageMultiplier = 1.0m + AllStats.MoreDamagePercent.ProductForMatchingTags(AllStats.ConditionalTags);
                return baseDamage * intelligenceMultiplier * abilityDamageMultiplier * increasedDamageMultiplier * moreDamageMultiplier;
            }
            else
            {
                return -1.0m;
            }
        }

        /// <summary>
        /// Calculates the damage per second based on <see cref="AllStats"/> and <see cref="AttackMode"/>.
        /// </summary>
        /// <returns></returns>
        public decimal CalculateDamagePerSecond()
        {
            decimal damagePerHit = CalculateAverageDamagePerHit();

            if (AttackMode == AttackMode.Melee)
            {
                decimal attackSpeedMultiplier = 1.0m + AllStats.AttackSpeedPercent.CalculateTotal(AllStats.ConditionalTags) / 100.0m;
                decimal ferocityMultiplier = 1.0m + AllStats.Ferocity.CalculateTotal(AllStats.ConditionalTags) / 1.00m;
                return 2 * damagePerHit * attackSpeedMultiplier * ferocityMultiplier;
            }
            else if (AttackMode == AttackMode.Ranged)
            {
                return -1.0m;
            }
            else if (AttackMode == AttackMode.Magic)
            {
                decimal castsPerSecond = 1.0m / AllStats.AbilityCooldown;
                return damagePerHit * castsPerSecond;
            }
            else
            {
                return -1.0m;
            }
        }
    }
}
