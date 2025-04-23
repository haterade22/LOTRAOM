using LOTRAOM.Extensions;
using LOTRAOM.Momentum;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;


namespace LOTRAOM.CampaignBehaviors
{
    public class ForceWar
    {
        public string WarEventId { get; }
        public int DaysAfterIsengardWar { get; }

        public ForceWar(string warEventId, int daysAfterIsengardWar)
        {
            WarEventId = warEventId;
            DaysAfterIsengardWar = daysAfterIsengardWar;
        }
        public static List<ForceWar> All = new()
        {
            new("mordor_war", 20),
            new("rhun_erebor_dale_war", 30),
            new("umbar_harad_khand_gondor_war", 35),
            new("dol_guldur_gundabad_mirkwood_lorien_war", 40),
        };
    }
    public class WarEvent
    {
        public Func<Kingdom, Kingdom, bool> isConditionMet;
        public Action Action;
        public Action? DelayedAction;
        public int delayedActionTime;
        public string StringId { get; }
        public WarEvent(string stringId, Func<Kingdom, Kingdom, bool> condition, Action action, Action? delayedAction = null, int delayedActionDays = 0)
        {
            StringId = stringId;
            isConditionMet = condition;
            Action = action;
            delayedActionTime = delayedActionDays;
            DelayedAction = delayedAction;
        }
        public static List<WarEvent> AllEvents = new()
        {
            new WarEvent("isengard_war", (kingdom1, kingdom2) => // isengard or dunland attacks gondor or rohan
            {
                return (kingdom1.StringId == Globals.IsengardKingdom?.StringId || kingdom1.StringId == Globals.DunlandKingdom?.StringId) && (kingdom2.StringId == Globals.GondorKingdom?.StringId || kingdom2.StringId == Globals.RohanKingdom?.StringId);
            },
            () =>
            {
                FactionManager.DeclareWar(Globals.IsengardKingdom, Globals.RohanKingdom);
                FactionManager.DeclareWar(Globals.IsengardKingdom, Globals.GondorKingdom);
                FactionManager.DeclareWar(Globals.DunlandKingdom, Globals.GondorKingdom);
                FactionManager.DeclareWar(Globals.DunlandKingdom, Globals.RohanKingdom);
                MomentumCampaignBehavior.Instance.hasIsengardAttacked = true;
                InquiryData data = new("The Gathering Storm", $"{CampaignTime.Now} marked the start of events, that would lead to a devastating conflict, of a scale, thought only left to the annals of history. The leader of the keepers of peace - the Grey Council, Saruman the White, supported by the northern tribes of the Dunlendings declared war, to shatter the allied kingdoms of Gondor and Rohan. The days of peace are over", true, false, new TextObject("The new era begins today!").ToString(), "", () => {}, () => {});
                InformationManager.ShowInquiry(data, true, false);
            }),
            new WarEvent("mordor_war", (kingdom1, kingdom2) => // mordor attacks gondor or rohan
            {
                return !MomentumCampaignBehavior.Instance.WarOfTheRingdata.HasWarStarted && kingdom1.StringId == Globals.MordorKingdom?.StringId && (kingdom2.StringId == Globals.RohanKingdom?.StringId || kingdom2.StringId == Globals.GondorKingdom?.StringId);
            },
            () =>
            {
                if (Globals.GondorKingdom != null)
                    MomentumCampaignBehavior.Instance.WarOfTheRingdata.AddKingdom(Globals.GondorKingdom);
                if (Globals.RohanKingdom != null)
                    MomentumCampaignBehavior.Instance.WarOfTheRingdata.AddKingdom(Globals.RohanKingdom);
                if(Globals.MordorKingdom != null) 
                    MomentumCampaignBehavior.Instance.WarOfTheRingdata.AddKingdom(Globals.MordorKingdom);
                if(Globals.DunlandKingdom != null) 
                    MomentumCampaignBehavior.Instance.WarOfTheRingdata.AddKingdom(Globals.DunlandKingdom);
                if(Globals.IsengardKingdom != null) 
                    MomentumCampaignBehavior.Instance.WarOfTheRingdata.AddKingdom(Globals.IsengardKingdom);
                InquiryData data = new("The War of the Ring begins", new TextObject($"{CampaignTime.Now} the forces for mordor began their march to conquer gondor, The war for the fate of the middle earth has begun").ToString(), true, false, new TextObject("Continue").ToString(), "", () => {}, () => {});
                InformationManager.ShowInquiry(data, true, false);
            }),
            new WarEvent("rhun_erebor_dale_war", (kingdom1, kingdom2) => // rhun attacks erebor/dale
            {
                return kingdom1.StringId == Globals.RhunKingdom?.StringId && (kingdom2.StringId == Globals.EreborKingdom?.StringId || kingdom2.StringId == Globals.DaleKingdom?.StringId);
            },
            () =>
            {
                FactionManager.DeclareWar(Globals.RhunKingdom, Globals.DaleKingdom);
                FactionManager.DeclareWar(Globals.RhunKingdom, Globals.EreborKingdom);
                InquiryData data = new("The War in the east", new TextObject($"The allied kingdom of Dale and Erebor has been attack by the forces of Rhun. Can there be no peace in these times?").ToString(), true, false, new TextObject("Continue").ToString(), "", () => {}, () => {});
                InformationManager.ShowInquiry(data, true, false);
            },
            () =>
            {
                if (Globals.DaleKingdom != null)
                    MomentumCampaignBehavior.Instance.WarOfTheRingdata.AddKingdom(Globals.DaleKingdom);
                if (Globals.EreborKingdom != null)
                    MomentumCampaignBehavior.Instance.WarOfTheRingdata.AddKingdom(Globals.EreborKingdom);
                if (Globals.RhunKingdom != null)
                    MomentumCampaignBehavior.Instance.WarOfTheRingdata.AddKingdom(Globals.RhunKingdom);
                InquiryData data = new("The War in the east escalates", new TextObject($"As the war between Rhun and Dale-Erebor drags on, one thing becomes clear, this is not a disjoined attack! The Rhun are working with mordor to undermine the factions of order!").ToString(), true, false, new TextObject("The war expands").ToString(), "", () => {}, () => {});
                InformationManager.ShowInquiry(data, true, false);
            }, 14),

            new WarEvent("umbar_harad_khand_gondor_war", (kingdom1, kingdom2) => // umbar harad khand - gondor war
            {
                return (kingdom1.StringId == Globals.UmbarKingdom?.StringId || kingdom1.StringId == Globals.HaradKingdom?.StringId || kingdom1.StringId == Globals.KhandKingdom?.StringId) && kingdom2.StringId == Globals.GondorKingdom?.StringId;
            },
            () =>
            {
                FactionManager.DeclareWar(Globals.UmbarKingdom, Globals.GondorKingdom);
                FactionManager.DeclareWar(Globals.HaradKingdom, Globals.GondorKingdom);
                FactionManager.DeclareWar(Globals.KhandKingdom, Globals.GondorKingdom);
                FactionManager.DeclareWar(Globals.UmbarKingdom, Globals.RohanKingdom);
                FactionManager.DeclareWar(Globals.HaradKingdom, Globals.RohanKingdom);
                FactionManager.DeclareWar(Globals.KhandKingdom, Globals.RohanKingdom);
                InquiryData data = new("The War in the south", new TextObject($"The raids into the Gondorian territory from the south escalate, The kingdoms or umbar, harad, and khand have found common ground against their enemy!").ToString(), true, false, new TextObject("Continue").ToString(), "", () => {}, () => {});
                InformationManager.ShowInquiry(data, true, false);
            },
            () =>
            {
                if (Globals.UmbarKingdom != null)
                    MomentumCampaignBehavior.Instance.WarOfTheRingdata.AddKingdom(Globals.UmbarKingdom);
                if (Globals.HaradKingdom != null)
                    MomentumCampaignBehavior.Instance.WarOfTheRingdata.AddKingdom(Globals.HaradKingdom);
                if (Globals.KhandKingdom != null)
                    MomentumCampaignBehavior.Instance.WarOfTheRingdata.AddKingdom(Globals.KhandKingdom);
                InquiryData data = new("The War in the south escalates", new TextObject($"This is more, than raids from the south... This is a full invasion of Gondor!").ToString(), true, false, new TextObject("The war expands").ToString(), "", () => {}, () => {});
                InformationManager.ShowInquiry(data, true, false);
            }, 14),

            new WarEvent("dol_guldur_gundabad_mirkwood_lorien_war", (kingdom1, kingdom2) =>
            {
                return (kingdom1.StringId == Globals.DolGuldurKingdom?.StringId || kingdom1.StringId == Globals.GundabadKingdom?.StringId) && (kingdom2.StringId == Globals.MirkwoodKingdom?.StringId || kingdom2.StringId == Globals.LorienKingdom?.StringId);
            },
            () =>
            {
                FactionManager.DeclareWar(Globals.DolGuldurKingdom, Globals.LorienKingdom);
                FactionManager.DeclareWar(Globals.DolGuldurKingdom, Globals.MirkwoodKingdom);
                FactionManager.DeclareWar(Globals.GundabadKingdom, Globals.LorienKingdom);
                FactionManager.DeclareWar(Globals.GundabadKingdom, Globals.MirkwoodKingdom);
                InquiryData data = new("The orctide", new TextObject($"An untold number of orks began their march towards the elfrealms of Mirkwood and Lorien, Fight or die!").ToString(), true, false, new TextObject("Continue").ToString(), "", () => {}, () => {});
                InformationManager.ShowInquiry(data, true, false);
            },
            () =>
            {
                if (Globals.DolGuldurKingdom != null)
                    MomentumCampaignBehavior.Instance.WarOfTheRingdata.AddKingdom(Globals.DolGuldurKingdom);
                if (Globals.GundabadKingdom != null)
                    MomentumCampaignBehavior.Instance.WarOfTheRingdata.AddKingdom(Globals.GundabadKingdom);
                if (Globals.LorienKingdom != null)
                    MomentumCampaignBehavior.Instance.WarOfTheRingdata.AddKingdom(Globals.LorienKingdom);
                if (Globals.MirkwoodKingdom != null)
                    MomentumCampaignBehavior.Instance.WarOfTheRingdata.AddKingdom(Globals.MirkwoodKingdom);
                InquiryData data = new("The Elven war", new TextObject($"There can be no hope of wait out the conflict, The elves need to join the global alliance, or die alone!").ToString(), true, false, new TextObject("The war expands").ToString(), "", () => {}, () => {});
                InformationManager.ShowInquiry(data, true, false);
            }, 14)
        };
    }

