using HarmonyLib;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace LOTRAOM.Patches
{
    //[HarmonyPatch(typeof(CampaignUIHelper), "GetCharacterTierData")]
    public class GetCharacterTierDataPatch
    {
        public static bool Prefix(ref StringItemWithHintVM __result, CharacterObject character, bool isBig = false)
        {
            int tier = character.Tier;
            if (tier <= 0 || tier > 10)
            {
                __result = new StringItemWithHintVM("", TextObject.Empty);
            }
            string text = "aom_icon_tier_" + tier;
            GameTexts.SetVariable("TIER_LEVEL", tier);
            TextObject hint = new TextObject("{=!}" + GameTexts.FindText("str_party_troop_tier", null).ToString(), null);
            __result = new StringItemWithHintVM(text, hint);
            return false;
        }
    }
}
