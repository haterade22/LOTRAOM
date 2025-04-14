using System;
using System.Xml.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace LOTRAOM.BalanceOfPower.ViewModel
{
    public class BalanceOfPowerStatVM : TaleWorlds.Library.ViewModel
    {
        [DataSourceProperty] public string Name { get; set; }
        [DataSourceProperty] public string ValueFactionGood { get; set; }
        [DataSourceProperty] public string ValueFactionEvil { get; set; }

        public BalanceOfPowerStatVM(string name, string valueGood, string valueEvil)
        {
            Name = name;
            ValueFactionGood = valueGood;
            ValueFactionEvil = valueEvil;
        }
    }
}