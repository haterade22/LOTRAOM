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

        // Class to store race-specific damage resistances
        public class RaceBonus
        {
            public int MeleeResistance { get; }
            public int RangedResistance { get; }

            public RaceBonus(int meleeResistance, int rangedResistance)
            {
                MeleeResistance = meleeResistance;
                RangedResistance = rangedResistance;
            }
        }

        // Dictionary of race bonuses for different races
        public static Dictionary<string, RaceBonus> RaceBonuses { get; } = new()
        {
            // Humans have no special resistances (baseline)
            ["human"] = new RaceBonus(0, 0),

            // Dwarves are resistant to damage
            ["dwarf"] = new RaceBonus(2, 1),

            // Uruk-hai are tough against ranged attacks
            ["uruk_hai"] = new RaceBonus(0, 2),

            // Berserkers have high melee resistance but vulnerability to arrows
            ["berserker"] = new RaceBonus(3, -1),

            // Regular uruks
            ["uruk"] = new RaceBonus(1, 1),

            // Orcs
            ["orc"] = new RaceBonus(0, 0),

            // Nazgûl have extreme resistances
            ["nazghul"] = new RaceBonus(5, 5)
        };
    }
}