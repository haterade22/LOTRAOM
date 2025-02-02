using LOTRAOM.CultureFeats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Diamond;
using TaleWorlds.Library;

namespace LOTRAOM.Models
{
    internal class LOTRAOMPartySizeModel : PartySizeLimitModel
    {
        PartySizeLimitModel defaultPartySizeModel;
        public LOTRAOMPartySizeModel(PartySizeLimitModel defaultModel)
        {
            defaultPartySizeModel = defaultModel;
        }
        private float MordorPartySizeMultiplier => CultureFeatsGlobals.MordorPartySizeMultiplier;
        public override int GetAssumedPartySizeForLordParty(Hero leaderHero, IFaction partyMapFaction, Clan actualClan)
        {
            int baseValue = defaultPartySizeModel.GetAssumedPartySizeForLordParty(leaderHero, partyMapFaction, actualClan);
            baseValue = AddSizeModifiers(actualClan, baseValue);
            return baseValue;
        }
        private int AddSizeModifiers(Clan clan, int baseValue)
        {
            double value = baseValue;
            if (Globals.IsMordor(clan.Culture.StringId)) value *= MordorPartySizeMultiplier;
            return (int)value;
        }

        private void AddSizeModifiers(PartyBase party, ref ExplainedNumber size)
        {
            if (Globals.IsMordor(party.Culture.StringId)) size.AddFactor(MordorPartySizeMultiplier, new("Mordor party size bonus."));
        }
        public override ExplainedNumber GetPartyMemberSizeLimit(PartyBase party, bool includeDescriptions = false)
        {
            ExplainedNumber baseValue = defaultPartySizeModel.GetPartyMemberSizeLimit(party, includeDescriptions);
            AddSizeModifiers(party, ref baseValue);
            return baseValue;
        }

        public override ExplainedNumber GetPartyPrisonerSizeLimit(PartyBase party, bool includeDescriptions = false)
        {
            return defaultPartySizeModel.GetPartyPrisonerSizeLimit(party, includeDescriptions);
        }

        public override int GetTierPartySizeEffect(int tier)
        {
            return defaultPartySizeModel.GetTierPartySizeEffect(tier);
        }
    }
}
