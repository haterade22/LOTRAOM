using HarmonyLib;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;

namespace LOTRAOM.DoubleSiege
{

    [HarmonyPatch]
    public class PlayerEncounterUpdateInternalPatch
    {
        static MethodBase TargetMethod()
        {
            return typeof(PlayerEncounter).GetMethod("UpdateInternal", BindingFlags.Instance | BindingFlags.NonPublic)!;
        }
        private static readonly PropertyInfo _diedInBattleProperty =
            typeof(MapEventParty).GetProperty("DiedInBattle", BindingFlags.Instance | BindingFlags.NonPublic)!;

        private static readonly PropertyInfo _woundedInBattleProperty =
            typeof(MapEventParty).GetProperty("WoundedInBattle", BindingFlags.Instance | BindingFlags.NonPublic)!;

        public static TroopRoster GetDiedInBattle(MapEventParty mapEventParty)
        {
            return (TroopRoster)_diedInBattleProperty.GetValue(mapEventParty)!;
        }

        public static TroopRoster GetWoundedInBattle(MapEventParty mapEventParty)
        {
            return (TroopRoster)_woundedInBattleProperty.GetValue(mapEventParty)!;
        }
        // the patch has to happen in PlayerEncounterState.Wait, because otherwise the screen goes straight to prisoners / troops rescued
        // before the PlayerEncounter.Update is called again
        static bool Prefix(PlayerEncounter __instance)
        {
            if (PlayerSiege.PlayerSiegeEvent == null
            || MobileParty.MainParty.MapEvent == null
            || !MobileParty.MainParty.MapEvent.IsSiegeAssault
            || MapEvent.PlayerMapEvent == null)
                return true;

            MapEventParty? playerParty = MapEvent.PlayerMapEvent.AttackerSide.Parties.FirstOrDefault(p => p.Party == PartyBase.MainParty);
            playerParty ??= MapEvent.PlayerMapEvent.DefenderSide.Parties.FirstOrDefault(p => p.Party == PartyBase.MainParty);
            if (playerParty == null || GetDiedInBattle(playerParty).Contains(CharacterObject.PlayerCharacter)) return true;
            int defendersLeft = MobileParty.MainParty.MapEvent.DefenderSide.RecalculateMemberCountOfSide();
            int attackersLeft = MobileParty.MainParty.MapEvent.AttackerSide.RecalculateMemberCountOfSide();

            // player as attacker
            if (DoubleSiegeCampaignBehavior.ContainsSecondSiege(MobileParty.MainParty.MapEvent.MapEventSettlement.StringId)
                && MobileParty.MainParty.MapEvent.HasWinner
                && (__instance.EncounterState == PlayerEncounterState.Wait)
                && !DoubleSiegeCampaignBehavior.Instance.HasFinishedTheSecondSiege
                && PlayerSiege.PlayerSide == BattleSideEnum.Attacker)
            {
                if (GetWoundedInBattle(playerParty).Contains(CharacterObject.PlayerCharacter))
                {
                    GetWoundedInBattle(playerParty).RemoveTroop(CharacterObject.PlayerCharacter);
                    Hero.MainHero.Heal(21);
                }
                GameMenu.ActivateGameMenu("double_siege_menu");
                return false;
            }
            // player as defender
            if (DoubleSiegeCampaignBehavior.ContainsSecondSiege(MobileParty.MainParty.MapEvent.MapEventSettlement.StringId)
                && PlayerSiege.PlayerSide == BattleSideEnum.Defender
                //&& MobileParty.MainParty.MapEvent.HasWinner // for some reason, when player is defending, this is set later
                && (__instance.EncounterState == PlayerEncounterState.Wait)
                && !DoubleSiegeCampaignBehavior.Instance.HasFinishedTheSecondSiege
                && defendersLeft == 0)
            {
                if (GetWoundedInBattle(playerParty).Contains(CharacterObject.PlayerCharacter))
                {
                    GetWoundedInBattle(playerParty).RemoveTroop(CharacterObject.PlayerCharacter);
                    Hero.MainHero.Heal(21);
                }
                GameMenu.ActivateGameMenu("double_siege_menu");
                return false;
            }

            return true;
        }
    }
}