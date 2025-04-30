using LOTRAOM.CultureFeats;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace LOTRAOM.Models
{
    public class LOTRAOMPartyWageModel : PartyWageModel
    {
        private readonly PartyWageModel _defaultPartyWageModel;
        private const float MountedTroopWageMultiplier = 0.5f; // Assume 50% extra wage for mounted units

        public LOTRAOMPartyWageModel(PartyWageModel previousModel)
        {
            _defaultPartyWageModel = previousModel;
        }

        public float MordorWageMultiplier => LOTRAOMCultureFeats.Instance.mordorWageMultiplierFeat?.EffectBonus ?? 0f;
        public float MordorRecruitmentMultiplier => LOTRAOMCultureFeats.Instance.mordorRecruitmentFeat?.EffectBonus ?? 1f;
        public float GondorInfantryWageReduction => LOTRAOMCultureFeats.Instance.gondorReduceInfantryWages?.EffectBonus ?? 0f;
        public float GondorGarrisonWageReduction => LOTRAOMCultureFeats.Instance.gondorReduceWagesInGarrisons?.EffectBonus ?? 0f;
        public float ElfArcherWageReduction => 0.15f; // Hardcoded for now; can be moved to LOTRAOMCultureFeats.cs
        public float EreborEliteWageIncrease => 0.1f; // Hardcoded for now; can be moved to LOTRAOMCultureFeats.cs

        public override int MaxWage => 1000;

        public override int GetCharacterWage(CharacterObject character)
        {
            if (character == null) return 0;

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
                value *= 1.5f;
            }

            if (character.IsMounted && (character.Culture?.HasFeat(LOTRAOMCultureFeats.Instance.rohanNoExtraWageForMounted) != true))
            {
                value += value * MountedTroopWageMultiplier;
            }

            if (character.IsInfantry && character.Culture?.HasFeat(LOTRAOMCultureFeats.Instance.gondorReduceInfantryWages) == true)
            {
                value *= (1f - GondorInfantryWageReduction);
            }

            if (character.IsRanged && character.Culture?.StringId is "rivendell" or "mirkwood" or "lothlorien")
            {
                value *= (1f - ElfArcherWageReduction);
            }

            if (character.Tier >= 4 && character.Culture?.StringId == "erebor")
            {
                value *= (1f + EreborEliteWageIncrease);
            }

            return (int)value;
        }

        public override ExplainedNumber GetTotalWage(MobileParty mobileParty, bool includeDescriptions = false)
        {
            ExplainedNumber wage = _defaultPartyWageModel.GetTotalWage(mobileParty, includeDescriptions);

            if (mobileParty?.Party?.Culture != null)
            {
                if (mobileParty.Party.Culture.HasFeat(LOTRAOMCultureFeats.Instance.mordorWageMultiplierFeat))
                {
                    wage.AddFactor(-MordorWageMultiplier, new TextObject("{=mordor_wage_multiplier}Mordor Wage Efficiency"));
                }
                if (mobileParty.IsGarrison && mobileParty.Party.Culture.HasFeat(LOTRAOMCultureFeats.Instance.gondorReduceWagesInGarrisons))
                {
                    wage.AddFactor(-GondorGarrisonWageReduction, new TextObject("{=gondor_reduce_wages_in_garrison}Gondorian Garrison Duty"));
                }
            }

            return wage;
        }

        public override int GetTroopRecruitmentCost(CharacterObject troop, Hero buyerHero, bool withoutItemCost = false)
        {
            if (troop == null) return 0;

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

            bool isMercenary = troop.Occupation is Occupation.Mercenary or Occupation.Gangster or Occupation.CaravanGuard;
            if (isMercenary)
            {
                baseCost *= 2f;
            }

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

            baseCost *= explainedNumber.ResultNumber;

            if (buyerHero.Culture?.HasFeat(LOTRAOMCultureFeats.Instance.mordorRecruitmentFeat) == true)
            {
                explainedNumber.AddFactor(-(1f - MordorRecruitmentMultiplier), new TextObject("{=mordor_recruitment}Orc Horde Recruitment"));
                baseCost *= MordorRecruitmentMultiplier;
            }

            return (int)MathF.Max(1f, baseCost); 
        }
    }
}