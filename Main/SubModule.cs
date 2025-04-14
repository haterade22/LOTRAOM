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
using LOTRAOM.BalanceOfPower;

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

            if (gameStarterObject is CampaignGameStarter campaignGameStarter)
            {
                campaignGameStarter.AddBehavior(new KeepHeroRaceCampaignBehavior());
                campaignGameStarter.AddBehavior(new BalanceOfPowerCampaignBehavior());

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