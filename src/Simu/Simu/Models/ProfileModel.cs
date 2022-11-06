using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Simu.Common;
using Simu.Common.Constants;
using Simu.Logic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Simu.Models
{
    public class ProfileStats : NotifyPropertyChanged
    {
        private const string MODIFIER_SKYBLOCK_LEVEL = "Skyblock Level";

        private const string MODIFIER_FARMING_LEVEL = "Farming Level";
        private const string MODIFIER_MINING_LEVEL = "Mining Level";
        private const string MODIFIER_COMBAT_LEVEL = "Combat Level";
        private const string MODIFIER_FORAGING_LEVEL = "Foraging Level";
        private const string MODIFIER_FISHING_LEVEL = "Fishing Level";
        private const string MODIFIER_ENCHANTING_LEVEL = "Enchanting Level";
        private const string MODIFIER_ALCHEMY_LEVEL = "Alchemy Level";
        private const string MODIFIER_CARPENTRY_LEVEL = "Carpentry Level";
        private const string MODIFIER_TAMING_LEVEL = "Taming Level";
        private const string MODIFIER_DUNGEONEERING_LEVEL = "Catacombs Level";

        private const string MODIFIER_MELODY_COMPLETION = "Melody Completion";

        private const string MODIFIER_SLAYERZOMBIE = "Zombie Slayer";
        private const string MODIFIER_SLAYERSPIDER = "Spider Slayer";
        private const string MODIFIER_SLAYERWOLF = "Wolf Slayer";
        private const string MODIFIER_SLAYERENDERMAN = "Enderman Slayer";
        private const string MODIFIER_SLAYERBLAZE = "Blaze Slayer";

        #region Modifiers

        public Modifier Modifier_SkyblockLevel_Health { get; set; }
        public Modifier Modifier_SkyblockLevel_Strength { get; set; }

        public Modifier Modifier_FarmingLevel_Health { get; set; }
        public Modifier Modifier_MiningLevel_Defense { get; set; }
        public Modifier Modifier_CombatLevel_IncreasedMeleeDamagePercent { get; set; }
        public Modifier Modifier_CombatLevel_IncreasedRangedDamagePercent { get; set; }
        public Modifier Modifier_CombatLevel_IncreasedMagicDamagePercent { get; set; }
        public Modifier Modifier_CombatLevel_CritChancePercent { get; set; }
        public Modifier Modifier_ForagingLevel_Strength { get; set; }
        public Modifier Modifier_FishingLevel_Health { get; set; }
        public Modifier Modifier_EnchantingLevel_Intelligence { get; set; }
        public Modifier Modifier_EnchantingLevel_AbilityDamage { get; set; }
        public Modifier Modifier_AlchemyLevel_Intelligence { get; set; }
        public Modifier Modifier_CarpentryLevel_Health { get; set; }
        public Modifier Modifier_TamingLevel_PetLuck { get; set; }
        public Modifier Modifier_DungeoneeringLevel_Health { get; set; }

        public Modifier Modifier_Melody_Intelligence { get; set; }

        public Modifier Modifier_SlayerZombie_Health { get; set; }
        public Modifier Modifier_SlayerSpider_CritDamagePercent { get; set; }
        public Modifier Modifier_SlayerSpider_CritChancePercent { get; set; }
        public Modifier Modifier_SlayerWolf_Speed { get; set; }
        public Modifier Modifier_SlayerWolf_Health { get; set; }
        public Modifier Modifier_SlayerWolf_CritDamage { get; set; }
        public Modifier Modifier_SlayerEnderman_Health { get; set; }
        public Modifier Modifier_SlayerEnderman_Intelligence { get; set; }
        public Modifier Modifier_SlayerBlaze_Health { get; set; }
        public Modifier Modifier_SlayerBlaze_Strength { get; set; }
        public Modifier Modifier_SlayerBlaze_TrueDefense { get; set; }

        //public Modifier TestModifier { get; set; }

        #endregion

        private bool _isInitialized;

        private ProfileLogic _logic;
        public Stats AllStats { get; set; }

        public event OnPropertyChanged? OnChange;

        /// <summary>
        /// Copy from other ProfileStats.
        /// </summary>
        /// <param name="stats"></param>
        /// <param name="other"></param>
        public ProfileStats(Stats stats, ProfileStats other)
        {
            AllStats = stats;
            _logic = new ProfileLogic();

            Modifier_SkyblockLevel_Health = stats.Health.BaseStats.Get(MODIFIER_SKYBLOCK_LEVEL);
            Modifier_SkyblockLevel_Strength = stats.Strength.BaseStats.Get(MODIFIER_SKYBLOCK_LEVEL);

            Modifier_FarmingLevel_Health = stats.Health.BaseStats.Get(MODIFIER_FARMING_LEVEL);
            Modifier_MiningLevel_Defense = stats.Defense.BaseStats.Get(MODIFIER_MINING_LEVEL);
            Modifier_CombatLevel_IncreasedMeleeDamagePercent = stats.IncreasedDamageMeleePercent.Get(MODIFIER_COMBAT_LEVEL);
            Modifier_CombatLevel_IncreasedRangedDamagePercent = stats.IncreasedDamageRangedPercent.Get(MODIFIER_COMBAT_LEVEL);
            Modifier_CombatLevel_IncreasedMagicDamagePercent = stats.IncreasedDamageMagicPercent.Get(MODIFIER_COMBAT_LEVEL);
            /* TODO: rest
            
            ...
            
            */
            SkyblockLevel = other.SkyblockLevel;
            Level_Farming = other.Level_Farming;
            Level_Mining = other.Level_Mining;
            Level_Combat = other.Level_Combat;
            Level_Foraging = other.Level_Foraging;
            Level_Fishing = other.Level_Fishing;
            Level_Enchanting = other.Level_Enchanting;
            Level_Alchemy = other.Level_Alchemy;
            Level_Carpentry = other.Level_Carpentry;
            Level_Taming = other.Level_Taming;
            Level_Dungeoneering = other.Level_Dungeoneering;
            MelodyCompletionNames = other.MelodyCompletionNames?.ToList();
        }

        public ProfileStats(Stats stats)
        {
            AllStats = stats;
            _logic = new ProfileLogic();

            Modifier_SkyblockLevel_Health = new(MODIFIER_SKYBLOCK_LEVEL, 0);

            Modifier_SkyblockLevel_Strength = new(MODIFIER_SKYBLOCK_LEVEL, 0);
            Modifier_FarmingLevel_Health = new(MODIFIER_FARMING_LEVEL, 0);
            Modifier_MiningLevel_Defense = new(MODIFIER_MINING_LEVEL, 0);
            Modifier_CombatLevel_IncreasedMeleeDamagePercent = new(MODIFIER_COMBAT_LEVEL, 0);
            Modifier_CombatLevel_IncreasedRangedDamagePercent = new(MODIFIER_COMBAT_LEVEL, 0);
            Modifier_CombatLevel_IncreasedMagicDamagePercent = new(MODIFIER_COMBAT_LEVEL, 0);
            Modifier_CombatLevel_CritChancePercent = new(MODIFIER_COMBAT_LEVEL, 0);
            Modifier_ForagingLevel_Strength = new(MODIFIER_FORAGING_LEVEL, 0);
            Modifier_FishingLevel_Health = new(MODIFIER_FISHING_LEVEL, 0);
            Modifier_EnchantingLevel_Intelligence = new(MODIFIER_ENCHANTING_LEVEL, 0);
            Modifier_EnchantingLevel_AbilityDamage = new(MODIFIER_ENCHANTING_LEVEL, 0);
            Modifier_AlchemyLevel_Intelligence = new(MODIFIER_ALCHEMY_LEVEL, 0);
            Modifier_CarpentryLevel_Health = new(MODIFIER_CARPENTRY_LEVEL, 0);
            Modifier_TamingLevel_PetLuck = new(MODIFIER_TAMING_LEVEL, 0);
            Modifier_DungeoneeringLevel_Health = new(MODIFIER_DUNGEONEERING_LEVEL, 0);
            /*
            TestModifier = new("scaling", 0);
            AllStats.Health.OnValueChanged += (res, total) =>
            {
                TestModifier.Value = res / 10.0m;
            };
            */

            Modifier_Melody_Intelligence = new(MODIFIER_MELODY_COMPLETION, 0);

            Modifier_SlayerZombie_Health = new(MODIFIER_SLAYERZOMBIE, 0);
            Modifier_SlayerSpider_CritDamagePercent = new(MODIFIER_SLAYERSPIDER, 0);
            Modifier_SlayerSpider_CritChancePercent = new(MODIFIER_SLAYERSPIDER, 0);
            Modifier_SlayerWolf_Speed = new(MODIFIER_SLAYERWOLF, 0);
            Modifier_SlayerWolf_Health = new(MODIFIER_SLAYERWOLF, 0);
            Modifier_SlayerWolf_CritDamage = new(MODIFIER_SLAYERWOLF, 0);
            Modifier_SlayerEnderman_Health = new(MODIFIER_SLAYERENDERMAN, 0);
            Modifier_SlayerEnderman_Intelligence = new(MODIFIER_SLAYERENDERMAN, 0);
            Modifier_SlayerBlaze_Health = new(MODIFIER_SLAYERBLAZE, 0);
            Modifier_SlayerBlaze_Strength = new(MODIFIER_SLAYERBLAZE, 0);
            Modifier_SlayerBlaze_TrueDefense = new(MODIFIER_SLAYERBLAZE, 0);

            //AllStats.Intelligence.BaseStats.Add(TestModifier);
            AllStats.Health.BaseStats.Add(Modifier_SkyblockLevel_Health);
            AllStats.Strength.BaseStats.Add(Modifier_SkyblockLevel_Strength);
            AllStats.Health.BaseStats.Add(Modifier_FarmingLevel_Health);
            AllStats.Defense.BaseStats.Add(Modifier_MiningLevel_Defense);
            AllStats.IncreasedDamageMeleePercent.Add(Modifier_CombatLevel_IncreasedMeleeDamagePercent);
            AllStats.IncreasedDamageRangedPercent.Add(Modifier_CombatLevel_IncreasedRangedDamagePercent);
            AllStats.IncreasedDamageMagicPercent.Add(Modifier_CombatLevel_IncreasedMagicDamagePercent);
            AllStats.CritChancePercent.BaseStats.Add(Modifier_CombatLevel_CritChancePercent);
            AllStats.Strength.BaseStats.Add(Modifier_ForagingLevel_Strength);
            AllStats.Health.BaseStats.Add(Modifier_FishingLevel_Health);
            AllStats.Intelligence.BaseStats.Add(Modifier_EnchantingLevel_Intelligence);
            AllStats.AbilityDamagePercent.BaseStats.Add(Modifier_EnchantingLevel_AbilityDamage);
            AllStats.Intelligence.BaseStats.Add(Modifier_AlchemyLevel_Intelligence);
            AllStats.Health.BaseStats.Add(Modifier_CarpentryLevel_Health);
            AllStats.PetLuck.BaseStats.Add(Modifier_TamingLevel_PetLuck);
            AllStats.Health.BaseStats.Add(Modifier_DungeoneeringLevel_Health);

            AllStats.Intelligence.BaseStats.Add(Modifier_Melody_Intelligence);

            AllStats.Health.BaseStats.Add(Modifier_SlayerZombie_Health);
            AllStats.CritDamagePercent.BaseStats.Add(Modifier_SlayerSpider_CritDamagePercent);
            AllStats.CritChancePercent.BaseStats.Add(Modifier_SlayerSpider_CritChancePercent);
            AllStats.Speed.BaseStats.Add(Modifier_SlayerWolf_Speed);
            AllStats.Health.BaseStats.Add(Modifier_SlayerWolf_Health);
            AllStats.CritDamagePercent.BaseStats.Add(Modifier_SlayerWolf_CritDamage);
            AllStats.Health.BaseStats.Add(Modifier_SlayerEnderman_Health);
            AllStats.Intelligence.BaseStats.Add(Modifier_SlayerEnderman_Intelligence);
            AllStats.Health.BaseStats.Add(Modifier_SlayerBlaze_Health);
            AllStats.Strength.BaseStats.Add(Modifier_SlayerBlaze_Strength);
            AllStats.TrueDefense.BaseStats.Add(Modifier_SlayerBlaze_TrueDefense);

            _isInitialized = true;
        }

        //TODO: add ctor for deserialization

        #region SkyblockLevel

        private int skyblockLevel;
        public int SkyblockLevel { get => skyblockLevel; set => Set(ref skyblockLevel, value, SetSkyblockLevel, OnValidInputSubmit, _isInitialized); }

        public void SetSkyblockLevel()
        {
            _logic.UpdateModifiersForSkyblockLevels(skyblockLevel, Modifier_SkyblockLevel_Health, Modifier_SkyblockLevel_Strength);
        }
        public IEnumerable<FormattedString> SkyblockLevelBonusFormatted()
        {
            return new List<FormattedString>
            {
                DisplayConstants.FormattedStringHealth(Modifier_SkyblockLevel_Health.Value),
                new() { IsNewline = true },
                DisplayConstants.FormattedStringStrength(Modifier_SkyblockLevel_Strength.Value)

            };
        }

        #endregion

        #region Skills

        #region Farming

        private int level_Farming;
        public int Level_Farming { get => level_Farming; set => Set(ref level_Farming, value, SetFarmingLevel, OnValidInputSubmit, _isInitialized); }

        public void SetFarmingLevel()
        {
            _logic.UpdateModifiersForFarmingLevels(level_Farming, Modifier_FarmingLevel_Health);
        }

        public IEnumerable<FormattedString> FarmingLevelBonusFormatted()
        {
            return new List<FormattedString>
            {
                DisplayConstants.FormattedStringHealth(Modifier_FarmingLevel_Health.Value)
            };
        }

        #endregion

        #region Mining

        private int level_Mining;
        public int Level_Mining { get => level_Mining; set => Set(ref level_Mining, value, SetMiningLevel, OnValidInputSubmit, _isInitialized); }

        public void SetMiningLevel()
        {
            _logic.UpdateModifiersForMiningLevels(level_Mining, Modifier_MiningLevel_Defense);
        }

        public IEnumerable<FormattedString> MiningLevelBonusFormatted()
        {
            return new List<FormattedString>
            {
                DisplayConstants.FormattedStringDefense(Modifier_MiningLevel_Defense.Value)
            };
        }

        #endregion

        #region Combat

        private int level_Combat;
        public int Level_Combat { get => level_Combat; set => Set(ref level_Combat, value, SetCombatLevel, OnValidInputSubmit, _isInitialized); }

        public void SetCombatLevel()
        {
            _logic.UpdateModifiersForCombatLevels(level_Combat,
                Modifier_CombatLevel_IncreasedMeleeDamagePercent,
                Modifier_CombatLevel_IncreasedRangedDamagePercent,
                Modifier_CombatLevel_IncreasedMagicDamagePercent,
                Modifier_CombatLevel_CritChancePercent);
        }

        public IEnumerable<FormattedString> CombatLevelBonusFormatted()
        {
            return new List<FormattedString>
            {
                DisplayConstants.FormattedStringCritChance(Modifier_CombatLevel_CritChancePercent.Value),
                new() { IsNewline = true},
                DisplayConstants.FormattedStringDamage(Modifier_CombatLevel_IncreasedMeleeDamagePercent.Value)
            };
        }

        #endregion

        #region Foraging

        private int level_Foraging;
        public int Level_Foraging { get => level_Foraging; set => Set(ref level_Foraging, value, SetForagingLevel, OnValidInputSubmit, _isInitialized); }

        public void SetForagingLevel()
        {
            _logic.UpdateModifiersForForagingLevels(level_Foraging, Modifier_ForagingLevel_Strength);
        }

        public IEnumerable<FormattedString> ForagingLevelBonusFormatted()
        {
            return new List<FormattedString>
            {
                DisplayConstants.FormattedStringStrength(Modifier_ForagingLevel_Strength.Value)
            };
        }

        #endregion

        #region Fishing

        private int level_Fishing;
        public int Level_Fishing { get => level_Fishing; set => Set(ref level_Fishing, value, SetFishingLevel, OnValidInputSubmit, _isInitialized); }

        public void SetFishingLevel()
        {
            _logic.UpdateModifiersForFishingLevels(level_Fishing, Modifier_FishingLevel_Health);
        }

        public IEnumerable<FormattedString> FishingLevelBonusFormatted()
        {
            return new List<FormattedString>
            {
                DisplayConstants.FormattedStringHealth(Modifier_FishingLevel_Health.Value)
            };
        }

        #endregion

        #region Enchanting

        private int level_Enchanting;
        public int Level_Enchanting { get => level_Enchanting; set => Set(ref level_Enchanting, value, SetEnchantingLevel, OnValidInputSubmit, _isInitialized); }

        public void SetEnchantingLevel()
        {
            _logic.UpdateModifiersForEnchantingLevels(level_Enchanting, Modifier_EnchantingLevel_Intelligence, Modifier_EnchantingLevel_AbilityDamage);
        }

        public IEnumerable<FormattedString> EnchantingLevelBonusFormatted()
        {
            return new List<FormattedString>
            {
                DisplayConstants.FormattedStringIntelligence(Modifier_EnchantingLevel_Intelligence.Value),
                new() { IsNewline = true },
                DisplayConstants.FormattedStringAbilityDamage(Modifier_EnchantingLevel_AbilityDamage.Value)
            };
        }

        #endregion

        #region Alchemy

        private int level_Alchemy;
        public int Level_Alchemy { get => level_Alchemy; set => Set(ref level_Alchemy, value, SetAlchemyLevel, OnValidInputSubmit, _isInitialized); }

        public void SetAlchemyLevel()
        {
            _logic.UpdateModifiersForAlchemyLevels(level_Alchemy, Modifier_AlchemyLevel_Intelligence);
        }

        public IEnumerable<FormattedString> AlchemyLevelBonusFormatted()
        {
            return new List<FormattedString>
            {
                DisplayConstants.FormattedStringIntelligence(Modifier_AlchemyLevel_Intelligence.Value)
            };
        }

        #endregion

        #region Carpentry

        private int level_Carpentry;
        public int Level_Carpentry { get => level_Carpentry; set => Set(ref level_Carpentry, value, SetCarpentryLevel, OnValidInputSubmit, _isInitialized); }

        public void SetCarpentryLevel()
        {
            _logic.UpdateModifiersForCarpentryLevels(level_Carpentry, Modifier_CarpentryLevel_Health);
        }

        public IEnumerable<FormattedString> CarpentryLevelBonusFormatted()
        {
            return new List<FormattedString>
            {
                DisplayConstants.FormattedStringHealth(Modifier_CarpentryLevel_Health.Value)
            };
        }

        #endregion

        #region Taming

        private int level_Taming;
        public int Level_Taming { get => level_Taming; set => Set(ref level_Taming, value, SetTamingLevel, OnValidInputSubmit, _isInitialized); }

        public void SetTamingLevel()
        {
            _logic.UpdateModifiersForTamingLevels(level_Taming, Modifier_TamingLevel_PetLuck);
        }

        public IEnumerable<FormattedString> TamingLevelBonusFormatted()
        {
            return new List<FormattedString>
            {
                DisplayConstants.FormattedStringPetLuck(Modifier_TamingLevel_PetLuck.Value)
            };
        }

        #endregion

        #region Dungeoneering

        private int level_Dungeoneering;
        public int Level_Dungeoneering { get => level_Dungeoneering; set => Set(ref level_Dungeoneering, value, SetDungeoneeringLevel, OnValidInputSubmit, _isInitialized); }

        public void SetDungeoneeringLevel()
        {
            _logic.UpdateModifiersForDungeoneeringLevels(level_Dungeoneering, Modifier_DungeoneeringLevel_Health, AllStats!.DungeoneeringDungeonizedMultiplier);
        }

        public IEnumerable<FormattedString> DungeoneeringLevelBonusFormatted()
        {
            return new List<FormattedString>
            {
                DisplayConstants.FormattedStringHealth(Modifier_DungeoneeringLevel_Health.Value),
                new() { IsNewline = true },
                DisplayConstants.FormattedStringStars(AllStats!.DungeoneeringDungeonizedMultiplier.Value)
            };
        }

        #endregion

        #endregion

        #region Melody

        /// <summary>
        /// Completed <see cref="Simu.Common.Systems.Profile.MelodySong"/>s by name.
        /// </summary>
        public IEnumerable<string>? MelodyCompletionNames { get; set; } = new string[] { };

        public void OnMelodySongsChange()
        {
            _logic.UpdateModifiersForMelodyCompletion(MelodyCompletionNames?.ToList(), Modifier_Melody_Intelligence);
            OnValidInputSubmit();
        }

        public IEnumerable<FormattedString> MelodySongsBonusFormatted()
        {
            return new List<FormattedString>
            {
                DisplayConstants.FormattedStringIntelligence(Modifier_Melody_Intelligence.Value)
            };
        }

        #endregion

        #region Slayer

        #region Zombie

        private int slayer_Zombie;
        public int Slayer_Zombie { get => slayer_Zombie; set => Set(ref slayer_Zombie, value, SetZombieSlayer, OnValidInputSubmit, _isInitialized); }

        public void SetZombieSlayer()
        {
            _logic.UpdateModifiersForZombieSlayerLevels(slayer_Zombie, Modifier_SlayerZombie_Health);
        }

        public IEnumerable<FormattedString> ZombieSlayerBonusFormatted()
        {
            return new List<FormattedString>
            {
                DisplayConstants.FormattedStringHealth(Modifier_SlayerZombie_Health.Value)
            };
        }

        #endregion

        #region Spider

        private int slayer_Spider;
        public int Slayer_Spider { get => slayer_Spider; set => Set(ref slayer_Spider, value, SetSpiderSlayer, OnValidInputSubmit, _isInitialized); }

        public void SetSpiderSlayer()
        {
            _logic.UpdateModifiersForSpiderSlayerLevels(slayer_Spider, Modifier_SlayerSpider_CritChancePercent, Modifier_SlayerSpider_CritDamagePercent);
        }

        public IEnumerable<FormattedString> SpiderSlayerBonusFormatted()
        {
            return new List<FormattedString>
            {
                DisplayConstants.FormattedStringCritChance(Modifier_SlayerSpider_CritChancePercent.Value),
                new() { IsNewline = true },
                DisplayConstants.FormattedStringCritDamage(Modifier_SlayerSpider_CritDamagePercent.Value)
            };
        }

        #endregion

        #region Wolf

        private int slayer_Wolf;
        public int Slayer_Wolf { get => slayer_Wolf; set => Set(ref slayer_Wolf, value, SetWolfSlayer, OnValidInputSubmit, _isInitialized); }

        public void SetWolfSlayer()
        {
            _logic.UpdateModifiersForWolfSlayerLevels(slayer_Wolf, Modifier_SlayerWolf_Speed, Modifier_SlayerWolf_Health, Modifier_SlayerWolf_CritDamage);
        }

        public IEnumerable<FormattedString> WolfSlayerBonusFormatted()
        {
            return new List<FormattedString>
            {
                DisplayConstants.FormattedStringSpeed(Modifier_SlayerWolf_Speed.Value),
                new() { IsNewline = true },
                DisplayConstants.FormattedStringHealth(Modifier_SlayerWolf_Health.Value),
                new() { IsNewline = true },
                DisplayConstants.FormattedStringCritDamage(Modifier_SlayerWolf_CritDamage.Value)
            };
        }

        #endregion

        #region Enderman

        private int slayer_Enderman;
        public int Slayer_Enderman { get => slayer_Enderman; set => Set(ref slayer_Enderman, value, SetEndermanSlayer, OnValidInputSubmit, _isInitialized); }

        public void SetEndermanSlayer()
        {
            _logic.UpdateModifiersForEndermanSlayerLevels(slayer_Enderman, Modifier_SlayerEnderman_Health, Modifier_SlayerEnderman_Intelligence);
        }

        public IEnumerable<FormattedString> EndermanSlayerBonusFormatted()
        {
            return new List<FormattedString>
            {
                DisplayConstants.FormattedStringHealth(Modifier_SlayerEnderman_Health.Value),
                new() { IsNewline = true },
                DisplayConstants.FormattedStringIntelligence(Modifier_SlayerEnderman_Intelligence.Value)
            };
        }

        #endregion

        #region Blaze

        private int slayer_Blaze;
        public int Slayer_Blaze { get => slayer_Blaze; set => Set(ref slayer_Blaze, value, SetBlazeSlayer, OnValidInputSubmit, _isInitialized); }

        public void SetBlazeSlayer()
        {
            _logic.UpdateModifiersForBlazeSlayerLevels(slayer_Blaze, Modifier_SlayerBlaze_Health, Modifier_SlayerBlaze_Strength, Modifier_SlayerBlaze_TrueDefense);
        }

        public IEnumerable<FormattedString> BlazeSlayerBonusFormatted()
        {
            return new List<FormattedString>
            {
                DisplayConstants.FormattedStringHealth(Modifier_SlayerBlaze_Health.Value),
                new() { IsNewline = true },
                DisplayConstants.FormattedStringStrength(Modifier_SlayerBlaze_Strength.Value),
                new() { IsNewline = true },
                DisplayConstants.FormattedStringTrueDefense(Modifier_SlayerBlaze_TrueDefense.Value)
            };
        }

        #endregion

        #endregion

        private void OnValidInputSubmit()
        {
            OnChange?.Invoke();
        }

    }
}
