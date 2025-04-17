using LOTRAOM.Momentum;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace LOTRAOM.Momentum.ViewModel
{
    internal class MomentumBreakdownVM : TaleWorlds.Library.ViewModel
    {
        [DataSourceProperty] public string Text { get; set; }

        [DataSourceProperty] public int ValueFaction1 { get; set; }

        [DataSourceProperty] public int ValueFaction2 { get; set; }

        [DataSourceProperty] public string WarExhaustionValueFaction1 { get; set; }

        [DataSourceProperty] public string WarExhaustionValueFaction2 { get; set; }

        public MomentumBreakdownVM(MomentumBreakdown breakdown)
        {
            switch (breakdown.Type)
            {
                case MomentumActionType.Casualty:
                    Text = new TextObject("Casualties").ToString();
                    break;
                case MomentumActionType.Raid:
                    Text = new TextObject("Villages Raided").ToString();
                    break;
                case MomentumActionType.Siege:
                    Text = new TextObject("Fiefs Lost").ToString();
                    break;
                case MomentumActionType.Occupied:
                    Text = new TextObject("Occupied").ToString();
                    break;
                default:
                    Text = TextObject.Empty.ToString();
                    break;
            }
            ValueFaction1 = breakdown.ValueFaction1;
            ValueFaction2 = breakdown.ValueFaction2;
            WarExhaustionValueFaction1 = string.Format("{1}{0:F1}{2}", breakdown.BalanceOfPowerFraction1, "+", "%");
            WarExhaustionValueFaction2 = string.Format("{1}{0:F1}{2}", breakdown.BalanceOfPowerFraction2, "+", "%");
        }
    }

}