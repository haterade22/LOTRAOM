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
using LOTRAOM.Extensions;
namespace LOTRAOM.Momentum
{
    public class MomentumCampaignBehavior : CampaignBehaviorBase
    {
        public Action? OnMomentumChanged;
        public static MomentumCampaignBehavior Instance { get { return Campaign.Current.GetCampaignBehavior<MomentumCampaignBehavior>(); } }
        [SaveableField(0)] private WarOfTheRingData _warOfTheRingData = new();
        [SaveableField(1)] public bool hasIsengardAttacked;
        private MapView? mapView; 
        public WarOfTheRingData warOfTheRingData
        {
            get
            {
                _warOfTheRingData ??= new WarOfTheRingData();
                return _warOfTheRingData;
            }
        }
        public override void RegisterEvents()
        {
            if (warOfTheRingData.HasWarEnded) return;
            CampaignEvents.ArmyGathered.AddNonSerializedListener(this, OnArmyGathered);
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, OnDailyTick);
            CampaignEvents.SiegeCompletedEvent.AddNonSerializedListener(this, OnSiegeCompletedEvent);
            CampaignEvents.MapEventEnded.AddNonSerializedListener(this, OnMapEventEnded);
            CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, OnGameLoadFinished);
            CampaignEvents.RaidCompletedEvent.AddNonSerializedListener(this, OnRaidCompletedEvent);
            CampaignEvents.KingdomDestroyedEvent.AddNonSerializedListener(this, OnKingdomDestroyed);
        }

        public void RemoveAllListeners()
        {
            RemoveMomentumUI();
            CampaignEvents.ArmyDispersed.ClearListeners(this);
            CampaignEvents.ArmyGathered.ClearListeners(this);
            CampaignEvents.DailyTickEvent.ClearListeners(this);
            CampaignEvents.SiegeCompletedEvent.ClearListeners(this);
            CampaignEvents.MapEventEnded.ClearListeners(this);
            CampaignEvents.OnGameLoadFinishedEvent.ClearListeners(this);
            CampaignEvents.RaidCompletedEvent.ClearListeners(this);
        }
        private void OnRaidCompletedEvent(BattleSideEnum SideEnum, RaidEventComponent component)
        {
            if (!warOfTheRingData.HasWarStarted) return;
            if (component.BattleState != BattleState.AttackerVictory) return;
            WarOfTheRingSide winningSide = component.AttackerSide.LeaderParty.Culture.IsGoodCulture() ? WarOfTheRingSide.Good : WarOfTheRingSide.Evil;
            if (winningSide == WarOfTheRingSide.Good)
                warOfTheRingData.GoodKingdoms.FactionTotalStats.AddRaid();
            else warOfTheRingData.EvilKingdoms.FactionTotalStats.AddRaid(); 
            TextObject text = new TextObject($"{component.AttackerSide.LeaderParty.Name} of {component.AttackerSide.LeaderParty.Owner.MapFaction.Name} raided the village of {component.MapEventSettlement.Name}");
            warOfTheRingData.AddEvent(winningSide, MomentumActionType.VillageRaided, new MomentumEvent(MomentumGlobals.MomentumFromVillageRaid, text, MomentumActionType.VillageRaided, GetEventEndTime(MomentumActionType.VillageRaided)));
        }
        private void OnGameLoadFinished()
        {
            if (AoMSettings.Instance.LoreAccurateDiplomacy && warOfTheRingData.HasWarStarted)
                AddMomentumUI();
        }
        private void OnMapEventEnded(MapEvent mapEvent)
        {
            if (!warOfTheRingData.HasWarStarted 
                || mapEvent.Winner == null 
                || !warOfTheRingData.DoesFactionTakePartInWar(mapEvent.DefenderSide.LeaderParty.MapFaction)
                || !warOfTheRingData.DoesFactionTakePartInWar(mapEvent.AttackerSide.LeaderParty.MapFaction))return;

            bool attackerWon = mapEvent.Winner == mapEvent.AttackerSide;
            MapEventSide lostSide = attackerWon ? mapEvent.DefenderSide : mapEvent.AttackerSide;

            MapEventSide goodSide;
            MapEventSide evilSide;
            if (mapEvent.Winner.LeaderParty.Culture.IsEvilCulture())
            {
                evilSide = mapEvent.Winner;
                goodSide = lostSide;
            }
            else
            {
                goodSide = mapEvent.Winner;
                evilSide = lostSide;
            }
            warOfTheRingData.GoodKingdoms.FactionTotalStats.AddKills(evilSide.Casualties);
            warOfTheRingData.EvilKingdoms.FactionTotalStats.AddKills(goodSide.Casualties);

            if ((mapEvent.EventType != MapEvent.BattleTypes.FieldBattle && mapEvent.EventType != MapEvent.BattleTypes.Raid && mapEvent.EventType != MapEvent.BattleTypes.SiegeOutside)
                || !mapEvent.DefenderSide.LeaderParty.IsMobile)
                return;
            if (!mapEvent.AttackerSide.MapFaction.IsKingdomFaction || !mapEvent.DefenderSide.MapFaction.IsKingdomFaction) return;

            string winnerLeaderName = "brave unnamed warrior";
            string loserLeaderName = "brave unnamed warrior";
            if (mapEvent.Winner.LeaderParty.Name.Contains("Party"))
                winnerLeaderName = mapEvent.Winner.LeaderParty.Owner.Name.ToString();
            if (lostSide.LeaderParty.Name.Contains("Party"))
                loserLeaderName = lostSide.LeaderParty.Owner.Name.ToString();
            int loserCasualties = lostSide.Casualties;
            
            TextObject text = new TextObject($"Army of {mapEvent.Winner.MapFaction.Name}, led by {winnerLeaderName} destroyed the army of {lostSide.MapFaction.Name}, led by {loserLeaderName}, killing {loserCasualties}.");
            int momentumGain = CalculateMomentumGainFromBattle((Kingdom)lostSide.LeaderParty.MapFaction, loserCasualties);
            warOfTheRingData.AddEvent((Kingdom)mapEvent.Winner.MapFaction, MomentumActionType.BattleWon, new MomentumEvent(momentumGain, text, MomentumActionType.BattleWon, GetEventEndTime(MomentumActionType.BattleWon)));
            OnMomentumChanged?.Invoke();
        }
        private int CalculateMomentumGainFromBattle(Kingdom loser, int casualties)
        {
            float percentageLost = loser.Culture.IsGoodCulture()? casualties / warOfTheRingData.GoodKingdoms.TotalStrength
                : casualties / warOfTheRingData.EvilKingdoms.TotalStrength;
            return (int)Math.Round(percentageLost * MomentumGlobals.MaxMomentumFromBattleWon);
        }
        private void OnKingdomDestroyed(Kingdom kingdom)
        {
            warOfTheRingData.RemoveKingdom(kingdom);
        }
        private void OnSiegeCompletedEvent(Settlement settlement, MobileParty party, bool isWin, MapEvent.BattleTypes types)
        {
            if (!warOfTheRingData.HasWarStarted
                || !isWin
                || !warOfTheRingData.DoesFactionTakePartInWar(party.MapFaction)
                || !warOfTheRingData.DoesFactionTakePartInWar(party.MapFaction))
                return;
            if (party.Owner.Culture.IsGoodCulture())
                warOfTheRingData.GoodKingdoms.FactionTotalStats.AddSettlementCaptured();
            else
                warOfTheRingData.EvilKingdoms.FactionTotalStats.AddSettlementCaptured();

            TextObject text = new TextObject($"Army of {party.MapFaction.Name}, led by {party.LeaderHero} captured the settlement of {settlement.Name}");
            warOfTheRingData.AddEvent((Kingdom)party.MapFaction, MomentumActionType.Sieges, new MomentumEvent(MomentumGlobals.MomentumFromSiege, text, MomentumActionType.Sieges, GetEventEndTime(MomentumActionType.Sieges)));
            OnMomentumChanged?.Invoke();
        }
        private void OnDailyTick()
        {
            if (!warOfTheRingData.HasWarStarted) return;
            if (warOfTheRingData.EndWarIfConditionsMet())
            {
                RemoveAllListeners();
                return;
            }
            int momentumChange = 0;
            CampaignTime now = CampaignTime.Now;
            foreach (Queue<MomentumEvent> eventQueue in warOfTheRingData.GoodKingdoms.WarOfTheRingEvents.Values)
            {
                if (eventQueue.IsEmpty()) continue;
                if (eventQueue.Peek().EndTime < now)
                {
                    MomentumEvent momentumEvent = eventQueue.Dequeue();
                    momentumChange -= momentumEvent.MomentumValue;
                }
            }
            if (momentumChange != 0)
                warOfTheRingData.GoodKingdoms.EditMomentum(momentumChange);
            momentumChange = 0;
            foreach (Queue<MomentumEvent> eventQueue in warOfTheRingData.EvilKingdoms.WarOfTheRingEvents.Values)
            {
                if (eventQueue.IsEmpty()) continue;
                if (eventQueue.Peek().EndTime < now)
                {
                    var momentumEvent = eventQueue.Dequeue();
                    momentumChange -= momentumEvent.MomentumValue; 
                }
            }
            if (momentumChange != 0)
                warOfTheRingData.EvilKingdoms.EditMomentum(momentumChange);
            CalculateDailyMomentumChangeFromFactionStrength();
            OnMomentumChanged?.Invoke();
        }
        public void CalculateDailyMomentumChangeFromFactionStrength()
        {
            float goodStrength = warOfTheRingData.GoodKingdoms.TotalStrength;
            float evilStrength = warOfTheRingData.EvilKingdoms.TotalStrength;
            if (goodStrength == 0 || evilStrength == 0) return;
            bool isGoodStronger = goodStrength > evilStrength;
            float percentage = isGoodStronger? goodStrength / evilStrength : evilStrength / goodStrength;
            percentage -= 1;
            int value = (int)Math.Min(
                percentage * MomentumGlobals.MomentumMultiplierFromTotalStrength,
                MomentumGlobals.MaxMomentumFromTotalStrength
            );
            TextObject text = new("We are {PERCENTAGE}% stronger than the enemies!");
            text.SetTextVariable("PERCENTAGE", percentage * 100);
            warOfTheRingData.AddEvent(isGoodStronger? WarOfTheRingSide.Good : WarOfTheRingSide.Evil, MomentumActionType.RelativeStrength, new MomentumEvent(value, text, MomentumActionType.RelativeStrength, GetEventEndTime(MomentumActionType.RelativeStrength)));
        }
        private void OnArmyGathered(Army army, Settlement settlement)
        {
            if (!warOfTheRingData.HasWarStarted) return;
            if (!warOfTheRingData.DoesFactionTakePartInWar(army.Kingdom)) return;
            CampaignTime endTime = GetEventEndTime(MomentumActionType.ArmyGathered);
            warOfTheRingData.AddEvent(army.Kingdom, MomentumActionType.ArmyGathered, new MomentumEvent(MomentumGlobals.MomentumFromArmyGathering, new TextObject($"Army led by {army.ArmyOwner.Name} has gathered"), MomentumActionType.ArmyGathered, endTime));
            OnMomentumChanged?.Invoke();
        }
        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("_warOfTheRingData", ref _warOfTheRingData);
            dataStore.SyncData("hasIsengardAttacked", ref hasIsengardAttacked);
            if (dataStore.IsLoading)
                warOfTheRingData.SyncData();
        }
        public static CampaignTime GetEventEndTime(MomentumActionType type)
        {
            return type switch
            {
                MomentumActionType.Sieges => CampaignTime.DaysFromNow(28),
                MomentumActionType.BattleWon => CampaignTime.DaysFromNow(21),
                MomentumActionType.ArmyGathered => CampaignTime.DaysFromNow(7),
                MomentumActionType.RelativeStrength => CampaignTime.HoursFromNow(22),
                MomentumActionType.VillageRaided=> CampaignTime.DaysFromNow(14),
                _ => CampaignTime.DaysFromNow(3),
            };
        }
        public void AddMomentumUI()
        {
            if (AoMSettings.Instance.LoreAccurateDiplomacy)
                mapView = MapScreen.Instance.AddMapView<MomentumIndicator>();
        }
        public void RemoveMomentumUI()
        {
            if (AoMSettings.Instance.LoreAccurateDiplomacy && mapView != null)
                MapScreen.Instance.RemoveMapView(mapView);
        }
    }
}
