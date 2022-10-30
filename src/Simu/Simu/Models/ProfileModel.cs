using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Simu.Common;
using Simu.Logic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Simu.Models
{
    public class ProfileModel
    {
        private const string MODIFIER_SKYBLOCK_LEVEL = "SkyblockLevel";
        private const string MODIFIER_FARMING_LEVEL = "FarmingLevel";

        #region Modifiers

        public Modifier Modifier_SkyblockLevel_Health { get; set; }
        public Modifier Modifier_SkyblockLevel_Strength { get; set; }
        public Modifier Modifier_FarmingLevel_Health { get; set; }

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
        }


        #region SkyblockLevel

        private int skyblockLevel;

        [Range(Common.Systems.Profile.Constants.SKYBLOCK_LEVEL_MIN, Common.Systems.Profile.Constants.SKYBLOCK_LEVEL_MAX, ErrorMessage = "Level must be between 1 and 500.")]
        public int SkyblockLevel { get => skyblockLevel; set => SetSkyblockLevel(value); }

        public void SetSkyblockLevel(int value)
        {
            if (SkyblockLevel != value && _isInitialized)
            {
                skyblockLevel = value;

                _logic.UpdateModifiersForSkyblockLevels(skyblockLevel, Modifier_SkyblockLevel_Health, Modifier_SkyblockLevel_Strength);
                AllStats.Health.BaseStats.InvalidateCacheAndReplaceModifier(Modifier_SkyblockLevel_Health);
                AllStats.Strength.BaseStats.InvalidateCacheAndReplaceModifier(Modifier_SkyblockLevel_Strength);

                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Skills


        #region Farming


        private int level_Farming;

        [Range(Common.Systems.Profile.Constants.SKYBLOCK_FARMING_MIN, Common.Systems.Profile.Constants.SKYBLOCK_FARMING_MAX, ErrorMessage = "Level must be between 0 and 60.")]
        public int Level_Farming { get => level_Farming; set => SetFarmingLevel(value); }

        public void SetFarmingLevel(int value)
        {
            if (SkyblockLevel != value && _isInitialized)
            {
                level_Farming = value;

                _logic.UpdateModifiersForFarmingLevels(level_Farming, Modifier_FarmingLevel_Health);
                AllStats.Health.BaseStats.InvalidateCacheAndReplaceModifier(Modifier_FarmingLevel_Health);
                
                NotifyPropertyChanged();
            }
        }

        #endregion
        public int Level_Mining { get; set; }
        public int Level_Combat { get; set; }
        public int Level_Foraging { get; set; }
        public int Level_Fishing { get; set; }
        public int Level_Enchanting { get; set; }
        public int Level_Alchemy { get; set; }
        public int Level_Carpentry { get; set; }
        public int Level_Taming { get; set; }
        public int Level_Dungeoneering { get; set; }

        #endregion

        private void NotifyPropertyChanged()
        {
            bool? res = Context?.Validate();
            if (res.HasValue && res.Value)
            {
                OnChange?.Invoke();
            }
        }

    }
}
