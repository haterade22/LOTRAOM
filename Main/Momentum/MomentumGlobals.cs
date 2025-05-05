using LOTRAOM.Momentum.ViewModel;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;

namespace LOTRAOM.Momentum
{
    public static class MomentumGlobals
    {
        public static Kingdom Mordor
        {
            get
            {
                return Kingdom.All.Where(x => x.Culture.StringId == "mordor").FirstOrDefault()!;
            }
        }
        public static Kingdom Gondor
        {
            get
            {
                return Kingdom.All.Where(x => x.Culture.StringId == "gondor").FirstOrDefault()!;
            }
        }
        // momentum is multiplied by 100, to allow for more precision from battles, it is divided by 100 when shown in UI
        public static int MomentumMultiplier { get { return 100; } }
        public static int MaxMomentumFromBattleWon { get { return 300 * MomentumMultiplier; } } // battle casualties divided by total faction strength
        public static int MomentumFromSiege { get { return 5 * MomentumMultiplier; } }
        public static int MomentumFromArmyGathering { get { return 2 * MomentumMultiplier; } }
        public static int MomentumFromVillageRaid { get { return 2 * MomentumMultiplier; } }
        public static int MomentumMultiplierFromTotalStrength { get { return 10 * MomentumMultiplier; } }
        public static int MaxMomentumFromTotalStrength { get { return 40 * MomentumMultiplier; } } // caps when one faction is 4 times stronger than the other

        public static float ScaleMomentum(float value)
        {
            return value / 100;
        }
    }
}
