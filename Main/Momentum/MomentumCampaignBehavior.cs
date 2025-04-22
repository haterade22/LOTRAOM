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
using TaleWorlds.Library;
using LOTRAOM.Extensions;
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
            CampaignEvents.MapEventEnded.AddNonSerializedListener(this, OnMapEventEnded);
            CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, OnGameLoadFinished);
        }

        private void OnGameLoadFinished()
        {
            if (AoMSettings.Instance.BalanceOfPower && WarOfTheRingdata.HasWarStarted())
            {
                AddMomentumUI();
            }
        }

        private void OnMapEventEnded(MapEvent mapEvent)
        {
            if (!WarOfTheRingdata.HasWarStarted() 
                || mapEvent.Winner == null
                || (mapEvent.EventType != MapEvent.BattleTypes.FieldBattle && mapEvent.EventType != MapEvent.BattleTypes.Raid && mapEvent.EventType != MapEvent.BattleTypes.SiegeOutside)
                || !mapEvent.DefenderSide.LeaderParty.IsMobile
                || !WarOfTheRingdata.DoesFactionTakePartInWar(mapEvent.DefenderSide.LeaderParty.MapFaction)
                || !WarOfTheRingdata.DoesFactionTakePartInWar(mapEvent.AttackerSide.LeaderParty.MapFaction)) 
                return;
            if (!mapEvent.AttackerSide.MapFaction.IsKingdomFaction || !mapEvent.DefenderSide.MapFaction.IsKingdomFaction) return;

            bool attackerWon = mapEvent.Winner == mapEvent.AttackerSide;
            MapEventSide lostSide = attackerWon ? mapEvent.DefenderSide : mapEvent.AttackerSide;
            string winnerLeaderName = "brave unnamed warrior";
            string loserLeaderName = "brave unnamed warrior";
            if (mapEvent.Winner.LeaderParty.Name.Contains("Party"))
                winnerLeaderName = mapEvent.AttackerSide.LeaderParty.Owner.Name.ToString();
            if (lostSide.LeaderParty.Name.Contains("Party"))
                loserLeaderName = mapEvent.DefenderSide.LeaderParty.Owner.Name.ToString();
            int loserCasualties = lostSide.Casualties;
            
            TextObject text = new TextObject($"Army of {mapEvent.Winner.MapFaction.Name}, led by {winnerLeaderName} destroyed the army of {lostSide.MapFaction.Name}, led by {loserLeaderName}, killing {loserCasualties}.");
            int momentumGain = CalculateMomentumGainFromBattle((Kingdom)lostSide.LeaderParty.MapFaction, loserCasualties);
            WarOfTheRingdata.AddEvent((Kingdom)mapEvent.Winner.MapFaction, MomentumActionType.BattleWon, new MomentumEvent(momentumGain, text, MomentumActionType.BattleWon, GetEventEndTime(MomentumActionType.ArmyGathered)));
            OnMomentumChanged.Invoke();
        }
        private int CalculateMomentumGainFromBattle(Kingdom loser, int casualties)
        {
            float percentageLost = 0;
            if (loser.Culture.IsGoodCulture())
            {
                percentageLost = casualties / WarOfTheRingdata.GoodKingdoms.TotalStrength;
            }
            else
            {
                percentageLost = casualties / WarOfTheRingdata.GoodKingdoms.TotalStrength;
            }
            return (int)Math.Round(percentageLost * 300);
        }

        private void OnSiegeCompletedEvent(Settlement settlement, MobileParty party, bool isWin, MapEvent.BattleTypes types)
        {
            if (!WarOfTheRingdata.HasWarStarted()
                || !isWin
                || !WarOfTheRingdata.DoesFactionTakePartInWar(party.MapFaction)
                || !WarOfTheRingdata.DoesFactionTakePartInWar(party.MapFaction))
                return;

            TextObject text = new TextObject($"Army of {party.MapFaction.Name}, led by {party.LeaderHero} captured the settlement of {settlement.Name}");
            WarOfTheRingdata.AddEvent((Kingdom)party.MapFaction, MomentumActionType.BattleWon, new MomentumEvent(MomentumGlobals.MomentumFromSiege, text, MomentumActionType.BattleWon, GetEventEndTime(MomentumActionType.ArmyGathered)));
            OnMomentumChanged.Invoke();
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
                    momentumChange -= momentumEvent.MomentumValue;
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
                    momentumChange -= momentumEvent.MomentumValue;
                }
            }
            WarOfTheRingdata.GoodKingdoms.EditMomentum(momentumChange);
            OnMomentumChanged.Invoke();
        }
        private void OnArmyGathered(Army army, Settlement settlement)
        {
            if (!WarOfTheRingdata.DoesFactionTakePartInWar(army.Kingdom)) return;
            CampaignTime endTime = GetEventEndTime(MomentumActionType.ArmyGathered);
            WarOfTheRingdata.AddEvent(army.Kingdom, MomentumActionType.ArmyGathered, new MomentumEvent(20, new TextObject($"Army led by {army.ArmyOwner.Name} has gathered"), MomentumActionType.ArmyGathered, endTime));
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
            return type switch
            {
                MomentumActionType.Siege => CampaignTime.DaysFromNow(21),
                MomentumActionType.BattleWon => CampaignTime.DaysFromNow(14),
                _ => CampaignTime.DaysFromNow(3),
            };
        }

        public void AddMomentumUI()
        {
            if (AoMSettings.Instance.BalanceOfPower)
                MapScreen.Instance.AddMapView<MomentumIndicator>();
        }
    }
}
