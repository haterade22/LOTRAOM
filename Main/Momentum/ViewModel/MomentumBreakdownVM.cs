using LOTRAOM.Momentum;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace LOTRAOM.Momentum.ViewModel
{
    public class MomentumBreakdownVM : TaleWorlds.Library.ViewModel
    {
        [DataSourceProperty] public string Text { get; set; }
        [DataSourceProperty] public string Number1 { 
            get;
            set; }
        [DataSourceProperty] public string Number2 { get; set; }

        //[DataSourceProperty] public MomentumSideBreakdownVM ValueFaction1 { get; set; }
        BasicTooltipViewModel _valueFaction1;
        [DataSourceProperty]
        public BasicTooltipViewModel ValueFaction1
        {
            get
            {
                return this._valueFaction1;
            }
            set
            {
                if (value != this._valueFaction1)
                {
                    this._valueFaction1 = value;
                    base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "ValueFaction1");
                }
            }
        }
        BasicTooltipViewModel _valueFaction2;
        [DataSourceProperty]
        public BasicTooltipViewModel ValueFaction2
        {
            get
            {
                return this._valueFaction2;
            }
            set
            {
                if (value != this._valueFaction2)
                {
                    this._valueFaction2 = value;
                    base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "ValueFaction2");
                }
            }
        }
        //[DataSourceProperty] public MomentumSideBreakdownVM ValueFaction2 { get; set; }

        public MomentumBreakdownVM(MomentumTempBreakdown breakdown)
        {
            switch (breakdown.MomentumActionType)
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
                case MomentumActionType.ArmyGathered:
                    Text = new TextObject("Army Gathered").ToString();
                    break;
                default:
                    Text = TextObject.Empty.ToString();
                    break;
            }
            ValueFaction1 = new BasicTooltipViewModel(() => GetPartyWageTooltip(breakdown.MomentumGoodSideValue));
            ValueFaction2 = new BasicTooltipViewModel(() => GetPartyWageTooltip(breakdown.MomentumEvilSideValue));
            Number1 = breakdown.MomentumGoodSideValue.ResultNumber.ToString();
            Number2 = breakdown.MomentumEvilSideValue.ResultNumber.ToString();
            //ValueFaction1 = new(breakdown.MomentumGoodSideValue.ResultNumber, breakdown.MomentumGoodSideValue.GetExplanations());
            //ValueFaction2 = new(breakdown.MomentumEvilSideValue.ResultNumber, breakdown.MomentumEvilSideValue.GetExplanations());
        }
        static List<TooltipProperty> GetPartyWageTooltip(ExplainedNumber number)
        {
            return CampaignUIHelper.GetTooltipForAccumulatingPropertyWithResult("Momentum", number.ResultNumber, ref number);
        }
    }
    public class MomentumSideBreakdownVM : TaleWorlds.Library.ViewModel
    {
        [DataSourceProperty] public string Value { get; set; }
        public string Explanation { get; }
        public void ExecuteBeginHint()
        {
            MBInformationManager.ShowHint(Explanation.ToString());
        }

        public void ExecuteEndHint()
        {
            MBInformationManager.HideInformations();
        }

        public MomentumSideBreakdownVM(float value, string hintText)
        {
            Value = value.ToString();
            Explanation = hintText;
        }
    }

}