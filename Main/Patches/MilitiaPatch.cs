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
        public static bool Prefix(Settlement __instance, MobileParty militaParty, int militiaToAdd)
        {
            // Use our custom model's implementation
            if (Campaign.Current.Models.SettlementMilitiaModel is LOTRAOMSettlementMilitiaModel customModel)
            {
                customModel.AssignMilitiaTroops(__instance, militaParty, militiaToAdd);
                return false; // Skip the original method
            }
            return true; // Use original method if our model isn't active
        }
    }
}