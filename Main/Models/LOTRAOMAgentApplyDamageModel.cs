using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Library;
using System;
using System.Collections.Generic;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace LOTRAOM.Models
{
    // This class overrides the default AgentApplyDamageModel to apply race-specific bonuses and negatives during combat
    // It supports Dwarves, Uruks, Uruk-hai, Berserkers, and Orcs, with configurable modifiers
    public class LOTRAOMAgentApplyDamageModel : AgentApplyDamageModel
    {
        // Store the base AgentApplyDamageModel to delegate default behavior (e.g., perk calculations, banner effects)
        private readonly AgentApplyDamageModel _previousModel;

        // Configurable variables for race-specific modifiers
        // Dwarves: 20% damage reduction due to their sturdy nature (like Gimli's resilience)
        private readonly float DwarfDamageAbsorption = 0.2f;
        // Uruks: 2% increased damage taken due to their aggressive nature
        private readonly float UrukDamageIncrease = 0.02f;
        // Uruk-hai: 15% increased melee damage dealt due to their superior strength (like in the Battle of Helm's Deep)
        private readonly float UrukHaiDamageDealtIncrease = 0.15f;
        // Berserkers: 20% increased melee damage dealt, but 5% increased damage taken due to their reckless fury
        private readonly float BerserkerDamageDealtIncrease = 0.25f;
        private readonly float BerserkerDamageTakenIncrease = 0.05f;
        // Orcs: 5% increased damage taken due to their weaker nature compared to Uruks
        private readonly float OrcDamageIncrease = 0.05f;
        // Elves: 20% increased ranged damage and improved accuracy (like Legolas' precision)
        private readonly float ElfRangedDamageIncrease = 0.2f;
        private readonly float ElfRangedDamageVarianceReduction = 0.1f; // Reduces damage variance for more consistent hits

        // Dictionary to map Character StringIds to races
        // Updated with troop IDs from troops_gondor.xml, troops_erebor.xml, troops_mordor.xml, and troops_isengard.xml
        private readonly Dictionary<string, string> _characterToRaceMap = new()
{
// Gondor troops (humans)
{ "gondor_levyman", "human" },
{ "gondor_militiaman", "human" },
{ "gondor_footman", "human" },
{ "gondor_bowman", "human" },
{ "gondor_archer", "human" },
{ "gondor_swordsman", "human" },
{ "gondor_spearman", "human" },
{ "gondor_veteran_spearman", "human" },
{ "gondor_fountain_guard_spearman", "human" },
{ "gondor_veteran_swordsman", "human" },
{ "gondor_tower_guard_swordsman", "human" },
{ "gondor_scout", "human" },
{ "gondor_knight", "human" },
{ "gondor_page", "human" },
{ "gondor_belfalas_footman", "human" },
{ "gondor_belfalas_pikemen", "human" },
{ "gondor_belfalas_guard", "human" },
{ "gondor_belfalas_elite_pikemen", "human" },
{ "gondor_blackroot_hunter", "human" },
{ "gondor_blackroot_skirmisher", "human" },
{ "gondor_blackroot_archer", "human" },
{ "gondor_blackroot_longbowmen", "human" },
{ "gondor_blackroot_elite_longbowmen", "human" },
{ "gondor_pg_levy", "human" },
{ "gondor_pg_footman", "human" },
{ "gondor_pg_warrior", "human" },
{ "gondor_pg_champion", "human" },
{ "gondor_pg_skirmisher", "human" },
{ "gondor_pg_bowguard", "human" },
{ "gondor_pg_hillwarden", "human" },
{ "gondor_pg_scout", "human" },
{ "gondor_pg_horseman", "human" },
{ "gondor_da_page", "human" },
{ "gondor_da_squire", "human" },
{ "gondor_da_footknight", "human" },
{ "gondor_da_knightsergeant", "human" },
{ "gondor_da_knightcommander", "human" },

// Erebor troops (dwarves)
{ "erebor_recruit", "dwarf" },
{ "erebor_scout", "dwarf" },
{ "erebor_archer", "dwarf" },
{ "erebor_veteran_archer", "dwarf" },
{ "erebor_warrior", "dwarf" },
{ "erebor_spear_infantry", "dwarf" },
{ "erebor_spear_guard", "dwarf" },
{ "erebor_veteran_spear_guard", "dwarf" },
{ "erebor_legionary", "dwarf" },
{ "erebor_axe_infantry", "dwarf" },
{ "erebor_axe_guard", "dwarf" },
{ "erebor_veteran_axe_guard", "dwarf" },
{ "erebor_shield_breaker", "dwarf" },
{ "erebor_shield_guard", "dwarf" },
{ "erebor_veteran_shield_guard", "dwarf" },
{ "erebor_gate_warden", "dwarf" },
{ "erebor_royal_warden", "dwarf" },
{ "erebor_royal_lord_placeholder", "dwarf" },
{ "erebor_tier9_test", "dwarf" },
{ "erebor_tier10_test", "dwarf" },

// Mordor troops (Uruks)
{ "mordor_uruk_grunt", "orc" },
{ "mordor_uruk_warrior", "uruk" },
{ "mordor_uruk_feller", "uruk" },
{ "mordor_uruk_ravager", "uruk" },
{ "mordor_uruk_executioner", "uruk" },
{ "mordor_uruk_darkblade", "uruk" },
{ "mordor_uruk_halberdier", "uruk" },
{ "mordor_uruk_reaper", "uruk" },
{ "mordor_uruk_deathwarden", "uruk" },
{ "mordor_uruk_scout", "uruk" },
{ "mordor_uruk_archer", "uruk" },
{ "mordor_uruk_bowman", "uruk" },
{ "mordor_uruk_marksman", "uruk" },
{ "mordor_uruk_sharpshooter", "uruk" },
{ "mordor_uruk_towerguard", "uruk" },
{ "mordor_uruk_baraddurguard", "uruk" },

// Mordor troops (Orcs)
{ "mordor_orc_warrior", "orc" },
{ "mordor_orc_warbrute", "orc" },
{ "mordor_orc_blackguard", "orc" },

// Mordor troops (Humans - Haradrim, Rhûn, Black Númenóreans)
{ "mordor_harad_initiate", "human" },
{ "mordor_harad_bladewielder", "human" },
{ "mordor_harad_vanguard", "human" },
{ "mordor_harad_serpent_guard", "human" },
{ "mordor_harad_golden_fang", "human" },
{ "mordor_rhun_servant", "human" },
{ "mordor_rhun_spearhand", "human" },
{ "mordor_rhun_bloodspear", "human" },
{ "mordor_rhun_bloodforged_warrior", "human" },
{ "mordor_rhun_blade_sentinel", "human" },
{ "mordor_black_numenorean", "human" },

// Isengard troops (Uruk-hai)
{ "urukhai_recruit", "urukhai" },
{ "urukhai_fighter", "urukhai" },
{ "urukhai_scout", "urukhai" },
{ "urukhai_warrior", "urukhai" },
{ "urukhai_skirmisher", "urukhai" },
{ "urukhai_feller", "urukhai" },
{ "urukhai_hunter", "urukhai" },
{ "urukhai_marauder", "urukhai" },
{ "urukhai_breaker", "urukhai" },
{ "urukhai_pikeman", "urukhai" },
{ "urukhai_crossbowman", "urukhai" },
{ "urukhai_assaulter", "urukhai" },
{ "urukhai_vanguard", "urukhai" },
{ "urukhai_reaver", "urukhai" },
{ "urukhai_crossbow_elite", "urukhai" },
{ "urukhai_destroyer", "urukhai" },
{ "orthanc_bodyguard", "urukhai" },

// Isengard troops (Berserkers)
{ "urukhai_berserker", "berserker" },
{ "urukhai_berserker_elite", "berserker" }
};

        // Constructor to initialize the base model
        public LOTRAOMAgentApplyDamageModel(AgentApplyDamageModel baseModel)
        {
            _previousModel = baseModel;
        }

        // Override CalculateDamage to apply race-specific damage modifiers after base calculations
        public override float CalculateDamage(in AttackInformation attackInformation, in AttackCollisionData collisionData, in MissionWeapon weapon, float baseDamage)
        {
            // First, calculate the base damage using the default model
            float damage = _previousModel.CalculateDamage(attackInformation, collisionData, weapon, baseDamage);

            // Apply race-specific modifiers to the victim
            if (attackInformation.VictimAgent != null)
            {
                try
                {
                    // Determine the victim's race
                    string victimRace = GetAgentRace(attackInformation.VictimAgent);

                    // Apply damage modifiers based on victim's race
                    if (victimRace == "dwarf")
                    {
                        // Dwarves take 20% less damage (sturdy like Gimli)
                        float reducedDamage = damage * (1f - DwarfDamageAbsorption);
                        TaleWorlds.Library.Debug.Print($"[LOTRAOM] CalculateDamage: Reduced damage for Dwarf victim by {DwarfDamageAbsorption * 100}% (Base={damage}, Reduced={reducedDamage}).");
                        damage = reducedDamage;
                    }
                    else if (victimRace == "uruk")
                    {
                        // Uruks take 10% more damage due to their aggressive nature
                        float increasedDamage = damage * (1f + UrukDamageIncrease);
                        TaleWorlds.Library.Debug.Print($"[LOTRAOM] CalculateDamage: Increased damage for Uruk victim by {UrukDamageIncrease * 100}% (Base={damage}, Increased={increasedDamage}).");
                        damage = increasedDamage;
                    }
                    else if (victimRace == "berserker")
                    {
                        // Berserkers take 15% more damage due to their reckless fury
                        float increasedDamage = damage * (1f + BerserkerDamageTakenIncrease);
                        TaleWorlds.Library.Debug.Print($"[LOTRAOM] CalculateDamage: Increased damage for Berserker victim by {BerserkerDamageTakenIncrease * 100}% (Base={damage}, Increased={increasedDamage}).");
                        damage = increasedDamage;
                    }
                    else if (victimRace == "orc")
                    {
                        // Orcs take 5% more damage due to their weaker nature
                        float increasedDamage = damage * (1f + OrcDamageIncrease);
                        TaleWorlds.Library.Debug.Print($"[LOTRAOM] CalculateDamage: Increased damage for Orc victim by {OrcDamageIncrease * 100}% (Base={damage}, Increased={increasedDamage}).");
                        damage = increasedDamage;
                    }
                }
                catch (Exception ex)
                {
                    TaleWorlds.Library.Debug.Print($"[LOTRAOM] CalculateDamage: Error applying race modifier - {ex.Message}\nStackTrace: {ex.StackTrace}");
                }
            }
            else
            {
                TaleWorlds.Library.Debug.Print("[LOTRAOM] CalculateDamage: VictimAgent is null. Skipping race modifier.");
            }

            // Apply race-specific modifiers to the attacker (damage dealt)
            // Apply race-specific modifiers to the attacker (damage dealt)
            if (attackInformation.AttackerAgent != null)
            {
                try
                {
                    // Determine the attacker's race
                    string attackerRace = GetAgentRace(attackInformation.AttackerAgent);

                    // Apply damage modifiers based on attacker's race
                    if (attackerRace == "urukhai")
                    {
                        // Uruk-hai deal 15% more melee damage (strong like in Helm's Deep)
                        if (!collisionData.IsMissile && !collisionData.IsAlternativeAttack)
                        {
                            float increasedDamage = damage * (1f + UrukHaiDamageDealtIncrease);
                            TaleWorlds.Library.Debug.Print($"[LOTRAOM] CalculateDamage: Increased melee damage for Uruk-hai attacker by {UrukHaiDamageDealtIncrease * 100}% (Base={damage}, Increased={increasedDamage}).");
                            damage = increasedDamage;
                        }
                    }
                    else if (attackerRace == "berserker")
                    {
                        // Berserkers deal 20% more melee damage due to their fury
                        if (!collisionData.IsMissile && !collisionData.IsAlternativeAttack)
                        {
                            float increasedDamage = damage * (1f + BerserkerDamageDealtIncrease);
                            TaleWorlds.Library.Debug.Print($"[LOTRAOM] CalculateDamage: Increased melee damage for Berserker attacker by {BerserkerDamageDealtIncrease * 100}% (Base={damage}, Increased={increasedDamage}).");
                            damage = increasedDamage;
                        }
                    }
                    else if (attackerRace == "elf")
                    {
                        // Elves deal 20% more ranged damage and have improved accuracy (like Legolas)
                        if (collisionData.IsMissile)
                        {
                            // Increase ranged damage
                            float increasedDamage = damage * (1f + ElfRangedDamageIncrease);
                            TaleWorlds.Library.Debug.Print($"[LOTRAOM] CalculateDamage: Increased ranged damage for Elf attacker by {ElfRangedDamageIncrease * 100}% (Base={damage}, Increased={increasedDamage}).");

                            // Simulate improved accuracy by reducing damage variance
                            // Assume baseDamage is the mean damage; reduce variance by scaling towards the mean
                            float varianceReduction = ElfRangedDamageVarianceReduction;
                            float adjustedDamage = increasedDamage + (baseDamage - increasedDamage) * varianceReduction;
                            TaleWorlds.Library.Debug.Print($"[LOTRAOM] CalculateDamage: Reduced ranged damage variance for Elf attacker by {varianceReduction * 100}% (Increased={increasedDamage}, Adjusted={adjustedDamage}).");
                            damage = adjustedDamage;
                        }
                    }
                }
                catch (Exception ex)
                {
                    TaleWorlds.Library.Debug.Print($"[LOTRAOM] CalculateDamage: Error applying attacker race modifier - {ex.Message}\nStackTrace: {ex.StackTrace}");
                }
            }
            else
            {
                TaleWorlds.Library.Debug.Print("[LOTRAOM] CalculateDamage: AttackerAgent is null. Skipping attacker race modifier.");
            }

            return damage;
        }

        // Helper method to determine an agent's race
        // Checks: 1) StringId mapping, 2) Monster.Id
        private string GetAgentRace(Agent agent)
        {
            // Check if the agent has a character object (NPC or player)
            if (agent == null || agent.Character == null)
            {
                TaleWorlds.Library.Debug.Print($"[LOTRAOM] GetAgentRace: Agent or Character is null. Defaulting to 'human'.");
                return "human"; // Default to human if agent is invalid
            }

            string characterId = agent.Character.StringId.ToString();

            // Step 1: Map race based on Character.StringId
            if (!string.IsNullOrEmpty(characterId) && _characterToRaceMap.TryGetValue(characterId, out string mappedRace))
            {
                TaleWorlds.Library.Debug.Print($"[LOTRAOM] GetAgentRace: Agent '{characterId}' race determined from StringId mapping as '{mappedRace}'.");
                return mappedRace.ToLower();
            }

            // Step 2: Fallback to Monster.Id to determine race (e.g., "dwarf", "uruk", "uruk_hai", "berserker", "orc")
            if (agent.Monster != null)
            {
                string monsterId = agent.Monster.Id;
                string race = monsterId switch
                {
                    "dwarf" => "dwarf",
                    "uruk" => "uruk",
                    "uruk_hai" => "urukhai",
                    "berserker" => "berserker",
                    "orc" => "orc",
                    _ => "human"
                };
                TaleWorlds.Library.Debug.Print($"[LOTRAOM] GetAgentRace: Agent '{characterId}' race determined from Monster.Id '{monsterId}' as '{race}'.");
                return race.ToLower();
            }

            // Default to human if race cannot be determined
            TaleWorlds.Library.Debug.Print($"[LOTRAOM] GetAgentRace: Could not determine race for agent '{characterId}'. Defaulting to 'human'.");
            return "human";
        }

        // Delegate other methods to the base model to maintain default behavior
        public override bool DecideCrushedThrough(Agent attackerAgent, Agent defenderAgent, float totalAttackEnergy, Agent.UsageDirection attackDirection, StrikeType strikeType, WeaponComponentData defendItem, bool isPassiveUsageHit)
        {
            return _previousModel.DecideCrushedThrough(attackerAgent, defenderAgent, totalAttackEnergy, attackDirection, strikeType, defendItem, isPassiveUsageHit);
        }

        public override void DecideMissileWeaponFlags(Agent attackerAgent, MissionWeapon missileWeapon, ref WeaponFlags missileWeaponFlags)
        {
            _previousModel.DecideMissileWeaponFlags(attackerAgent, missileWeapon, ref missileWeaponFlags);
        }

        public override bool CanWeaponIgnoreFriendlyFireChecks(WeaponComponentData weapon)
        {
            return _previousModel.CanWeaponIgnoreFriendlyFireChecks(weapon);
        }

        public override bool CanWeaponDismount(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData)
        {
            return _previousModel.CanWeaponDismount(attackerAgent, attackerWeapon, blow, collisionData);
        }

        public override void CalculateDefendedBlowStunMultipliers(Agent attackerAgent, Agent defenderAgent, CombatCollisionResult collisionResult, WeaponComponentData attackerWeapon, WeaponComponentData defenderWeapon, out float attackerStunMultiplier, out float defenderStunMultiplier)
        {
            _previousModel.CalculateDefendedBlowStunMultipliers(attackerAgent, defenderAgent, collisionResult, attackerWeapon, defenderWeapon, out attackerStunMultiplier, out defenderStunMultiplier);
        }

        public override bool CanWeaponKnockback(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData)
        {
            return _previousModel.CanWeaponKnockback(attackerAgent, attackerWeapon, blow, collisionData);
        }

        public override bool CanWeaponKnockDown(Agent attackerAgent, Agent victimAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData)
        {
            return _previousModel.CanWeaponKnockDown(attackerAgent, victimAgent, attackerWeapon, blow, collisionData);
        }

        public override float GetDismountPenetration(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData)
        {
            return _previousModel.GetDismountPenetration(attackerAgent, attackerWeapon, blow, collisionData);
        }

        public override float GetKnockBackPenetration(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData)
        {
            return _previousModel.GetKnockBackPenetration(attackerAgent, attackerWeapon, blow, collisionData);
        }

        public override float GetKnockDownPenetration(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData)
        {
            return _previousModel.GetKnockDownPenetration(attackerAgent, attackerWeapon, blow, collisionData);
        }

        public override float GetHorseChargePenetration()
        {
            return _previousModel.GetHorseChargePenetration();
        }

        public override float CalculateStaggerThresholdDamage(Agent defenderAgent, in Blow blow)
        {
            return _previousModel.CalculateStaggerThresholdDamage(defenderAgent, blow);
        }

        public override float CalculateAlternativeAttackDamage(BasicCharacterObject attackerCharacter, WeaponComponentData weapon)
        {
            return _previousModel.CalculateAlternativeAttackDamage(attackerCharacter, weapon);
        }

        public override float CalculatePassiveAttackDamage(BasicCharacterObject attackerCharacter, in AttackCollisionData collisionData, float baseDamage)
        {
            return _previousModel.CalculatePassiveAttackDamage(attackerCharacter, collisionData, baseDamage);
        }

        public override MeleeCollisionReaction DecidePassiveAttackCollisionReaction(Agent attacker, Agent defender, bool isFatalHit)
        {
            return _previousModel.DecidePassiveAttackCollisionReaction(attacker, defender, isFatalHit);
        }

        public override float CalculateShieldDamage(in AttackInformation attackInformation, float baseDamage)
        {
            return _previousModel.CalculateShieldDamage(attackInformation, baseDamage);
        }

        public override float GetDamageMultiplierForBodyPart(BoneBodyPartType bodyPart, DamageTypes type, bool isHuman, bool isMissile)
        {
            return _previousModel.GetDamageMultiplierForBodyPart(bodyPart, type, isHuman, isMissile);
        }

        public override bool DecideAgentShrugOffBlow(Agent victimAgent, AttackCollisionData collisionData, in Blow blow)
        {
            return _previousModel.DecideAgentShrugOffBlow(victimAgent, collisionData, blow);
        }

        public override bool DecideAgentDismountedByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
        {
            return _previousModel.DecideAgentDismountedByBlow(attackerAgent, victimAgent, collisionData, attackerWeapon, blow);
        }

        public override bool DecideAgentKnockedBackByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
        {
            return _previousModel.DecideAgentKnockedBackByBlow(attackerAgent, victimAgent, collisionData, attackerWeapon, blow);
        }

        public override bool DecideAgentKnockedDownByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
        {
            return _previousModel.DecideAgentKnockedDownByBlow(attackerAgent, victimAgent, collisionData, attackerWeapon, blow);
        }

        public override bool DecideMountRearedByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
        {
            return _previousModel.DecideMountRearedByBlow(attackerAgent, victimAgent, collisionData, attackerWeapon, blow);
        }
    }
}