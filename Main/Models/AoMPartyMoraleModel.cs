using LOTRAOM.Momentum;
using LOTRAOM.Momentum.Effects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;

namespace LOTRAOM.Models
{
    internal class AoMPartyMoraleModel : DefaultPartyMoraleModel
    {
        public override ExplainedNumber GetEffectivePartyMorale(MobileParty mobileParty, bool includeDescription = false)
        {
            ExplainedNumber value = base.GetEffectivePartyMorale(mobileParty, includeDescription);
            if (!AoMSettings.Instance.Momentum 
                || !MomentumCampaignBehavior.Instance.warOfTheRingData.HasWarStarted
                || MomentumCampaignBehavior.Instance.warOfTheRingData.HasWarEnded)
                return value;
            if (BuffHolder.Instance.MoraleBuff.Condition(new object[] { mobileParty}))
                BuffHolder.Instance.MoraleBuff.Consequence(new object[] { value });
            return value;
        }
    }
}
