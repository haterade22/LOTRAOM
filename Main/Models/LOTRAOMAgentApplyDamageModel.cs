using TaleWorlds.MountAndBlade;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.MountAndBlade.ComponentInterfaces;
using System;
using TaleWorlds.Localization;

namespace LOTRAOM
{
    public class LOTRAOMAgentApplyDamageModel : AgentApplyDamageModel
    {
        private const float SallyOutSiegeEngineDamageMultiplier = 4.5f;
        private static readonly AgentApplyDamageModel _defaultModel = MissionGameModels.Current.AgentApplyDamageModel;

        // Property to store whether the current attack targets a siege engine
        public bool IsSiegeEngineHit { get; set; }

        public override float CalculateDamage(in AttackInformation attackInformation, in AttackCollisionData collisionData, in MissionWeapon weapon, float baseDamage)
        {
            try
            {
                // Initialize with default model’s damage, including campaign perks and banner effects
                ExplainedNumber modifiedDamage = new ExplainedNumber(
                    baseNumber: _defaultModel.CalculateDamage(attackInformation, collisionData, weapon, baseDamage),
                    includeDescriptions: true,
                    baseText: new TextObject("{=lotraom_base_damage}Base Damage")
                );

                // Apply race-specific damage bonuses for the attacker
                if (attackInformation.AttackerAgentCharacter != null)
                {
                    RaceManager.DamageType damageType = RaceManager.GetDefaultDamage(weapon);
                    if (damageType != RaceManager.DamageType.Other)
                    {
                        RaceManager.ApplyRaceBonusWhenDealingDamage(
                            character: attackInformation.AttackerAgentCharacter,
                            damageType: damageType,
                            damage: ref modifiedDamage,
                            description: new TextObject("{=lotraom_attacker_bonus}{CULTURE} {TYPE} Damage Bonus")
                                .SetTextVariable("CULTURE", attackInformation.AttackerAgentCharacter.Culture.Name)
                                .SetTextVariable("TYPE", damageType == RaceManager.DamageType.Melee ? "Melee" : "Ranged")
                        );
                    }
                }

                // Apply race-specific damage resistances for the defender
                if (attackInformation.VictimAgentCharacter != null)
                {
                    RaceManager.DamageType damageType = RaceManager.GetDefaultDamage(weapon);
                    if (damageType != RaceManager.DamageType.Other)
                    {
                        RaceManager.ApplyRaceBonusWhenGotHit(
                            character: attackInformation.VictimAgentCharacter,
                            damageType: damageType,
                            damage: ref modifiedDamage,
                            description: new TextObject("{=lotraom_defender_resistance}{CULTURE} {TYPE} Damage Resistance")
                                .SetTextVariable("CULTURE", attackInformation.VictimAgentCharacter.Culture.Name)
                                .SetTextVariable("TYPE", damageType == RaceManager.DamageType.Melee ? "Melee" : "Ranged")
                        );
                    }
                }

                // Apply siege engine damage multiplier for sally-out battles
                if (IsSiegeEngineHit && Mission.Current?.IsSallyOutBattle == true)
                {
                    modifiedDamage.AddFactor(
                        value: SallyOutSiegeEngineDamageMultiplier - 1f,
                        description: new TextObject("{=lotraom_siege_bonus}Sally-Out Siege Engine Bonus")
                    );
                }

                // Ensure non-negative damage
                modifiedDamage.LimitMin(0f);

                return modifiedDamage.ResultNumber;
            }
            catch (Exception ex)
            {
                InformationManager.DisplayMessage(new InformationMessage(
                    $"LOTRAOM Error: Damage calculation failed: {ex.Message}",
                    Colors.Red
                ));
                return _defaultModel.CalculateDamage(attackInformation, collisionData, weapon, baseDamage); // Fallback
            }
        }

        // Implement other required methods, delegating to the default model
        public override float CalculateStaggerThresholdDamage(Agent defenderAgent, in Blow blow)
        {
            return _defaultModel.CalculateStaggerThresholdDamage(defenderAgent, blow);
        }

        public override float CalculateAlternativeAttackDamage(BasicCharacterObject attackerCharacter, WeaponComponentData weapon)
        {
            return _defaultModel.CalculateAlternativeAttackDamage(attackerCharacter, weapon);
        }

        public override float CalculatePassiveAttackDamage(BasicCharacterObject attackerCharacter, in AttackCollisionData collisionData, float baseDamage)
        {
            return _defaultModel.CalculatePassiveAttackDamage(attackerCharacter, collisionData, baseDamage);
        }

