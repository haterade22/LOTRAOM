using LOTRAOM.Momentum.Views;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;

namespace LOTRAOM.Momentum.ViewModel
{
    internal sealed class MomentumIndicatorItemVM : TaleWorlds.Library.ViewModel
    {
        private string _TestText = "War of the Ring";
        [DataSourceProperty]
        public string Text
        {
            get => _TestText;
            set
            {
                if (value != _TestText)
                {
                    _TestText = value;
                    OnPropertyChanged(nameof(Text));
                }
            }
        }

        private int _momentum;
        [DataSourceProperty] public int Momentum         {
            get => _momentum;
            set
            {
                if (value != _momentum)
                {
                    _momentum = value;
                    OnPropertyChanged(nameof(Momentum));
                }
            }
        }
        // multiply by -1, so good factions are on the left, evil on the right side of the slider
        private void OnMomentumChanged()
        {
            Momentum = -1 * (int)MomentumCampaignBehavior.Instance.warOfTheRingData.Momentum;
        }
        public MomentumIndicatorItemVM()
        {
            Momentum = 5;
            MomentumCampaignBehavior.Instance.OnMomentumChanged += OnMomentumChanged;
            RefreshValues();
        }
        public override void RefreshValues()
        {
            Momentum = (int)MomentumCampaignBehavior.Instance.warOfTheRingData.Momentum;
            base.RefreshValues();
        }
        private void OpenDetailedBalanceOfPowerView()
        {
            new MomentumInterface().ShowInterface(ScreenManager.TopScreen);
        }
    }
}