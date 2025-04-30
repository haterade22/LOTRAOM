using LOTRAOM.Extensions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Localization;

namespace LOTRAOM.Momentum.Effects
{
    public enum MomentumBuffType
    {
        Continuous,
        OneTime,
    }

    public class MoraleMomentumBuff : MomentumBuff
    {
        public override TextObject Text => new("From Momentum");

        public MoraleMomentumBuff() : base(MomentumBuffType.Continuous, 0, 0, 10) { }

        public override void Consequence(object[] args)
        {
            ExplainedNumber number = (ExplainedNumber)args[0];
            number.Add(GetEffect(), Text);
        }
        public override bool Condition(object[] args)
        {
            MobileParty party = (MobileParty)args[0];
            Hero owner = party.Owner;
            if (owner == null) return false;
            bool isGoodWinning = MomentumCampaignBehavior.Instance.warOfTheRingData.Momentum > 0;
            if (isGoodWinning && owner.Culture.IsGoodCulture()) return true;
            if (!isGoodWinning && owner.Culture.IsEvilCulture()) return true;
            return false;
        }
    }
}