    public class DelayedDiplomaticEvent
    {
        [SaveableField(0)] public string StringId;
        [SaveableField(1)] public CampaignTime delayedActionTime;
        public DelayedDiplomaticEvent(string stringId, CampaignTime delayedActionTime)
        {
            StringId = stringId;
            this.delayedActionTime = delayedActionTime;
        }
    }
    public class IsengardTextEvent
    {
        public string Title { get; }
        public string Text { get; }
        public string ContinueText { get; }
        public float ThresholdToHappen { get; }
        public IsengardTextEvent(string title, string text, string continueText, float time)
        {
            Title = title;
            Text = text;
            ContinueText = continueText;
            ThresholdToHappen = time;
        }
        public static List<IsengardTextEvent> All = new()
        {
            new IsengardTextEvent("June 3018 TA", "Whispers from the Shire speak of old Gandalf the Grey visiting Hobbiton, his face grim with purpose. Strange business for a wizard in such quiet lands.", "continue", 0.9f),
            new IsengardTextEvent("July", "Rangers in the North speak of shadowed figures skulking near the Shire. The Dúnedain watch the roads with wary eyes.", "continue", 0.8f),
            new IsengardTextEvent("August", "Rumors drift from Isengard that the White Wizard has barred his gates. None have seen Gandalf since he rode south.", "continue", 0.7f),
            new IsengardTextEvent("September", "Tales from Bree tell of black-cloaked riders on dark steeds, asking after hobbits. Fear grips the hearts of men in the taverns", "continue", 0.6f),
            new IsengardTextEvent("October", "Horsemen of Rohan whisper of orc bands prowling the Westfold. The king’s hall is heavy with foreboding.", "continue", 0.5f),
            new IsengardTextEvent("November", "You hear rumblings that trees are falling in Fangorn Deep. The forest groans with an ancient anger.", "continue", 0.4f),
            new IsengardTextEvent("January 3019 TA", "The Fords of Isen have mysteriously dried up. Travelers speak of strange workings in the shadow of Isengard", "continue", 0.3f),
            new IsengardTextEvent("April", "The Wild men of Dunland are seen gathering near Isengard’s walls. Rohan’s riders sharpen their spears", "continue", 0.2f),
            new IsengardTextEvent("May", "In Gondor, the beacons stand ready to blaze. Men speak of a darkness gathering beyond the Anduin", "continue", 0.1f)
        };
    }
    public class AoMDiplomacy : CampaignBehaviorBase
    {
        public WarOfTheRingData WarOfTheRingdata => MomentumCampaignBehavior.Instance.WarOfTheRingdata;
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
                    WarEvent warEvent = WarDeclaredEvents.First(e => e.StringId == eventId);
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
                "rohan_attack_dunland",
                "gondor_attack_mordor"
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
                InquiryData data = new(new TextObject(textEvent.Title).ToString(), new TextObject(textEvent.Text).ToString(), true, false, new TextObject(textEvent.ContinueText).ToString(), "", () => { }, () => { });
                InformationManager.ShowInquiry(data, true, false);
            }
            if (timeTill <= 0)
                StartIsengardWar();
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
            if (AoMSettings.Instance.BalanceOfPower) kingdoms = Kingdom.All.Where(k => k.Culture.IsEvilCulture()).ToList();
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
                if (MomentumCampaignBehavior.Instance.WarOfTheRingdata.HasWarStarted && CampaignTime.Now > delayedEvents[i].delayedActionTime)
                {
                    WarEvent? warEvent = WarEvent.AllEvents.FirstOrDefault(e => e.StringId == delayedEvents[i].StringId);
                    warEvent?.DelayedAction?.Invoke();
                    delayedEvents.RemoveAt(i);
                }
            }
            TryForceStartWar();
        }

        private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
        {
            EvilFactionsDaysWithoutWar[faction1.StringId] = 0;
            if (!AoMSettings.Instance.BalanceOfPower) return;
            if (faction1 is not Kingdom kingdom1 || faction2 is not Kingdom kingdom2) return;

            //if (!MomentumCampaignBehavior.Instance.WarOfTheRingdata.HasWarStarted()) return;
            for (int i = WarDeclaredEvents.Count - 1; i >= 0; i--)
            {
                var warEvent = WarDeclaredEvents[i];
                if (warEvent.isConditionMet(kingdom1, kingdom2))
                {
                    warEvent.Action();
                    WarDeclaredEvents.RemoveAt(i);
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