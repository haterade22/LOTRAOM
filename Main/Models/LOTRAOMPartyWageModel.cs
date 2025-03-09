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
        public float MordorWageMultiplier => LOTRAOMCultureFeats.Instance.mordorWageMultiplierFeat.EffectBonus;
        public double MordorRecruitmentMulitpler => LOTRAOMCultureFeats.Instance.mordorRecruitmentFeat.EffectBonus;
        

        public override int MaxWage { get { return 1000; } }

        public override int GetCharacterWage(CharacterObject character)
        {
            float value = defaultPartyWageModel.GetCharacterWage(character);
            if (character.IsMounted && !character.Culture.HasFeat(LOTRAOMCultureFeats.Instance.rohanNoExtraWageForMounted))
                value += value * Globals.MountedTroopWageMultiplier;
            if (character.IsInfantry && character.Culture.HasFeat(LOTRAOMCultureFeats.Instance.gondorReduceInfantryWages)) 
                value -= value * LOTRAOMCultureFeats.Instance.gondorReduceInfantryWages.EffectBonus;
            return (int)value;
        }

        public override ExplainedNumber GetTotalWage(MobileParty mobileParty, bool includeDescriptions = false)
        {
            ExplainedNumber wage = defaultPartyWageModel.GetTotalWage(mobileParty, includeDescriptions);
            if (mobileParty.Party.Culture.HasFeat(LOTRAOMCultureFeats.Instance.mordorWageMultiplierFeat))
                wage.Add(wage.ResultNumber * -LOTRAOMCultureFeats.Instance.mordorWageMultiplierFeat.EffectBonus, new("{=mordor_wage_reduction}Mordor wage reduction")); //the text does not seem to be shown anywhere?
            if (mobileParty.IsGarrison && mobileParty.Party.Culture.HasFeat(LOTRAOMCultureFeats.Instance.gondorReduceWagesInGarrisons))
                wage.AddFactor(-LOTRAOMCultureFeats.Instance.gondorReduceWagesInGarrisons.EffectBonus, new("{=gondor_garrison_wage}Gondor garrison wage reduction"));
            return wage;
        }

        public override int GetTroopRecruitmentCost(CharacterObject troop, Hero buyerHero, bool withoutItemCost = false)
        {
            int baseCost = defaultPartyWageModel.GetTroopRecruitmentCost(troop, buyerHero, withoutItemCost);
            if (buyerHero == null) return baseCost;
            double realCost = baseCost;
            if (buyerHero.Culture.HasFeat(LOTRAOMCultureFeats.Instance.mordorRecruitmentFeat))
                realCost *= LOTRAOMCultureFeats.Instance.mordorRecruitmentFeat.EffectBonus;
            return (int)realCost;
        }
    }
}
