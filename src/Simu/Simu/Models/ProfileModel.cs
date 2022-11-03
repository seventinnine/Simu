using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Simu.Common;
using Simu.Common.Constants;
using Simu.Logic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Simu.Models
{
    public class ProfileModel : NotifyPropertyChanged
    {
        private const string MODIFIER_SKYBLOCK_LEVEL = "SkyblockLevel";
        private const string MODIFIER_FARMING_LEVEL = "FarmingLevel";
        private const string MODIFIER_MINING_LEVEL = "MiningLevel";
        private const string MODIFIER_COMBAT_LEVEL = "CombatLevel";
        private const string MODIFIER_FORAGING_LEVEL = "ForagingLevel";
        private const string MODIFIER_FISHING_LEVEL = "FishingLevel";
        private const string MODIFIER_ENCHANTING_LEVEL = "EnchantingLevel";
        private const string MODIFIER_ALCHEMY_LEVEL = "AlchemyLevel";
        private const string MODIFIER_CARPENTRY_LEVEL = "CarpentryLevel";
        private const string MODIFIER_TAMING_LEVEL = "TamingLevel";
        private const string MODIFIER_DUNGEONEERING_LEVEL = "DungeoneeringLevel";

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

        #endregion

        private bool _isInitialized = false;

        private ProfileLogic _logic;

        public Stats? AllStats { get; set; }
        public event OnPropertyChanged? OnChange;
        public EditContext? Context { get; set; }

        public ProfileModel(ProfileLogic logic)
        {
            _logic = logic;
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
        }

        //TODO: add ctor for deserialization

        public void Initialize(Stats stats, EditContext context)
        {
            AllStats = stats;
            Context = context;
            _isInitialized = true;
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
        }


        #region SkyblockLevel

        private int skyblockLevel;

        [Range(ProfileConstants.SKYBLOCK_LEVEL_MIN, ProfileConstants.SKYBLOCK_LEVEL_MAX, ErrorMessage = "Level must be between 1 and 500.")]
        public int SkyblockLevel { get => skyblockLevel; set => Set(ref skyblockLevel, value, SetSkyblockLevel, OnValidInputSubmit, _isInitialized); }

        public void SetSkyblockLevel()
        {
            _logic.UpdateModifiersForSkyblockLevels(skyblockLevel, Modifier_SkyblockLevel_Health, Modifier_SkyblockLevel_Strength);
            AllStats!.Health.BaseStats.InvalidateCacheAndReplaceModifier(Modifier_SkyblockLevel_Health);
            AllStats.Strength.BaseStats.InvalidateCacheAndReplaceModifier(Modifier_SkyblockLevel_Strength);
        }
        public IEnumerable<FormattedString> SkyblockLevelBonusFormatted()
        {
            return new List<FormattedString>
            {
                new() { ColorCodeHex = ColorCodeHex.RED, Content = $"{Modifier_SkyblockLevel_Health.Value:N0} {DisplayConstants.SYMBOL_HEALTH} "},
                new() { Content = $" + "},
                new() { ColorCodeHex = ColorCodeHex.DARK_RED, Content = $"{Modifier_SkyblockLevel_Strength.Value:N0} {DisplayConstants.SYMBOL_STRENGTH} "}
                
            };
        }

        #endregion

        #region Skills

        #region Farming

        private int level_Farming;

        [Range(ProfileConstants.SKYBLOCK_FARMING_MIN, ProfileConstants.SKYBLOCK_FARMING_MAX, ErrorMessage = "Level must be between 0 and 60.")]
        public int Level_Farming { get => level_Farming; set => Set(ref level_Farming, value, SetFarmingLevel, OnValidInputSubmit, _isInitialized); }

        public void SetFarmingLevel()
        {
            _logic.UpdateModifiersForFarmingLevels(level_Farming, Modifier_FarmingLevel_Health);
            AllStats!.Health.BaseStats.InvalidateCacheAndReplaceModifier(Modifier_FarmingLevel_Health);
        }

        public IEnumerable<FormattedString> FarmingLevelBonusFormatted()
        {
            return new List<FormattedString>
            {
                new() { ColorCodeHex = ColorCodeHex.RED, Content = $"{Modifier_FarmingLevel_Health.Value:N0} {DisplayConstants.SYMBOL_HEALTH}"}
            };
        }

        #endregion

        #region Mining

        private int level_Mining;

        [Range(ProfileConstants.SKYBLOCK_MINING_MIN, ProfileConstants.SKYBLOCK_MINING_MAX, ErrorMessage = "Level must be between 0 and 60.")]
        public int Level_Mining { get => level_Mining; set => Set(ref level_Mining, value, SetMiningLevel, OnValidInputSubmit, _isInitialized); }

        public void SetMiningLevel()
        {
            _logic.UpdateModifiersForMiningLevels(level_Mining, Modifier_MiningLevel_Defense);
            AllStats!.Defense.BaseStats.InvalidateCacheAndReplaceModifier(Modifier_MiningLevel_Defense);
        }

        public IEnumerable<FormattedString> MiningLevelBonusFormatted()
        {
            return new List<FormattedString>
            {
                new() { ColorCodeHex = ColorCodeHex.GREEN, Content = $"{Modifier_MiningLevel_Defense.Value:N0} {DisplayConstants.SYMBOL_DEFENSE}"}
            };
        }

        #endregion

        #region Combat
        private int level_Combat;

        [Range(ProfileConstants.SKYBLOCK_COMBAT_MIN, ProfileConstants.SKYBLOCK_COMBAT_MAX, ErrorMessage = "Level must be between 0 and 60.")]
        public int Level_Combat { get => level_Combat; set => Set(ref level_Combat, value, SetCombatLevel, OnValidInputSubmit, _isInitialized); }

        public void SetCombatLevel()
        {
            _logic.UpdateModifiersForCombatLevels(level_Combat,
                Modifier_CombatLevel_IncreasedMeleeDamagePercent,
                Modifier_CombatLevel_IncreasedRangedDamagePercent,
                Modifier_CombatLevel_IncreasedMagicDamagePercent,
                Modifier_CombatLevel_CritChancePercent);
            AllStats!.IncreasedDamageMeleePercent.InvalidateCacheAndReplaceModifier(Modifier_CombatLevel_IncreasedMeleeDamagePercent);
            AllStats!.IncreasedDamageRangedPercent.InvalidateCacheAndReplaceModifier(Modifier_CombatLevel_IncreasedRangedDamagePercent);
            AllStats!.IncreasedDamageMagicPercent.InvalidateCacheAndReplaceModifier(Modifier_CombatLevel_IncreasedMagicDamagePercent);
            AllStats!.CritChancePercent.BaseStats.InvalidateCacheAndReplaceModifier(Modifier_CombatLevel_CritChancePercent);
        }

        public IEnumerable<FormattedString> CombatLevelBonusFormatted()
        {
            return new List<FormattedString>
            {
                new() { ColorCodeHex = ColorCodeHex.DARK_BLUE, Content = $"{Modifier_CombatLevel_CritChancePercent.Value:N1} {DisplayConstants.SYMBOL_CRITCHANCE}"},
                new() { Content = $" + "},
                new() { ColorCodeHex = ColorCodeHex.DARK_AQUA, Content = $"{Modifier_CombatLevel_IncreasedMeleeDamagePercent.Value:N0} % {DisplayConstants.SYMBOL_DAMAGE}"}
            };
        }

        #endregion

        #region Foraging

        private int level_Foraging;
        [Range(ProfileConstants.SKYBLOCK_FORAGING_MIN, ProfileConstants.SKYBLOCK_FORAGING_MAX, ErrorMessage = "Level must be between 0 and 50.")]
        public int Level_Foraging { get => level_Foraging; set => Set(ref level_Foraging, value, SetForagingLevel, OnValidInputSubmit, _isInitialized); }

        public void SetForagingLevel()
        {
            _logic.UpdateModifiersForForagingLevels(level_Foraging, Modifier_ForagingLevel_Strength);
            AllStats!.Strength.BaseStats.InvalidateCacheAndReplaceModifier(Modifier_ForagingLevel_Strength);
        }

        public IEnumerable<FormattedString> ForagingLevelBonusFormatted()
        {
            return new List<FormattedString>
            {
                new() { ColorCodeHex = ColorCodeHex.DARK_RED, Content = $"{Modifier_ForagingLevel_Strength.Value:N0} {DisplayConstants.SYMBOL_STRENGTH}"}
            };
        }

        #endregion

        #region Fishing

        private int level_Fishing;
        [Range(ProfileConstants.SKYBLOCK_FISHING_MIN, ProfileConstants.SKYBLOCK_FISHING_MAX, ErrorMessage = "Level must be between 0 and 50.")]
        public int Level_Fishing { get => level_Fishing; set => Set(ref level_Fishing, value, SetFishingLevel, OnValidInputSubmit, _isInitialized); }

        public void SetFishingLevel()
        {
            _logic.UpdateModifiersForFishingLevels(level_Fishing, Modifier_FishingLevel_Health);
            AllStats!.Health.BaseStats.InvalidateCacheAndReplaceModifier(Modifier_FishingLevel_Health);
        }

        public IEnumerable<FormattedString> FishingLevelBonusFormatted()
        {
            return new List<FormattedString>
            {
                new() { ColorCodeHex = ColorCodeHex.RED, Content = $"{Modifier_FishingLevel_Health.Value:N0} {DisplayConstants.SYMBOL_HEALTH}"}
            };
        }

        #endregion

        #region Enchanting

        private int level_Enchanting;
        [Range(ProfileConstants.SKYBLOCK_ENCHANTING_MIN, ProfileConstants.SKYBLOCK_ENCHANTING_MAX, ErrorMessage = "Level must be between 0 and 60.")]
        public int Level_Enchanting { get => level_Enchanting; set => Set(ref level_Enchanting, value, SetEnchantingLevel, OnValidInputSubmit, _isInitialized); }

        public void SetEnchantingLevel()
        {
            _logic.UpdateModifiersForEnchantingLevels(level_Enchanting, Modifier_EnchantingLevel_Intelligence, Modifier_EnchantingLevel_AbilityDamage);
            AllStats!.Intelligence.BaseStats.InvalidateCacheAndReplaceModifier(Modifier_EnchantingLevel_Intelligence);
            AllStats!.AbilityDamagePercent.BaseStats.InvalidateCacheAndReplaceModifier(Modifier_EnchantingLevel_AbilityDamage);
        }

        public IEnumerable<FormattedString> EnchantingLevelBonusFormatted()
        {
            return new List<FormattedString>
            {
                new() { ColorCodeHex = ColorCodeHex.AQUA, Content = $"{Modifier_EnchantingLevel_Intelligence.Value:N0} {DisplayConstants.SYMBOL_INTELLIGENCE}"},
                new() {Content = " + "},
                new() { ColorCodeHex = ColorCodeHex.RED, Content = $"{Modifier_EnchantingLevel_AbilityDamage.Value:N1} {DisplayConstants.SYMBOL_ABILITYDAMAGE}"}
            };
        }

        #endregion

        #region Alchemy

        private int level_Alchemy;
        [Range(ProfileConstants.SKYBLOCK_ALCHEMY_MIN, ProfileConstants.SKYBLOCK_ALCHEMY_MAX, ErrorMessage = "Level must be between 0 and 50.")]
        public int Level_Alchemy { get => level_Alchemy; set => Set(ref level_Alchemy, value, SetAlchemyLevel, OnValidInputSubmit, _isInitialized); }

        public void SetAlchemyLevel()
        {
            _logic.UpdateModifiersForAlchemyLevels(level_Alchemy, Modifier_AlchemyLevel_Intelligence);
            AllStats!.Intelligence.BaseStats.InvalidateCacheAndReplaceModifier(Modifier_AlchemyLevel_Intelligence);
        }

        public IEnumerable<FormattedString> AlchemyLevelBonusFormatted()
        {
            return new List<FormattedString>
            {
                new() { ColorCodeHex = ColorCodeHex.AQUA, Content = $"{Modifier_AlchemyLevel_Intelligence.Value:N0} {DisplayConstants.SYMBOL_INTELLIGENCE}"}
            };
        }

        #endregion

        #region Carpentry

        private int level_Carpentry;
        [Range(ProfileConstants.SKYBLOCK_CARPENTRY_MIN, ProfileConstants.SKYBLOCK_CARPENTRY_MAX, ErrorMessage = "Level must be between 0 and 50.")]
        public int Level_Carpentry { get => level_Carpentry; set => Set(ref level_Carpentry, value, SetCarpentryLevel, OnValidInputSubmit, _isInitialized); }

        public void SetCarpentryLevel()
        {
            _logic.UpdateModifiersForCarpentryLevels(level_Carpentry, Modifier_CarpentryLevel_Health);
            AllStats!.Health.BaseStats.InvalidateCacheAndReplaceModifier(Modifier_CarpentryLevel_Health);
        }

        public IEnumerable<FormattedString> CarpentryLevelBonusFormatted()
        {
            return new List<FormattedString>
            {
                new() { ColorCodeHex = ColorCodeHex.RED, Content = $"{Modifier_CarpentryLevel_Health.Value:N0} {DisplayConstants.SYMBOL_HEALTH}"}
            };
        }

        #endregion

        #region Taming

        private int level_Taming;
        [Range(ProfileConstants.SKYBLOCK_TAMING_MIN, ProfileConstants.SKYBLOCK_TAMING_MAX, ErrorMessage = "Level must be between 0 and 50.")]
        public int Level_Taming { get => level_Taming; set => Set(ref level_Taming, value, SetTamingLevel, OnValidInputSubmit, _isInitialized); }

        public void SetTamingLevel()
        {
            _logic.UpdateModifiersForTamingLevels(level_Taming, Modifier_TamingLevel_PetLuck);
            //TODO: add backref to modifierlist in modiifer and make this a method
            AllStats!.PetLuck.BaseStats.InvalidateCacheAndReplaceModifier(Modifier_TamingLevel_PetLuck);
        }

        public IEnumerable<FormattedString> TamingLevelBonusFormatted()
        {
            return new List<FormattedString>
            {
                new() { ColorCodeHex = ColorCodeHex.LIGHT_PURPLE, Content = $"{Modifier_TamingLevel_PetLuck.Value:N0} {DisplayConstants.SYMBOL_PETLUCK}"}
            };
        }

        #endregion

        #region Dungeoneering

        private int level_Dungeoneering;
        [Range(ProfileConstants.SKYBLOCK_DUNGEONEERING_MIN, ProfileConstants.SKYBLOCK_DUNGEONEERING_MAX, ErrorMessage = "Level must be between 0 and 50.")]
        public int Level_Dungeoneering { get => level_Dungeoneering; set => Set(ref level_Dungeoneering, value, SetDungeoneeringLevel, OnValidInputSubmit, _isInitialized); }

        public void SetDungeoneeringLevel()
        {
            _logic.UpdateModifiersForDungeoneeringLevels(level_Dungeoneering, Modifier_DungeoneeringLevel_Health, AllStats!.DungeoneeringDungeonizedMultiplier);
            AllStats!.Health.BaseStats.InvalidateCacheAndReplaceModifier(Modifier_DungeoneeringLevel_Health);
        }

        public IEnumerable<FormattedString> DungeoneeringLevelBonusFormatted()
        {
            return new List<FormattedString>
            {
                new() { ColorCodeHex = ColorCodeHex.RED, Content = $"{Modifier_DungeoneeringLevel_Health.Value:N0} {DisplayConstants.SYMBOL_HEALTH}"},
                new() { Content = " + "},
                new() { ColorCodeHex = ColorCodeHex.GOLD, Content = $"{AllStats!.DungeoneeringDungeonizedMultiplier.Value:N0} {DisplayConstants.SYMBOL_STAR}"},
            };
        }

        #endregion

        #endregion

        private void OnValidInputSubmit()
        {
            bool? res = Context?.Validate();
            if (res.HasValue && res.Value)
            {
                OnChange?.Invoke();
            }
        }

    }
}
