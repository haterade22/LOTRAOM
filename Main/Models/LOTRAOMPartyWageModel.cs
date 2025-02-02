using LOTRAOM.CultureFeats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;

namespace LOTRAOM.Models
{
    public class LOTRAOMPartyWageModel : PartyWageModel
    {
        private PartyWageModel defaultPartyWageModel;
        public LOTRAOMPartyWageModel(PartyWageModel previousModel)
        {
            defaultPartyWageModel = previousModel;
        }
        public float MordorWageMultiplier => CultureFeatsGlobals.MordorWageMultiplier;
        public double MordorRecruitmentMulitpler => CultureFeatsGlobals.MordorRecruitmentMulitpler;
        

        public override int MaxWage { get { return 1000; } }

        public override int GetCharacterWage(CharacterObject character)
        {
            return defaultPartyWageModel.GetCharacterWage(character);
        }

        public override ExplainedNumber GetTotalWage(MobileParty mobileParty, bool includeDescriptions = false)
        {
            ExplainedNumber wage = defaultPartyWageModel.GetTotalWage(mobileParty, includeDescriptions);
            if (Globals.IsMordor(mobileParty.Party.Culture.StringId))
                wage.Add(wage.ResultNumber * -MordorWageMultiplier, new("{=mordor_wage_reduction}Mordor wage reduction")); //the text does not seem to be shown anywhere?
            return wage;
        }

        public override int GetTroopRecruitmentCost(CharacterObject troop, Hero buyerHero, bool withoutItemCost = false)
        {
            int baseCost = defaultPartyWageModel.GetTroopRecruitmentCost(troop, buyerHero, withoutItemCost);
            if (buyerHero == null) return baseCost;
            double realCost = baseCost;
            if (Globals.IsMordor(buyerHero.Culture.StringId))
                realCost *= MordorRecruitmentMulitpler;
            return (int)realCost;
        }
    }
}
