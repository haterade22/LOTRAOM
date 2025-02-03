using HarmonyLib;
using LOTRAOM.CampaignStart;
using LOTRAOM;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.Selector;
namespace LOTRAOM
{
    public class SubModule : MBSubModuleBase
    {
        private Harmony? harmony;

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            harmony = new Harmony("com.lotrmod.lotr_lome");
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
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            if (gameStarterObject is CampaignGameStarter campaignGameStarter)
            {
                    campaignGameStarter.AddBehavior(new KeepHeroRaceCampaignBehavior());
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
    }
}
