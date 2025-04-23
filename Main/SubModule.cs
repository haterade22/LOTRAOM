using HarmonyLib;
using LOTRAOM.CampaignStart;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using LOTRAOM.Models;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using LOTRAOM.Patches;
using LOTRAOM.Extensions;
using LOTRAOM.Momentum;
using LOTRAOM.CampaignBehaviors;

namespace LOTRAOM
{
    public class SubModule : MBSubModuleBase
    {
        private Harmony harmony = new Harmony("com.lotrmod.lotr_lome");
        private bool manualPatchesHaveFired;
        protected override void OnSubModuleLoad()
        {
            Harmony.DEBUG = true;
            base.OnSubModuleLoad();
            harmony.PatchAll();

            CampaignTime startTime = CampaignTime.Years(3017) + CampaignTime.Hours(12);//CampaignTime.Weeks(4) + CampaignTime.Days(5) + CampaignTime.Hours(12);
            typeof(CampaignData).GetField("CampaignStartTime", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)?.SetValue(null, startTime);

            RemoveSandboxAndStoryOptions();
            Module.CurrentModule.AddInitialStateOption(
                new InitialStateOption("LOTRAOM", name: new TextObject("{=lotraom_start_game}Enter The Age of Man", null), 3,
                () => MBGameManager.StartNewGame(new LotrAOMCampaignManager()),
                () => (Module.CurrentModule.IsOnlyCoreContentEnabled, new("Disabled during installation.", null)))
            );
        }
        private static void RemoveSandboxAndStoryOptions()
        {
            List<InitialStateOption> initialOptionsList = Module.CurrentModule.GetInitialStateOptions().ToList();
            initialOptionsList.RemoveAll(x => x.Id == "SandBoxNewGame" || x.Id == "StoryModeNewGame");
            Module.CurrentModule.ClearStateOptions();
            foreach (InitialStateOption initialStateOption in initialOptionsList)
            {
                Module.CurrentModule.AddInitialStateOption(initialStateOption);
            }
        }
        public override void OnAfterGameInitializationFinished(Game game, object starterObject)
        {
            Globals.IsNewCampaignCreating = false;
        }
        public override void OnNewGameCreated(Game game, object initializerObject)
        {
            base.OnNewGameCreated(game, initializerObject);
            Globals.IsNewCampaignCreating = true;
        }
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            //test 
                //foreach (Kingdom kingdom in Kingdom.All)
                //    foreach (Kingdom kingdom2 in Kingdom.All)
                //    FactionManager.SetNeutral

           if (gameStarterObject is CampaignGameStarter campaignGameStarter)
           {
                campaignGameStarter.AddBehavior(new KeepHeroRaceCampaignBehavior());
                campaignGameStarter.AddBehavior(new AoMDiplomacy());
                campaignGameStarter.AddBehavior(new MomentumCampaignBehavior());
                
                // models
                campaignGameStarter.AddModel(new LOTRAOMNotableSpawnModel(campaignGameStarter.GetExistingModel<NotableSpawnModel>()));
                campaignGameStarter.AddModel(new LOTRAOMPartyWageModel(campaignGameStarter.GetExistingModel<PartyWageModel>()));
                campaignGameStarter.AddModel(new LOTRAOMPartySizeModel(campaignGameStarter.GetExistingModel<PartySizeLimitModel>()));
                campaignGameStarter.AddModel(new LOTRAOMVillageProductionCalculatorModel(campaignGameStarter.GetExistingModel<VillageProductionCalculatorModel>()));
                campaignGameStarter.AddModel(new LOTRAOMBuildingConstructionModel(campaignGameStarter.GetExistingModel<BuildingConstructionModel>()));
                campaignGameStarter.AddModel(new LOTRAOMArmyManagementCalculationModel(campaignGameStarter.GetExistingModel<ArmyManagementCalculationModel>()));
                campaignGameStarter.AddModel(new LOTRAOMSettlementMilitiaModel(campaignGameStarter.GetExistingModel<SettlementMilitiaModel>()));
                campaignGameStarter.AddModel(new AOMVolunteerModel(campaignGameStarter.GetExistingModel<VolunteerModel>()));
                campaignGameStarter.AddModel(new AOMCharacterStatsModel(campaignGameStarter.GetExistingModel<CharacterStatsModel>()));
                campaignGameStarter.AddModel(new AOMTroopUpgradeModel(campaignGameStarter.GetExistingModel<PartyTroopUpgradeModel>()));
                campaignGameStarter.AddModel(new AOMDiplomacyModel(campaignGameStarter.GetExistingModel<DiplomacyModel>()));
                campaignGameStarter.AddModel(new AoMSettlementFoodModel(campaignGameStarter.GetExistingModel<SettlementFoodModel>()));
                campaignGameStarter.AddModel(new AoMSettlementProsperityModel(campaignGameStarter.GetExistingModel<SettlementProsperityModel>()));
                campaignGameStarter.AddModel(new AOMKingdomDecisionPermissionModel());
                campaignGameStarter.AddModel(new LOTRAOMPartyFoodModel(campaignGameStarter.GetExistingModel<MobilePartyFoodConsumptionModel>()));
                campaignGameStarter.AddModel(new LOTRAOMSiegeEventModel(campaignGameStarter.GetExistingModel<SiegeEventModel>()));

                //we can edit this to make factions based on raiding (raiding gives more items)
                //campaignGameStarter.GetExistingModel<DefaultRaidModel>
            }
        }
        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();
            harmony?.UnpatchAll("com.lotrmod.lotr_lome");
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();
        }
        public override void OnGameLoaded(Game game, object initializerObject)
        {
            base.OnGameLoaded(game, initializerObject);
            LOTRAOMCharacterCreationContent.SetCultureFeats();
        }
        public override void OnGameInitializationFinished(Game game)
        {
            if (!manualPatchesHaveFired)
            {
                manualPatchesHaveFired = true;
                RunManualPatches();
            }
        }
        private void RunManualPatches()
        {
#pragma warning disable BHA0003 // Type was not found
            System.Reflection.MethodInfo originalTierMethod = AccessTools.Method("CampaignUIHelper:GetCharacterTierData");
            System.Reflection.MethodInfo originalDeserterMethod = AccessTools.Method("DesertionCampaignBehavior:PartiesCheckDesertionDueToPartySizeExceedsPaymentRatio");
#pragma warning restore BHA0003 // Type was not found
            harmony.Patch(originalTierMethod, prefix: new HarmonyMethod(typeof(GetCharacterTierDataPatch), nameof(GetCharacterTierDataPatch.Prefix)));
            harmony.Patch(originalDeserterMethod, prefix: new HarmonyMethod(typeof(PartiesDesertionPatch), nameof(PartiesDesertionPatch.Prefix)));
        }

    }
}