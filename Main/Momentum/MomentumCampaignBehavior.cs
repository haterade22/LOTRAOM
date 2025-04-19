using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;
namespace LOTRAOM.Momentum
{
    public class MomentumCampaignBehavior : CampaignBehaviorBase
    {
        public Action OnMomentumChanged;
        public static MomentumCampaignBehavior Instance { get { return Campaign.Current.GetCampaignBehavior<MomentumCampaignBehavior>(); } }
        [SaveableField(0)] private WarOfTheRingData _warOfTheRingData;
        public WarOfTheRingData WarOfTheRingdata
        {
            get
            {
                _warOfTheRingData ??= new WarOfTheRingData();
                return _warOfTheRingData;
            }
        }
        public override void RegisterEvents()
        {
            CampaignEvents.ArmyDispersed.AddNonSerializedListener(this, OnArmyCreated);
            CampaignEvents.ArmyGathered.AddNonSerializedListener(this, OnArmyGathered);
        }

        private void OnArmyGathered(Army army, Settlement settlement)
        {
            if (!WarOfTheRingdata.DoesFactionTakePartInWar(army.Kingdom)) return;
            WarOfTheRingdata.AddEvent(army.Kingdom, MomentumActionType.ArmyGathered, new MomentumEvent(20, new TextObject($"Army led by {army.ArmyOwner.Name} has gathered"), MomentumActionType.ArmyGathered, GetEventEndTime(MomentumActionType.ArmyGathered)));
            OnMomentumChanged.Invoke();
        }

        private void OnArmyCreated(Army army, Army.ArmyDispersionReason reason, bool arg3)
        {
        }

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("_warOfTheRingData", ref _warOfTheRingData);
        }
        public static CampaignTime GetEventEndTime(MomentumActionType type)
        {
            return CampaignTime.DaysFromNow(2);
        }
    }
}
