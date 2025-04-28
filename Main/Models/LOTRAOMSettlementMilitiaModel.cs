using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;
using LOTRAOM.CultureFeats;
using System.Collections.Generic;
using System;
using TaleWorlds.CampaignSystem.Roster;

namespace LOTRAOM.Models
{
    public class LOTRAOMSettlementMilitiaModel : SettlementMilitiaModel
    {
        private readonly SettlementMilitiaModel _previousModel;

        public LOTRAOMSettlementMilitiaModel(SettlementMilitiaModel baseModel)
        {
            _previousModel = baseModel;
        }

        public override float CalculateEliteMilitiaSpawnChance(Settlement settlement)
        {
            float baseChance = _previousModel.CalculateEliteMilitiaSpawnChance(settlement);

            // Specific settlements with custom elite rates
            if (settlement.StringId == "town_SWAN_ISENGARD1")
                return Math.Max(baseChance, 0.3f); // Orthanc: 30% elite
            if (settlement.StringId == "town_ES1") // Minas Tirith
                return Math.Max(baseChance, 0.4f); // Minas Tirith: 40% elite - higher than normal Gondor

            // Culture-based elite rates
            if (settlement.Culture.StringId == "gondor")
                return Math.Max(baseChance, 0.25f); // Gondor: 25% elite

            return baseChance;
        }

        public override ExplainedNumber CalculateMilitiaChange(Settlement settlement, bool includeDescriptions = false)
        {
            ExplainedNumber value = _previousModel.CalculateMilitiaChange(settlement, includeDescriptions);

            // Cultural militia bonuses
            if (settlement.OwnerClan.Culture.HasFeat(LOTRAOMCultureFeats.Instance.humanPlusMilitiaProduction))
            {
                value.Add(LOTRAOMCultureFeats.Instance.humanPlusMilitiaProduction.EffectBonus, new("{=human_militia}Human culture militia bonus"), null);
            }

            // Orthanc: Maintain ~800 militia
            if (settlement.StringId == "town_SWAN_ISENGARD1")
            {
                float targetMilitia = 750f;
                float currentMilitia = settlement.Militia;
                if (currentMilitia < targetMilitia)
                {
                    value.Add(Math.Min(12f, targetMilitia - currentMilitia), new("{=orthanc_militia_growth}Orthanc War Machine"), null);
                }
                else if (currentMilitia > targetMilitia)
                {
                    value.Add(targetMilitia - currentMilitia, new("{=orthanc_militia_cap}Orthanc militia cap"), null);
                }
            }
            // Minas Tirith: Maintain ~1000 militia
            else if (settlement.StringId == "town_ES1") // Minas Tirith
            {
                float targetMilitia = 1000f;
                float currentMilitia = settlement.Militia;
                if (currentMilitia < targetMilitia)
                {
                    value.Add(Math.Min(15f, targetMilitia - currentMilitia), new("{=minas_tirith_militia_growth}Citadel of the White Tower"), null);
                }
                else if (currentMilitia > targetMilitia)
                {
                    value.Add(targetMilitia - currentMilitia, new("{=minas_tirith_militia_cap}Minas Tirith militia cap"), null);
                }
            }

            return value;
        }

        public override void CalculateMilitiaSpawnRate(Settlement settlement, out float meleeTroopRate, out float rangedTroopRate)
        {
            if (settlement.StringId == "town_SWAN_ISENGARD1")
            {
                meleeTroopRate = 0.55f; // Orthanc: 55% melee
                rangedTroopRate = 0.45f;
            }
            else if (settlement.StringId == "town_ES1") // Minas Tirith
            {
                meleeTroopRate = 0.35f; // Minas Tirith: Heavy focus on archers
                rangedTroopRate = 0.65f;
            }
            else if (settlement.Culture.StringId == "gondor")
            {
                meleeTroopRate = 0.4f; // Gondor: Favor archers
                rangedTroopRate = 0.6f;
            }
            else if (settlement.Culture.StringId == "rohan")
            {
                meleeTroopRate = 0.6f; // Rohan: Favor melee/mounted
                rangedTroopRate = 0.4f;
            }
            else
            {
                meleeTroopRate = 0.5f;
                rangedTroopRate = 0.5f;
            }
        }

