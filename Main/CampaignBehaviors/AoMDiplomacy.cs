using LOTRAOM.Events;
using LOTRAOM.Extensions;
using LOTRAOM.Momentum;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace LOTRAOM.CampaignBehaviors
{
    public class AoMDiplomacy : CampaignBehaviorBase
    {
        public WarOfTheRingData WarOfTheRingdata => MomentumCampaignBehavior.Instance.warOfTheRingData;
         public static List<WarEvent> WarDeclaredEvents = new();
        [SaveableField(0)] private static List<string> notUsedEvents = new();
        [SaveableField(1)] public static Dictionary<string, int> EvilFactionsDaysWithoutWar = new();
        [SaveableField(2)] public static List<DelayedDiplomaticEvent> delayedEvents = new(); //event id
        [SaveableField(3)] public CampaignTime isengardAttackDay;
        public Queue<IsengardTextEvent> IsengardEvents = new();
        public override void RegisterEvents()
        {
            CampaignEvents.WarDeclared.AddNonSerializedListener(this, OnWarDeclared);
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, OnDailyTick);
            CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, OnNewGameCreated);
            CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, OnGameLoaded);
        }

        private void OnGameLoaded(CampaignGameStarter starter)
        {
            foreach (string eventId in notUsedEvents)
            {
                WarEvent? warEvent = WarEvent.AllEvents.FirstOrDefault(e => e.StringId == eventId);
                if (warEvent != null)
                    WarDeclaredEvents.Add(warEvent);
            }
            InitializeIsengardTextEvents();
        }
        public void InitializeIsengardTextEvents()
        {
            Queue<IsengardTextEvent> queue = new();
            double percentageTillAttack = PercentageTimeTillIsengardAttacks();
            List<IsengardTextEvent> sortedList = new(IsengardTextEvent.All);
            sortedList.Sort((a, b) => a.ThresholdToHappen.CompareTo(b.ThresholdToHappen));
            for (int i = sortedList.Count - 1; i >= 0; i--)
            {
                IsengardTextEvent isengardEvent = sortedList[i];
                if (isengardEvent.ThresholdToHappen < percentageTillAttack)
                    queue.Enqueue(isengardEvent);
            }
            IsengardEvents = queue;
        }
        private void TryForceStartWar()
        {
            double daysSinceIsengardWar = (CampaignTime.Now - isengardAttackDay).ToDays;
            foreach(var forceWar in ForceWar.All)
            {
                string eventId = forceWar.WarEventId;
                if (forceWar.DaysAfterIsengardWar < daysSinceIsengardWar && notUsedEvents.Contains(eventId))
                {
                    WarEvent? warEvent = WarDeclaredEvents.FirstOrDefault(e => e.StringId == eventId);
                    if (warEvent == null) continue;
                    warEvent.Action();
                    WarDeclaredEvents.Remove(warEvent);
                    notUsedEvents.Remove(eventId);
                    if (warEvent.delayedActionTime != 0)
                        delayedEvents.Add(new(warEvent.StringId, CampaignTime.DaysFromNow(warEvent.delayedActionTime)));
                }
            }
        }
        private void OnNewGameCreated(CampaignGameStarter starter)
        {
            notUsedEvents = new()
            {
                "isengard_war",
                "mordor_war",
                "rhun_erebor_dale_war",
                "umbar_harad_khand_gondor_war",
                "dol_guldur_gundabad_mirkwood_lorien_war",
                "rivendell_joins_war"
            };
            foreach (string eventId in notUsedEvents)
            {
                WarEvent? warEvent = WarEvent.AllEvents.FirstOrDefault(e => e.StringId == eventId);
                if (warEvent != null)
                    WarDeclaredEvents.Add(warEvent);
            }
            Random random = new();
            int earliestDay = Math.Min(AoMSettings.Instance.IsengardEarliestAttackDate, AoMSettings.Instance.IsengardLatestAttackDate);
            int latestDay = Math.Max(AoMSettings.Instance.IsengardEarliestAttackDate, AoMSettings.Instance.IsengardLatestAttackDate);
            int day = random.Next(earliestDay, latestDay);
            isengardAttackDay = CampaignTime.DaysFromNow(day);
            InitializeIsengardTextEvents();
        }
        public double PercentageTimeTillIsengardAttacks()
        {
            double daysUntil = isengardAttackDay.RemainingDaysFromNow;
            double attackDateAsFloat = isengardAttackDay.ToDays - CampaignData.CampaignStartTime.ToDays;
            return daysUntil / attackDateAsFloat;
        }
        private void FireEventsLeadingToIsengardInvasion()
        {
            double timeTill = PercentageTimeTillIsengardAttacks();
            while (!IsengardEvents.IsEmpty() && IsengardEvents.Peek().ThresholdToHappen > timeTill) 
            {
                IsengardTextEvent textEvent = IsengardEvents.Dequeue();
                MBTextManager.SetTextVariable("CURRENT_DAY", CampaignTime.Now.ToString(), false);
                InquiryData data = new(new TextObject(textEvent.Title).ToString(), new TextObject(textEvent.Text).ToString(), true, false, new TextObject(textEvent.ContinueText).ToString(), "", () => { }, () => { });
                InformationManager.ShowInquiry(data, true, false);
            }
            if (timeTill <= 0)
                StartIsengardWar();
        }
        private void TryHaveRivendellJoinWar()
        {
            string warEventId = "rivendell_joins_war";
            if (!notUsedEvents.Contains(warEventId)) return;
            WarEvent rivendellEvent = WarDeclaredEvents.First(x => x.StringId == warEventId);
            if (!rivendellEvent.isConditionMet(null!, null!)) return;
            notUsedEvents.Remove(warEventId);
            WarDeclaredEvents.RemoveAll(e => e.StringId == "warEventId");
            rivendellEvent.Action();
        }
        private void StartIsengardWar()
        {
            string warEventId = "isengard_war";
            notUsedEvents.Remove(warEventId);
            WarEvent isengardEvent = WarDeclaredEvents.First(x => x.StringId == warEventId);
            WarDeclaredEvents.RemoveAll(e => e.StringId == "isengard_war");
            isengardEvent.Action();
        }
        private void OnDailyTick()
        {
            if (!MomentumCampaignBehavior.Instance.hasIsengardAttacked)
                FireEventsLeadingToIsengardInvasion();
            List<Kingdom> kingdoms;
            if (AoMSettings.Instance.LoreAccurateDiplomacy) kingdoms = Kingdom.All.Where(k => k.Culture.IsEvilCulture()).ToList();
            else kingdoms = Kingdom.All;

            foreach (Kingdom kingdom in kingdoms)
            {
                if (!kingdom.Stances.Any(s => s.IsAtWar == true && s.Faction2.IsKingdomFaction && s.Faction1.IsKingdomFaction))
                {
                    if (!EvilFactionsDaysWithoutWar.ContainsKey(kingdom.StringId))
                        EvilFactionsDaysWithoutWar.Add(kingdom.StringId, 0);
                    else
                        EvilFactionsDaysWithoutWar[kingdom.StringId]++;
                }
            }
            for (int i = delayedEvents.Count - 1; i >= 0; i--)
            {
                if (MomentumCampaignBehavior.Instance.warOfTheRingData.HasWarStarted && CampaignTime.Now > delayedEvents[i].delayedActionTime)
                {
                    WarEvent? warEvent = WarEvent.AllEvents.FirstOrDefault(e => e.StringId == delayedEvents[i].StringId);
                    warEvent?.DelayedAction?.Invoke();
                    delayedEvents.RemoveAt(i);
                }
            }
            TryForceStartWar();
            TryHaveRivendellJoinWar();
        }

        private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
        {
            EvilFactionsDaysWithoutWar[faction1.StringId] = 0;
            if (!AoMSettings.Instance.LoreAccurateDiplomacy) return;
            if (faction1 is not Kingdom kingdom1 || faction2 is not Kingdom kingdom2) return;

            //if (!MomentumCampaignBehavior.Instance.WarOfTheRingdata.HasWarStarted()) return;
            for (int i = WarDeclaredEvents.Count - 1; i >= 0; i--)
            {
                var warEvent = WarDeclaredEvents[i];
                if (warEvent.isConditionMet(kingdom1, kingdom2))
                {
                    warEvent.Action();
                    WarDeclaredEvents.RemoveAt(i);
                    notUsedEvents.Remove(warEvent.StringId);
                    if(warEvent.delayedActionTime != 0)
                        delayedEvents.Add(new(warEvent.StringId, CampaignTime.DaysFromNow(warEvent.delayedActionTime)));
                }
            }
        }
        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("evilFactionsDaysWithoutWar", ref EvilFactionsDaysWithoutWar);
            dataStore.SyncData("notUsedEvents", ref notUsedEvents);
            dataStore.SyncData("delayedEvents", ref delayedEvents);
            dataStore.SyncData("isengardAttackDay", ref isengardAttackDay);
        }
    }
}