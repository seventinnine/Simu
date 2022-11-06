using Simu.Common.Equipment;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    public class MeleeDamageBreakdown : AttackDamageBreakdown
    {
        public override string GetBreakdownFormatted()
        {
            throw new NotImplementedException();
        }
    }

    public class RangedDamageBreakdown : AttackDamageBreakdown
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
    public class DamageCalculator
    {
        public Stats AllStats { get; set; }
        public AttackMode AttackMode { get; set; }

        public DamageBreakdown Breakdown { get; set; }

        //TODO: abstract class?
        public Weapon Weapon { get; set; }

        public DamageCalculator(Stats allStats, AttackMode attackMode = AttackMode.Melee)
        {
            AllStats = allStats;
            AttackMode = attackMode;
            Breakdown = new MeleeDamageBreakdown();
            Weapon = new Weapon() { Name = "Fist" };
        }

        //TODO: impl clone

        public void RecalculateAllStats()
        {
            Constants.ProfileConstants.calculations = 0;
            //TODO: move to Stats
            //AllStats.FlatAttackDamage.CalculateTotal(AllStats.ConditionalTags);
            //AllStats.FlatAbilityDamage.CalculateTotal(AllStats.ConditionalTags);
            AllStats.IncreasedDamageMeleePercent.CalculateTotal(AllStats.ConditionalTags);
            AllStats.IncreasedDamageRangedPercent.CalculateTotal(AllStats.ConditionalTags);
            AllStats.IncreasedDamageMagicPercent.CalculateTotal(AllStats.ConditionalTags);
            AllStats.MoreDamagePercent.CalculateTotal(AllStats.ConditionalTags);
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
            AllStats.DamageReductionPercent.CalculateTotal(AllStats.ConditionalTags);
            Console.WriteLine($"Calculations: {Constants.ProfileConstants.calculations}");
        }

        /// <summary>
        /// Calculates the damage per hit based on <see cref="AllStats"/> and <see cref="AttackMode"/>.
        /// </summary>
        /// <returns></returns>
        private decimal CalculateAverageDamagePerHit()
        {
            if (AttackMode == AttackMode.Melee)
            {
                decimal baseDamage = Weapon.Damage;
                decimal strengthMultiplier = 1.0m + AllStats.Strength.CalculateTotal(AllStats.ConditionalTags) / 100.0m;
                decimal critChance = Math.Min(AllStats.CritChancePercent.CalculateTotal(AllStats.ConditionalTags) / 100.0m, 1.0m);
                decimal critDamageMuliplier = 1.0m + AllStats.CritDamagePercent.CalculateTotal(AllStats.ConditionalTags) / 100.0m;
                decimal increasedDamageMultiplier = 1.0m + AllStats.IncreasedDamageMeleePercent.CalculateTotal(AllStats.ConditionalTags) / 100.0m;
                decimal moreDamageMultiplier = 1.0m + AllStats.MoreDamagePercent.CalculateTotal(AllStats.ConditionalTags);
                //TODO: fabled reforges and stuff
                
                return baseDamage * strengthMultiplier * (critDamageMuliplier * critChance + (1.0m - critChance)) * increasedDamageMultiplier * moreDamageMultiplier; 
            }
            if (AttackMode == AttackMode.Ranged)
            {
                decimal baseDamage = Weapon.Damage;
                decimal strengthMultiplier = 1.0m + AllStats.Strength.CalculateTotal(AllStats.ConditionalTags) / 100.0m;
                decimal critChance = Math.Min(AllStats.CritChancePercent.CalculateTotal(AllStats.ConditionalTags) / 100.0m, 1.0m);
                decimal critDamageMuliplier = 1.0m + AllStats.CritDamagePercent.CalculateTotal(AllStats.ConditionalTags) / 100.0m;
                decimal increasedDamageMultiplier = 1.0m + AllStats.IncreasedDamageRangedPercent.CalculateTotal(AllStats.ConditionalTags) / 100.0m;
                decimal moreDamageMultiplier = 1.0m + AllStats.MoreDamagePercent.CalculateTotal(AllStats.ConditionalTags);
                return baseDamage * strengthMultiplier * (critDamageMuliplier * critChance + (1.0m - critChance)) * increasedDamageMultiplier * moreDamageMultiplier;
            }
            if (AttackMode == AttackMode.Magic)
            {
                decimal baseDamage = Weapon.Damage;
                decimal intelligenceMultiplier = (AllStats.Intelligence.CalculateTotal(AllStats.ConditionalTags) / 100.0m) * AllStats.IntelligenceScaleFactor;
                decimal abilityDamageMultiplier = 1.0m + AllStats.AbilityDamagePercent.CalculateTotal(AllStats.ConditionalTags) / 100.0m;
                decimal increasedDamageMultiplier = 1.0m + AllStats.IncreasedDamageMagicPercent.CalculateTotal(AllStats.ConditionalTags) / 100.0m;
                decimal moreDamageMultiplier = 1.0m + AllStats.MoreDamagePercent.CalculateTotal(AllStats.ConditionalTags);
                return baseDamage * intelligenceMultiplier * abilityDamageMultiplier * increasedDamageMultiplier * moreDamageMultiplier;
            }
            return -1.0m;
        }

        /// <summary>
        /// Calculates the <see cref="DamageBreakdown"/> based on <see cref="AllStats"/> and <see cref="AttackMode"/>.
        /// </summary>
        /// <returns></returns>
        public DamageBreakdown CalculateDamageBreakdown()
        {
            decimal damagePerHit = CalculateAverageDamagePerHit();
            if (AttackMode == AttackMode.Melee)
            {
                decimal attackSpeedMultiplier = 2.0m * (1.0m + AllStats.AttackSpeedPercent.CalculateTotal(AllStats.ConditionalTags) / 100.0m);
                decimal ferocityMultiplier = 1.0m + AllStats.Ferocity.CalculateTotal(AllStats.ConditionalTags) / 1.00m;
                decimal dps = damagePerHit * attackSpeedMultiplier * ferocityMultiplier;
                return new MeleeDamageBreakdown { DamagePerHit = damagePerHit, DamagePerSecond = dps, HitsPerSecond = attackSpeedMultiplier * ferocityMultiplier, AdditionalHitsChance = ferocityMultiplier};
            }
            else if (AttackMode == AttackMode.Ranged)
            {
                //TODO: attack speed table, shortbow or normal?
                return new RangedDamageBreakdown();
            }
            else if (AttackMode == AttackMode.Magic)
            {
                decimal castsPerSecond = 1.0m / AllStats.AbilityCooldown;
                decimal dps = damagePerHit * castsPerSecond;
                return new MagicDamageBreakdown() { DamagePerHit = damagePerHit, HitsPerSecond = castsPerSecond, DamagePerSecond = dps};
            }
            else
            {
                return new MeleeDamageBreakdown();
            }
        }

        public void RefreshDamageBreakdown()
        {
            Breakdown = CalculateDamageBreakdown();
        }
    }
}