        public override int MilitiaToSpawnAfterSiege(Town town)
        {
            if (town.Settlement.StringId == "town_SWAN_ISENGARD1")
                return 300; // Orthanc: 300 Uruk-hai post-siege
            if (town.Settlement.StringId == "town_ES1") // Minas Tirith
                return 400; // Minas Tirith: 400 soldiers post-siege
            return _previousModel.MilitiaToSpawnAfterSiege(town);
        }

        // Custom method to assign militia troops - not part of base class but used by our Harmony patch
        public void AssignMilitiaTroops(Settlement settlement, MobileParty militaParty, int militiaToAdd)
        {
            if (Globals.IsNewCampaignCreating)
            {
                foreach (TroopRosterElement item in militaParty.MemberRoster.GetTroopRoster())
                {
                    militaParty.MemberRoster.RemoveTroop(item.Character, item.Number);
                }
            }

            // Special handling for specific settlements
            if (settlement.StringId == "town_SWAN_ISENGARD1" && settlement.Culture.StringId == "isengard")
            {
                militiaToAdd = Math.Max(0, 800 - militaParty.MemberRoster.TotalManCount);
            }
            else if (settlement.StringId == "town_EW1" && settlement.Culture.StringId == "gondor") // Minas Tirith
            {
                militiaToAdd = Math.Max(0, 1000 - militaParty.MemberRoster.TotalManCount);
            }

            CalculateMilitiaSpawnRate(settlement, out float troopRatio, out _);

            CharacterObject meleeBase = settlement.Culture.MeleeMilitiaTroop;
            CharacterObject meleeElite = settlement.Culture.MeleeEliteMilitiaTroop;
            CharacterObject rangedBase = settlement.Culture.RangedMilitiaTroop;
            CharacterObject rangedElite = settlement.Culture.RangedEliteMilitiaTroop;

            foreach (Func<Settlement, MilitiaData?> func in CallHierarchy)
            {
                MilitiaData? value = func(settlement);
                if (value != null)
                {
                    meleeBase = MBObjectManager.Instance.GetObject<CharacterObject>(value.MeleeBasicCharacterId);
                    meleeElite = MBObjectManager.Instance.GetObject<CharacterObject>(value.MeleeEliteCharacterId);
                    rangedBase = MBObjectManager.Instance.GetObject<CharacterObject>(value.RangedBasicCharacterId);
                    rangedElite = MBObjectManager.Instance.GetObject<CharacterObject>(value.RangedEliteCharacterId);
                }
            }

            AddTroopToMilitiaParty(settlement, militaParty, meleeBase, meleeElite, troopRatio, ref militiaToAdd);
            AddTroopToMilitiaParty(settlement, militaParty, rangedBase, rangedElite, 1f - troopRatio, ref militiaToAdd);
        }

        private static void AddTroopToMilitiaParty(Settlement settlement, MobileParty militaParty, CharacterObject militiaTroop, CharacterObject eliteMilitiaTroop, float troopRatio, ref int numberToAddRemaining)
        {
            if (numberToAddRemaining > 0)
            {
                int num = MBRandom.RoundRandomized(troopRatio * numberToAddRemaining);
                float eliteChance = Campaign.Current.Models.SettlementMilitiaModel.CalculateEliteMilitiaSpawnChance(settlement);
                for (int i = 0; i < num; i++)
                {
                    if (MBRandom.RandomFloat < eliteChance)
                    {
                        militaParty.MemberRoster.AddToCounts(eliteMilitiaTroop, 1, false, 0, 0, true, -1);
                    }
                    else
                    {
                        militaParty.MemberRoster.AddToCounts(militiaTroop, 1, false, 0, 0, true, -1);
                    }
                }
                numberToAddRemaining -= num;
            }
        }

