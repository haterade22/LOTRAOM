using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.Core;
using SandBox.View.Map;
using LOTRAOM.Momentum.Views;
namespace LOTRAOM.Momentum
{
    public class MomentumCampaignBehavior : CampaignBehaviorBase
    {
        public Action OnMomentumChanged;
        public static MomentumCampaignBehavior Instance { get { return Campaign.Current.GetCampaignBehavior<MomentumCampaignBehavior>(); } }
        [SaveableField(0)] private WarOfTheRingData _warOfTheRingData;
        [SaveableField(1)] public bool hasIsengardAttacked;
        public WarOfTheRingData WarOfTheRingdata
        {
            get
            {
                _warOfTheRingData ??= new WarOfTheRingData();
                return _warOfTheRingData;
            }
        }
        public override void RegisterEvents()
        {
            CampaignEvents.ArmyDispersed.AddNonSerializedListener(this, OnArmyDispersed);
            CampaignEvents.ArmyGathered.AddNonSerializedListener(this, OnArmyGathered);
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, OnDailyTick);
            CampaignEvents.SiegeCompletedEvent.AddNonSerializedListener(this, OnSiegeCompletedEvent);
        }

        private void OnSiegeCompletedEvent(Settlement settlement, MobileParty party, bool isWin, MapEvent.BattleTypes types)
        {
            if (!isWin) return;

        }

        private void OnDailyTick()
        {
            int momentumChange = 0;
            CampaignTime now = CampaignTime.Now;
            foreach (Queue<MomentumEvent> eventQueue in WarOfTheRingdata.GoodKingdoms.WarOfTheRingEvents.Values)
            {
                if (eventQueue.IsEmpty()) continue;
                if (eventQueue.Peek().EndTime < now)
                {
                    MomentumEvent momentumEvent = eventQueue.Dequeue();
                    momentumChange += momentumEvent.MomentumValue;
                }
            }

            WarOfTheRingdata.GoodKingdoms.EditMomentum(momentumChange);
            momentumChange = 0;
            foreach (Queue<MomentumEvent> eventQueue in WarOfTheRingdata.EvilKingdoms.WarOfTheRingEvents.Values)
            {
                if (eventQueue.IsEmpty()) continue;
                if (eventQueue.Peek().EndTime < now)
                {
                    var momentumEvent = eventQueue.Dequeue();
                    momentumChange += momentumEvent.MomentumValue;
                }
            }
            WarOfTheRingdata.GoodKingdoms.EditMomentum(momentumChange);
        }

        private void OnArmyGathered(Army army, Settlement settlement)
        {
            if (!WarOfTheRingdata.DoesFactionTakePartInWar(army.Kingdom)) return;
            WarOfTheRingdata.AddEvent(army.Kingdom, MomentumActionType.ArmyGathered, new MomentumEvent(20, new TextObject($"Army led by {army.ArmyOwner.Name} has gathered"), MomentumActionType.ArmyGathered, GetEventEndTime(MomentumActionType.ArmyGathered)));
            OnMomentumChanged.Invoke();
        }
        private void OnArmyDispersed(Army army, Army.ArmyDispersionReason reason, bool arg3)
        {
            if (reason != Army.ArmyDispersionReason.LeaderPartyRemoved) return;
        }
        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("_warOfTheRingData", ref _warOfTheRingData);
            dataStore.SyncData("hasIsengardAttacked", ref hasIsengardAttacked);
        }
        public static CampaignTime GetEventEndTime(MomentumActionType type)
        {
            return CampaignTime.DaysFromNow(2);
        }

        public void AddMomentumUI()
        {
            if (AoMSettings.Instance.BalanceOfPower)
                MapScreen.Instance.AddMapView<MomentumIndicator>();
        }
    }
}
