using LOTRAOM.Extensions;
using LOTRAOM.Momentum;
using LOTRAOM.Momentum.Views;
using SandBox.View.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;


namespace LOTRAOM.CampaignBehaviors
{
    public class WarEvent
    {
        public Func<Kingdom, Kingdom, bool> isConditionMet;
        public Action<Kingdom, Kingdom> Action;
        public Action DelayedAction;
        CampaignTime delayedActionTime;
        public string StringId { get; }
        public WarEvent(string stringId, Func<Kingdom, Kingdom, bool> condition, Action<Kingdom, Kingdom> action, Action? delayedAction = null, CampaignTime? delayedActionTime = null)
        {
            StringId = stringId;
            isConditionMet = condition;
            Action = action;
            if (delayedActionTime != null)
                this.delayedActionTime = (CampaignTime)delayedActionTime;
        }
        public static List<WarEvent> AllEvents = new()
        {
            new WarEvent("isengard_war", (kingdom1, kingdom2) => // isengard or dunland attacks gondor or rohan
            {
                return (kingdom1.StringId == Globals.IsengardKingdom?.StringId || kingdom1.StringId == Globals.DunlandKingdom?.StringId) && (kingdom2.StringId == Globals.GondorKingdom?.StringId || kingdom2.StringId == Globals.RohanKingdom?.StringId);
            },
            (kingdom1, kingdom2) =>
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
                return !MomentumCampaignBehavior.Instance.WarOfTheRingdata.HasWarStarted() && kingdom1.StringId == Globals.MordorKingdom?.StringId && (kingdom2.StringId == Globals.RohanKingdom?.StringId || kingdom2.StringId == Globals.GondorKingdom?.StringId);
            },
            (kingdom1, kingdom2) =>
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
            (kingdom1, kingdom2) =>
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
            }, CampaignTime.WeeksFromNow(2)),

