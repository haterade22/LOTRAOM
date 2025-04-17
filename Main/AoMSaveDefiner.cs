using LOTRAOM.Momentum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.SaveSystem;

namespace LOTRAOM
{
    internal class AoMSaveDefiner : SaveableTypeDefiner
    {
        public AoMSaveDefiner() : base(2_28472_23) { }
        protected override void DefineClassTypes()
        {
            AddClassDefinition(typeof(WarOfTheRingData), 1);
            AddClassDefinition(typeof(MomentumEvent), 2);
            AddClassDefinition(typeof(WarOfTheRingFactionData), 3);
            AddEnumDefinition(typeof(MomentumActionType), 4);
        }
        protected override void DefineContainerDefinitions()
        {
            ConstructContainerDefinition(typeof(Dictionary<string, int>));
            ConstructContainerDefinition(typeof(Queue<MomentumEvent>));
            ConstructContainerDefinition(typeof(Dictionary<MomentumActionType, Queue<MomentumEvent>>));
            ConstructContainerDefinition(typeof(List<Kingdom>));
        }
    }
}