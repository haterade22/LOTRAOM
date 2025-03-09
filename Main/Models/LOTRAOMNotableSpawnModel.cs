using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;

namespace LOTRAOM.Models
{
    internal class LOTRAOMNotableSpawnModel : NotableSpawnModel
    {
        NotableSpawnModel previousModel;
        public LOTRAOMNotableSpawnModel(NotableSpawnModel previousModel)
        {
            this.previousModel = previousModel;
        }

        public override int GetTargetNotableCountForSettlement(Settlement settlement, Occupation occupation)
        {
            int baseValue = previousModel.GetTargetNotableCountForSettlement(settlement, occupation);
            if (settlement.Culture.StringId == "mordor" && settlement.IsTown && occupation == Occupation.Merchant)
                baseValue += 2;
            return baseValue;
        }
    }
}
