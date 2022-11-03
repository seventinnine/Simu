using Simu.Common;
using Simu.Common.Constants;

namespace Simu.Logic
{
    public class ProfileLogic
    {
        public void UpdateModifiersForSkyblockLevels(int level, Modifier health, Modifier strength)
        {
            if (level < ProfileConstants.SKYBLOCK_LEVEL_MIN || level > ProfileConstants.SKYBLOCK_LEVEL_MAX) return;

            health.Value = level * 5 + (level / 10) * 10;
            strength.Value = (level / 5) * 1;
        }
        public void UpdateModifiersForFarmingLevels(int level, Modifier health)
        {
            if (level < ProfileConstants.SKYBLOCK_FARMING_MIN || level > ProfileConstants.SKYBLOCK_FARMING_MAX) return;

            health.Value = level switch
            {
                (< 15) => level * 2,
                (< 20) => 28 + (level - 14) * 3, // 28 health for levels 0-14
                (< 26) => 43 + (level - 19) * 4, // 43 health for levels 0-19
                _      => 67 + (level - 25) * 5  // 67 health for levels 0-25
            };
        }
        public void UpdateModifiersForMiningLevels(int level, Modifier defense)
        {
            if (level < ProfileConstants.SKYBLOCK_MINING_MIN || level > ProfileConstants.SKYBLOCK_MINING_MAX) return;

            defense.Value = level switch
            {
                (< 15) => level * 1,
                _      => 14 + (level - 14) * 2, // 14 defense for levels 0-14
            };
        }
        public void UpdateModifiersForCombatLevels(int level, Modifier melee, Modifier ranged, Modifier magic, Modifier critChance)
        {
            if (level < ProfileConstants.SKYBLOCK_COMBAT_MIN || level > ProfileConstants.SKYBLOCK_COMBAT_MAX) return;

            decimal damageBonus = level switch
            {
                (< 51) => level * 4.0m,
                _      => 200.0m + (level - 50) * 1.0m, // 200 damagePercent for levels 0-50
            };
            melee.Value = damageBonus;
            ranged.Value = damageBonus;
            magic.Value = damageBonus;

            critChance.Value = level * 0.5m;
        }
        public void UpdateModifiersForForagingLevels(int level, Modifier strength)
        {
            if (level < ProfileConstants.SKYBLOCK_FORAGING_MIN || level > ProfileConstants.SKYBLOCK_FORAGING_MAX) return;

            strength.Value = level switch
            {
                (< 15) => level * 1,
                _      => 14 + (level - 14) * 2, // 14 strength for levels 0-14
            };
        }
        public void UpdateModifiersForFishingLevels(int level, Modifier health)
        {
            if (level < ProfileConstants.SKYBLOCK_FISHING_MIN || level > ProfileConstants.SKYBLOCK_FISHING_MAX) return;

            health.Value = level switch
            {
                (< 15) => level * 2,
                (< 20) => 28 + (level - 14) * 3, // 28 health for levels 0-14
                (< 26) => 43 + (level - 19) * 4, // 43 health for levels 0-19
                _      => 67 + (level - 25) * 5  // 67 health for levels 0-25
            };
        }
        public void UpdateModifiersForEnchantingLevels(int level, Modifier intelligence, Modifier abilityDamage)
        {
            if (level < ProfileConstants.SKYBLOCK_ENCHANTING_MIN || level > ProfileConstants.SKYBLOCK_ENCHANTING_MAX) return;

            intelligence.Value = level switch
            {
                (< 15) => level * 1,
                _      => 14 + (level - 14) * 2, // 14 intelligence for levels 0-14
            };
            abilityDamage.Value = level * 0.5m;
        }
        public void UpdateModifiersForAlchemyLevels(int level, Modifier intelligence)
        {
            if (level < ProfileConstants.SKYBLOCK_ALCHEMY_MIN || level > ProfileConstants.SKYBLOCK_ALCHEMY_MAX) return;

            intelligence.Value = level switch
            {
                (< 15) => level * 1,
                _      => 14 + (level - 14) * 2, // 14 intelligence for levels 0-14
            };
        }
        public void UpdateModifiersForCarpentryLevels(int level, Modifier health)
        {
            if (level < ProfileConstants.SKYBLOCK_CARPENTRY_MIN || level > ProfileConstants.SKYBLOCK_CARPENTRY_MAX) return;

            health.Value = level switch
            {
                (< 50) => level * 1,
                _ => 49         // 49 health for levels 0-49
            };
        }
        public void UpdateModifiersForTamingLevels(int level, Modifier petluck)
        {
            if (level < ProfileConstants.SKYBLOCK_TAMING_MIN || level > ProfileConstants.SKYBLOCK_TAMING_MAX) return;

            petluck.Value = level switch
            {
                _ => level * 1
            };
        }
        public void UpdateModifiersForDungeoneeringLevels(int level, Modifier health, Modifier stars)
        {
            if (level < ProfileConstants.SKYBLOCK_DUNGEONEERING_MIN || level > ProfileConstants.SKYBLOCK_DUNGEONEERING_MAX) return;

            health.Value = level switch
            {
                _ => level * 2
            };
            stars.Value = level switch
            {
                (< 6) => level * 4,
                (< 11) => 20 + (level - 5) * 5, //  20 from levels 0-5
                (< 16) => 45 + (level - 10) * 6, //  45 from levels 0-10
                (< 21) => 75 + (level - 15) * 7, //  75 from levels 0-15
                (< 26) => 110 + (level - 20) * 8, // 110 from levels 0-20
                (< 31) => 150 + (level - 25) * 9, // 150 from levels 0-25
                (< 36) => 195 + (level - 30) * 10, // 195 from levels 0-30
                (< 41) => 245 + (level - 35) * 12, // 245 from levels 0-35
                (< 46) => 305 + (level - 40) * 14, // 305 from levels 0-40
                (  46) => 391, 
                (  47) => 408,               
                (  48) => 426,                
                (  49) => 445,                
                _      => 465,                
            };
        }
    }
}