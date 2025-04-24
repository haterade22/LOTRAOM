using HarmonyLib;
using SandBox.BoardGames.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.GauntletUI.BodyGenerator;
using static TaleWorlds.CampaignSystem.CampaignBehaviors.LordConversationsCampaignBehavior;
using static TaleWorlds.MountAndBlade.Agent;

namespace LOTRAOM.Patches
{
    [HarmonyPatch]
    public static class CampaignTimeToStringPatch
    {
        [HarmonyPatch(typeof(CampaignTime), "ToString")]
        static void Postfix(ref string __result, ref CampaignTime __instance)
        {
            int getYear = __instance.GetYear;
            string season = "";
            switch (__instance.GetSeasonOfYear)
            {
                case CampaignTime.Seasons.Spring:
                    season = "Tuilë";
                    break;
                case CampaignTime.Seasons.Summer:
                    season = "Lairë";
                    break;
                case CampaignTime.Seasons.Autumn:
                    season = "Yávië";
                    break;
                case CampaignTime.Seasons.Winter:
                    season = "Hrívë";
                    break;
            }
            int num = __instance.GetDayOfSeason + 1;
            TextObject textObject = GameTexts.FindText("str_date_format", null);
            textObject.SetTextVariable("SEASON", "T.A. " + season);
            textObject.SetTextVariable("YEAR", getYear.ToString());
            textObject.SetTextVariable("DAY", num.ToString());
            __result = textObject.ToString();
        }
    }

}
