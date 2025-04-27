using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace LOTRAOM.DoubleSiege
{
    public class DoubleSiegeCampaignBehavior : CampaignBehaviorBase
    {
        static DoubleSiegeCampaignBehavior? _instance;
        public DoubleSiegeCampaignBehavior()
        {
            _instance = this;
        }
        public static DoubleSiegeCampaignBehavior Instance 
        {
            get
            {
                _instance ??= Campaign.Current.GetCampaignBehavior<DoubleSiegeCampaignBehavior>();
                return _instance; 
            }
        }
        public static bool ContainsSecondSiege(string settlementId)
        {
            return SecondSiegeData.All.ContainsKey(settlementId);
        }
        public bool HasFinishedTheSecondSiege = false;

        public override void RegisterEvents()
        {
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
            CampaignEvents.MapEventEnded.AddNonSerializedListener(this, OnMapEventEnded);
        }

        private void OnMapEventEnded(MapEvent @event)
        {
            HasFinishedTheSecondSiege = false;
        }
        private void OnSessionLaunched(CampaignGameStarter starter)
        {
            AddGameMenus(starter);
        }
        private void AddGameMenus(CampaignGameStarter starter)
        {
            starter.AddGameMenu("double_siege_menu", "Double siege Text", new OnInitDelegate(this.game_menu_double_siege_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
            starter.AddGameMenuOption("double_siege_menu", "start", "Start Text", (MenuCallbackArgs args) => { return true; }, new GameMenuOption.OnConsequenceDelegate(this.game_menu_double_siege_start_on_consequence), false, -1, false);
        }

        private void game_menu_double_siege_start_on_consequence(MenuCallbackArgs args)
        {
            if (Hero.MainHero.HitPoints < 20)
                Hero.MainHero.HitPoints = 20;
            Settlement curSettlement = MobileParty.MainParty.MapEvent.MapEventSettlement;
            string settlementId = curSettlement.StringId;
            SecondSiegeData secondSiegeData = SecondSiegeData.GetSecondSiegeData(settlementId)!;
            PartyTemplateObject partyTemplate = MBObjectManager.Instance.GetObject<PartyTemplateObject>(secondSiegeData.PartyTemplateId);
            Random random = new();
            foreach(var stack in partyTemplate.Stacks)
                curSettlement.Town.GarrisonParty.AddElementToMemberRoster(stack.Character, random.Next(stack.MinValue, stack.MaxValue));

            float[] walls = { 1, 1 };
            List<MissionSiegeWeapon> preparedAndActiveSiegeEngines = PlayerSiege.PlayerSiegeEvent.GetPreparedAndActiveSiegeEngines(PlayerSiege.PlayerSiegeEvent.GetSiegeEventSide(BattleSideEnum.Attacker));
            List<MissionSiegeWeapon> preparedAndActiveSiegeEngines2 = PlayerSiege.PlayerSiegeEvent.GetPreparedAndActiveSiegeEngines(PlayerSiege.PlayerSiegeEvent.GetSiegeEventSide(BattleSideEnum.Defender));
            bool hasAnySiegeTower = preparedAndActiveSiegeEngines.Exists((MissionSiegeWeapon data) => data.Type == DefaultSiegeEngineTypes.SiegeTower);
            CustomSiegeMission.OpenSiegeMissionWithDeployment(secondSiegeData.SecondSiegeSceneId, walls, hasAnySiegeTower, preparedAndActiveSiegeEngines, preparedAndActiveSiegeEngines2);
        }

        private void game_menu_double_siege_on_init(MenuCallbackArgs args)
        {
            SecondSiegeData? secondSiegeData = SecondSiegeData.GetSecondSiegeData(MobileParty.MainParty.MapEvent.MapEventSettlement.StringId);
            if (secondSiegeData == null)
            {
                InformationManager.DisplayMessage(new InformationMessage("ERROR, No siege data found for this settlement.", new Color(255, 0, 0)));
                HasFinishedTheSecondSiege = true;
                GameMenu.ActivateGameMenu("encounter");
                return;
            }
            string meshId = secondSiegeData.BackgroundMeshId;
            args.MenuContext.SetBackgroundMeshName(meshId);
            if (HasFinishedTheSecondSiege)
                GameMenu.ActivateGameMenu("encounter");
        }
        public override void SyncData(IDataStore dataStore) { }
    }
}
