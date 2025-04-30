using TaleWorlds.CampaignSystem;
using TaleWorlds.SaveSystem;


namespace LOTRAOM.Events
{
    public class DelayedDiplomaticEvent
    {
        [SaveableField(0)] public string StringId;
        [SaveableField(1)] public CampaignTime delayedActionTime;
        public DelayedDiplomaticEvent(string stringId, CampaignTime delayedActionTime)
        {
            StringId = stringId;
            this.delayedActionTime = delayedActionTime;
        }
    }
}