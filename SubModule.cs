using HarmonyLib;
using LOTRLOME.Patches;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace LOTRLOME
{
    public class SubModule : MBSubModuleBase
    {
        private Harmony _harmony;

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            // Initialize and apply Harmony patches
            _harmony = new Harmony("com.lotrmod.lotr_lome");
            LOTRLOMEPatches.ApplyPatches();
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            if (game.GameType is Campaign campaign)
            {
                // Add behaviors for the campaign
                var campaignGameStarter = gameStarterObject as CampaignGameStarter;
                if (campaignGameStarter != null)
                {
                    // Add your campaign-specific behaviors here if needed
                }
            }
        }

        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();

            // Unpatch Harmony patches to clean up
            _harmony?.UnpatchAll("com.lotrmod.lotr_lome");
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            // Any additional initialization can be done here
        }
    }
}
