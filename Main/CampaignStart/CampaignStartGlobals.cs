using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace LOTRAOM.CampaignStart
{
    static class CampaignStartGlobals
    {
        public static Dictionary<string, string> StartingSettlement = new()
        {
            // if the mod keeps has multiple factions with same culture (like empire in vanilla) this code will have to be refactored
             ["aserai"] = "town_A1",
             ["empire"] = "town_EN1",
             ["khuzait"] = "town_K1",
             ["sturgia"] = "town_S1",
             ["battania"] = "town_EW1",
             ["vlandia"] = "town_V1",
             ["gondor"] = "town_EW1",
             ["mordor"] = "town_ES1",
             ["rivendell"] = "town_SWAN_RIVENDELL1",
             ["mirkwood"] = "town_SWAN_MIRKWOOD1",
             ["lothlorien"] = "town_SWAN_LOTHLORIEN1",
             ["erebor"] = "town_SWAN_EREBOR1",
             ["umbar"] = "town_SWAN_UMBAR1",
             ["dolguldur"] = "town_SWAN_DOL_GOLDUR1",
             ["isengard"] = "town_SWAN_ISENGARD1",
             ["gundabad"] = "town_SWAN_GUNDABAD1",
        };
        internal class CCCultureData
        {
            public CCCultureData(List<string> possibleRaces, string bodyPropertiesString)
            {
                PossibleRaces = possibleRaces;
                BodyPropertiesString = bodyPropertiesString;
            }

            public List<string> PossibleRaces { get; }
            public string BodyPropertiesString { get; }
        }
        public static Dictionary<string, CCCultureData> CCCulturesRaceData = new()
        {
            ["aserai"] = new CCCultureData(new List<string>() { "human"}, HumanBodyPropString),
            ["battania"] = new CCCultureData(new List<string>() { "human"}, HumanBodyPropString),
            ["empire"] = new CCCultureData(new List<string>() { "human"}, DunlandBodyPropString),
            ["khuzait"] = new CCCultureData(new List<string>() { "human" }, HumanBodyPropString),
            ["sturgia"] = new CCCultureData(new List<string>() { "human" }, HumanBodyPropString),
            ["vlandia"] = new CCCultureData(new List<string>() { "human" }, RohanBodyPropString),
            ["gondor"] = new CCCultureData(new List<string>() { "human"}, HumanBodyPropString),
            ["mordor"] = new CCCultureData(new List<string>() { "uruk", "orc", "human" }, HumanBodyPropString),
            ["erebor"] = new CCCultureData(new List<string>() { "dwarf" }, DwarfBodyPropString),
            ["rivendell"] = new CCCultureData(new List<string>() { "human" }, HumanBodyPropString),
            ["mirkwood"] = new CCCultureData(new List<string>() { "human" }, HumanBodyPropString),
            ["lothlorien"] = new CCCultureData(new List<string>() { "human" }, HumanBodyPropString),
            ["umbar"] = new CCCultureData(new List<string>() { "human"}, HumanBodyPropString),
            ["isengard"] = new CCCultureData(new List<string>() { "uruk_hai", "berserker", "cave_troll", "hill_troll" }, HumanBodyPropString),
            ["gundabad"] = new CCCultureData(new List<string>() { "uruk" }, HumanBodyPropString),
            ["dolguldur"] = new CCCultureData(new List<string>() { "uruk" }, HumanBodyPropString),
            ["default"] = new CCCultureData(new List<string>() { "human", "uruk", "orc", "dwarf" }, HumanBodyPropString) // in case culture is missing
        };
        private const string HumanBodyPropString = "<BodyProperties version=\"4\" age=\"22.35\" weight=\"0.5417\" build=\"0.5231\"  key=\"000DF00FC00033CD8771188F38770F8801F188778888888888888888546AF0F90088860308888888000000000000000000000000000000000000000043044144\"  />";

        private const string DunlandBodyPropString = "<BodyProperties version=\"4\" age=\"22.01\" weight=\"0.5139\" build=\"0.6435\"  key=\"0008F006CAFC1A54878F1FFF4F9F1EEE7F018877888888888888888F7718F5F5008A8603088888880000000000000000000000000000000000000000431C4142\"  />";

        private const string RohanBodyPropString = "<BodyProperties version=\"4\" age=\"22\" weight=\"0.5139\" build=\"0.6435\" key=\"0005280140001242947E068A709500460C7250703EB70F135C85021887733A070089B6030822BA9000000000000000000000000000000000000000003F1C7002\"  />";

        private const string DwarfBodyPropString = "<BodyProperties version=\"4\" age=\"22.01\" weight=\"0.5648\" build=\"0.5347\"  key=\"0000040F0000300008801FFEFF8F0FFE109588878FF88067888878888D3010F30088860308FFF0AE000000000000000000000000000000000000000043040142\"  />";

    }
}
