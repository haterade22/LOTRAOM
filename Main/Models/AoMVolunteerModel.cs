using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.ObjectSystem;

namespace LOTRAOM.Models
{
    public class AOMVolunteerModel : VolunteerModel
    {
        readonly VolunteerModel defaultModel;
        readonly Random random;
        public AOMVolunteerModel(VolunteerModel baseModel)
        {
            defaultModel = baseModel;
            random = new Random();
        }
        public override int MaxVolunteerTier => defaultModel.MaxVolunteerTier;
        public override bool CanHaveRecruits(Hero hero)
        {
            return defaultModel.CanHaveRecruits(hero);
        }
        public override float GetDailyVolunteerProductionProbability(Hero hero, int index, Settlement settlement)
        {
            return defaultModel.GetDailyVolunteerProductionProbability(hero, index, settlement);
        }
        public override int MaximumIndexHeroCanRecruitFromHero(Hero buyerHero, Hero sellerHero, int useValueAsRelation = -101)
        {
            return defaultModel.MaximumIndexHeroCanRecruitFromHero(buyerHero, sellerHero, useValueAsRelation);
        }
        public override CharacterObject GetBasicVolunteer(Hero hero)
        {
            foreach (Func<Hero, List<VolunteerChance>?> func in CallHierarchy)
            {
                List<VolunteerChance>? value = func(hero);
                if (value == null) continue;

                int total = value.Sum(x => x.Probability);
                int roll = random.Next(total);
                foreach (VolunteerChance volunteer in value)
                {
                    if (roll < volunteer.Probability)
                        return MBObjectManager.Instance.GetObject<CharacterObject>(volunteer.CharacterId);
                    roll -= volunteer.Probability;
                }
            }
            return defaultModel.GetBasicVolunteer(hero);
        }
        private static readonly List<Func<Hero, List<VolunteerChance>?>> CallHierarchy = new() { GetVolunteerFromOwnerClanStringId, GetVolunteerFromSettlementStringId, GetVolunteerFromSettlementFactionStringId };

        static List<VolunteerChance>? GetVolunteerFromSettlementStringId(Hero notable)
        {
            FromSettlementStringId.TryGetValue(notable.CurrentSettlement.StringId, out List<VolunteerChance>? value);
            return value;
        }
        static List<VolunteerChance>? GetVolunteerFromOwnerClanStringId(Hero notable)
        {
            FromSettlementOwnerClanStringId.TryGetValue(notable.CurrentSettlement.OwnerClan.StringId, out List<VolunteerChance>? value);
            return value;
        }
        static List<VolunteerChance>? GetVolunteerFromSettlementFactionStringId(Hero notable)
        {
            FromSettlementFactionStringId.TryGetValue(notable.CurrentSettlement.MapFaction.StringId, out List<VolunteerChance>? value);
            return value;
        }

        private static readonly Dictionary<string, List<VolunteerChance>> FromNotableStringId = new() { };
        private static readonly Dictionary<string, List<VolunteerChance>> FromSettlementStringId = new()
        {
            ["town_ES1"] = new List<VolunteerChance> { new("mordor_rhun_bloodspear", 5), new("mordor_harad_golden_fang", 1) }
        };
        private static readonly Dictionary<string, List<VolunteerChance>> FromSettlementOwnerClanStringId = new()
        {
            ["clan_empire_south_1"] = new List<VolunteerChance> { new("looter", 1) }
        };
        private static readonly Dictionary<string, List<VolunteerChance>> FromSettlementFactionStringId = new()
        {
            ["mordor"] = new List<VolunteerChance> { new("mordor_uruk_grunt", 3), new("mordor_rhun_servant", 1) }
        };

        public class VolunteerChance
        {
            public string CharacterId { get; private set; }
            public int Probability { get; private set; }
            public VolunteerChance(string characterId, int probability)
            {
                CharacterId = characterId;
                Probability = probability;
            }
        }
    }
}
