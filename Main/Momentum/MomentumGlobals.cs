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
        public static MBBindingList<MomentumStatVM> MockTotalStats()
        {
            return new MBBindingList<MomentumStatVM>()
            {
                new("Total Deaths", "0", "0"),
                new("Total Successful Sieges", "0", "0"),
                new("Total Lords Defeated", "0", "0"),
                new("Total Elite Units Killed", "1", "1")
            };
        }
        public static int MaxMomentumFromBattleWon { get { return 300; } } // battle casualties divided by total faction strength
        public static int MomentumFromSiege { get { return 5; } }
        public static int MomentumFromArmyGathering { get { return 2; } }
    }
}
