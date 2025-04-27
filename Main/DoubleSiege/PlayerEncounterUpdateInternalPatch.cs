using HarmonyLib;
using System.Reflection;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;

namespace LOTRAOM.DoubleSiege
{

    [HarmonyPatch]
    public class PlayerEncounterUpdateInternalPatch
    {
        static MethodBase TargetMethod()
        {
            return typeof(PlayerEncounter).GetMethod("UpdateInternal", BindingFlags.Instance | BindingFlags.NonPublic);
        }
        static bool Prefix(PlayerEncounter __instance)
        {
            if (PlayerSiege.PlayerSiegeEvent != null 
                && PlayerSiege.PlayerSide == BattleSideEnum.Attacker 
                && MobileParty.MainParty.MapEvent != null 
                && MobileParty.MainParty.MapEvent.IsSiegeAssault 
                && DoubleSiegeCampaignBehavior.ContainsSecondSiege(MobileParty.MainParty.MapEvent.MapEventSettlement.StringId)
                && MobileParty.MainParty.MapEvent.HasWinner
                && (__instance.EncounterState == PlayerEncounterState.PlayerVictory || __instance.EncounterState == PlayerEncounterState.Wait)
                && !DoubleSiegeCampaignBehavior.Instance.HasFinishedTheSecondSiege)
            {
                GameMenu.ActivateGameMenu("double_siege_menu");
                return false;
            }
            return true;
        }
    }
}