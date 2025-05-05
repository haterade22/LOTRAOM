using System.Linq;
using System.Text.RegularExpressions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace LOTRAOM.Momentum.ViewModel
{
    internal sealed class MomentumIndicatorVM : TaleWorlds.Library.ViewModel
    {

        private MomentumIndicatorItemVM _momentumIndicatorVM;

        [DataSourceProperty] public MomentumIndicatorItemVM MomentumIndicator
        {
            get => _momentumIndicatorVM;
            set
            {
                if (value != _momentumIndicatorVM)
                {
                    _momentumIndicatorVM = value;
                    OnPropertyChanged(nameof(MomentumIndicator));
                }
            }
        }
        [DataSourceProperty] public string MomentumIndicatorText { get { return new TextObject("Momentum").ToString(); } }
        public MomentumIndicatorVM()
        {
            MomentumIndicator = new MomentumIndicatorItemVM();
        }
    }
}