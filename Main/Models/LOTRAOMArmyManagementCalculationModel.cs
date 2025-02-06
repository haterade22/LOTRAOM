using Helpers;
using LOTRAOM.CultureFeats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;

namespace LOTRAOM.Models
{

    public class LOTRAOMArmyManagementCalculationModel : ArmyManagementCalculationModel
    {
        ArmyManagementCalculationModel previousModel;
        public LOTRAOMArmyManagementCalculationModel(ArmyManagementCalculationModel baseModel)
        {
            previousModel = baseModel;
        }

        public override int InfluenceValuePerGold => previousModel.InfluenceValuePerGold;

        public override int AverageCallToArmyCost => previousModel.AverageCallToArmyCost;

        public override int CohesionThresholdForDispersion => previousModel.CohesionThresholdForDispersion;

        public override ExplainedNumber CalculateDailyCohesionChange(Army army, bool includeDescriptions = false)
        {
            ExplainedNumber value = previousModel.CalculateDailyCohesionChange(army, includeDescriptions);
            return value;
        }

        public override int CalculateNewCohesion(Army army, PartyBase newParty, int calculatedCohesion, int sign)
        {
            int value = previousModel.CalculateNewCohesion(army, newParty, calculatedCohesion, sign);
            return value;
        }

        public override int CalculatePartyInfluenceCost(MobileParty armyLeaderParty, MobileParty party)
        {
            float value = previousModel.CalculatePartyInfluenceCost(armyLeaderParty, party);
            if (PartyBaseHelper.HasFeat(armyLeaderParty.Party, LOTRAOMCultureFeats.Instance.rohanMoreInfluenceToRecruitToArmy))
            {
                value += value * LOTRAOMCultureFeats.Instance.rohanMoreInfluenceToRecruitToArmy.EffectBonus;
            }
            return (int)value;
        }

        public override int CalculateTotalInfluenceCost(Army army, float percentage)
        {
            int value = previousModel.CalculateTotalInfluenceCost(army, percentage);
            return value;
        }

        public override bool CheckPartyEligibility(MobileParty party)
        {
            bool value = previousModel.CheckPartyEligibility(party);
            return value;
        }

        public override float DailyBeingAtArmyInfluenceAward(MobileParty armyMemberParty)
        {
            float value = previousModel.DailyBeingAtArmyInfluenceAward(armyMemberParty);
            if (PartyBaseHelper.HasFeat(armyMemberParty.Party, LOTRAOMCultureFeats.Instance.gondorMoreInfluenceInArmy))
            {
                value += value * LOTRAOMCultureFeats.Instance.gondorMoreInfluenceInArmy.EffectBonus;
            }
            return value;
        }

        public override int GetCohesionBoostGoldCost(Army army, float percentageToBoost = 100)
        {
            int value = previousModel.GetCohesionBoostGoldCost(army, percentageToBoost);
            return value;
        }

        public override int GetCohesionBoostInfluenceCost(Army army, int percentageToBoost = 100)
        {
            int value = previousModel.GetCohesionBoostInfluenceCost(army, percentageToBoost);
            return value;
        }

        public override List<MobileParty> GetMobilePartiesToCallToArmy(MobileParty leaderParty)
        {
            List<MobileParty> value = previousModel.GetMobilePartiesToCallToArmy(leaderParty);
            return value;
        }

        public override int GetPartyRelation(Hero hero)
        {
            int value = previousModel.GetPartyRelation(hero);
            return value;
        }

        public override float GetPartySizeScore(MobileParty party)
        {
            float value = previousModel.GetPartySizeScore(party);
            return value;
        }

        public override int GetPartyStrength(PartyBase party)
        {
            int value = previousModel.GetPartyStrength(party);
            return value;
        }
    }
}
