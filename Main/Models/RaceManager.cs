using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
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

            List<string> everyRace = new()
            {
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
        public static void ApplyRaceBonusWhenGotHit(BasicCharacterObject character, DamageType damage, ref ExplainedNumber damageValue, TextObject description)
        {
            if (character == null || description == null)
                return;

            RaceBonus bonus = GetRacialData(character);
            switch (damage)
            {
                case DamageType.Ranged:
                    if (bonus.RangedResistance != 0)
                    {
                        damageValue.Add(-bonus.RangedResistance, description);
                    }
                    break;
                case DamageType.Melee:
                    if (bonus.MeleeResistance != 0)
                    {
                        damageValue.Add(-bonus.MeleeResistance, description);
                    }
                    break;
            }
        }

        // Apply race-specific damage bonuses when a character deals damage
        public static void ApplyRaceBonusWhenDealingDamage(BasicCharacterObject attacker, DamageType damage, ref ExplainedNumber dealtDamage, TextObject description)
        {
            if (attacker == null || description == null)
                return;

            RaceBonus bonus = GetRacialData(attacker);
            switch (damage)
            {
                case DamageType.Ranged:
                    if (!bonus.RangedDamageBonus.ApproximatelyEqualsTo(0f, 1E-05f))
                    {
                        dealtDamage.AddFactor(bonus.RangedDamageBonus, description); // Apply ranged damage bonus
                    }
                    break;
                case DamageType.Melee:
                    if (!bonus.MeleeDamageBonus.ApproximatelyEqualsTo(0f, 1E-05f))
                    {
                        dealtDamage.AddFactor(bonus.MeleeDamageBonus, description); // Apply melee damage bonus
                    }
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
            public float MeleeDamageBonus { get; }
            public float RangedDamageBonus { get; }
            public float KnockbackResistance { get; } // Chance to resist knockback (0.0f to 1.0f)
            public float KnockdownChance { get; } // Chance to cause knockdown (0.0f to 1.0f)

            public RaceBonus(int meleeResistance, int rangedResistance, float meleeDamageBonus, float rangedDamageBonus, float knockbackResistance, float knockdownChance)
            {
                MeleeResistance = meleeResistance;
                RangedResistance = rangedResistance;
                MeleeDamageBonus = meleeDamageBonus;
                RangedDamageBonus = rangedDamageBonus;
                KnockbackResistance = knockbackResistance;
                KnockdownChance = knockdownChance;
            }
        }

        // Dictionary of race bonuses for different races
        public static Dictionary<string, RaceBonus> RaceBonuses { get; } = new()
        {
            ["human"] = new RaceBonus(0, 0, 0f, 0f, 0f, 0f),
            ["dwarf"] = new RaceBonus(2, 1, 0.15f, 0.05f, 0.5f, 0f), // 50% knockback resistance
            ["uruk_hai"] = new RaceBonus(0, 2, 0.1f, 0f, 0f, 0.3f), // 30% knockdown chance
            ["berserker"] = new RaceBonus(3, -1, 0.2f, 0f, 0f, 0.4f), // 40% knockdown chance
            ["uruk"] = new RaceBonus(1, 1, 0.05f, 0f, 0f, 0.2f), // 20% knockdown chance
            ["orc"] = new RaceBonus(0, 0, 0f, 0f, 0f, 0f),
            ["nazghul"] = new RaceBonus(5, 5, 0.3f, 0.3f, 0.2f, 0f), // 20% knockback resistance
            ["cave_troll"] = new RaceBonus(2, 3, 0.5f, 0f, 0.3f, 0.5f), // 30% knockback resistance, 50% knockdown
            ["hill_troll"] = new RaceBonus(2, 3, 0.4f, 0f, 0.3f, 0.5f) // 30% knockback resistance, 50% knockdown
        };
    }
}