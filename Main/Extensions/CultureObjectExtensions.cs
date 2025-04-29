using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace LOTRAOM.Extensions
{
    public static class CultureObjectExtensions
    {
        public static bool IsGoodCulture(this CultureObject culture)
        {
            return Globals.HumanGoodCulture.Contains(culture.StringId) || Globals.ElvenCulture.Contains(culture.StringId) || Globals.DwarvenCulture.Contains(culture.StringId);
        }
        public static bool IsEvilCulture(this CultureObject culture)
        {
            return Globals.HumanEvilCulture.Contains(culture.StringId) || Globals.OrcishCulture.Contains(culture.StringId);
        }
    }
}
