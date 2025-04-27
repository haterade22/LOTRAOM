using Helpers;
using SandBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Source.Missions;
using SandBox.Missions.MissionLogics;
using TaleWorlds.MountAndBlade.Source.Missions.Handlers.Logic;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Missions.Handlers;
using TaleWorlds.CampaignSystem.TroopSuppliers;

namespace LOTRAOM.DoubleSiege
{
    public static class CustomSiegeMission
    {
        [MissionMethod]
        public static Mission OpenSiegeMissionWithDeployment(string scene, float[] wallHitPointPercentages, bool hasAnySiegeTower, List<MissionSiegeWeapon> siegeWeaponsOfAttackers, List<MissionSiegeWeapon> siegeWeaponsOfDefenders)
        {
            string text = Campaign.Current.Models.LocationModel.GetUpgradeLevelTag(1);
            text += " siege";
            bool isPlayerSergeant = MobileParty.MainParty.MapEvent.IsPlayerSergeant();
            bool isPlayerInArmy = MobileParty.MainParty.Army != null;
            List<string> heroesOnPlayerSideByPriority = HeroHelper.OrderHeroesOnPlayerSideByPriority();
            return MissionState.OpenNew("SiegeMissionWithDeployment", SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, text, false, DecalAtlasGroup.Town), delegate (Mission mission)
            {
                List<MissionBehavior> list = new List<MissionBehavior>
                {
                    new BattleSpawnLogic("battle_set"),
                    new MissionOptionsComponent(),
                    new CampaignMissionComponent(),
                    new BattleReinforcementsSpawnController(),
                    new MissionCombatantsLogic(MobileParty.MainParty.MapEvent.InvolvedParties, PartyBase.MainParty, MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Defender), MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Attacker), Mission.MissionTeamAITypeEnum.Siege, isPlayerSergeant),
                    new SiegeMissionPreparationHandler(false, false, wallHitPointPercentages, hasAnySiegeTower),
                    new CampaignSiegeStateHandler(),
                    new BattlePowerCalculationLogic(),
                    new BattleObserverMissionLogic(),
                    new BattleAgentLogic(),
                    new BattleSurgeonLogic(),
                    new MountAgentLogic(),
                    new BannerBearerLogic(),
                    new AgentHumanAILogic(),
                    new AmmoSupplyLogic(new List<BattleSideEnum> { BattleSideEnum.Defender }),
                    new AgentVictoryLogic(),
                    new AssignPlayerRoleInTeamMissionController(!isPlayerSergeant, isPlayerSergeant, isPlayerInArmy, heroesOnPlayerSideByPriority, FormationClass.NumberOfRegularFormations),
                    new MissionAgentPanicHandler(),
                    new MissionBoundaryPlacer(),
                    new MissionBoundaryCrossingHandler(),
                    new AgentMoraleInteractionLogic(),
                    new HighlightsController(),
                    new BattleHighlightsController(),
                    new EquipmentControllerLeaveLogic(),
                    new MissionSiegeEnginesLogic(siegeWeaponsOfDefenders, siegeWeaponsOfAttackers),
                    new SiegeDeploymentHandler(true),
                    new SiegeDeploymentMissionController(true),
                    new BattleEndLogic(),
                    new SandBoxSiegeMissionSpawnHandler(),
                    new MissionAgentSpawnLogic(new IMissionTroopSupplier[]
                    {
                        new PartyGroupTroopSupplier(MapEvent.PlayerMapEvent, BattleSideEnum.Defender, null, null),
                        new PartyGroupTroopSupplier(MapEvent.PlayerMapEvent, BattleSideEnum.Attacker, null, null)
                    }, PartyBase.MainParty.Side, Mission.BattleSizeType.Siege),
                    new CustomSiegeMissionLogic()
                };
                List<MissionBehavior> list2 = list;
                Hero leaderHero = MapEvent.PlayerMapEvent.AttackerSide.LeaderParty.LeaderHero;
                TextObject attackerGeneralName = (leaderHero != null) ? leaderHero.Name : null;
                Hero leaderHero2 = MapEvent.PlayerMapEvent.DefenderSide.LeaderParty.LeaderHero;
                list2.Add(new SandboxGeneralsAndCaptainsAssignmentLogic(attackerGeneralName, (leaderHero2 != null) ? leaderHero2.Name : null, null, null, false));
                return list.ToArray();
            }, true, true);
        }
    }
}
