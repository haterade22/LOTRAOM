using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using static LOTRAOM.CampaignStart.CampaignStartGlobals;

namespace LOTRAOM
{
    public static class Globals
    {
        public static float MountedTroopWageMultiplier => 0.3f;
        public static bool IsNewCampaignCreating = false;
        public static string DunlandCulture { get { return "empire"; } }
        public static string HaradCulture { get { return "aserai"; } }
        public static string RhunCulture { get { return "khuzait"; } }
        public static string RohanCulture { get { return "vlandia"; } }
        public static string DaleCulture { get { return "sturgia"; } }
        public static string KhandCulture { get { return "battania"; } }

        public static string Gondorculture { get { return "gondor"; } }
        public static string RivendellCulture { get { return "rivendell"; } }
        public static string MirkwoodCulture { get { return "mirkwood"; } }
        public static string LothlorienCulture { get { return "lothlorien"; } }
        public static string EreborCulture { get { return "erebor"; } }
        public static string MordorCulture { get { return "mordor"; } }
        public static string GundabadCulture { get { return "gundabad"; } }
        public static string DolguldurCulture { get { return "dolguldur"; } }
        public static string IsengardCulture { get { return "isengard"; } }
        public static string UmbarCulture { get { return "umbar"; } }

        // culture groups
        public static readonly List<string> HumanGoodCulture = new() { Gondorculture, RohanCulture, DaleCulture };
        public static readonly List<string> ElvenCulture = new() { RivendellCulture, MirkwoodCulture, LothlorienCulture };
        public static readonly List<string> DwarvenCulture = new() { EreborCulture };
        public static readonly List<string> OrcishCulture = new() { MordorCulture, GundabadCulture, DolguldurCulture, IsengardCulture };
        public static readonly List<string> HumanEvilCulture = new() { HaradCulture, RhunCulture, UmbarCulture, KhandCulture, DunlandCulture };
        public static readonly List<List<string>> CultureGroups = new() { HumanGoodCulture, ElvenCulture, DwarvenCulture, OrcishCulture, HumanEvilCulture };
        public static bool BelongsToSameCultureGroup(CultureObject culture1, CultureObject culture2)
        {
            foreach (List<string> cultureGroup in CultureGroups)
                if (cultureGroup.Contains(culture1.StringId) && cultureGroup.Contains(culture2.StringId))
                    return true;
            return false;
        }

    }
}