using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;

namespace LOTRAOM.Models
{
    public class AoMSettlementFoodModel : SettlementFoodModel
    {
        public SettlementFoodModel defaultModel;
        public AoMSettlementFoodModel(SettlementFoodModel baseModel)
        {
            defaultModel = baseModel;
        }
        public override int FoodStocksUpperLimit => defaultModel.FoodStocksUpperLimit;

        public override int NumberOfProsperityToEatOneFood => defaultModel.NumberOfProsperityToEatOneFood;

        public override int NumberOfMenOnGarrisonToEatOneFood => defaultModel.NumberOfMenOnGarrisonToEatOneFood;

        public override int CastleFoodStockUpperLimitBonus => defaultModel.CastleFoodStockUpperLimitBonus;

        public override ExplainedNumber CalculateTownFoodStocksChange(Town town, bool includeMarketStocks = true, bool includeDescriptions = false)
        {
            if(shouldFoodStockMechanicBeRemoved(town))
                return new ExplainedNumber(0, includeDescriptions); 

            return defaultModel.CalculateTownFoodStocksChange(town, includeMarketStocks, includeDescriptions);
        }
        public static Func<Town, bool> shouldFoodStockMechanicBeRemoved = (Town t) =>
        {
            if (t.Culture.StringId == Globals.IsengardCulture && t.StringId == Globals.Orthanc)
                return true;
            return false; 
        };


    }
}