            new WarEvent("umbar_harad_khand_gondor_war", (kingdom1, kingdom2) => // umbar harad khand - gondor war
            {
                return (kingdom1.StringId == Globals.UmbarKingdom?.StringId || kingdom1.StringId == Globals.HaradKingdom?.StringId || kingdom1.StringId == Globals.KhandKingdom?.StringId) && kingdom2.StringId == Globals.GondorKingdom?.StringId;
            },
            (kingdom1, kingdom2) =>
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
            }, CampaignTime.WeeksFromNow(2)),

            new WarEvent("dol_guldur_gundabad_mirkwood_lorien_war", (kingdom1, kingdom2) =>
            {
                return (kingdom1.StringId == Globals.DolGuldurKingdom?.StringId || kingdom1.StringId == Globals.GundabadKingdom?.StringId) && (kingdom2.StringId == Globals.MirkwoodKingdom?.StringId || kingdom2.StringId == Globals.LorienKingdom?.StringId);
            },
            (kingdom1, kingdom2) =>
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
            }, CampaignTime.WeeksFromNow(2)),

            new WarEvent("rohan_attack_dunland", (kingdom1, kingdom2) =>
            {
                return (kingdom1.StringId == Globals.RohanKingdom?.StringId || kingdom1.StringId == Globals.GondorKingdom?.StringId) && (kingdom2.StringId == Globals.DunlandKingdom?.StringId);
            },
            (kingdom1, kingdom2) =>
            {
                FactionManager.DeclareWar(Globals.IsengardKingdom, Globals.RohanKingdom);
                FactionManager.DeclareWar(Globals.IsengardKingdom, Globals.GondorKingdom);
                FactionManager.DeclareWar(Globals.DunlandKingdom, Globals.DunlandKingdom);
                FactionManager.DeclareWar(Globals.DunlandKingdom, Globals.GondorKingdom);
                MomentumCampaignBehavior.Instance.hasIsengardAttacked = true;
                InquiryData data = new("The preemptive strike", new TextObject($"The council of Rohan decided for a preemptive strike against the forces of Dunland, however Saruman the white backstabbs his allies in gondor and rohan, and sends his forces to conquer the kingdoms!").ToString(), true, false, new TextObject("What more is to come?").ToString(), "", () => {}, () => {});
                InformationManager.ShowInquiry(data, true, false);
            }),
            new WarEvent("gondor_attack_mordor", (kingdom1, kingdom2) =>
            {
                return (kingdom1.StringId == Globals.GondorKingdom?.StringId || kingdom1.StringId == Globals.RohanKingdom?.StringId) && (kingdom2.StringId == Globals.MordorKingdom?.StringId);
            },
            (kingdom1, kingdom2) =>
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
                MomentumCampaignBehavior.Instance.hasIsengardAttacked = true;
                InquiryData data = new("The preemptive strike", new TextObject($"The council of Gondor decided to perform a preemptive military cmpaign against the growing darkness of Mordor, however Saruman the white backstabbs his allies in gondor and rohan, and sends his forces to conquer the kingdoms!").ToString(), true, false, new TextObject("What war of the ring begins?").ToString(), "", () => {}, () => {});
                InformationManager.ShowInquiry(data, true, false);
            })
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
    public class AoMDiplomacy : CampaignBehaviorBase
    {
        public WarOfTheRingData WarOfTheRingdata => MomentumCampaignBehavior.Instance.WarOfTheRingdata;
         public static List<WarEvent> WarDeclaredEvents = new();
        [SaveableField(0)] private static List<string> notUsedEvents = new();
        [SaveableField(1)] public static Dictionary<string, int> EvilFactionsDaysWithoutWar = new();
        [SaveableField(2)] public static List<DelayedDiplomaticEvent> delayedEvents = new(); //event id

        static readonly Dictionary<string, string> Allies = new()
        {
            ["mordor"] = "isengard",
            ["isengard"] = "mordor",
            ["gondor"] = "rohan",
            ["rohan"] = "gondor",
            ["dale"] = "erebor",
            ["erebor"] = "dale",
        };
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
        }

        private void OnDailyTick()
        {
            List<Kingdom> kingdoms;
            if (AoMSettings.Instance.BalanceOfPower)
                kingdoms = Kingdom.All.Where(k => k.Culture.IsEvilCulture()).ToList();
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
                if (CampaignTime.Now > delayedEvents[i].delayedActionTime)
                {
                    WarEvent? warEvent = WarEvent.AllEvents.FirstOrDefault(e => e.StringId == delayedEvents[i].StringId);
                    warEvent?.DelayedAction?.Invoke();
                    delayedEvents.RemoveAt(i);
                }
            }
        }

        private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
        {
            EvilFactionsDaysWithoutWar[faction1.StringId] = 0;
            if (!AoMSettings.Instance.BalanceOfPower) return;

            //if (Allies.TryGetValue(faction2.Culture.StringId, out string? allyId) && allyId != null)
            //{
            //    Kingdom? ally = Kingdom.All.Where(k => k.Culture.StringId == allyId).FirstOrDefault();
            //    if (ally != null)
            //    {
            //        FactionManager.DeclareWar(ally, faction1);
            //        InformationManager.DisplayMessage(new(new TextObject($"{ally.Name}'s ally has been attacked, {ally.Name} declares war on {faction1}").ToString()));
            //    }
            //}
            if (faction1 is not Kingdom kingdom1 || faction2 is not Kingdom kingdom2) return;

            //if (!MomentumCampaignBehavior.Instance.WarOfTheRingdata.HasWarStarted()) return;
            for (int i = WarDeclaredEvents.Count - 1; i >= 0; i--)
            {
                var warEvent = WarDeclaredEvents[i];
                if (warEvent.isConditionMet(kingdom1, kingdom2))
                {
                    warEvent.Action(kingdom1, kingdom2);
                    WarDeclaredEvents.RemoveAt(i);
                }
            }
        }
        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("evilFactionsDaysWithoutWar", ref EvilFactionsDaysWithoutWar);
            dataStore.SyncData("notUsedEvents", ref notUsedEvents);
            dataStore.SyncData("delayedEvents", ref delayedEvents);
        }
    }
}