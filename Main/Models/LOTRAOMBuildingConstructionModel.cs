using LOTRAOM.CultureFeats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Settlements;

namespace LOTRAOM.Models
{
    public class LOTRAOMBuildingConstructionModel : BuildingConstructionModel
    {
        BuildingConstructionModel previousModel;
        public LOTRAOMBuildingConstructionModel(BuildingConstructionModel baseModel)
        {
            previousModel = baseModel;
        }

        public override int TownBoostCost => previousModel.TownBoostCost;

        public override int TownBoostBonus => previousModel.TownBoostBonus;

        public override int CastleBoostCost => previousModel.CastleBoostCost;

        public override int CastleBoostBonus => previousModel.CastleBoostBonus;

        public override ExplainedNumber CalculateDailyConstructionPower(Town town, bool includeDescriptions = false)
        {
            ExplainedNumber value = previousModel.CalculateDailyConstructionPower(town, includeDescriptions);
            AddBonuses(value.ResultNumber, town);
            return value;
        }

        public override int CalculateDailyConstructionPowerWithoutBoost(Town town)
        {
            int value = previousModel.CalculateDailyConstructionPowerWithoutBoost(town);
            AddBonuses(value, town);
            return value;
        }
        private static int AddBonuses(float value, Town town)
        {
            if (town.Loyalty > 25f && town.OwnerClan.Culture.HasFeat(DefaultCulturalFeats.BattanianConstructionFeat))
            {
                value += (value * LOTRAOMCultureFeats.Instance.rohanSlowerBuildRate.EffectBonus);
            }
            return (int)value;
        }

        public override int GetBoostAmount(Town town)
        {
            int value = previousModel.GetBoostAmount(town);
            return value;
        }

        public override int GetBoostCost(Town town)
        {
            int value = previousModel.GetBoostCost(town);
            return value;
        }
    }
}