        private static readonly List<Func<Settlement, MilitiaData?>> CallHierarchy = new() { GetVolunteerFromClanStringId };

        private static MilitiaData? GetVolunteerFromClanStringId(Settlement settlement)
        {
            FromSettlementOwnerClanStringId.TryGetValue(settlement.OwnerClan.StringId, out MilitiaData? value);
            return value;
        }

        private static readonly Dictionary<string, MilitiaData> FromSettlementOwnerClanStringId = new()
        {
            //Mordor
            ["clan_empire_south_1"] = new MilitiaData("mordor_uruk_vanguard", "mordor_uruk_baraddurguard", "mordor_uruk_archer", "mordor_uruk_marksman"),
            ["clan_empire_south_2"] = new MilitiaData("mordor_uruk_vanguard", "mordor_uruk_baraddurguard", "mordor_uruk_archer", "mordor_uruk_marksman"),
            ["clan_empire_south_3"] = new MilitiaData("mordor_uruk_vanguard", "mordor_uruk_baraddurguard", "mordor_uruk_archer", "mordor_uruk_marksman"),
            ["clan_empire_south_4"] = new MilitiaData("mordor_uruk_vanguard", "mordor_uruk_baraddurguard", "mordor_uruk_archer", "mordor_uruk_marksman"),
            ["clan_empire_south_5"] = new MilitiaData("mordor_uruk_vanguard", "mordor_uruk_baraddurguard", "mordor_uruk_archer", "mordor_uruk_marksman"),
            ["clan_empire_south_6"] = new MilitiaData("mordor_uruk_vanguard", "mordor_uruk_baraddurguard", "mordor_uruk_archer", "mordor_uruk_marksman"),
            ["clan_empire_south_7"] = new MilitiaData("mordor_uruk_vanguard", "mordor_uruk_baraddurguard", "mordor_uruk_archer", "mordor_uruk_marksman"),
            ["clan_empire_south_8"] = new MilitiaData("mordor_uruk_vanguard", "mordor_uruk_baraddurguard", "mordor_uruk_archer", "mordor_uruk_marksman"),
            ["clan_empire_south_9"] = new MilitiaData("mordor_uruk_vanguard", "mordor_uruk_baraddurguard", "mordor_uruk_archer", "mordor_uruk_marksman"),
            ["clan_empire_south_10"] = new MilitiaData("mordor_uruk_vanguard", "mordor_uruk_baraddurguard", "mordor_uruk_archer", "mordor_uruk_marksman"),
            ["clan_empire_south_11"] = new MilitiaData("mordor_uruk_vanguard", "mordor_uruk_baraddurguard", "mordor_uruk_archer", "mordor_uruk_marksman"),
            ["clan_empire_south_12"] = new MilitiaData("mordor_uruk_vanguard", "mordor_uruk_baraddurguard", "mordor_uruk_archer", "mordor_uruk_marksman"),
            ["clan_empire_south_13"] = new MilitiaData("mordor_uruk_vanguard", "mordor_uruk_baraddurguard", "mordor_uruk_archer", "mordor_uruk_marksman"),
            ["clan_empire_south_14"] = new MilitiaData("mordor_uruk_vanguard", "mordor_uruk_baraddurguard", "mordor_uruk_archer", "mordor_uruk_marksman"),
            ["clan_empire_south_15"] = new MilitiaData("mordor_uruk_vanguard", "mordor_uruk_baraddurguard", "mordor_uruk_archer", "mordor_uruk_marksman"),
            ["clan_empire_south_16"] = new MilitiaData("mordor_uruk_vanguard", "mordor_uruk_baraddurguard", "mordor_uruk_archer", "mordor_uruk_marksman"),
            ["clan_empire_south_17"] = new MilitiaData("mordor_uruk_vanguard", "mordor_uruk_baraddurguard", "mordor_uruk_archer", "mordor_uruk_marksman"),
            ["clan_empire_south_18"] = new MilitiaData("mordor_uruk_vanguard", "mordor_uruk_baraddurguard", "mordor_uruk_archer", "mordor_uruk_marksman"),

            //Gondor
            ["clan_empire_west_1"] = new MilitiaData("gondor_veteran_swordsman", "gondor_fountain_guard_spearman", "gondor_bowman", "gondor_archer"),
            ["clan_empire_west_2"] = new MilitiaData("gondor_da_knightsergeant", "gondor_da_knightcommander", "gondor_bowman", "gondor_archer"),
            ["clan_empire_west_3"] = new MilitiaData("gondor_pelargir_marine", "gondor_pelargir_veteran_marine", "gondor_bowman", "gondor_pelargir_archer"),
            ["clan_empire_west_4"] = new MilitiaData("gondor_veteran_swordsman", "gondor_fountain_guard_spearman", "gondor_bowman", "gondor_archer"),
            ["clan_empire_west_5"] = new MilitiaData("gondor_lossarnach_axeguard", "gondor_lossarnach_wrathbearers", "gondor_bowman", "gondor_archer"),
            ["clan_empire_west_6"] = new MilitiaData("gondor_pg_warrior", "gondor_pg_champion", "gondor_pg_bowguard", "gondor_pg_hillwarden"),
            ["clan_empire_west_7"] = new MilitiaData("gondor_lamedon_guard", "gondor_lamedon_noble", "gondor_bowman", "gondor_archer"),
            ["clan_empire_west_8"] = new MilitiaData("gondor_veteran_swordsman", "gondor_fountain_guard_spearman", "gondor_bowman", "gondor_archer"),
            ["clan_empire_west_9"] = new MilitiaData("gondor_veteran_swordsman", "gondor_fountain_guard_spearman", "gondor_blackroot_longbowmen", "gondor_blackroot_elite_longbowmen"),
            ["clan_empire_west_10"] = new MilitiaData("gondor_veteran_swordsman", "gondor_fountain_guard_spearman", "gondor_bowman", "gondor_archer"),
            ["clan_empire_west_11"] = new MilitiaData("gondor_veteran_swordsman", "gondor_fountain_guard_spearman", "gondor_bowman", "gondor_archer"),
            ["clan_empire_west_12"] = new MilitiaData("gondor_veteran_swordsman", "gondor_fountain_guard_spearman", "veteran_ranger_of_ithilien", "master_ranger_of_ithilien"),

            //Rohan
            ["clan_vlandia_1"] = new MilitiaData("rohan_veteran_axeman", "rohan_helmingas_axeman", "rohan_skirmisher", "rohan_bowman"),
            ["clan_vlandia_2"] = new MilitiaData("rohan_veteran_axeman", "rohan_helmingas_axeman", "rohan_skirmisher", "rohan_bowman"),
            ["clan_vlandia_3"] = new MilitiaData("rohan_veteran_axeman", "rohan_helmingas_axeman", "rohan_skirmisher", "rohan_bowman"),
            ["clan_vlandia_4"] = new MilitiaData("rohan_veteran_axeman", "rohan_helmingas_axeman", "rohan_skirmisher", "rohan_bowman"),
            ["clan_vlandia_5"] = new MilitiaData("rohan_veteran_axeman", "rohan_helmingas_axeman", "rohan_skirmisher", "rohan_bowman"),
            ["clan_vlandia_6"] = new MilitiaData("rohan_veteran_axeman", "rohan_helmingas_axeman", "rohan_skirmisher", "rohan_bowman"),
            ["clan_vlandia_7"] = new MilitiaData("rohan_veteran_axeman", "rohan_helmingas_axeman", "rohan_skirmisher", "rohan_bowman"),
            ["clan_vlandia_8"] = new MilitiaData("rohan_veteran_axeman", "rohan_helmingas_axeman", "rohan_skirmisher", "rohan_bowman"),
            ["clan_vlandia_9"] = new MilitiaData("rohan_veteran_axeman", "rohan_helmingas_axeman", "rohan_skirmisher", "rohan_bowman"),
            ["clan_vlandia_10"] = new MilitiaData("rohan_veteran_axeman", "rohan_helmingas_axeman", "rohan_skirmisher", "rohan_bowman"),
            ["clan_vlandia_11"] = new MilitiaData("rohan_veteran_axeman", "rohan_helmingas_axeman", "rohan_skirmisher", "rohan_bowman"),

            //Harad
            ["clan_aserai_1"] = new MilitiaData("harad_footman", "harad_champion", "harad_marksman", "harad_serpent_eye"),
            ["clan_aserai_2"] = new MilitiaData("harad_footman", "harad_champion", "harad_marksman", "harad_serpent_eye"),
            ["clan_aserai_3"] = new MilitiaData("harad_footman", "harad_champion", "harad_marksman", "harad_serpent_eye"),
            ["clan_aserai_4"] = new MilitiaData("harad_footman", "harad_champion", "harad_marksman", "harad_serpent_eye"),
            ["clan_aserai_5"] = new MilitiaData("harad_footman", "harad_champion", "harad_marksman", "harad_serpent_eye"),
            ["clan_aserai_6"] = new MilitiaData("harad_footman", "harad_champion", "harad_marksman", "harad_serpent_eye"),
            ["clan_aserai_7"] = new MilitiaData("harad_footman", "harad_champion", "harad_marksman", "harad_serpent_eye"),
            ["clan_aserai_8"] = new MilitiaData("harad_footman", "harad_champion", "harad_marksman", "harad_serpent_eye"),
            ["clan_aserai_9"] = new MilitiaData("harad_footman", "harad_champion", "harad_marksman", "harad_serpent_eye"),

            //Khand
            ["clan_battantia_1"] = new MilitiaData("harad_footman", "harad_champion", "harad_marksman", "harad_serpent_eye"),
            ["clan_battantia_2"] = new MilitiaData("harad_footman", "harad_champion", "harad_marksman", "harad_serpent_eye"),
            ["clan_battantia_3"] = new MilitiaData("harad_footman", "harad_champion", "harad_marksman", "harad_serpent_eye"),
            ["clan_battantia_4"] = new MilitiaData("harad_footman", "harad_champion", "harad_marksman", "harad_serpent_eye"),
            ["clan_battantia_5"] = new MilitiaData("harad_footman", "harad_champion", "harad_marksman", "harad_serpent_eye"),
            ["clan_battantia_6"] = new MilitiaData("harad_footman", "harad_champion", "harad_marksman", "harad_serpent_eye"),
            ["clan_battantia_7"] = new MilitiaData("harad_footman", "harad_champion", "harad_marksman", "harad_serpent_eye"),
            ["clan_battantia_8"] = new MilitiaData("harad_footman", "harad_champion", "harad_marksman", "harad_serpent_eye"),

            //Rhun
            ["clan_khuzait_1"] = new MilitiaData("easterling_veteran_swordsman", "easterling_chosen", "easterling_marksman", "easterling_eye"),
            ["clan_khuzait_2"] = new MilitiaData("easterling_veteran_swordsman", "easterling_chosen", "easterling_marksman", "easterling_eye"),
            ["clan_khuzait_3"] = new MilitiaData("easterling_veteran_swordsman", "easterling_chosen", "easterling_marksman", "easterling_eye"),
            ["clan_khuzait_4"] = new MilitiaData("easterling_veteran_swordsman", "easterling_chosen", "easterling_marksman", "easterling_eye"),
            ["clan_khuzait_5"] = new MilitiaData("easterling_veteran_swordsman", "easterling_chosen", "easterling_marksman", "easterling_eye"),
            ["clan_khuzait_6"] = new MilitiaData("easterling_veteran_swordsman", "easterling_chosen", "easterling_marksman", "easterling_eye"),
            ["clan_khuzait_7"] = new MilitiaData("easterling_veteran_swordsman", "easterling_chosen", "easterling_marksman", "easterling_eye"),
            ["clan_khuzait_8"] = new MilitiaData("easterling_veteran_swordsman", "easterling_chosen", "easterling_marksman", "easterling_eye"),
            ["clan_khuzait_9"] = new MilitiaData("easterling_veteran_swordsman", "easterling_chosen", "easterling_marksman", "easterling_eye"),

            //Dale
            ["clan_sturgia_1"] = new MilitiaData("gondor_veteran_swordsman", "gondor_fountain_guard_spearman", "gondor_bowman", "gondor_archer"),
            ["clan_sturgia_2"] = new MilitiaData("gondor_veteran_swordsman", "gondor_fountain_guard_spearman", "gondor_bowman", "gondor_archer"),
            ["clan_sturgia_3"] = new MilitiaData("gondor_veteran_swordsman", "gondor_fountain_guard_spearman", "gondor_bowman", "gondor_archer"),
            ["clan_sturgia_4"] = new MilitiaData("gondor_veteran_swordsman", "gondor_fountain_guard_spearman", "gondor_bowman", "gondor_archer"),
            ["clan_sturgia_5"] = new MilitiaData("gondor_veteran_swordsman", "gondor_fountain_guard_spearman", "gondor_bowman", "gondor_archer"),
            ["clan_sturgia_6"] = new MilitiaData("gondor_veteran_swordsman", "gondor_fountain_guard_spearman", "gondor_bowman", "gondor_archer"),
            ["clan_sturgia_7"] = new MilitiaData("gondor_veteran_swordsman", "gondor_fountain_guard_spearman", "gondor_bowman", "gondor_archer"),
            ["clan_sturgia_8"] = new MilitiaData("gondor_veteran_swordsman", "gondor_fountain_guard_spearman", "gondor_bowman", "gondor_archer"),
            ["clan_sturgia_9"] = new MilitiaData("gondor_veteran_swordsman", "gondor_fountain_guard_spearman", "gondor_bowman", "gondor_archer"),

            //Dunland
            ["clan_empire_north_1"] = new MilitiaData("dunland_heavy_infantry", "dunland_elite_axemen", "dunland_light_archer", "dunland_medium_archer"),
            ["clan_empire_north_2"] = new MilitiaData("dunland_heavy_infantry", "dunland_elite_axemen", "dunland_light_archer", "dunland_medium_archer"),
            ["clan_empire_north_3"] = new MilitiaData("dunland_heavy_infantry", "dunland_elite_axemen", "dunland_light_archer", "dunland_medium_archer"),
            ["clan_empire_north_4"] = new MilitiaData("dunland_heavy_infantry", "dunland_elite_axemen", "dunland_light_archer", "dunland_medium_archer"),
            ["clan_empire_north_5"] = new MilitiaData("dunland_heavy_infantry", "dunland_elite_axemen", "dunland_light_archer", "dunland_medium_archer"),
            ["clan_empire_north_6"] = new MilitiaData("dunland_heavy_infantry", "dunland_elite_axemen", "dunland_light_archer", "dunland_medium_archer"),
            ["clan_empire_north_7"] = new MilitiaData("dunland_heavy_infantry", "dunland_elite_axemen", "dunland_light_archer", "dunland_medium_archer"),
            ["clan_empire_north_8"] = new MilitiaData("dunland_heavy_infantry", "dunland_elite_axemen", "dunland_light_archer", "dunland_medium_archer"),
            ["clan_empire_north_9"] = new MilitiaData("dunland_heavy_infantry", "dunland_elite_axemen", "dunland_light_archer", "dunland_medium_archer"),

            //Erebor
            ["clan_erebor_1"] = new MilitiaData("erebor_veteran_shield_guard", "erebor_gate_warden", "erebor_archer", "erebor_veteran_archer"),
            ["clan_erebor_2"] = new MilitiaData("erebor_veteran_shield_guard", "erebor_gate_warden", "erebor_archer", "erebor_veteran_archer"),
            ["clan_erebor_3"] = new MilitiaData("erebor_veteran_shield_guard", "erebor_gate_warden", "erebor_archer", "erebor_veteran_archer"),
            ["clan_erebor_4"] = new MilitiaData("erebor_veteran_shield_guard", "erebor_gate_warden", "erebor_archer", "erebor_veteran_archer"),
            ["clan_erebor_5"] = new MilitiaData("erebor_veteran_shield_guard", "erebor_gate_warden", "erebor_archer", "erebor_veteran_archer"),
            ["clan_erebor_6"] = new MilitiaData("erebor_veteran_shield_guard", "erebor_gate_warden", "erebor_archer", "erebor_veteran_archer"),
            ["clan_erebor_7"] = new MilitiaData("erebor_veteran_shield_guard", "erebor_gate_warden", "erebor_archer", "erebor_veteran_archer"),

            //Rivendell
            ["clan_rivendell_1"] = new MilitiaData("rivendell_swordguard", "rivendell_swords", "rivendell_marksman", "rivendell_ohtarion"),
            ["clan_rivendell_2"] = new MilitiaData("rivendell_swordguard", "rivendell_swords", "rivendell_marksman", "rivendell_ohtarion"),

            //Mirkwood
            ["clan_mirkwood_1"] = new MilitiaData("mirkwood_guards", "mirkwood_palaceguard", "mirkwood_sentinels", "mirkwood_thingolheir"),

            //Lothlorien
            ["clan_lothlorien_1"] = new MilitiaData("rivendell_swordguard", "rivendell_swords", "rivendell_marksman", "rivendell_ohtarion"),

            //Isengard
            ["clan_isengard_1"] = new MilitiaData("orthanc_warden", "orthanc_bodyguard", "urukhai_archer", "urukhai_veterancrossbowman"),
            ["clan_isengard_2"] = new MilitiaData("orthanc_warden", "orthanc_bodyguard", "urukhai_archer", "urukhai_veterancrossbowman"),
            ["clan_isengard_3"] = new MilitiaData("orthanc_warden", "orthanc_bodyguard", "urukhai_archer", "urukhai_veterancrossbowman"),
            ["clan_isengard_4"] = new MilitiaData("orthanc_warden", "orthanc_bodyguard", "urukhai_archer", "urukhai_veterancrossbowman"),

            //Gundabad
            ["clan_gundabad_1"] = new MilitiaData("mordor_uruk_vanguard", "mordor_uruk_baraddurguard", "mordor_uruk_archer", "mordor_uruk_marksman"),

            //Dol Guldur
            ["clan_dolguldur_1"] = new MilitiaData("mordor_uruk_vanguard", "mordor_uruk_baraddurguard", "mordor_uruk_archer", "mordor_uruk_marksman"),

            //Umbar
            ["clan_umbar_1"] = new MilitiaData("harad_footman", "harad_champion", "harad_marksman", "harad_serpent_eye")
        };

        public class MilitiaData
        {
            public string MeleeBasicCharacterId { get; }
            public string MeleeEliteCharacterId { get; }
            public string RangedBasicCharacterId { get; }
            public string RangedEliteCharacterId { get; }

            public MilitiaData(string meleeBasicCharacterId, string meleeEliteCharacterId, string rangedBasicCharacterId, string rangedEliteCharacterId)
            {
                MeleeBasicCharacterId = meleeBasicCharacterId;
                MeleeEliteCharacterId = meleeEliteCharacterId;
                RangedBasicCharacterId = rangedBasicCharacterId;
                RangedEliteCharacterId = rangedEliteCharacterId;
            }
        }
    }
}