using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Engine;
using TaleWorlds.Localization;
using static TaleWorlds.Library.Debug;
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

            // Increase prosperity for Orthanc when held by Isengard
            if (fortification.Culture.StringId == Globals.IsengardCulture && fortification.StringId == Globals.Orthanc)
            {
                value.Add(30, new TextObject("{=orthanc_prosperity_boost}Orthanc held by Isengard"), null);
            }
            // Reduce prosperity growth for Minas Tirith to lower food consumption
            //if (fortification.Culture.StringId == Globals.GondorCulture && fortification.StringId == Globals.MinasTirith) // Minas Tirith
           // {
              //  value.Add(60, new TextObject("{=MinasTirith_prosperity_reduction}Minas Tirith held by Gondor"), null);
            //}

            // Debug log to verify prosperity changes
            MBDebug.Print($"Prosperity change for {fortification.Name} (ID: {fortification.StringId}): Change={value.ResultNumber}", 0, DebugColor.Green);

            return value;
        }
    }
}


