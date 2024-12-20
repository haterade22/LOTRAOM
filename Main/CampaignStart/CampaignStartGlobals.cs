using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
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
            ["battania"] = "town_B1",
            ["vlandia"] = "town_V1",
            ["gondor"] = "town_EW1",
            ["mordor"] = "town_ES1",
            ["rivendell"] = "town_EW1",
            ["mirkwood"] = "town_EW1",
            ["lothlorien"] = "town_EW1",
            ["erebor"] = "town_EW1",
            ["umbar"] = "town_ES1",
        };
        public static void OnCharacterCreationFinalized()
        {
            string settlementId = "town_EN1";
            string playersCulture = Hero.MainHero.Culture.StringId;
            if (StartingSettlement.ContainsKey(playersCulture))
                settlementId = StartingSettlement[playersCulture];
            
            Hero.MainHero.PartyBelongedTo.Position2D = Settlement.Find(settlementId).GatePosition;
            if (GameStateManager.Current.ActiveState is MapState mapState)
            {
                mapState.Handler.ResetCamera(true, true);
                mapState.Handler.TeleportCameraToMainParty();
            }
        }
    }
}