        public override MeleeCollisionReaction DecidePassiveAttackCollisionReaction(Agent attacker, Agent defender, bool isFatalHit)
        {
            return _defaultModel.DecidePassiveAttackCollisionReaction(attacker, defender, isFatalHit);
        }

        public override float CalculateShieldDamage(in AttackInformation attackInformation, float baseDamage)
        {
            return _defaultModel.CalculateShieldDamage(attackInformation, baseDamage);
        }

        public override float GetDamageMultiplierForBodyPart(BoneBodyPartType bodyPart, DamageTypes type, bool isHuman, bool isMissile)
        {
            return _defaultModel.GetDamageMultiplierForBodyPart(bodyPart, type, isHuman, isMissile);
        }

        public override bool DecideAgentShrugOffBlow(Agent victimAgent, AttackCollisionData collisionData, in Blow blow)
        {
            return _defaultModel.DecideAgentShrugOffBlow(victimAgent, collisionData, blow);
        }

        public override bool DecideAgentDismountedByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
        {
            return _defaultModel.DecideAgentDismountedByBlow(attackerAgent, victimAgent, collisionData, attackerWeapon, blow);
        }

        public override bool DecideAgentKnockedBackByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
        {
            return _defaultModel.DecideAgentKnockedBackByBlow(attackerAgent, victimAgent, collisionData, attackerWeapon, blow);
        }

        public override bool DecideAgentKnockedDownByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
        {
            return _defaultModel.DecideAgentKnockedDownByBlow(attackerAgent, victimAgent, collisionData, attackerWeapon, blow);
        }

        public override bool DecideMountRearedByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
        {
            return _defaultModel.DecideMountRearedByBlow(attackerAgent, victimAgent, collisionData, attackerWeapon, blow);
        }

        public float GetAgentStateProbability(Agent attacker, Agent victim, DamageTypes damageType, WeaponFlags weaponFlags, out float unconsciousnessProbability)
        {
            unconsciousnessProbability = 0.5f;
            return damageType == DamageTypes.Invalid ? 0f : 0.5f;
        }

        public override float GetHorseChargePenetration()
        {
            return _defaultModel.GetHorseChargePenetration();
        }

        public override float GetKnockBackPenetration(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData)
        {
            return _defaultModel.GetKnockBackPenetration(attackerAgent, attackerWeapon, blow, collisionData);
        }

        public override float GetKnockDownPenetration(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData)
        {
            return _defaultModel.GetKnockDownPenetration(attackerAgent, attackerWeapon, blow, collisionData);
        }

        public override float GetDismountPenetration(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData)
        {
            return _defaultModel.GetDismountPenetration(attackerAgent, attackerWeapon, blow, collisionData);
        }

        public override bool CanWeaponKnockback(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData)
        {
            return _defaultModel.CanWeaponKnockback(attackerAgent, attackerWeapon, blow, collisionData);
        }

        public override bool CanWeaponKnockDown(Agent attackerAgent, Agent victimAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData)
        {
            return _defaultModel.CanWeaponKnockDown(attackerAgent, victimAgent, attackerWeapon, blow, collisionData);
        }

        public override bool CanWeaponDismount(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData)
        {
            return _defaultModel.CanWeaponDismount(attackerAgent, attackerWeapon, blow, collisionData);
        }

        public override bool CanWeaponIgnoreFriendlyFireChecks(WeaponComponentData weapon)
        {
            return _defaultModel.CanWeaponIgnoreFriendlyFireChecks(weapon);
        }

        public override void DecideMissileWeaponFlags(Agent attackerAgent, MissionWeapon missileWeapon, ref WeaponFlags missileWeaponFlags)
        {
            _defaultModel.DecideMissileWeaponFlags(attackerAgent, missileWeapon, ref missileWeaponFlags);
        }

        public override bool DecideCrushedThrough(Agent attackerAgent, Agent defenderAgent, float totalAttackEnergy, Agent.UsageDirection attackDirection, StrikeType strikeType, WeaponComponentData defendItem, bool isPassiveUsage)
        {
            return _defaultModel.DecideCrushedThrough(attackerAgent, defenderAgent, totalAttackEnergy, attackDirection, strikeType, defendItem, isPassiveUsage);
        }

        public override void CalculateDefendedBlowStunMultipliers(Agent attackerAgent, Agent defenderAgent, CombatCollisionResult collisionResult, WeaponComponentData attackerWeapon, WeaponComponentData defenderWeapon, out float attackerStunMultiplier, out float defenderStunMultiplier)
        {
            _defaultModel.CalculateDefendedBlowStunMultipliers(attackerAgent, defenderAgent, collisionResult, attackerWeapon, defenderWeapon, out attackerStunMultiplier, out defenderStunMultiplier);
        }
    }
}