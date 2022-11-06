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
        public void UpdateModifiersForMelodyCompletion(List<string>? selectedSongNames, Modifier intelligence)
        {

            try
            {
                int sum = 0;
                if (selectedSongNames != null)
                {
                    foreach (var item in selectedSongNames)
                    {
                        sum += Simu.Common.Systems.Profile.MelodySong.IntelligenceForSongs[item].Intelligence;
                    }
                }
                
                intelligence.Value = sum;
            }
            catch (Exception ex) 
            {
                //TODO: log?
                intelligence.Value = 0;
            }
        }
        public void UpdateModifiersForZombieSlayerLevels(int level, Modifier health)
        {
            if (level < ProfileConstants.SLAYER_LEVEL_MIN || level > ProfileConstants.SLAYER_LEVEL_MAX) return;

            health.Value = level switch
            {
                1 => 2,
                2 => 4,
                3 => 7,
                4 => 10,
                5 => 14,
                6 => 18,
                7 => 23,
                8 => 28,
                9 => 34,
                _ => 0
            };
        }
        public void UpdateModifiersForSpiderSlayerLevels(int level, Modifier critChance, Modifier critDamage)
        {
            if (level < ProfileConstants.SLAYER_LEVEL_MIN || level > ProfileConstants.SLAYER_LEVEL_MAX) return;

            critDamage.Value = level switch
            {
                1 => 1,
                2 => 2,
                3 => 3,
                4 => 4,
                5 => 6,
                6 or 7 => 8,
                8 => 11,
                9 => 14,
                _ => 0
            };
            critChance.Value = level switch
            {
                >= 7 and <= 9 => 1,
                _ => 0
            };
        }
        public void UpdateModifiersForWolfSlayerLevels(int level, Modifier speed, Modifier health, Modifier critDamage)
        {
            if (level < ProfileConstants.SLAYER_LEVEL_MIN || level > ProfileConstants.SLAYER_LEVEL_MAX) return;

            speed.Value = level switch
            {
                1 or 2 => 1,
                >= 3 and <= 8 => 2,
                8 or 9 => 3,
                _ => 0
            };
            health.Value = level switch
            {
                2 or 3 => 2,
                4 or 5 => 4,
                >= 6 and <= 8 => 7,
                9 => 12,
                _ => 0
            };
            critDamage.Value = level switch
            {
                < 5 => 0,
                5 or 6 => 1,
                >= 7 and <= 9 => 3,
                _ => 0
            };
        }
        public void UpdateModifiersForEndermanSlayerLevels(int level, Modifier health, Modifier intelligence)
        {
            if (level < ProfileConstants.SLAYER_LEVEL_MIN || level > ProfileConstants.SLAYER_LEVEL_MAX) return;

            health.Value = level switch
            {
                1 or 2 => 1,
                3 or 4 => 3,
                5 or 6 => 6,
                7 or 8 => 10,
                9 => 15,
                _ => 0
            };
            intelligence.Value = level switch
            {
                1 => 0,
                2 or 3 => 1,
                4 or 5 => 3,
                6 or 7 => 6,
                8 or 9 => 10,
                _ => 0
            };
        }
        public void UpdateModifiersForBlazeSlayerLevels(int level, Modifier health, Modifier strength, Modifier trueDefense)
        {
            if (level < ProfileConstants.SLAYER_LEVEL_MIN || level > ProfileConstants.SLAYER_LEVEL_MAX) return;

            health.Value = level switch
            {
                1 or 2 => 3,
                3 or 4 => 7,
                5 or 6 => 12,
                7 or 8 => 18,
                9 => 25,
                _ => 0
            };
            strength.Value = level switch
            {
                >= 2 and <= 5 => 1,
                >= 6 and <= 9 => 3,
                _ => 0
            };
            trueDefense.Value = level switch
            {
                >= 4 and <= 7 => 1,
                >= 8 and <= 9 => 3,
                _ => 0
            };
        }
    }
}