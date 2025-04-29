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
        [DataSourceProperty] public string Number1 { get; set; }
        [DataSourceProperty] public string Number2 { get; set; }

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
            Text = breakdown.MomentumActionType switch
            {
                MomentumActionType.BattleWon => new TextObject("Battles Won").ToString(),
                MomentumActionType.VillagesRaided => new TextObject("Villages Raided").ToString(),
                MomentumActionType.Sieges => new TextObject("Fiefs Captured").ToString(),
                MomentumActionType.ArmyGathered => new TextObject("Army Gathered").ToString(),
                MomentumActionType.RelativeStrength => new TextObject("Relative Strength").ToString(),
                _ => TextObject.Empty.ToString(),
            };
            ValueFaction1 = new BasicTooltipViewModel(() => GetExplainerTooltip(breakdown.MomentumGoodSideValue));
            ValueFaction2 = new BasicTooltipViewModel(() => GetExplainerTooltip(breakdown.MomentumEvilSideValue));
            Number1 = breakdown.MomentumGoodSideValue.ResultNumber.ToString();
            Number2 = breakdown.MomentumEvilSideValue.ResultNumber.ToString();
        }
        static List<TooltipProperty> GetExplainerTooltip(ExplainedNumber number)
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