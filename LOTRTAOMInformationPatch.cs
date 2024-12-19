using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Settlements;

namespace LOTRLOME.Patches
{
    [HarmonyPatch(typeof(DefaultInformationRestrictionModel))]
    public class LOTRLOME_DefaultInformationRestrictionModelPatch
    {
        // Prefix Patch for DoesPlayerKnowDetailsOf Settlement
        [HarmonyPrefix]
        [HarmonyPatch(nameof(DefaultInformationRestrictionModel.DoesPlayerKnowDetailsOf), new[] { typeof(Settlement) })]
        private static bool DoesPlayerKnowDetailsOfSettlementPrefix(ref bool __result)
        {
            __result = true;
            return false; // Skip original method
        }

        // Prefix Patch for DoesPlayerKnowDetailsOf Hero
        [HarmonyPrefix]
        [HarmonyPatch(nameof(DefaultInformationRestrictionModel.DoesPlayerKnowDetailsOf), new[] { typeof(Hero) })]
        private static bool DoesPlayerKnowDetailsOfHeroPrefix(ref bool __result)
        {
            __result = true;
            return false; // Skip original method
        }
    }
}

namespace LOTRLOME
{
    public class LOTRLOMEPatches
    {
        public static void ApplyPatches()
        {
            var harmony = new Harmony("com.lotrmod.lotr_lome");
            harmony.PatchAll();
        }
    }
}
