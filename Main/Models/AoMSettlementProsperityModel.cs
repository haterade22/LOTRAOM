using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Localization;
namespace LOTRAOM.Models
{
    public class AoMSettlementProsperityModel : SettlementProsperityModel
    {
        private SettlementProsperityModel baseModel;
        public AoMSettlementProsperityModel(SettlementProsperityModel defaultModel)
        {
            this.baseModel = defaultModel;
        }
        public override ExplainedNumber CalculateHearthChange(Village village, bool includeDescriptions = false)
        {
            return baseModel.CalculateHearthChange(village, includeDescriptions);
        }

        public override ExplainedNumber CalculateProsperityChange(Town fortification, bool includeDescriptions = false)
        {
            ExplainedNumber value = baseModel.CalculateProsperityChange(fortification, includeDescriptions);
            if (fortification.Culture.StringId == Globals.IsengardCulture && fortification.StringId == Globals.Orthanc) //orthanc
                value.Add(5, new TextObject("Orthanc held by Isengard")); //orthanc should not have prosperity change
            return value;
        }
    }
}
