using LOTRAOM.CampaignBehaviors;
using LOTRAOM.Extensions;
using LOTRAOM.Momentum;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.BarterSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Localization;

namespace LOTRAOM.Models
{
    internal class AOMDiplomacyModel : DiplomacyModel
    {

        /*  @TODO To edit
         * GetRelationChangeAfterClanLeaderIsDead????
         */
        private DiplomacyModel baseModel;

        public AOMDiplomacyModel(DiplomacyModel diplomacyModel)
        {
            this.baseModel = diplomacyModel;
        }
        public override int MaxRelationLimit => baseModel.MaxRelationLimit;

        public override int MinRelationLimit => baseModel.MinRelationLimit;

        public override int MaxNeutralRelationLimit => baseModel.MaxNeutralRelationLimit;

        public override int MinNeutralRelationLimit => baseModel.MinNeutralRelationLimit;

        public override int MinimumRelationWithConversationCharacterToJoinKingdom => baseModel.MinimumRelationWithConversationCharacterToJoinKingdom;

        public override int GiftingTownRelationshipBonus => baseModel.GiftingTownRelationshipBonus;
        public override int GiftingCastleRelationshipBonus => baseModel.GiftingCastleRelationshipBonus;
        public override bool CanSettlementBeGifted(Settlement settlement)
        {
            return baseModel.CanSettlementBeGifted(settlement);
        }
        public override float DenarsToInfluence()
        {
            return baseModel.DenarsToInfluence();
        }
        public override IEnumerable<BarterGroup> GetBarterGroups()
        {
            return baseModel.GetBarterGroups();
        }
        public override int GetBaseRelation(Hero hero, Hero hero1)
        {
            return baseModel.GetBaseRelation(hero, hero1);
        }
        public override int GetCharmExperienceFromRelationGain(Hero hero, float relationChange, ChangeRelationAction.ChangeRelationDetail detail)
        {
            return baseModel.GetCharmExperienceFromRelationGain(hero, relationChange, detail);
        }
        public override float GetClanStrength(Clan clan)
        {
            return baseModel.GetClanStrength(clan);
        }
        public override int GetDailyTributeForValue(int value)
        {
            return baseModel.GetDailyTributeForValue(value);
        }
        public override int GetEffectiveRelation(Hero hero, Hero hero1)
        {
            return baseModel.GetEffectiveRelation(hero, hero1);
        }
        public override float GetHeroCommandingStrengthForClan(Hero hero)
        {
            return baseModel.GetHeroCommandingStrengthForClan(hero);
        }
        public override void GetHeroesForEffectiveRelation(Hero hero1, Hero hero2, out Hero effectiveHero1, out Hero effectiveHero2)
        {
            baseModel.GetHeroesForEffectiveRelation(hero1, hero2, out effectiveHero1, out effectiveHero2);
        }
        public override float GetHeroGoverningStrengthForClan(Hero hero)
        {
            return baseModel.GetHeroGoverningStrengthForClan(hero);
        }
        public override float GetHourlyInfluenceAwardForBeingArmyMember(MobileParty mobileParty)
        {
            return baseModel.GetHourlyInfluenceAwardForBeingArmyMember(mobileParty);
        }
        public override float GetHourlyInfluenceAwardForBesiegingEnemyFortification(MobileParty mobileParty)
        {
            return baseModel.GetHourlyInfluenceAwardForBesiegingEnemyFortification(mobileParty);
        }
        public override float GetHourlyInfluenceAwardForRaidingEnemyVillage(MobileParty mobileParty)
        {
            return baseModel.GetHourlyInfluenceAwardForRaidingEnemyVillage(mobileParty);
        }
        public override int GetInfluenceAwardForSettlementCapturer(Settlement settlement)
        {
            return baseModel.GetInfluenceAwardForSettlementCapturer(settlement);
        }
        public override int GetInfluenceCostOfAbandoningArmy()
        {
            return baseModel.GetInfluenceCostOfAbandoningArmy();
        }
        public override int GetInfluenceCostOfAnnexation(Clan proposingClan)
        {
            return baseModel.GetInfluenceCostOfAnnexation(proposingClan);
        }
        public override int GetInfluenceCostOfChangingLeaderOfArmy()
        {
            return baseModel.GetInfluenceCostOfChangingLeaderOfArmy();
        }
        public override int GetInfluenceCostOfDisbandingArmy()
        {
            return baseModel.GetInfluenceCostOfDisbandingArmy();
        }
        public override int GetInfluenceCostOfExpellingClan(Clan proposingClan)
        {
            return baseModel.GetInfluenceCostOfExpellingClan(proposingClan);
        }
        public override int GetInfluenceCostOfPolicyProposalAndDisavowal(Clan proposingClan)
        {
            return baseModel.GetInfluenceCostOfPolicyProposalAndDisavowal(proposingClan);
        }
        public override int GetInfluenceCostOfProposingPeace(Clan proposingClan)
        {
            return baseModel.GetInfluenceCostOfProposingPeace(proposingClan);
        }
        public override int GetInfluenceCostOfProposingWar(Clan proposingClan)
        {
            return baseModel.GetInfluenceCostOfProposingWar(proposingClan);
        }
        public override int GetInfluenceCostOfSupportingClan()
        {
            return baseModel.GetInfluenceCostOfSupportingClan();
        }
        public override int GetInfluenceValueOfSupportingClan()
        {
            return baseModel.GetInfluenceValueOfSupportingClan();
        }
        public override uint GetNotificationColor(ChatNotificationType notificationType)
        {
            return baseModel.GetNotificationColor(notificationType);
        }
        public override int GetRelationChangeAfterClanLeaderIsDead(Hero deadLeader, Hero relationHero)
        {
            return baseModel.GetRelationChangeAfterClanLeaderIsDead(deadLeader, relationHero);
        }
        public override int GetRelationChangeAfterVotingInSettlementOwnerPreliminaryDecision(Hero supporter, bool hasHeroVotedAgainstOwner)
        {
            return baseModel.GetRelationChangeAfterVotingInSettlementOwnerPreliminaryDecision(supporter, hasHeroVotedAgainstOwner);
        }
        public override int GetRelationCostOfDisbandingArmy(bool isLeaderParty)
        {
            return baseModel.GetRelationCostOfDisbandingArmy(isLeaderParty);
        }
        public override int GetRelationCostOfExpellingClanFromKingdom()
        {
            return baseModel.GetRelationCostOfExpellingClanFromKingdom();
        }
        public override float GetRelationIncreaseFactor(Hero hero1, Hero hero2, float relationValue)
        {
            return baseModel.GetRelationIncreaseFactor(hero1, hero2, relationValue);
        }
        public override int GetRelationValueOfSupportingClan()
        {
            return baseModel.GetRelationValueOfSupportingClan();
        }
        public override float GetScoreOfClanToJoinKingdom(Clan clan, Kingdom kingdom)
        {
            if (clan.Culture.IsGoodCulture() && kingdom.Culture.IsEvilCulture() ||
                clan.Culture.IsEvilCulture() && kingdom.Culture.IsGoodCulture())
                return float.MinValue;
            return baseModel.GetScoreOfClanToJoinKingdom(clan, kingdom);
        }
        public override float GetScoreOfClanToLeaveKingdom(Clan clan, Kingdom kingdom)
        {
            return baseModel.GetScoreOfClanToLeaveKingdom(clan, kingdom);
        }
        public override float GetScoreOfDeclaringPeace(IFaction factionDeclaresPeace, IFaction factionDeclaredPeace, IFaction evaluatingFaction, out TextObject reason)
        {
            WarOfTheRingData data = MomentumCampaignBehavior.Instance.warOfTheRingData;
            return baseModel.GetScoreOfDeclaringPeace(factionDeclaresPeace, factionDeclaredPeace, evaluatingFaction, out reason);
        }
        public override float GetScoreOfDeclaringWar(IFaction factionDeclaresWar, IFaction factionDeclaredWar, IFaction evaluatingFaction, out TextObject reason)
        {
            float value = baseModel.GetScoreOfDeclaringWar(factionDeclaresWar, factionDeclaredWar, evaluatingFaction, out reason);
            AoMDiplomacy.EvilFactionsDaysWithoutWar.TryGetValue(factionDeclaresWar.StringId, out int daysWithoutWar);
            value += daysWithoutWar * 10000;
            if (factionDeclaresWar.Culture.StringId == Globals.MordorCulture && factionDeclaredWar.Culture.StringId == Globals.GondorCulture && factionDeclaredWar.Culture.StringId == Globals.RohanCulture)
                value = int.MaxValue;
            return value;
        }
        public override float GetScoreOfKingdomToGetClan(Kingdom kingdom, Clan clan)
        {
            if (kingdom.Culture.IsGoodCulture() && clan.Culture.IsEvilCulture() ||
                kingdom.Culture.IsEvilCulture() && clan.Culture.IsGoodCulture())
                return float.MinValue;
            return baseModel.GetScoreOfKingdomToGetClan(kingdom, clan);
        }
        public override float GetScoreOfKingdomToHireMercenary(Kingdom kingdom, Clan mercenaryClan)
        {
            if (kingdom.Culture.IsGoodCulture() && mercenaryClan.Culture.IsEvilCulture() ||
                kingdom.Culture.IsEvilCulture() && mercenaryClan.Culture.IsGoodCulture())
                return float.MinValue;
            return baseModel.GetScoreOfKingdomToHireMercenary(kingdom, mercenaryClan);
        }
        public override float GetScoreOfKingdomToSackClan(Kingdom kingdom, Clan clan)
        {
            return baseModel.GetScoreOfKingdomToSackClan(kingdom, clan);
        }
        public override float GetScoreOfKingdomToSackMercenary(Kingdom kingdom, Clan mercenaryClan)
        {
            return baseModel.GetScoreOfKingdomToSackMercenary(kingdom, mercenaryClan);
        }
        public override float GetScoreOfLettingPartyGo(MobileParty party, MobileParty partyToLetGo)
        {
            if (party.Owner.Culture.IsGoodCulture() && partyToLetGo.Owner.Culture.IsEvilCulture() ||
                party.Owner.Culture.IsEvilCulture() && partyToLetGo.Owner.Culture.IsGoodCulture())
                    return float.MinValue;
            return baseModel.GetScoreOfLettingPartyGo(party, partyToLetGo);
        }
        public override float GetScoreOfMercenaryToJoinKingdom(Clan clan, Kingdom kingdom)
        {
            if (clan.Culture.IsGoodCulture() && kingdom.Culture.IsEvilCulture() ||
                clan.Culture.IsEvilCulture() && kingdom.Culture.IsGoodCulture())
                return float.MinValue;
            return baseModel.GetScoreOfMercenaryToJoinKingdom(clan, kingdom);
        }
        public override float GetScoreOfMercenaryToLeaveKingdom(Clan clan, Kingdom kingdom)
        {
            return baseModel.GetScoreOfMercenaryToLeaveKingdom(clan, kingdom);
        }
        public override float GetStrengthThresholdForNonMutualWarsToBeIgnoredToJoinKingdom(Kingdom kingdomToJoin)
        {
            return baseModel.GetStrengthThresholdForNonMutualWarsToBeIgnoredToJoinKingdom(kingdomToJoin);
        }
        public override int GetValueOfDailyTribute(int dailyTributeAmount)
        {
            return baseModel.GetValueOfDailyTribute(dailyTributeAmount);
        }
        public override float GetValueOfHeroForFaction(Hero examinedHero, IFaction targetFaction, bool forMarriage = false)
        {
            return baseModel.GetValueOfHeroForFaction(examinedHero, targetFaction, forMarriage);
        }
        public override bool IsClanEligibleToBecomeRuler(Clan clan)
        {
            return baseModel.IsClanEligibleToBecomeRuler(clan);
        }
    }
}