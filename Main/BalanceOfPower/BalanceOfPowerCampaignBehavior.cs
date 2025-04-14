using LOTRAOM.BalanceOfPower.Views;
using SandBox.View.Map;

using TaleWorlds.CampaignSystem;

namespace LOTRAOM.BalanceOfPower
{
    internal sealed class BalanceOfPowerCampaignBehavior : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            CampaignEvents.TickEvent.AddNonSerializedListener(this, AddUIElements);

        }
        private void AddUIElements(float obj)
        {
            if (AoMSettings.Instance.BalanceOfPower)
                MapScreen.Instance.AddMapView<BalanceOfPowerIndicator>();
            CampaignEvents.TickEvent.ClearListeners(this);
        }
        public override void SyncData(IDataStore dataStore) { }
    }
}