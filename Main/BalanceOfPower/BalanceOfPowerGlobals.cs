using LOTRAOM.BalanceOfPower.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;

namespace LOTRAOM.BalanceOfPower
{
    public static class BalanceOfPowerGlobals
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
        public static List<BalanceOfPowerBreakdown> MockBalanceOfPowerBreakdown()
        {
            List<BalanceOfPowerBreakdown> list = new()
            {
                new BalanceOfPowerBreakdown()
                {
                    Type = BalanceOfPowerActionType.Casualty,
                    ValueFaction1 = 0,
                    ValueFaction2 = 0,
                    BalanceOfPowerFraction1 = 0,
                    BalanceOfPowerFraction2 = 0
                },
                new BalanceOfPowerBreakdown()
                {
                    Type = BalanceOfPowerActionType.Occupied,
                    ValueFaction1 = 1,
                    ValueFaction2 = 1,
                    BalanceOfPowerFraction1 = 1,
                    BalanceOfPowerFraction2 = 1
                }
            };
            return list;
        }
        public static MBBindingList<BalanceOfPowerStatVM> MockTotalStats()
        {
            return new MBBindingList<BalanceOfPowerStatVM>()
            {
                new("Total Deaths", "0", "0"),
                new("Total Successful Sieges", "0", "0"),
                new("Total Lords Defeated", "0", "0"),
                new("Total Elite Units Killed", "1", "1")
            };
        }
    }
}
