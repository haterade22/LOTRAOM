using System.Reflection;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace LOTRAOM.DoubleSiege
{
    internal class CustomSiegeMissionLogic : MissionLogic
    {
        public static bool HasFinishedTheSecondSiege = false;

        private static readonly PropertyInfo _battleStateProperty = typeof(MapEvent).GetProperty("BattleState", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        public override void AfterStart()
        {
            _battleStateProperty.SetValue(MobileParty.MainParty.MapEvent, BattleState.None);
        }

        protected override void OnEndMission()
        {
            DoubleSiegeCampaignBehavior.Instance.HasFinishedTheSecondSiege = true;
            base.OnEndMission();
        }
    }
}
