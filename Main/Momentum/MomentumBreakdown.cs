using TaleWorlds.CampaignSystem;
using TaleWorlds.Localization;

namespace LOTRAOM.Momentum
{
    public class MomentumEvent
    {
        public CampaignTime EndTime { get; }
        public int MomentumValue { get; }
        public TextObject Description { get; }
        MomentumActionType MomentumActionType { get; }
        public MomentumEvent(int value, TextObject description, MomentumActionType type, CampaignTime endTime)
        {
            MomentumValue = value;
            Description = description;
            MomentumActionType = type;
        }
    }
    public enum MomentumActionType
    {
        ArmyGathered,
        Casualty,
        Raid,
        Siege,
        Daily,
        Occupied
    }
    public struct MomentumBreakdown
    {
        public MomentumActionType Type { init; get; }
        public int ValueFaction1 { init; get; }
        public int ValueFaction2 { init; get; }
        public float BalanceOfPowerFraction1 { init; get; }
        public float BalanceOfPowerFraction2 { init; get; }
    }
}