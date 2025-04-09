using HarmonyLib;
using SandBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace LOTRAOM.Patches
{
    [HarmonyPatch(typeof(Settlement), "AddMilitiasToParty")]
    public class MilitiaPatch
    {
        public static bool Prefix(Settlement __instance, MobileParty militaParty, int militiaToAdd)
        {
            if (Globals.IsNewCampaignCreating)
            {
                foreach (TroopRosterElement item in militaParty.MemberRoster.GetTroopRoster())
                {
                    militaParty.MemberRoster.RemoveTroop(item.Character, item.Number);
                }
            }
            Campaign.Current.Models.SettlementMilitiaModel.CalculateMilitiaSpawnRate(__instance, out float troopRatio, out float num);

            CharacterObject meleeBase = __instance.Culture.MeleeMilitiaTroop;
            CharacterObject meleeElite = __instance.Culture.MeleeEliteMilitiaTroop;
            CharacterObject rangedBase = __instance.Culture.RangedMilitiaTroop;
            CharacterObject rangedElite = __instance.Culture.RangedEliteMilitiaTroop;

            foreach (Func<Settlement, MilitiaData?> func in CallHierarchy)
            {
                MilitiaData? value = func(__instance);
                if (value != null)
                {
                    meleeBase = MBObjectManager.Instance.GetObject<CharacterObject>(value.MeleeBasicCharacterId);
                    meleeElite = MBObjectManager.Instance.GetObject<CharacterObject>(value.MeleeEliteCharacterId);
                    rangedBase = MBObjectManager.Instance.GetObject<CharacterObject>(value.RangedBasicCharacterId);
                    rangedElite = MBObjectManager.Instance.GetObject<CharacterObject>(value.RangedEliteCharacterId);
                }
            }
            AddTroopToMilitiaParty(__instance, militaParty, meleeBase, meleeElite, troopRatio, ref militiaToAdd);
            AddTroopToMilitiaParty(__instance, militaParty, rangedBase, rangedElite, 1f, ref militiaToAdd);
            return false;
        }
        private static void AddTroopToMilitiaParty(Settlement settlement, MobileParty militaParty, CharacterObject militiaTroop, CharacterObject eliteMilitiaTroop, float troopRatio, ref int numberToAddRemaining)
        {
            if (numberToAddRemaining > 0)
            {
                int num = MBRandom.RoundRandomized(troopRatio * (float)numberToAddRemaining);
                float num2 = Campaign.Current.Models.SettlementMilitiaModel.CalculateEliteMilitiaSpawnChance(settlement);
                for (int i = 0; i < num; i++)
                {
                    if (MBRandom.RandomFloat < num2)
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

        private static readonly List<Func<Settlement, MilitiaData?>> CallHierarchy = new() { GetVolunteerFromClanStringId};

        static MilitiaData? GetVolunteerFromClanStringId(Settlement settlement)
        {
            FromSettlementOwnerClanStringId.TryGetValue(settlement.OwnerClan.StringId, out MilitiaData? value);
            return value;
        }
        private static readonly Dictionary<string, MilitiaData> FromSettlementOwnerClanStringId = new()
        {
            ["clan_empire_south_1"] = new MilitiaData("mordor_uruk_grunt", "looter", "looter", "looter")
        };
        public class MilitiaData
        {
            public string MeleeBasicCharacterId { get; private set; }
            public string MeleeEliteCharacterId { get; private set; }
            public string RangedBasicCharacterId { get; private set; }
            public string RangedEliteCharacterId { get; private set; }

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
