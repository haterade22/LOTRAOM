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
        private static readonly List<Func<Hero, List<VolunteerChance>?>> CallHierarchy = new() { GetVolunteerFromOwnerClanStringId, GetVolunteerFromSettlementStringId, GetVolunteerFromSettlementCultureStringId };

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
        static List<VolunteerChance>? GetVolunteerFromSettlementCultureStringId(Hero notable)
        {
            FromSettlementFactionStringId.TryGetValue(notable.CurrentSettlement.MapFaction.Culture.StringId, out List<VolunteerChance>? value);
            return value;
        }

        private static readonly Dictionary<string, List<VolunteerChance>> FromSettlementStringId = new()
        {
            //["town_ES1"] = new List<VolunteerChance> { new("mordor_rhun_bloodspear", 5), new("mordor_harad_golden_fang", 1) }
            // This is set up to be Clan > Settlement > Culture. Therefore, Dol Amroth will always spawn Gondor units, even if the settlement is owned by a different clan. So this will need to be empty.
        };
        private static readonly Dictionary<string, List<VolunteerChance>> FromSettlementOwnerClanStringId = new()
        {
            //Mordor
            ["clan_empire_south_1"] = new List<VolunteerChance> { new("mordor_uruk_grunt", 1) },
            ["clan_empire_south_2"] = new List<VolunteerChance> { new("mordor_uruk_grunt", 1) },
            ["clan_empire_south_3"] = new List<VolunteerChance> { new("mordor_uruk_grunt", 1) },
            ["clan_empire_south_4"] = new List<VolunteerChance> { new("mordor_uruk_grunt", 1) },
            ["clan_empire_south_5"] = new List<VolunteerChance> { new("mordor_uruk_grunt", 1) },
            ["clan_empire_south_6"] = new List<VolunteerChance> { new("mordor_uruk_grunt", 1) },
            ["clan_empire_south_7"] = new List<VolunteerChance> { new("mordor_uruk_grunt", 1) },
            ["clan_empire_south_8"] = new List<VolunteerChance> { new("mordor_uruk_grunt", 1) },
            ["clan_empire_south_9"] = new List<VolunteerChance> { new("mordor_uruk_grunt", 1) },
            ["clan_empire_south_10"] = new List<VolunteerChance> { new("mordor_uruk_grunt", 1) },
            ["clan_empire_south_11"] = new List<VolunteerChance> { new("mordor_uruk_grunt", 1) },
            ["clan_empire_south_12"] = new List<VolunteerChance> { new("mordor_uruk_grunt", 1) },
            ["clan_empire_south_13"] = new List<VolunteerChance> { new("mordor_uruk_grunt", 1) },
            ["clan_empire_south_14"] = new List<VolunteerChance> { new("mordor_uruk_grunt", 1) },
            ["clan_empire_south_15"] = new List<VolunteerChance> { new("mordor_uruk_grunt", 1) },
            ["clan_empire_south_16"] = new List<VolunteerChance> { new("mordor_uruk_grunt", 1) },
            ["clan_empire_south_17"] = new List<VolunteerChance> { new("mordor_uruk_grunt", 1) },
            ["clan_empire_south_18"] = new List<VolunteerChance> { new("mordor_uruk_grunt", 1) },
          
            //Gondor
            ["clan_empire_west_1"] = new List<VolunteerChance> { new("gondor_militiaman", 9), new("gondor_page", 1)}, 
            ["clan_empire_west_2"] = new List<VolunteerChance> { new("gondor_da_page", 9), new("gondor_page", 1), new("gondor_belfalas_footman", 2)},       
            ["clan_empire_west_3"] = new List<VolunteerChance> { new("gondor_pelargir_sailor", 9), new("gondor_page", 1) },           
            ["clan_empire_west_4"] = new List<VolunteerChance> { new("gondor_militiaman", 9), new("gondor_page", 1) },
            ["clan_empire_west_5"] = new List<VolunteerChance> { new("gondor_lossarnach_footman", 9), new("gondor_page", 1) },
            ["clan_empire_west_6"] = new List<VolunteerChance> { new("gondor_pg_levygondor_pg_levy", 9), new("gondor_page", 1) },
            ["clan_empire_west_7"] = new List<VolunteerChance> { new("gondor_lamedon_swordsman", 9), new("gondor_page", 1) },
            ["clan_empire_west_8"] = new List<VolunteerChance> { new("gondor_militiaman", 9), new("gondor_page", 1) },
            ["clan_empire_west_9"] = new List<VolunteerChance> { new("gondor_blackroot_hunter",9), new("gondor_page", 1) },
            ["clan_empire_west_10"] = new List<VolunteerChance> { new("gondor_militiaman", 9), new("gondor_page", 1) },
            ["clan_empire_west_11"] = new List<VolunteerChance> { new("gondor_militiaman", 9), new("gondor_page", 1) },
            ["clan_empire_west_12"] = new List<VolunteerChance> { new("ranger_of_ithilien", 3), new("gondor_page", 1) },
           
            //Rohan
            ["clan_vlandia_1"] = new List<VolunteerChance> { new("rohan_peasant", 8), new("rohan_outrider", 1) },         
            ["clan_vlandia_2"] = new List<VolunteerChance> { new("rohan_peasant", 8), new("rohan_outrider", 1) },
            ["clan_vlandia_3"] = new List<VolunteerChance> { new("rohan_peasant", 8), new("rohan_outrider", 1) },
            ["clan_vlandia_4"] = new List<VolunteerChance> { new("rohan_peasant", 8), new("rohan_outrider", 1) },
            ["clan_vlandia_5"] = new List<VolunteerChance> { new("rohan_peasant", 8), new("rohan_outrider", 1) },
            ["clan_vlandia_6"] = new List<VolunteerChance> { new("rohan_peasant", 8), new("rohan_outrider", 1) },
            ["clan_vlandia_7"] = new List<VolunteerChance> { new("rohan_peasant", 8), new("rohan_outrider", 1) },
            ["clan_vlandia_8"] = new List<VolunteerChance> { new("rohan_peasant", 8), new("rohan_outrider", 1) },
            ["clan_vlandia_9"] = new List<VolunteerChance> { new("rohan_peasant", 8), new("rohan_outrider", 1) },
            ["clan_vlandia_10"] = new List<VolunteerChance> { new("rohan_peasant", 8), new("rohan_outrider", 1) },
            ["clan_vlandia_11"] = new List<VolunteerChance> { new("rohan_peasant", 8), new("rohan_outrider", 1) },
            ["clan_vlandia_12"] = new List<VolunteerChance> { new("rohan_peasant", 8), new("rohan_outrider", 1) },


            //Harad
            ["clan_aserai_1"] = new List<VolunteerChance> { new("harad_levy", 7), new("harad_noble", 1) },
            ["clan_aserai_2"] = new List<VolunteerChance> { new("harad_levy", 7), new("harad_noble", 1) },
            ["clan_aserai_3"] = new List<VolunteerChance> { new("harad_levy", 7), new("harad_noble", 1) },
            ["clan_aserai_4"] = new List<VolunteerChance> { new("harad_levy", 7), new("harad_noble", 1) },
            ["clan_aserai_5"] = new List<VolunteerChance> { new("harad_levy", 7), new("harad_noble", 1) },
            ["clan_aserai_6"] = new List<VolunteerChance> { new("harad_levy", 7), new("harad_noble", 1) },
            ["clan_aserai_7"] = new List<VolunteerChance> { new("harad_levy", 7), new("harad_noble", 1) },
            ["clan_aserai_8"] = new List<VolunteerChance> { new("harad_levy", 7), new("harad_noble", 1) },           
            ["clan_aserai_9"] = new List<VolunteerChance> { new("harad_levy", 7), new("harad_noble", 1) },


            //Khand
            ["clan_battania_1"] = new List<VolunteerChance> { new("easterling_tribesman", 7), new("rhun_noble_youth", 1) },
            ["clan_battania_2"] = new List<VolunteerChance> { new("easterling_tribesman", 7), new("rhun_noble_youth", 1) },
            ["clan_battania_3"] = new List<VolunteerChance> { new("easterling_tribesman", 7), new("rhun_noble_youth", 1) },          
            ["clan_battania_4"] = new List<VolunteerChance> { new("easterling_tribesman", 7), new("rhun_noble_youth", 1) },
            ["clan_battania_5"] = new List<VolunteerChance> { new("easterling_tribesman", 7), new("rhun_noble_youth", 1) },
            ["clan_battania_6"] = new List<VolunteerChance> { new("easterling_tribesman", 7), new("rhun_noble_youth", 1) },
            ["clan_battania_7"] = new List<VolunteerChance> { new("easterling_tribesman", 7), new("rhun_noble_youth", 1) },
            ["clan_battania_8"] = new List<VolunteerChance> { new("easterling_tribesman", 7), new("rhun_noble_youth", 1) },


            //Rhun
            ["clan_khuzait_1"] = new List<VolunteerChance> { new("easterling_tribesman", 7), new("rhun_noble_youth", 1) },
            ["clan_khuzait_2"] = new List<VolunteerChance> { new("easterling_tribesman", 7), new("rhun_noble_youth", 1) },
            ["clan_khuzait_3"] = new List<VolunteerChance> { new("easterling_tribesman", 7), new("rhun_noble_youth", 1) },         
            ["clan_khuzait_4"] = new List<VolunteerChance> { new("easterling_tribesman", 7), new("rhun_noble_youth", 1) },
            ["clan_khuzait_5"] = new List<VolunteerChance> { new("easterling_tribesman", 7), new("rhun_noble_youth", 1) },
            ["clan_khuzait_6"] = new List<VolunteerChance> { new("easterling_tribesman", 7), new("rhun_noble_youth", 1) },
            ["clan_khuzait_7"] = new List<VolunteerChance> { new("easterling_tribesman", 7), new("rhun_noble_youth", 1) },
            ["clan_khuzait_8"] = new List<VolunteerChance> { new("easterling_tribesman", 7), new("rhun_noble_youth", 1) },
            ["clan_khuzait_9"] = new List<VolunteerChance> { new("easterling_tribesman", 7), new("rhun_noble_youth", 1) },


            //Dale
            ["clan_sturgia_1"] = new List<VolunteerChance> { new("erebor_recruit", 1) },
            ["clan_sturgia_2"] = new List<VolunteerChance> { new("erebor_recruit", 1) },
            ["clan_sturgia_3"] = new List<VolunteerChance> { new("erebor_recruit", 1) },
            ["clan_sturgia_4"] = new List<VolunteerChance> { new("erebor_recruit", 1) },
            ["clan_sturgia_5"] = new List<VolunteerChance> { new("erebor_recruit", 1) },
            ["clan_sturgia_6"] = new List<VolunteerChance> { new("erebor_recruit", 1) },
            ["clan_sturgia_7"] = new List<VolunteerChance> { new("erebor_recruit", 1) },
            ["clan_sturgia_8"] = new List<VolunteerChance> { new("erebor_recruit", 1) },
            ["clan_sturgia_9"] = new List<VolunteerChance> { new("erebor_recruit", 1) },
            
            //Dunland
            ["clan_empire_north_1"] = new List<VolunteerChance> { new("dunland_peasant", 7), new("wulf_raider", 1) },        
            ["clan_empire_north_2"] = new List<VolunteerChance> { new("dunland_peasant", 7), new("wulf_raider", 1) },
            ["clan_empire_north_3"] = new List<VolunteerChance> { new("dunland_peasant", 7), new("wulf_raider", 1) },
            ["clan_empire_north_4"] = new List<VolunteerChance> { new("dunland_peasant", 7), new("wulf_raider", 1) },
            ["clan_empire_north_5"] = new List<VolunteerChance> { new("dunland_peasant", 7), new("wulf_raider", 1) },           
            ["clan_empire_north_6"] = new List<VolunteerChance> { new("dunland_peasant", 7), new("wulf_raider", 1) },
            ["clan_empire_north_7"] = new List<VolunteerChance> { new("dunland_peasant", 7), new("wulf_raider", 1) },
            ["clan_empire_north_8"] = new List<VolunteerChance> { new("dunland_peasant", 7), new("wulf_raider", 1) },           
            ["clan_empire_north_9"] = new List<VolunteerChance> { new("dunland_peasant", 7), new("wulf_raider", 1) },
            
            //Erebor
            ["clan_erebor_1"] = new List<VolunteerChance> { new("erebor_recruit", 1) },
            ["clan_erebor_2"] = new List<VolunteerChance> { new("erebor_recruit", 1) },
            ["clan_erebor_3"] = new List<VolunteerChance> { new("erebor_recruit", 1) },
            ["clan_erebor_4"] = new List<VolunteerChance> { new("erebor_recruit", 1) },
            ["clan_erebor_5"] = new List<VolunteerChance> { new("erebor_recruit", 1) },
            ["clan_erebor_6"] = new List<VolunteerChance> { new("erebor_recruit", 1) },
            ["clan_erebor_7"] = new List<VolunteerChance> { new("erebor_recruit", 1) },
            
            //Rivendell
            ["clan_rivendell_1"] = new List<VolunteerChance> { new("rivendell_noble", 7), new("rivendell_recruit", 3) },           
            ["clan_rivendell_2"] = new List<VolunteerChance> { new("rivendell_noble", 7), new("rivendell_recruit", 3) },

            //Mirkwood
            ["clan_mirkwood_1"] = new List<VolunteerChance> { new("rivendell_noble", 7), new("rivendell_recruit", 3) },

            //Lothlorien
            ["clan_lothlorien_1"] = new List<VolunteerChance> { new("rivendell_noble", 7), new("rivendell_recruit", 3) },

            //isengard
            ["clan_isengard_1"] = new List<VolunteerChance> { new("urukhai_recruit", 8), new("orthanc_chosen", 1) },         
            
            //gundabad
            ["clan_gundabad_1"] = new List<VolunteerChance> { new("mordor_uruk_grunt", 1) },
            
            //dolguldur
            ["clan_dolguldur_1"] = new List<VolunteerChance> { new("mordor_uruk_grunt", 1) },
            
            //umbar
            ["clan_umbar_1"] = new List<VolunteerChance> { new("mordor_uruk_grunt", 1) }
        };
        private static readonly Dictionary<string, List<VolunteerChance>> FromSettlementFactionStringId = new()
        {
            ["mordor"] = new List<VolunteerChance> { new("mordor_uruk_grunt", 3)},
            ["gondor"] = new List<VolunteerChance> { new("gondor_levyman", 3), new("gondor_page", 1) },
            ["erebor"] = new List<VolunteerChance> { new("erebor_recruit", 3)},
            ["rivendell"] = new List<VolunteerChance> { new("rivendell_recruit", 3), new("rivendell_noble", 1) },
            ["mirkwood"] = new List<VolunteerChance> { new("rivendell_recruit", 3), new("rivendell_noble", 1) },
            ["lothlorien"] = new List<VolunteerChance> { new("rivendell_recruit", 3), new("rivendell_noble", 1) },
            ["umbar"] = new List<VolunteerChance> { new("mordor_uruk_grunt", 3)},
            ["isengard"] = new List<VolunteerChance> { new("urukhai_recruit", 3), new("orthanc_chosen", 1) },
            ["gundabad"] = new List<VolunteerChance> { new("mordor_uruk_grunt", 3)},
            ["dolguldur"] = new List<VolunteerChance> { new("mordor_uruk_grunt", 3) },
            ["empire"] = new List<VolunteerChance> { new("dunland_peasant", 3), new("wulf_raider", 1) },
            ["aserai"] = new List<VolunteerChance> { new("harad_levy", 3), new("harad_noble", 1) },
            ["sturgia"] = new List<VolunteerChance> { new("erebor_recruit", 3)},
            ["vlandia"] = new List<VolunteerChance> { new("rohan_peasant", 3), new("rohan_outrider", 1) },
            ["battania"] = new List<VolunteerChance> { new("harad_levy", 3), new("harad_noble", 1) },
            ["khuzait"] = new List<VolunteerChance> { new("easterling_tribesman", 3), new("rhun_noble_youth", 1) },
           
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
