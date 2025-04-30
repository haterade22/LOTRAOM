using TaleWorlds.CampaignSystem;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace LOTRAOM.Momentum
{
    public class MomentumEvent
    {
        [SaveableField(0)] public CampaignTime _endTime;
        [SaveableField(1)] public int _momentumValue;
        [SaveableField(2)] public TextObject _description;
        [SaveableField(3)] MomentumActionType _momentumActionType;
        public CampaignTime EndTime { get { return _endTime; } }
        public int MomentumValue { get { return _momentumValue; } }
        public TextObject Description { get { return _description; } }
        MomentumActionType MomentumActionType { get { return _momentumActionType; } }
        public MomentumEvent(int value, TextObject description, MomentumActionType type, CampaignTime endTime)
        {
            _momentumValue = value;
            _description = description;
            _momentumActionType = type;
            _endTime = endTime;
        }
    }
}