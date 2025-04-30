using LOTRAOM.Momentum;
using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;


namespace LOTRAOM.Events
{
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
                return !MomentumCampaignBehavior.Instance.warOfTheRingData.HasWarStarted && kingdom1.StringId == Globals.MordorKingdom?.StringId && (kingdom2.StringId == Globals.RohanKingdom?.StringId || kingdom2.StringId == Globals.GondorKingdom?.StringId);
            },
            () =>
            {
                if (Globals.GondorKingdom != null)
                    MomentumCampaignBehavior.Instance.warOfTheRingData.AddKingdom(Globals.GondorKingdom);
                if (Globals.RohanKingdom != null)
                    MomentumCampaignBehavior.Instance.warOfTheRingData.AddKingdom(Globals.RohanKingdom);
                if(Globals.MordorKingdom != null)
                    MomentumCampaignBehavior.Instance.warOfTheRingData.AddKingdom(Globals.MordorKingdom);
                if(Globals.DunlandKingdom != null)
                    MomentumCampaignBehavior.Instance.warOfTheRingData.AddKingdom(Globals.DunlandKingdom);
                if(Globals.IsengardKingdom != null)
                    MomentumCampaignBehavior.Instance.warOfTheRingData.AddKingdom(Globals.IsengardKingdom);
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
                    MomentumCampaignBehavior.Instance.warOfTheRingData.AddKingdom(Globals.DaleKingdom);
                if (Globals.EreborKingdom != null)
                    MomentumCampaignBehavior.Instance.warOfTheRingData.AddKingdom(Globals.EreborKingdom);
                if (Globals.RhunKingdom != null)
                    MomentumCampaignBehavior.Instance.warOfTheRingData.AddKingdom(Globals.RhunKingdom);
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
                    MomentumCampaignBehavior.Instance.warOfTheRingData.AddKingdom(Globals.UmbarKingdom);
                if (Globals.HaradKingdom != null)
                    MomentumCampaignBehavior.Instance.warOfTheRingData.AddKingdom(Globals.HaradKingdom);
                if (Globals.KhandKingdom != null)
                    MomentumCampaignBehavior.Instance.warOfTheRingData.AddKingdom(Globals.KhandKingdom);
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
                    MomentumCampaignBehavior.Instance.warOfTheRingData.AddKingdom(Globals.DolGuldurKingdom);
                if (Globals.GundabadKingdom != null)
                    MomentumCampaignBehavior.Instance.warOfTheRingData.AddKingdom(Globals.GundabadKingdom);
                if (Globals.LorienKingdom != null)
                    MomentumCampaignBehavior.Instance.warOfTheRingData.AddKingdom(Globals.LorienKingdom);
                if (Globals.MirkwoodKingdom != null)
                    MomentumCampaignBehavior.Instance.warOfTheRingData.AddKingdom(Globals.MirkwoodKingdom);
                InquiryData data = new("The Elven war", new TextObject($"There can be no hope of wait out the conflict, The elves need to join the global alliance, or die alone!").ToString(), true, false, new TextObject("The war expands").ToString(), "", () => {}, () => {});
                InformationManager.ShowInquiry(data, true, false);
            }, 14),

            new WarEvent("rivendell_joins_war", (kingdom1, kingdom2) =>
            {
                var data = MomentumCampaignBehavior.Instance.warOfTheRingData;
                return data.HasWarStarted && data.Momentum <= -40;
            },
            () =>
            {
                if (Globals.RivendellKingdom != null)
                    MomentumCampaignBehavior.Instance.warOfTheRingData.AddKingdom(Globals.RivendellKingdom);
                InquiryData data = new("The council", new TextObject($"The elfrealm of Rivendell cannot stay idle, lest they witness the end of civilization as we know it.").ToString(), true, false, new TextObject("Continue").ToString(), "", () => {}, () => {});
                InformationManager.ShowInquiry(data, true, false);
            },
            () =>
            {
                if (Globals.DolGuldurKingdom != null)
                    MomentumCampaignBehavior.Instance.warOfTheRingData.AddKingdom(Globals.DolGuldurKingdom);
                if (Globals.GundabadKingdom != null)
                    MomentumCampaignBehavior.Instance.warOfTheRingData.AddKingdom(Globals.GundabadKingdom);
                if (Globals.LorienKingdom != null)
                    MomentumCampaignBehavior.Instance.warOfTheRingData.AddKingdom(Globals.LorienKingdom);
                if (Globals.MirkwoodKingdom != null)
                    MomentumCampaignBehavior.Instance.warOfTheRingData.AddKingdom(Globals.MirkwoodKingdom);
                InquiryData data = new("The Elven war", new TextObject($"There can be no hope of wait out the conflict, The elves need to join the global alliance, or die alone!").ToString(), true, false, new TextObject("The war expands").ToString(), "", () => {}, () => {});
                InformationManager.ShowInquiry(data, true, false);
            }, 14)
        };
    }
}