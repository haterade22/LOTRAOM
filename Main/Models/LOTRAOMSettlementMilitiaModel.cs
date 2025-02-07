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

    public class LOTRAOMSettlementMilitiaModel : SettlementMilitiaModel
    {
        SettlementMilitiaModel previousModel;
        public LOTRAOMSettlementMilitiaModel(SettlementMilitiaModel baseModel)
        {
            this.previousModel = baseModel;
        }

        public override float CalculateEliteMilitiaSpawnChance(Settlement settlement)
        {
            float value = previousModel.CalculateEliteMilitiaSpawnChance(settlement);
            return value;
        }

        public override ExplainedNumber CalculateMilitiaChange(Settlement settlement, bool includeDescriptions = false)
        {
            ExplainedNumber value = previousModel.CalculateMilitiaChange(settlement, includeDescriptions);
            if (settlement.OwnerClan.Culture.HasFeat(LOTRAOMCultureFeats.Instance.humanPlusMilitiaProduction))
            {
                value.Add(LOTRAOMCultureFeats.Instance.humanPlusMilitiaProduction.EffectBonus, new("{=human_militia}Human culture militia bonus"), null);
            }
            return value;
        }

        public override void CalculateMilitiaSpawnRate(Settlement settlement, out float meleeTroopRate, out float rangedTroopRate)
        {
            meleeTroopRate = 0.5f;
            rangedTroopRate = 1f - meleeTroopRate;
        }

        public override int MilitiaToSpawnAfterSiege(Town town)
        {
            int value = previousModel.MilitiaToSpawnAfterSiege(town);
            return value;
        }
    }
}
