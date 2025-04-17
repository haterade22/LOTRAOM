using LOTRAOM.Momentum.Views;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;

namespace LOTRAOM.Momentum.ViewModel
{
    internal sealed class MomentumIndicatorItemVM : TaleWorlds.Library.ViewModel
    {
        private bool _isCriticalFaction1;
        private bool _isCriticalFaction2;
        private int _playerWarExhaustion, _opponentWarExhaustion = 10;
        private ImageIdentifierVM _faction1Visual = null!;
        private ImageIdentifierVM _faction2Visual = null!;

        private string _TestText = "my test";
        [DataSourceProperty]
        public string TestText
        {
            get => _TestText;
            set
            {
                if (value != _TestText)
                {
                    _TestText = value;
                    OnPropertyChanged(nameof(TestText));
                }
            }
        }

        [DataSourceProperty]
        public int Momentum
        {
            get => MomentumCampaignBehavior.Instance.WarOfTheRingdata.Momentum;
        }

        [DataSourceProperty]
        public int OpponentWarExhaustion
        {
            get => _opponentWarExhaustion;
            set
            {
                if (value != _opponentWarExhaustion)
                {
                    _opponentWarExhaustion = value;
                    OnPropertyChanged(nameof(OpponentWarExhaustion));
                }
            }
        }

        [DataSourceProperty]
        public int PlayerWarExhaustion
        {
            get => _playerWarExhaustion;
            set
            {
                if (value != _playerWarExhaustion)
                {
                    _playerWarExhaustion = value;
                    OnPropertyChanged(nameof(PlayerWarExhaustion));
                }
            }
        }

        [DataSourceProperty]
        public ImageIdentifierVM Faction1Visual
        {
            get => _faction1Visual;
            set { _faction1Visual = value; OnPropertyChanged(nameof(Faction1Visual)); }
        }

        [DataSourceProperty]
        public ImageIdentifierVM Faction2Visual
        {
            get => _faction2Visual;
            set { _faction2Visual = value; OnPropertyChanged(nameof(Faction2Visual)); }
        }

        [DataSourceProperty]
        public bool IsCriticalFaction1
        {
            get => _isCriticalFaction1;
            set
            {
                if (value != _isCriticalFaction1)
                {
                    _isCriticalFaction1 = value;
                    OnPropertyChangedWithValue(value, nameof(IsCriticalFaction1));
                }
            }
        }

        [DataSourceProperty]
        public bool IsCriticalFaction2
        {
            get => _isCriticalFaction2;
            set
            {
                if (value != _isCriticalFaction2)
                {
                    _isCriticalFaction2 = value;
                    OnPropertyChangedWithValue(value, nameof(IsCriticalFaction2));
                }
            }
        }
        public MomentumIndicatorItemVM()
        {
            RefreshValues();
        }
        public override void RefreshValues()
        {
            base.RefreshValues();
            var playerKingdom = Clan.PlayerClan.Kingdom;
            Faction1Visual = new ImageIdentifierVM(BannerCode.CreateFrom(MomentumGlobals.Gondor.Banner), true);
            Faction2Visual = new ImageIdentifierVM(BannerCode.CreateFrom(MomentumGlobals.Mordor.Banner), true);
        }
        private void OpenDetailedBalanceOfPowerView()
        {
            InformationManager.DisplayMessage(new InformationMessage("Factions"));
            new MomentumInterface().ShowInterface(ScreenManager.TopScreen);
        }
    }
}