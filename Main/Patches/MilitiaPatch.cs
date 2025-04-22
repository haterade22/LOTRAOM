using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using LOTRAOM.Models;

namespace LOTRAOM.Patches
{
    [HarmonyPatch(typeof(Settlement), "AddMilitiasToParty")]
    public class MilitiaPatch
    {
        public static bool Prefix(Settlement __instance, MobileParty militiaParty, int militiaToAdd)
        {
            if (Campaign.Current.Models.SettlementMilitiaModel is LOTRAOMSettlementMilitiaModel model)
            {
                model.AssignMilitiaTroops(__instance, militiaParty, militiaToAdd);
                return false; // Skip vanilla method
            }
            return true; // Fallback to vanilla
        }
    }
}