using LOTRAOM.CultureFeats;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace LOTRAOM.Models
{
    internal class LOTRAOMPartySizeModel : PartySizeLimitModel
    {
        private readonly PartySizeLimitModel _defaultPartySizeModel;

        public LOTRAOMPartySizeModel(PartySizeLimitModel defaultModel)
        {
            _defaultPartySizeModel = defaultModel;
        }

        private float MordorPartySizeMultiplier => LOTRAOMCultureFeats.Instance.mordorPartySizeFeat?.EffectBonus ?? 0f;
        private float GondorPartySizeMultiplier => LOTRAOMCultureFeats.Instance.gondorMoreInfluenceInArmy?.EffectBonus ?? 0f; // Reusing influence feat for disciplined armies
        private float RohanPartySizeMultiplier => 0.15f; // New feat for Rohan cavalry bands

        public override int GetAssumedPartySizeForLordParty(Hero leaderHero, IFaction partyMapFaction, Clan actualClan)
        {
            ExplainedNumber size = new ExplainedNumber(_defaultPartySizeModel.GetAssumedPartySizeForLordParty(leaderHero, partyMapFaction, actualClan), false);
            AddSizeModifiers(actualClan, ref size);
            return (int)size.ResultNumber;
        }

        private void AddSizeModifiers(Clan clan, ref ExplainedNumber size)
        {
            if (clan == null || clan.Culture == null) return;

            if (clan.Culture.HasFeat(LOTRAOMCultureFeats.Instance.mordorPartySizeFeat))
            {
                size.AddFactor(MordorPartySizeMultiplier, new TextObject("{=mordor_party_size}Mordor Swarm"));
            }
            if (clan.Culture.HasFeat(LOTRAOMCultureFeats.Instance.gondorMoreInfluenceInArmy))
            {
                size.AddFactor(GondorPartySizeMultiplier * 0.5f, new TextObject("{=gondor_army_influence}Gondorian Discipline")); // 12.5% bonus
            }
            if (clan.Culture.StringId == "vlandia")
            {
                size.AddFactor(RohanPartySizeMultiplier, new TextObject("{=rohan_cavalry_size}Rohirrim Cavalry Bands"));
            }
        }

        private void AddSizeModifiers(PartyBase party, ref ExplainedNumber size)
        {
            if (party == null || party.Culture == null) return;

            if (party.Culture.HasFeat(LOTRAOMCultureFeats.Instance.mordorPartySizeFeat))
            {
                size.AddFactor(MordorPartySizeMultiplier, new TextObject("{=mordor_party_size}Mordor Swarm"));
            }
            if (party.Culture.HasFeat(LOTRAOMCultureFeats.Instance.gondorMoreInfluenceInArmy))
            {
                size.AddFactor(GondorPartySizeMultiplier * 0.5f, new TextObject("{=gondor_army_influence}Gondorian Discipline"));
            }
            if (party.Culture.StringId == "vlandia")
            {
                size.AddFactor(RohanPartySizeMultiplier, new TextObject("{=rohan_cavalry_size}Rohirrim Cavalry Bands"));
            }
        }

        public override ExplainedNumber GetPartyMemberSizeLimit(PartyBase party, bool includeDescriptions = false)
        {
            ExplainedNumber size = _defaultPartySizeModel.GetPartyMemberSizeLimit(party, includeDescriptions);
            AddSizeModifiers(party, ref size);
            return size;
        }

        public override ExplainedNumber GetPartyPrisonerSizeLimit(PartyBase party, bool includeDescriptions = false)
        {
            return _defaultPartySizeModel.GetPartyPrisonerSizeLimit(party, includeDescriptions);
        }

        public override int GetTierPartySizeEffect(int tier)
        {
            return _defaultPartySizeModel.GetTierPartySizeEffect(tier);
        }
    }
}