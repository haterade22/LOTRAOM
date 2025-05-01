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
                InquiryData data = new("The Shadow of Betryal", $"In the waning days of the Third Age, on this day in {CampaignTime.Now}, a shadow fell upon the free peoples of Middle-earth. Saruman the White, once chief of the Istari and head of the White Council, has forsaken his charge as a guardian of peace. Swayed by ambition and the whispers of a darker power, he has rallied the wild Dunlending tribes to his banner, proclaiming war upon the allied realms of Gondor and Rohan. The horns of battle sound, and the fragile peace of this age shatters. A storm gathers, and the fate of Men hangs in the balance.", true, false, new TextObject("A New Age Dawns").ToString(), "", () => {}, () => {});
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
                InquiryData data = new("The Shadow Stirs in Mordor", new TextObject($"On this day in {{CampaignTime.Now}}, the black gates of Mordor creak open, and the hosts of the Enemy pour forth. From the ash-choked plains of Gorgoroth, Orcs, Trolls, and fell beasts march under the banner of the Lidless Eye, their sights set on the white walls of Minas Tirith. The free peoples of Middle-earth tremble, for the war to decide the fate of all has begun.\"").ToString(), true, false, new TextObject("The Age of Men Hangs in the Balance").ToString(), "", () => {}, () => {});
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
                InquiryData data = new("The East grows Bold", new TextObject($"Whispers reach your ears from the distant East, where the shadow of Sauron stirs unrest. The fierce Easterlings, clad in gold and scarlet, have crossed the borders of Rhûn to harry the stalwart Kingdoms of Dale and Erebor. Villages burn, and the Men of Dale stand shield-to-shoulder with the Dwarves of the Lonely Mountain. The war for Middle-earth’s heart kindles in the shadow of the Dragon’s hoard.").ToString(), true, false, new TextObject("The North Must Hold").ToString(), "", () => {}, () => {});
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
                InquiryData data = new("The East Falls Under Shadow", new TextObject($"The raids upon Dale and Erebor grow ever fiercer, and a grim truth dawns upon the North. These are no mere skirmishes of scattered Easterling tribes. From the shadowed lands of Rhûn, the Men of the East march in lockstep with the will of Mordor, their banners bearing the mark of the Lidless Eye. The Lonely Mountain trembles, and the Men of Dale falter, for the war in the East now serves a darker purpose.").ToString(), true, false, new TextObject("The Shadow Spreads").ToString(), "", () => {}, () => {});
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
                InquiryData data = new("The South Rises in Wrath", new TextObject($"From the sun-scorched deserts and shadowed havens of the South, a storm gathers against Gondor. The Corsairs of Umbar, the fierce Haradrim, and the warlords of Khand have forged a dire alliance, bound by the will of the Lidless Eye. Their raids upon the borderlands of Ithilien and Anórien grow bold, and the White Tower stands defiant but beleaguered. The war for Middle-earth’s soul now burns in the South.").ToString(), true, false, new TextObject("Gondor Stands Alone").ToString(), "", () => {}, () => {});
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
                InquiryData data = new("The South Unleashes War", new TextObject($"The southern skies darken as the hosts of Umbar, Harad, and Khand surge across Gondor’s borders. No longer content with raids, their armies—borne by Corsair ships, led by oliphaunts, and driven by the Lidless Eye’s command—now wage open war. From Pelargir to the fields of Lebennin, the Men of Gondor stand resolute, yet the tide of invasion threatens to drown the White Tower’s light.").ToString(), true, false, new TextObject("The War for Gondor Rages").ToString(), "", () => {}, () => {});
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
                InquiryData data = new("The Orctide Rises", new TextObject($"From the shadowed depths of Dol Guldur, a vast horde of Orcs spills forth, their crude blades gleaming with malice. Like a black tide, they sweep toward the ancient woods of Mirkwood and the golden boughs of Lothlórien, driven by the will of the Dark Lord. The Elves of Thranduil and Galadriel stand resolute, their bows strung and hearts unyielding, yet the hour is dire. Fight, or see the light of the Eldar fade.").ToString(), true, false, new TextObject("The Elves Must Endure").ToString(), "", () => {}, () => {});
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
                InquiryData data = new("The Twilight of the Eldar", new TextObject($"The ancient realms of Mirkwood and Lothlórien stand besieged, their glades ringing with the clamor of Orcish blades. The Elves, keepers of Middle-earth’s oldest songs, can no longer hold aloof from the gathering storm. Thranduil and Galadriel must join the fleeting hopes of Men and Dwarves in a grand alliance, or face the shadow alone, their light extinguished in the Third Age’s twilight").ToString(), true, false, new TextObject("The Fate of the Free Peoples").ToString(), "", () => {}, () => {});
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
                InquiryData data = new("The council", new TextObject($"Rivendell cannot stay idle, lest they witness the end of civilization as we know it.").ToString(), true, false, new TextObject("Continue").ToString(), "", () => {}, () => {});
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
                InquiryData data = new("The Council of Rivendell", new TextObject($"In the hidden valley of Imladris, where the Bruinen sings and ancient wisdom endures, the Last Homely House can no longer stand apart. The shadow of Sauron lengthens, threatening to unmake all that the Free Peoples hold dear. Lord Elrond, master of Rivendell, must summon a council to forge a path against the Dark Lord, lest Middle-earth fall into an age of endless night.").ToString(), true, false, new TextObject("Go Forth and Clap Some Cheeks").ToString(), "", () => {}, () => {});
                InformationManager.ShowInquiry(data, true, false);
            }, 14)
        };
    }
}