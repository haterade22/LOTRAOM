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
            if (settlement.StringId == "town_SWAN_ISENGARD1")
                return Math.Max(baseChance, 0.3f); // Orthanc: 30% elite
            if (settlement.Culture.StringId == "gondor")
                return Math.Max(baseChance, 0.25f); // Gondor: 25% elite
            return baseChance;
        }

        public override ExplainedNumber CalculateMilitiaChange(Settlement settlement, bool includeDescriptions = false)
        {
            ExplainedNumber value = _previousModel.CalculateMilitiaChange(settlement, includeDescriptions);
            if (settlement.OwnerClan.Culture.HasFeat(LOTRAOMCultureFeats.Instance.humanPlusMilitiaProduction))
            {
                value.Add(LOTRAOMCultureFeats.Instance.humanPlusMilitiaProduction.EffectBonus, new("{=human_militia}Human culture militia bonus"), null);
            }
            // Orthanc: Maintain ~800 militia
            if (settlement.StringId == "town_SWAN_ISENGARD1")
            {
                float targetMilitia = 800f;
                float currentMilitia = settlement.Militia;
                if (currentMilitia < targetMilitia)
                {
                    value.Add(Math.Min(10f, targetMilitia - currentMilitia), new("{=orthanc_militia_growth}Orthanc war machine"), null);
                }
                else if (currentMilitia > targetMilitia)
                {
                    value.Add(targetMilitia - currentMilitia, new("{=orthanc_militia_cap}Orthanc militia cap"), null);
                }
            }
            return value;
        }

        public override void CalculateMilitiaSpawnRate(Settlement settlement, out float meleeTroopRate, out float rangedTroopRate)
        {
            if (settlement.StringId == "town_SWAN_ISENGARD1")
            {
                meleeTroopRate = 0.75f; // Orthanc: 75% melee
                rangedTroopRate = 0.25f;
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
                return 200; // Orthanc: 200 Uruk-hai post-siege
            return _previousModel.MilitiaToSpawnAfterSiege(town);
        }

        public void AssignMilitiaTroops(Settlement settlement, MobileParty militiaParty, int militiaToAdd)
        {
            if (Globals.IsNewCampaignCreating || militiaParty.PartyTemplate != settlement.Culture.MilitiaPartyTemplate)
            {
                foreach (TroopRosterElement item in militiaParty.MemberRoster.GetTroopRoster())
                {
                    militiaParty.MemberRoster.RemoveTroop(item.Character, item.Number);
                }
            }

            if (settlement.StringId == "town_SWAN_ISENGARD1" && settlement.OwnerClan.StringId == "clan_isengard_1")
            {
                militiaToAdd = Math.Max(0, 800 - militiaParty.MemberRoster.TotalManCount);
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

            AddTroopToMilitiaParty(settlement, militiaParty, meleeBase, meleeElite, troopRatio, ref militiaToAdd);
            AddTroopToMilitiaParty(settlement, militiaParty, rangedBase, rangedElite, 1f - troopRatio, ref militiaToAdd);
        }

        private static void AddTroopToMilitiaParty(Settlement settlement, MobileParty militiaParty, CharacterObject militiaTroop, CharacterObject eliteMilitiaTroop, float troopRatio, ref int numberToAddRemaining)
        {
            if (numberToAddRemaining > 0)
            {
                int num = MBRandom.RoundRandomized(troopRatio * numberToAddRemaining);
                float eliteChance = Campaign.Current.Models.SettlementMilitiaModel.CalculateEliteMilitiaSpawnChance(settlement);
                for (int i = 0; i < num; i++)
                {
                    if (MBRandom.RandomFloat < eliteChance)
                    {
                        militiaParty.MemberRoster.AddToCounts(eliteMilitiaTroop, 1, false, 0, 0, true, -1);
                    }
                    else
                    {
                        militiaParty.MemberRoster.AddToCounts(militiaTroop, 1, false, 0, 0, true, -1);
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
            // Mordor
            ["clan_empire_south_1"] = new MilitiaData("mordor_uruk_vanguard", "mordor_uruk_baraddurguard", "mordor_uruk_archer", "mordor_uruk_marksman"),
            ["clan_empire_south_2"] = new MilitiaData("mordor_uruk_vanguard", "mordor_uruk_baraddurguard", "mordor_uruk_archer", "mordor_uruk_marksman"),
            // Gondor
            ["clan_empire_west_1"] = new MilitiaData("gondor_veteran_swordsman", "gondor_fountain_guard_spearman", "gondor_bowman", "gondor_archer"),
            ["clan_empire_west_2"] = new MilitiaData("gondor_da_knightsergeant", "gondor_da_knightcommander", "gondor_bowman", "gondor_archer"),
            // Rohan
            ["clan_vlandia_1"] = new MilitiaData("rohan_veteran_axeman", "rohan_helmingas_axeman", "rohan_skirmisher", "rohan_bowman"),
            // Harad
            ["clan_aserai_1"] = new MilitiaData("harad_footman", "harad_champion", "harad_marksman", "harad_serpent_eye"),
            // Khand
            ["clan_battania_1"] = new MilitiaData("khand_warrior", "khand_elite_warrior", "khand_bowman", "khand_marksman"),
            // Rhun
            ["clan_khuzait_1"] = new MilitiaData("easterling_veteran_swordsman", "easterling_chosen", "easterling_marksman", "easterling_eye"),
            // Dale
            ["clan_sturgia_1"] = new MilitiaData("dale_spearman", "dale_veteran_spearman", "dale_bowman", "dale_marksman"),
            // Dunland
            ["clan_empire_north_1"] = new MilitiaData("dunland_heavy_infantry", "dunland_elite_axemen", "dunland_light_archer", "dunland_medium_archer"),
            // Erebor
            ["clan_erebor_1"] = new MilitiaData("erebor_veteran_shield_guard", "erebor_gate_warden", "erebor_archer", "erebor_veteran_archer"),
            // Rivendell
            ["clan_rivendell_1"] = new MilitiaData("rivendell_swordguard", "rivendell_swords", "rivendell_marksman", "rivendell_ohtarion"),
            ["clan_rivendell_2"] = new MilitiaData("rivendell_swordguard", "rivendell_swords", "rivendell_marksman", "rivendell_ohtarion"),
            // Mirkwood
            ["clan_mirkwood_1"] = new MilitiaData("mirkwood_guards", "mirkwood_palaceguard", "mirkwood_sentinels", "mirkwood_thingolheir"),
            // Lothlorien
            ["clan_lothlorien_1"] = new MilitiaData("lothlorien_swordguard", "lothlorien_galathrim", "lothlorien_archer", "lothlorien_marchwarden"),
            // Isengard
            ["clan_isengard_1"] = new MilitiaData("orthanc_warden", "orthanc_bodyguard", "urukhai_archer", "urukhai_veterancrossbowman"),
            ["clan_isengard_2"] = new MilitiaData("orthanc_warden", "orthanc_bodyguard", "urukhai_archer", "urukhai_veterancrossbowman"),
            ["clan_isengard_3"] = new MilitiaData("orthanc_warden", "orthanc_bodyguard", "urukhai_archer", "urukhai_veterancrossbowman"),
            ["clan_isengard_4"] = new MilitiaData("orthanc_warden", "orthanc_bodyguard", "urukhai_archer", "urukhai_veterancrossbowman"),
            // Gundabad
            ["clan_gundabad_1"] = new MilitiaData("gundabad_orc_warrior", "gundabad_orc_champion", "gundabad_orc_archer", "gundabad_warg_rider"),
            // Dol Guldur
            ["clan_dolguldur_1"] = new MilitiaData("dolguldur_orc_warrior", "dolguldur_orc_elite", "dolguldur_orc_archer", "dolguldur_spider_rider"),
            // Umbar
            ["clan_umbar_1"] = new MilitiaData("umbar_corsair", "umbar_corsair_elite", "umbar_bowman", "umbar_marksman")
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