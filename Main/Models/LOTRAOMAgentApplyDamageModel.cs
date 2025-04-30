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
        private const float RohirrimMountedBonus = 0.1f; // 10% damage bonus for Rohirrim when mounted
        private static readonly AgentApplyDamageModel _defaultModel = MissionGameModels.Current.AgentApplyDamageModel;

        public bool IsSiegeEngineHit { get; set; }

        public override float CalculateDamage(in AttackInformation attackInformation, in AttackCollisionData collisionData, in MissionWeapon weapon, float baseDamage)
        {
            try
            {
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
                        // Race bonus
                        RaceManager.ApplyRaceBonusWhenDealingDamage(
                            attacker: attackInformation.AttackerAgentCharacter,
                            damage: damageType,
                            dealtDamage: ref modifiedDamage,
                            description: new TextObject("{=lotraom_attacker_bonus}{CULTURE} {TYPE} Damage Bonus")
                                .SetTextVariable("CULTURE", attackInformation.AttackerAgentCharacter.Culture.Name)
                                .SetTextVariable("TYPE", damageType == RaceManager.DamageType.Melee ? "Melee" : "Ranged")
                        );

                        // Culture-specific terrain bonus
                        TerrainType terrain = Mission.Current?.TerrainType ?? TerrainType.None;
                        string cultureId = attackInformation.AttackerAgentCharacter.Culture.StringId.ToLower();
                        if (CultureManager.TryGetTerrainBonus(cultureId, terrain, out float terrainBonus))
                        {
                            bool applyBonus = true;

                            // Cavalry-specific restriction for Rohirrim and Variags
                            if ((cultureId == "vlandia" && terrain == TerrainType.Plain) ||
                                (cultureId == "battania" && terrain == TerrainType.Steppe))
                            {
                                applyBonus = attackInformation.AttackerAgent?.MountAgent != null;
                            }
                            // Ranged-specific restriction for Elves, Bardings, and Drúedain
                            else if ((cultureId == "rivendell" && terrain == TerrainType.Forest) ||
                                     (cultureId == "mirkwood" && terrain == TerrainType.Forest) ||
                                     (cultureId == "lothlorien" && terrain == TerrainType.Forest) ||
                                     (cultureId == "sturgia" && terrain == TerrainType.RuralArea) ||
                                     (cultureId == "darshi" && terrain == TerrainType.Forest))
                            {
                                applyBonus = damageType == RaceManager.DamageType.Ranged;
                            }

                            if (applyBonus)
                            {
                                modifiedDamage.AddFactor(
                                    terrainBonus,
                                    new TextObject("{=lotraom_terrain_bonus}{CULTURE} {TERRAIN} Bonus")
                                        .SetTextVariable("CULTURE", attackInformation.AttackerAgentCharacter.Culture.Name)
                                        .SetTextVariable("TERRAIN", terrain.ToString())
                                );
                            }
                        }

                        // Rohirrim-specific mounted bonus
                        if (cultureId == "vlandia" && attackInformation.AttackerAgent?.MountAgent != null)
                        {
                            modifiedDamage.AddFactor(
                                RohirrimMountedBonus,
                                new TextObject("{=LOTRAOM_MountedBonus}Your Rohirrim horsemanship grants a {BONUS}% damage bonus while mounted!")
                                    .SetTextVariable("BONUS", (RohirrimMountedBonus * 100f).ToString("F0"))
                            );
                        }
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
                            damage: damageType,
                            damageValue: ref modifiedDamage,
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

                modifiedDamage.LimitMin(0f);
                return modifiedDamage.ResultNumber;
            }
            catch (Exception ex)
            {
                InformationManager.DisplayMessage(new InformationMessage(
                    $"LOTRAOM Error: Damage calculation failed: {ex.Message}",
                    Colors.Red
                ));
                return _defaultModel.CalculateDamage(attackInformation, collisionData, weapon, baseDamage);
            }
        }

        public override bool DecideAgentKnockedBackByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
        {
            try
            {
                bool shouldKnockBack = _defaultModel.DecideAgentKnockedBackByBlow(attackerAgent, victimAgent, collisionData, attackerWeapon, blow);
                if (shouldKnockBack && victimAgent.Character is BasicCharacterObject victimCharacter)
                {
                    RaceManager.RaceBonus bonus = RaceManager.GetRacialData(victimCharacter);
                    if (!bonus.KnockbackResistance.ApproximatelyEqualsTo(0f, 1E-05f) && MBRandom.RandomFloat < bonus.KnockbackResistance)
                    {
                        InformationManager.DisplayMessage(new InformationMessage(
                            new TextObject("{=lotraom_knockback_resisted}").SetTextVariable("CULTURE", victimCharacter.Culture.Name).ToString(),
                            Colors.Green
                        ));
                        return false;
                    }
                }
                return shouldKnockBack;
            }
            catch (Exception ex)
            {
                InformationManager.DisplayMessage(new InformationMessage(
                    $"LOTRAOM Error: Knockback calculation failed: {ex.Message}",
                    Colors.Red
                ));
                return _defaultModel.DecideAgentKnockedBackByBlow(attackerAgent, victimAgent, collisionData, attackerWeapon, blow);
            }
        }

        public override bool DecideAgentKnockedDownByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
        {
            try
            {
                bool shouldKnockDown = _defaultModel.DecideAgentKnockedDownByBlow(attackerAgent, victimAgent, collisionData, attackerWeapon, blow);
                if (attackerAgent.Character is BasicCharacterObject attackerCharacter)
                {
                    RaceManager.RaceBonus bonus = RaceManager.GetRacialData(attackerCharacter);
                    if (!bonus.KnockdownChance.ApproximatelyEqualsTo(0f, 1E-05f) && MBRandom.RandomFloat < bonus.KnockdownChance)
                    {
                        InformationManager.DisplayMessage(new InformationMessage(
                            new TextObject("{=lotraom_knockdown_triggered}").SetTextVariable("CULTURE", attackerCharacter.Culture.Name).ToString(),
                            Colors.Green
                        ));
                        return true;
                    }
                }
                return shouldKnockDown;
            }
            catch (Exception ex)
            {
                InformationManager.DisplayMessage(new InformationMessage(
                    $"LOTRAOM Error: Knockdown calculation failed: {ex.Message}",
                    Colors.Red
                ));
                return _defaultModel.DecideAgentKnockedDownByBlow(attackerAgent, victimAgent, collisionData, attackerWeapon, blow);
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