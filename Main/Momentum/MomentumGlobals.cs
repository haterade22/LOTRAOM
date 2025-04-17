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
        public static List<MomentumBreakdown> MockBalanceOfPowerBreakdown()
        {
            List<MomentumBreakdown> list = new()
            {
                new MomentumBreakdown()
                {
                    Type = MomentumActionType.Casualty,
                    ValueFaction1 = 0,
                    ValueFaction2 = 0,
                    BalanceOfPowerFraction1 = 0,
                    BalanceOfPowerFraction2 = 0
                },
                new MomentumBreakdown()
                {
                    Type = MomentumActionType.Occupied,
                    ValueFaction1 = 1,
                    ValueFaction2 = 1,
                    BalanceOfPowerFraction1 = 1,
                    BalanceOfPowerFraction2 = 1
                }
            };
            return list;
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
    }
}
