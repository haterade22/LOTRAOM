using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Settlements;

namespace LOTRAOM.Patches
{
    [HarmonyPatch(typeof(DefaultInformationRestrictionModel))]
    public class LOTRLOME_DefaultInformationRestrictionModelPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(DefaultInformationRestrictionModel.DoesPlayerKnowDetailsOf), new[] { typeof(Settlement) })]
        private static bool DoesPlayerKnowDetailsOfSettlementPrefix(ref bool __result)
        {
            __result = true;
            return false; // Skip original method
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(DefaultInformationRestrictionModel.DoesPlayerKnowDetailsOf), new[] { typeof(Hero) })]
        private static bool DoesPlayerKnowDetailsOfHeroPrefix(ref bool __result)
        {
            __result = true;
            return false; // Skip original method
        }
    }
}