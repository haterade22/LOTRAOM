using System.Collections;
using System.Collections.Generic;
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
            EndTime = endTime;
        }
    }
    public class MomentumTempBreakdown
    {
        public CampaignTime EndTime { get; }
        public ExplainedNumber MomentumGoodSideValue;
        public ExplainedNumber MomentumEvilSideValue;
        public TextObject Description { get; }
        public MomentumActionType MomentumActionType { get; }
        public MomentumTempBreakdown(MomentumActionType type, Queue<MomentumEvent> goodSideEvents, Queue<MomentumEvent> evilSideEvents)
        {
            MomentumGoodSideValue = new(0, true);
            MomentumEvilSideValue = new(0, true);
            MomentumActionType = type;
            foreach (var goodSideEvent in goodSideEvents)
            {
                //MomentumGoodSideValue.Add(30, new TextObject("test"));
                MomentumGoodSideValue.Add((float)goodSideEvent.MomentumValue, goodSideEvent.Description);
            }
            foreach (var evilSideEvent in evilSideEvents)
            {
                MomentumEvilSideValue.Add((float)evilSideEvent.MomentumValue, evilSideEvent.Description);
            }
        }
    }
    public enum MomentumActionType
    {
        ArmyGathered,
        BattleWon,
        Casualty,
        Raid,
        Siege,
        Occupied
    }
    //public struct MomentumBreakdown
    //{
    //    public MomentumActionType Type { init; get; }
    //    public int ValueFaction1 { init; get; }
    //    public int ValueFaction2 { init; get; }
    //    public float BalanceOfPowerFraction1 { init; get; }
    //    public float BalanceOfPowerFraction2 { init; get; }
    //}
}