using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.SaveSystem.Load;

namespace LOTRAOM.CampaignStart
{
    public class LotrAOMCampaignManager : MBGameManager
    {
        private readonly bool _loadingSavedGame;
        private LoadResult? _loadedGameResult;

        public LotrAOMCampaignManager()
        {
            _loadingSavedGame = false;
        }

        public LotrAOMCampaignManager(LoadResult loadedGameResult)
        {
            _loadingSavedGame = true;
            _loadedGameResult = loadedGameResult;
        }

        public override void OnGameEnd(Game game)
        {
            MBDebug.SetErrorReportScene(null);
        }

        public override void OnGameInitializationFinished(Game game)
        {
            base.OnGameInitializationFinished(game);
        }

        protected override void DoLoadingForGameManager(GameManagerLoadingSteps gameManagerLoadingStep, out GameManagerLoadingSteps nextStep)
        {
            nextStep = GameManagerLoadingSteps.None;
            switch (gameManagerLoadingStep)
            {
                case GameManagerLoadingSteps.PreInitializeZerothStep:
                    nextStep = GameManagerLoadingSteps.FirstInitializeFirstStep;
                    return;
                case GameManagerLoadingSteps.FirstInitializeFirstStep:
                    LoadModuleData(_loadingSavedGame);
                    nextStep = GameManagerLoadingSteps.WaitSecondStep;
                    return;
                case GameManagerLoadingSteps.WaitSecondStep:
                    if (!_loadingSavedGame)
                        StartNewGame();
                    nextStep = GameManagerLoadingSteps.SecondInitializeThirdState;
                    return;
                case GameManagerLoadingSteps.SecondInitializeThirdState:
                    MBGlobals.InitializeReferences();
                    if (!_loadingSavedGame)
                    {
                        MBDebug.Print("Initializing new game begin...", 0, Debug.DebugColor.White, 17592186044416UL);
                        Campaign campaign = new(CampaignGameMode.Campaign);
                        Game.CreateGame(campaign, this);
                        campaign.SetLoadingParameters(Campaign.GameLoadingType.NewCampaign);
                        MBDebug.Print("Initializing new game end...", 0, Debug.DebugColor.White, 17592186044416UL);
                    }
                    else
                    {
                        MBDebug.Print("Initializing saved game begin...", 0, Debug.DebugColor.White, 17592186044416UL);
                        ((Campaign)Game.LoadSaveGame(_loadedGameResult, this).GameType).SetLoadingParameters(Campaign.GameLoadingType.SavedCampaign);
                        _loadedGameResult = null;
                        Common.MemoryCleanupGC(false);
                        MBDebug.Print("Initializing saved game end...", 0, Debug.DebugColor.White, 17592186044416UL);
                    }
                    Game.Current.DoLoading();
                    nextStep = GameManagerLoadingSteps.PostInitializeFourthState;
                    return;
                case GameManagerLoadingSteps.PostInitializeFourthState:
                    bool submodulesLoaded = true;
                    foreach (MBSubModuleBase mbsubModuleBase in TaleWorlds.MountAndBlade.Module.CurrentModule.SubModules)
                    {
                        submodulesLoaded = submodulesLoaded && mbsubModuleBase.DoLoading(Game.Current);
                    }
                    nextStep = submodulesLoaded ? GameManagerLoadingSteps.FinishLoadingFifthStep : GameManagerLoadingSteps.PostInitializeFourthState;
                    return;
                case GameManagerLoadingSteps.FinishLoadingFifthStep:
                    nextStep = Game.Current.DoLoading() ? GameManagerLoadingSteps.None : GameManagerLoadingSteps.FinishLoadingFifthStep;
                    return;
                default:
                    return;
            }
        }

        public override void OnLoadFinished()
        {
            if (!_loadingSavedGame) // here you should add an intro video
            {
                MBDebug.Print("Switching to menu window...", 0, Debug.DebugColor.White, 17592186044416UL);

                //VideoPlaybackState state = Game.Current.GameStateManager.CreateState<VideoPlaybackState>();
                //string str = ModuleHelper.GetModuleFullPath(folderName + "Videos/CampaignIntro/";
                //string subtitleFileBasePath = str + "intro";
                //string videoPath = str + "intro.ivf";
                //string audioPath = str + "intro.ogg";
                //state.SetStartingParameters(videoPath, audioPath, subtitleFileBasePath);
                //state.SetOnVideoFinisedDelegate(new Action(this.LaunchCharacterCreation));
                //Game.Current.GameStateManager.CleanAndPushState((GameState)state);
                LaunchCharacterCreation();
            }
            else
            {
                Game.Current.GameStateManager.OnSavedGameLoadFinished();
                Game.Current.GameStateManager.CleanAndPushState(Game.Current.GameStateManager.CreateState<MapState>(), 0);
                if (Game.Current.GameStateManager.ActiveState is not MapState mapState)
                {
                    MBDebug.Print("ERROR LOADING THE GAME", 0, Debug.DebugColor.Red, 17592186044416UL);
                    return;
                }
                string text = mapState.GameMenuId;
                if (!string.IsNullOrEmpty(text))
                {
                    PlayerEncounter playerEncounter = PlayerEncounter.Current;
                    playerEncounter?.OnLoad();
                    Campaign.Current.GameMenuManager.SetNextMenu(text);
                }
                PartyBase.MainParty.SetVisualAsDirty();
                Campaign.Current.CampaignInformationManager.OnGameLoaded();
                foreach (Settlement settlement in Settlement.All)
                {
                    settlement.Party.SetLevelMaskIsDirty();
                }
                CampaignEventDispatcher.Instance.OnGameLoadFinished();
            }
            IsLoaded = true;
        }

        private static void LaunchCharacterCreation()
        {
            CharacterCreationState gameState = Game.Current.GameStateManager.CreateState<CharacterCreationState>(new object[]
            {
                new SandboxCharacterCreationContent()
            });
            Game.Current.GameStateManager.CleanAndPushState(gameState);
        }

        public override void OnAfterCampaignStart(Game game)
        {
            
        }
    }
}
