using LOTRAOM.Extensions;
using LOTRAOM.Momentum;
using LOTRAOM.Momentum.Views;
using SandBox.View.Map;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace LOTRAOM.CampaignBehaviors
{
    public class AoMDiplomacy : CampaignBehaviorBase
    {
        public WarOfTheRingData WarOfTheRingdata => MomentumCampaignBehavior.Instance.WarOfTheRingdata;
        [SaveableField(1)] public static Dictionary<string, int> EvilFactionsDaysWithoutWar = new();
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
            CampaignEvents.TickEvent.AddNonSerializedListener(this, AddUIElements);
            CampaignEvents.WarDeclared.AddNonSerializedListener(this, OnWarDeclared);
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, OnDailyTick);
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
        }

        private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
        {
            EvilFactionsDaysWithoutWar[faction1.StringId] = 0;
            if (!AoMSettings.Instance.BalanceOfPower) return;

            Kingdom? mordor = Kingdom.All.Where(k => k.Culture.StringId == Globals.MordorCulture).FirstOrDefault();
            Kingdom? isengard = Kingdom.All.Where(k => k.Culture.StringId == Globals.IsengardCulture).FirstOrDefault();
            Kingdom? rohan = Kingdom.All.Where(k => k.Culture.StringId == Globals.RohanCulture).FirstOrDefault();
            Kingdom? gondor = Kingdom.All.Where(k => k.Culture.StringId == Globals.Gondorculture).FirstOrDefault();
            if (mordor == null || isengard == null || rohan == null || gondor == null) return;

            if (!WarOfTheRingdata.HasWarStarted() && (faction1 == mordor && faction2 == gondor || faction1 == gondor && faction2 == mordor))
            {
                WarOfTheRingdata.GoodKingdoms.Kingdoms.Add(gondor);
                WarOfTheRingdata.GoodKingdoms.Kingdoms.Add(rohan);
                WarOfTheRingdata.EvilKingdoms.Kingdoms.Add(mordor);
                WarOfTheRingdata.EvilKingdoms.Kingdoms.Add(isengard);
                FactionManager.DeclareWar(isengard, rohan);
                FactionManager.DeclareWar(isengard, gondor);
                FactionManager.DeclareWar(mordor, gondor);
                FactionManager.DeclareWar(mordor, rohan);
                var time = CampaignTime.Now;
                InformationManager.DisplayMessage(new(new TextObject($"{time} marked the beginning of a test of Gondor Rohan alliance, as the countries have been assaulted by the foul forces of Mordor and Isengard").ToString()));
            }
            if (Allies.TryGetValue(faction2.Culture.StringId, out string allyId))
            {
                Kingdom? ally = Kingdom.All.Where(k => k.Culture.StringId == allyId).FirstOrDefault();
                if (ally != null)
                {
                    FactionManager.DeclareWar(ally, faction1);
                    InformationManager.DisplayMessage(new(new TextObject($"{ally.Name}'s ally has been attacked, {ally.Name} declares war on {faction1}").ToString()));
                }
            }
        }
        private void AddUIElements(float obj)
        {
            if (AoMSettings.Instance.BalanceOfPower)
                MapScreen.Instance.AddMapView<MomentumIndicator>();
            CampaignEvents.TickEvent.ClearListeners(this);
        }
        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("evilFactionsDaysWithoutWar", ref EvilFactionsDaysWithoutWar);
        }
    }
}