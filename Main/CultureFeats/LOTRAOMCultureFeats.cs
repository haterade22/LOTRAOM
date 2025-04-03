using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;

namespace LOTRAOM.CultureFeats
{
    public class LOTRAOMCultureFeats
    {
        private static LOTRAOMCultureFeats _instance = new();
        public static LOTRAOMCultureFeats Instance 
        {
            get
            {
                _instance ??= new LOTRAOMCultureFeats();
                return _instance;
            }
        }
        public FeatObject exampleFeat;
        public FeatObject mordorWageMultiplierFeat;
        public FeatObject mordorRecruitmentFeat;
        public FeatObject mordorPartySizeFeat;

        public FeatObject gondorMoreInfluenceInArmy;
        public FeatObject gondorReduceInfantryWages;
        public FeatObject humanPlusMilitiaProduction;
        public FeatObject gondorReduceWagesInGarrisons;

        public FeatObject rohanNoExtraWageForMounted;
        public FeatObject rohanAnimalProductionBonus;
        public FeatObject rohanSlowerBuildRate;
        public FeatObject rohanMoreInfluenceToRecruitToArmy;
        public LOTRAOMCultureFeats()
        {
            exampleFeat = Create("feat name");
            mordorWageMultiplierFeat = Create("mordor_wage_multiplier");
            mordorRecruitmentFeat = Create("mordor_recruitment");
            mordorPartySizeFeat = Create("mordor_party_size");


            humanPlusMilitiaProduction = Create("human_militia_production");

            gondorMoreInfluenceInArmy = Create("gondor_army_influence");
            gondorReduceInfantryWages = Create("gondor_less_infantry_cost");
            gondorReduceWagesInGarrisons = Create("gondor_reduce_wages_in_garrison");

            rohanAnimalProductionBonus = Create("rohan_animal_production");
            rohanNoExtraWageForMounted = Create("rohan_reduce_mounted_cost");
            rohanSlowerBuildRate = Create("rohan_slower_build");
            rohanMoreInfluenceToRecruitToArmy = Create("rohan_army_lord_influence");
            InitializeAll();
        }
        private static FeatObject Create(string stringId)
        {
            return Game.Current.ObjectManager.RegisterPresumedObject(new FeatObject(stringId));
        }
        private void InitializeAll()
        {
            exampleFeat.Initialize("{=!}loc", "{=localisation_string_id}repeated english localisation text for no reason, but idk, TaleWorlds does it this way", 0.20f, true, FeatObject.AdditionType.AddFactor);
            mordorWageMultiplierFeat.Initialize("{=!}mordor_wage_multiplier", "{=mordor_wage_multiplier_desc}20% lower troop wages", 0.2f, true, FeatObject.AdditionType.AddFactor);
            mordorRecruitmentFeat.Initialize("{=!}mordor_recruitment", "{=mordor_recruitment_desc}20% lower recruitment cost", 0.8f, true, FeatObject.AdditionType.AddFactor);
            mordorPartySizeFeat.Initialize("{=!}mordor_party_size", "{=mordor_party_size_desc}30% bigger party size", 0.3f, true, FeatObject.AdditionType.AddFactor);

            humanPlusMilitiaProduction.Initialize("{=}human_militia_production", "{=human_militia_production_desc}Towns owned by a human culture rulers have +1 militia production", 1f, true, FeatObject.AdditionType.Add);

            gondorMoreInfluenceInArmy.Initialize("{=}gondor_influence_army", "{=gondor_influence_army_desc}Being in army brings 25% more influence", 0.25f, true, FeatObject.AdditionType.AddFactor);
            gondorReduceInfantryWages.Initialize("{=}gondor_infantry_wage", "{=gondor_infantry_wage_desc}Wages of gondorian infantry are 15% lower", 0.15f, true, FeatObject.AdditionType.AddFactor);
            gondorReduceWagesInGarrisons.Initialize("{=}gondor_reduce_wages_in_garrison", "{=gondor_reduce_wages_in_garrison_desc}Wages of troops in garrisons are 20% lower", 0.20f, true, FeatObject.AdditionType.AddFactor);

            rohanAnimalProductionBonus.Initialize("{=!}rohan_animal_production", "{=!rohan_animal_production_desc}25% production bonus to horse, mule, cow and sheep in villages owned by Rohan rulers", 0.3f, true, FeatObject.AdditionType.AddFactor);
            rohanNoExtraWageForMounted.Initialize("{=!}rohan_no_extra_wage_for_mounted", "{=!rohan_no_extra_wage_for_mounted_desc}No extra wage for mounted units", 1f, true, FeatObject.AdditionType.Add);
            rohanSlowerBuildRate.Initialize("{=!}rohan_slower_build", "{=!rohan_slower_build_desc}10% slower build rate for town projects in settlements", 0.1f, false, FeatObject.AdditionType.AddFactor);
            rohanMoreInfluenceToRecruitToArmy.Initialize("{=!}rohan_army_lord_influence", "{=!rohan_army_lord_influence_desc}Recruiting lords to armies costs 30% more influence", 0.3f, false, FeatObject.AdditionType.AddFactor);
        }
    }
}