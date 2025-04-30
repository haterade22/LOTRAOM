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
            if (settlement.Culture.StringId == Globals.MordorCulture && settlement.IsTown && occupation == Occupation.GangLeader)
                baseValue += 3;
            if (settlement.Culture.StringId == Globals.IsengardCulture && settlement.StringId == "town_SWAN_ISENGARD1" && occupation == Occupation.GangLeader) //orthanc
                return 20;
            return baseValue;
        }
    }
}
