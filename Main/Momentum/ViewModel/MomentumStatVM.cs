using System;
using System.Xml.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace LOTRAOM.Momentum.ViewModel
{
    public class MomentumStatVM : TaleWorlds.Library.ViewModel
    {
        [DataSourceProperty] public string Name { get; set; }
        [DataSourceProperty] public string ValueFactionGood { get; set; }
        [DataSourceProperty] public string ValueFactionEvil { get; set; }

        public MomentumStatVM(string name, string valueGood, string valueEvil)
        {
            Name = name;
            ValueFactionGood = valueGood;
            ValueFactionEvil = valueEvil;
        }
    }
}