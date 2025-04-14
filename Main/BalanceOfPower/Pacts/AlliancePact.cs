using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace LOTRAOM.BalanceOfPower.Pacts
{
    internal sealed class DeclareAllianceAction : AbstractDiplomaticPact<DeclareAllianceAction>
    {
        public override bool PassesConditions(Kingdom kingdom, Kingdom otherKingdom, bool forcePlayerCosts = false, bool bypassCosts = false)
        {
            return true;
            //return AbstractConditionEvaluator<FormAllianceConditions>.Instance.CanApply(kingdom, kingdom, forcePlayerCosts, bypassCosts);
        }
        protected override void ApplyInternal(Kingdom proposingKingdom, Kingdom otherKingdom, float? customDurationInDays)
        {
            //LoggerExtensions.LogTrace(LogFactory.Get<DeclareAllianceAction>(), string.Format("[{0}] {1} secured an alliance with {2}.", CampaignTime.Now, proposingKingdom.Name, otherKingdom.Name), Array.Empty<object>());
            FactionManager.DeclareAlliance(proposingKingdom, otherKingdom);
            DiplomacyEvents.Instance.OnAllianceFormed(new AllianceEvent(proposingKingdom, otherKingdom));
        }
        protected override void AssessCosts(Kingdom proposingKingdom, Kingdom otherKingdom, bool forcePlayerCosts)
        {
            //DiplomacyCostCalculator.DetermineCostForFormingAlliance(proposingKingdom, otherKingdom, forcePlayerCosts).ApplyCost();
        }
        protected override void ShowPlayerInquiry(Kingdom proposingKingdom, Action acceptAction)
        {
            _TInquiry.SetTextVariable("KINGDOM", proposingKingdom.Name);
            _TInquiry.SetTextVariable("PLAYER_KINGDOM", Clan.PlayerClan.Kingdom.Name);
            InformationManager.ShowInquiry(new InquiryData(_TAllianceProposal.ToString(), _TInquiry.ToString(), true, true, new TextObject("{=Y94H6XnK}Accept", null).ToString(), new TextObject("{=cOgmdp9e}Decline", null).ToString(), acceptAction, null, "", 0f, null, null, null), true, false);
        }
        private static readonly TextObject _TInquiry = new TextObject("{=QbOqatd7}{KINGDOM} is proposing an alliance with {PLAYER_KINGDOM}.", null);
        private static readonly TextObject _TAllianceProposal = new TextObject("{=3pbwc8sh}Alliance Proposal", null);
    }
}
