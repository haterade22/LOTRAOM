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

        [DataSourceProperty]
        public MomentumIndicatorItemVM MomentumIndicator
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
            RefreshValues();
            //CampaignEvents.WarDeclared.AddNonSerializedListener(this, HandleStanceChange);
            //CampaignEvents.MakePeace.AddNonSerializedListener(this, HandleStanceChange);
            //CampaignEvents.ClanChangedKingdom.AddNonSerializedListener(this, (x, _, _, _, _) => HandleClanChangedKingdom(x));
        }

        private void HandleClanChangedKingdom(Clan clan)
        {
            if (Clan.PlayerClan == clan) RefreshValues();
        }
        private void HandleStanceChange(IFaction arg1, IFaction arg2)
        {
            if (Clan.PlayerClan.MapFaction is Kingdom playerKingdom
                && arg1 is Kingdom kingdom1
                && arg2 is Kingdom kingdom2
                && (kingdom1 == playerKingdom || kingdom2 == playerKingdom))
                RefreshValues();
        }

        public override void OnFinalize()
        {
            base.OnFinalize();
            CampaignEvents.WarDeclared.ClearListeners(this);
            CampaignEvents.MakePeace.ClearListeners(this);
        }

        public void UpdateBanners()
        {
            MomentumIndicator.RefreshValues();
        }

        public override void RefreshValues()
        {
            MomentumIndicator = new MomentumIndicatorItemVM();
        }
    }
}