using System.Linq;
using System.Text.RegularExpressions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;

namespace LOTRAOM.Momentum.ViewModel
{
    internal sealed class MomentumIndicatorVM : TaleWorlds.Library.ViewModel
    {

        private MomentumIndicatorItemVM _balanceOfPowerView;

        [DataSourceProperty]
        public MomentumIndicatorItemVM OneKingdom
        {
            get => _balanceOfPowerView;
            set
            {
                if (value != _balanceOfPowerView)
                {
                    _balanceOfPowerView = value;
                    OnPropertyChanged(nameof(OneKingdom));
                }
            }
        }
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
            OneKingdom.RefreshValues();
        }

        public override void RefreshValues()
        {
            OneKingdom = new MomentumIndicatorItemVM();
        }
    }
}