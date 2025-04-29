using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace LOTRAOM
{
    public static class RaceManager
    {
        // Dictionary to map race IDs to race string identifiers
        public static Dictionary<int, string> RaceIdToStringMap { get; private set; } = new();

        // Initialize all races when the game starts
        public static void InitializeRaces()
        {
            RaceIdToStringMap.Clear();

            List<string> everyRace = new() {
            "human",
            "dwarf",
            "uruk_hai",
            "berserker",
            "uruk",
            "orc",
            "nazghul",
            "cave_troll",
            "hill_troll"
        };

            foreach (string race in everyRace)
            {
                // Explicitly reference TaleWorlds.Core.FaceGen to resolve ambiguity
                int raceId = TaleWorlds.Core.FaceGen.GetRaceOrDefault(race);
                RaceIdToStringMap.Add(raceId, race);
            }
        }

        // Get racial data for a specific character
        public static RaceBonus GetRacialData(BasicCharacterObject character)
        {
            RaceIdToStringMap.TryGetValue(character.Race, out string? value);
            value ??= "human";
            return RaceBonuses[value];
        }

        // Apply race-specific damage bonuses when a character is hit
        public static void ApplyRaceBonusWhenGotHit(BasicCharacterObject character, DamageType damage, ref float returnDamage)
        {
            RaceBonus bonus = GetRacialData(character);
            switch (damage)
            {
                case DamageType.Ranged:
                    returnDamage -= bonus.RangedResistance;
                    break;
                case DamageType.Melee:
                    returnDamage -= bonus.MeleeResistance;
                    break;
            }
        }
        // Apply race-specific damage bonuses when a character deals damage
        public static void ApplyRaceBonusWhenDealingDamage(BasicCharacterObject attacker, DamageType damage, ref float dealtDamage)
        {
            RaceBonus bonus = GetRacialData(attacker);
            switch (damage)
            {
                case DamageType.Ranged:
                    dealtDamage *= (1f + bonus.RangedDamageBonus); // Apply ranged damage bonus
                    break;
                case DamageType.Melee:
                    dealtDamage *= (1f + bonus.MeleeDamageBonus); // Apply melee damage bonus
                    break;
            }
        }

        // Determine the damage type from a weapon
        public static DamageType GetDefaultDamage(MissionWeapon missionWeapon)
        {
            if (missionWeapon.CurrentUsageItem == null)
                return DamageType.Other;

            return missionWeapon.CurrentUsageItem.WeaponClass switch
            {
                WeaponClass.Arrow or WeaponClass.Bolt or
                WeaponClass.Javelin or WeaponClass.ThrowingAxe or
                WeaponClass.ThrowingKnife or WeaponClass.Stone => DamageType.Ranged,

                _ => DamageType.Melee, // Standard melee weapon
            };
        }

        // Enum to categorize damage types
        public enum DamageType
        {
            Melee,
            Ranged,
            Other // Fall damage, kick, other
        }

       

        // Class to store race-specific damage resistances/bonuses
        public class RaceBonus
        {
            public int MeleeResistance { get; }
            public int RangedResistance { get; }
            public float MeleeDamageBonus { get; } // Percentage increase to melee damage (e.g., 0.2f = +20%)
            public float RangedDamageBonus { get; } // Percentage increase to ranged damage (e.g., 0.1f = +10%)

            public RaceBonus(int meleeResistance, int rangedResistance, float meleeDamageBonus, float rangedDamageBonus)
            {
                MeleeResistance = meleeResistance;
                RangedResistance = rangedResistance;
                MeleeDamageBonus = meleeDamageBonus;
                RangedDamageBonus = rangedDamageBonus;
            }
        }

        // Dictionary of race bonuses for different races
        public static Dictionary<string, RaceBonus> RaceBonuses { get; } = new()
        {
            // Humans have no special bonuses (baseline)
            ["human"] = new RaceBonus(0, 0, 0f, 0f),

            // Dwarves excel in melee combat, slight ranged bonus
            ["dwarf"] = new RaceBonus(2, 1, 0.15f, 0.05f), // +15% melee, +5% ranged

            // Uruk-hai are strong melee fighters
            ["uruk_hai"] = new RaceBonus(0, 2, 0.1f, 0f), // +10% melee, 0% ranged

            // Berserkers deal high melee damage, no ranged bonus
            ["berserker"] = new RaceBonus(3, -1, 0.2f, 0f), // +20% melee, 0% ranged

            // Regular uruks have minor melee bonus
            ["uruk"] = new RaceBonus(1, 1, 0.05f, 0f), // +5% melee, 0% ranged

            // Orcs have no special damage bonuses
            ["orc"] = new RaceBonus(0, 0, 0f, 0f),

            // Nazgûl deal high damage in both melee and ranged
            ["nazghul"] = new RaceBonus(5, 5, 0.3f, 0.3f), // +30% melee, +30% ranged

            // Cave trolls deal massive melee damage
            ["cave_troll"] = new RaceBonus(2, 3, 0.5f, 0f), // +50% melee, 0% ranged

            // Hill trolls deal high melee damage
            ["hill_troll"] = new RaceBonus(2, 3, 0.4f, 0f) // +40% melee, 0% ranged
        };
        // Apply race-specific damage bonuses when a character deals damage
        
    }


    }