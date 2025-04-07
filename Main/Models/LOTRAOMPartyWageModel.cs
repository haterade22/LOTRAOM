using LOTRAOM.CultureFeats;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

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
            float value = character.Tier switch
            {
                0 => 1,
                1 => 2,
                2 => 3,
                3 => 5,
                4 => 8,
                5 => 12,
                6 => 17,
                7 => 23,
                8 => 30,
                9 => 38,
                10 => 47,
                _ => 57
            };
            if (character.Occupation == Occupation.Mercenary)
            {
                value = (int)((float)value * 1.5f);
            }
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
            float baseCost = troop.Level switch
            {
                <= 1 => 10,
                <= 6 => 20,
                <= 11 => 50,
                <= 16 => 200,
                <= 21 => 400,
                <= 26 => 600,
                <= 31 => 1000,
                <= 36 => 1500,
                <= 41 => 2100,
                <= 46 => 2800,
                <= 51 => 3600,
                _ => 4000
            };

            bool isMercenary = troop.Occupation == Occupation.Mercenary || troop.Occupation == Occupation.Gangster || troop.Occupation == Occupation.CaravanGuard;
            if (isMercenary)
                baseCost = MathF.Round(baseCost * 2f);

            if (buyerHero == null) return (int)baseCost;

            ExplainedNumber explainedNumber = new ExplainedNumber(1f, false, null);
            if (troop.Tier >= 2 && buyerHero.GetPerkValue(DefaultPerks.Throwing.HeadHunter))
            {
                explainedNumber.AddFactor(DefaultPerks.Throwing.HeadHunter.SecondaryBonus, null);
            }
            if (troop.IsInfantry)
            {
                if (buyerHero.GetPerkValue(DefaultPerks.OneHanded.ChinkInTheArmor))
                {
                    explainedNumber.AddFactor(DefaultPerks.OneHanded.ChinkInTheArmor.SecondaryBonus, null);
                }
                if (buyerHero.GetPerkValue(DefaultPerks.TwoHanded.ShowOfStrength))
                {
                    explainedNumber.AddFactor(DefaultPerks.TwoHanded.ShowOfStrength.SecondaryBonus, null);
                }
                if (buyerHero.GetPerkValue(DefaultPerks.Polearm.HardyFrontline))
                {
                    explainedNumber.AddFactor(DefaultPerks.Polearm.HardyFrontline.SecondaryBonus, null);
                }
                if (buyerHero.Culture.HasFeat(DefaultCulturalFeats.SturgianRecruitUpgradeFeat))
                {
                    explainedNumber.AddFactor(DefaultCulturalFeats.SturgianRecruitUpgradeFeat.EffectBonus, GameTexts.FindText("str_culture", null));
                }
            }
            else if (troop.IsRanged)
            {
                if (buyerHero.GetPerkValue(DefaultPerks.Bow.RenownedArcher))
                {
                    explainedNumber.AddFactor(DefaultPerks.Bow.RenownedArcher.SecondaryBonus, null);
                }
                if (buyerHero.GetPerkValue(DefaultPerks.Crossbow.Piercer))
                {
                    explainedNumber.AddFactor(DefaultPerks.Crossbow.Piercer.SecondaryBonus, null);
                }
            }
            if (troop.IsMounted && buyerHero.Culture.HasFeat(DefaultCulturalFeats.KhuzaitRecruitUpgradeFeat))
            {
                explainedNumber.AddFactor(DefaultCulturalFeats.KhuzaitRecruitUpgradeFeat.EffectBonus, GameTexts.FindText("str_culture", null));
            }
            if (buyerHero.IsPartyLeader && buyerHero.GetPerkValue(DefaultPerks.Steward.Frugal))
            {
                explainedNumber.AddFactor(DefaultPerks.Steward.Frugal.SecondaryBonus, null);
            }
            if (isMercenary)
            {
                if (buyerHero.GetPerkValue(DefaultPerks.Trade.SwordForBarter))
                {
                    explainedNumber.AddFactor(DefaultPerks.Trade.SwordForBarter.PrimaryBonus, null);
                }
                if (buyerHero.GetPerkValue(DefaultPerks.Charm.SlickNegotiator))
                {
                    explainedNumber.AddFactor(DefaultPerks.Charm.SlickNegotiator.PrimaryBonus, null);
                }
            }
            baseCost = MathF.Max(1, MathF.Round((float)baseCost * explainedNumber.ResultNumber));

            if (buyerHero.Culture.HasFeat(LOTRAOMCultureFeats.Instance.mordorRecruitmentFeat))
                baseCost *= LOTRAOMCultureFeats.Instance.mordorRecruitmentFeat.EffectBonus;
            return (int)baseCost;
        }
    }
}
