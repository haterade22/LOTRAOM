using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;

namespace LOTRAOM.Extensions
{
    public static class KingdomExtensions
    {
        public static List<Kingdom> AllActiveKingdoms
        {
            get
            {
                return Kingdom.All.Where(k => !k.IsEliminated).ToList<Kingdom>();
            }
        }
        public static bool IsAlliedWith(this IFaction faction1, IFaction faction2)
        {
            if (faction1 == faction2)
            {
                return false;
            }
            var stanceLink = faction1.GetStanceWith(faction2);
            return stanceLink.IsAllied;
        }

    }
}