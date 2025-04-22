using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace LOTRAOM.Models
{
    public class LOTRAOMPartyFoodModel : MobilePartyFoodConsumptionModel
    {
        private readonly MobilePartyFoodConsumptionModel _previousModel;

        public LOTRAOMPartyFoodModel(MobilePartyFoodConsumptionModel baseModel)
        {
            _previousModel = baseModel;
        }

        public override int NumberOfMenOnMapToEatOneFood => _previousModel.NumberOfMenOnMapToEatOneFood;

        public override ExplainedNumber CalculateDailyBaseFoodConsumptionf(MobileParty party, bool includeDescription = false)
        {
            if (party?.IsMilitia == true)
            {
                Debug.Print($"[LOTRAOM] CalculateDailyBaseFoodConsumptionf: Militia party for {party.CurrentSettlement?.StringId ?? "null"} consumes no food.");
                return new ExplainedNumber(0f, includeDescription, new TextObject("{=lotraom_militia_food_exemption}Militia food exemption"));
            }
            return _previousModel.CalculateDailyBaseFoodConsumptionf(party, includeDescription);
        }

        public override ExplainedNumber CalculateDailyFoodConsumptionf(MobileParty party, ExplainedNumber baseConsumption)
        {
            if (party?.IsMilitia == true)
            {
                Debug.Print($"[LOTRAOM] CalculateDailyFoodConsumptionf: Militia party for {party.CurrentSettlement?.StringId ?? "null"} consumes no food.");
                return new ExplainedNumber(0f, baseConsumption.IncludeDescriptions, new TextObject("{=lotraom_militia_food_exemption}Militia food exemption"));
            }
            return _previousModel.CalculateDailyFoodConsumptionf(party, baseConsumption);
        }

        public override bool DoesPartyConsumeFood(MobileParty mobileParty)
        {
            if (mobileParty?.IsMilitia == true)
            {
                Debug.Print($"[LOTRAOM] DoesPartyConsumeFood: Militia party for {mobileParty.CurrentSettlement?.StringId ?? "null"} is exempt from food consumption.");
                return false;
            }
            return _previousModel.DoesPartyConsumeFood(mobileParty);
        }
    }
}