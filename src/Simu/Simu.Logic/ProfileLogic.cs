using Simu.Common;
using Simu.Common.Systems.Profile;

namespace Simu.Logic
{
    public class ProfileLogic
    {
        public void UpdateModifiersForSkyblockLevels(int level, Modifier health, Modifier strength)
        {
            if (level < Constants.SKYBLOCK_LEVEL_MIN || level > Constants.SKYBLOCK_LEVEL_MAX) return;

            health.Value = level * 5 + (level / 10) * 10;
            strength.Value = (level / 5) * 1;
        }
        public void UpdateModifiersForFarmingLevels(int level, Modifier health)
        {
            if (level < Constants.SKYBLOCK_FARMING_MIN || level > Constants.SKYBLOCK_FARMING_MAX) return;

            health.Value = level switch
            {
                (< 15) => level * 2,
                (< 20) => 28 + (level - 14) * 3, // 28 health for levels 0-14
                (< 26) => 43 + (level - 19) * 4, // 43 health for levels 0-19
                _      => 67 + (level - 25) * 5  // 67 health for levels 0-25
            };
        }
    }
}