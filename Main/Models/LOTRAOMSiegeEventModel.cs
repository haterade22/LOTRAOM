using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace LOTRAOM.Models
{
    public class LOTRAOMSiegeEventModel : SiegeEventModel
    {
        private readonly SiegeEventModel _previousModel;

        public LOTRAOMSiegeEventModel(SiegeEventModel baseModel)
        {
            _previousModel = baseModel;
        }

        public override float GetConstructionProgressPerHour(SiegeEngineType type, SiegeEvent siegeEvent, ISiegeEventSide side)
        {
            ExplainedNumber result = new ExplainedNumber(_previousModel.GetConstructionProgressPerHour(type, siegeEvent, side));
            if (side.BattleSide == BattleSideEnum.Defender && siegeEvent.BesiegedSettlement?.StringId == "town_SWAN_ISENGARD1")
            {
                float isengardBonus = 0.3f; // 30% faster construction for Isengard's war machine
                result.AddFactor(isengardBonus, new TextObject("{=lotraom_isengard_construction}Isengard war machine efficiency"));
                Debug.Print($"[LOTRAOM] GetConstructionProgressPerHour: Applied {isengardBonus * 100}% construction bonus for Isengard militia at {siegeEvent.BesiegedSettlement.StringId}");
            }
            return result.ResultNumber;
        }

        public override float GetCasualtyChance(MobileParty siegeParty, SiegeEvent siegeEvent, BattleSideEnum side)
        {
            if (siegeParty?.IsMilitia == true && side == BattleSideEnum.Defender)
            {
                float baseChance = _previousModel.GetCasualtyChance(siegeParty, siegeEvent, side);
                float militiaReduction = -0.2f; // 20% reduced casualty chance for militia
                if (siegeParty.CurrentSettlement?.StringId == "town_SWAN_ISENGARD1")
                {
                    militiaReduction = -0.3f; // 30% reduced casualty chance for Isengard militia
                    Debug.Print($"[LOTRAOM] GetCasualtyChance: Applied {militiaReduction * 100}% casualty reduction for Isengard militia at {siegeParty.CurrentSettlement.StringId}");
                }
                else
                {
                    Debug.Print($"[LOTRAOM] GetCasualtyChance: Applied {militiaReduction * 100}% casualty reduction for militia at {siegeParty.CurrentSettlement?.StringId ?? "null"}");
                }
                return MathF.Max(0f, baseChance + militiaReduction);
            }
            return _previousModel.GetCasualtyChance(siegeParty, siegeEvent, side);
        }

        public override float GetSiegeEngineHitPoints(SiegeEvent siegeEvent, SiegeEngineType engineType, BattleSideEnum side)
        {
            return _previousModel.GetSiegeEngineHitPoints(siegeEvent, engineType, side);
        }

        public override float GetSiegeStrategyScore(SiegeEvent siege, BattleSideEnum side, SiegeStrategy strategy)
        {
            return _previousModel.GetSiegeStrategyScore(siege, side, strategy);
        }

        public override string GetSiegeEngineMapPrefabName(SiegeEngineType type, int wallLevel, BattleSideEnum side)
        {
            return _previousModel.GetSiegeEngineMapPrefabName(type, wallLevel, side);
        }

        public override string GetSiegeEngineMapProjectilePrefabName(SiegeEngineType type)
        {
            return _previousModel.GetSiegeEngineMapProjectilePrefabName(type);
        }

        public override string GetSiegeEngineMapReloadAnimationName(SiegeEngineType type, BattleSideEnum side)
        {
            return _previousModel.GetSiegeEngineMapReloadAnimationName(type, side);
        }

        public override string GetSiegeEngineMapFireAnimationName(SiegeEngineType type, BattleSideEnum side)
        {
            return _previousModel.GetSiegeEngineMapFireAnimationName(type, side);
        }

        public override sbyte GetSiegeEngineMapProjectileBoneIndex(SiegeEngineType type, BattleSideEnum side)
        {
            return _previousModel.GetSiegeEngineMapProjectileBoneIndex(type, side);
        }

        public override float GetSiegeEngineHitChance(SiegeEngineType siegeEngineType, BattleSideEnum battleSide, SiegeBombardTargets target, Town town)
        {
            return _previousModel.GetSiegeEngineHitChance(siegeEngineType, battleSide, target, town);
        }

        public override int GetSiegeEngineDestructionCasualties(SiegeEvent siegeEvent, BattleSideEnum side, SiegeEngineType destroyedSiegeEngine)
        {
            return _previousModel.GetSiegeEngineDestructionCasualties(siegeEvent, side, destroyedSiegeEngine);
        }

        public override float GetSiegeEngineDamage(SiegeEvent siegeEvent, BattleSideEnum battleSide, SiegeEngineType siegeEngine, SiegeBombardTargets target)
        {
            return _previousModel.GetSiegeEngineDamage(siegeEvent, battleSide, siegeEngine, target);
        }

        public override int GetRangedSiegeEngineReloadTime(SiegeEvent siegeEvent, BattleSideEnum side, SiegeEngineType siegeEngine)
        {
            return _previousModel.GetRangedSiegeEngineReloadTime(siegeEvent, side, siegeEngine);
        }

        public override FlattenedTroopRoster GetPriorityTroopsForSallyOutAmbush()
        {
            return _previousModel.GetPriorityTroopsForSallyOutAmbush();
        }

        public override IEnumerable<SiegeEngineType> GetPrebuiltSiegeEnginesOfSiegeCamp(BesiegerCamp besiegerCamp)
        {
            return _previousModel.GetPrebuiltSiegeEnginesOfSiegeCamp(besiegerCamp);
        }

        public override IEnumerable<SiegeEngineType> GetPrebuiltSiegeEnginesOfSettlement(Settlement settlement)
        {
            return _previousModel.GetPrebuiltSiegeEnginesOfSettlement(settlement);
        }

        public override MobileParty GetEffectiveSiegePartyForSide(SiegeEvent siegeEvent, BattleSideEnum battleSide)
        {
            return _previousModel.GetEffectiveSiegePartyForSide(siegeEvent, battleSide);
        }

        public override int GetColleteralDamageCasualties(SiegeEngineType siegeEngineType, MobileParty party)
        {
            return _previousModel.GetColleteralDamageCasualties(siegeEngineType, party);
        }

        public override float GetAvailableManDayPower(ISiegeEventSide side)
        {
            return _previousModel.GetAvailableManDayPower(side);
        }

        public override IEnumerable<SiegeEngineType> GetAvailableDefenderSiegeEngines(PartyBase party)
        {
            return _previousModel.GetAvailableDefenderSiegeEngines(party);
        }

        public override IEnumerable<SiegeEngineType> GetAvailableAttackerTowerSiegeEngines(PartyBase party)
        {
            return _previousModel.GetAvailableAttackerTowerSiegeEngines(party);
        }

        public override IEnumerable<SiegeEngineType> GetAvailableAttackerRangedSiegeEngines(PartyBase party)
        {
            return _previousModel.GetAvailableAttackerRangedSiegeEngines(party);
        }

        public override IEnumerable<SiegeEngineType> GetAvailableAttackerRamSiegeEngines(PartyBase party)
        {
            return _previousModel.GetAvailableAttackerRamSiegeEngines(party);
        }
    }
}
