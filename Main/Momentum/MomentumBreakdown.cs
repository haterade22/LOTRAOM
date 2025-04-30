using System.Collections;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Localization;

namespace LOTRAOM.Momentum
{
    public class MomentumBreakdown
    {
        public CampaignTime EndTime { get; }
        public ExplainedNumber MomentumGoodSideValue;
        public ExplainedNumber MomentumEvilSideValue;
        public MomentumActionType MomentumActionType { get; }
        public MomentumBreakdown(MomentumActionType type, Queue<MomentumEvent> goodSideEvents, Queue<MomentumEvent> evilSideEvents)
        {
            MomentumGoodSideValue = new(0, true);
            MomentumEvilSideValue = new(0, true);
            MomentumActionType = type;
            foreach (var goodSideEvent in goodSideEvents)
            {
                MomentumGoodSideValue.Add(MomentumGlobals.ScaleMomentum(goodSideEvent.MomentumValue), goodSideEvent.Description);
            }
            foreach (var evilSideEvent in evilSideEvents)
            {
                MomentumEvilSideValue.Add(MomentumGlobals.ScaleMomentum(evilSideEvent.MomentumValue), evilSideEvent.Description);
            }
        }
    }
    public enum MomentumActionType
    {
        ArmyGathered,
        BattleWon,
        VillageRaided,
        Sieges,
        RelativeStrength
    }
}