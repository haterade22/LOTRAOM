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

        // Existing feats
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
        public FeatObject elfForestProsperityFeat;
        public FeatObject ereborMiningIncomeFeat;
        public FeatObject aseraiDesertTradeFeat;
        public FeatObject umbarNavalTradeFeat;
        public FeatObject khuzaitSteppeMobilityFeat;
        public FeatObject battaniaSteppeRecruitmentFeat;
        public FeatObject sturgiaTradeSecurityFeat;
        public FeatObject darshiForestScoutingFeat;
        public FeatObject empireMarketplaceIncomeFeat;

        // New feats
        public FeatObject elfArcherWageFeat;
        public FeatObject ereborEliteWageFeat;

        public LOTRAOMCultureFeats()
        {
            // Existing feats
            exampleFeat = Create("example_feat");
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
            elfForestProsperityFeat = Create("elf_forest_prosperity");
            ereborMiningIncomeFeat = Create("erebor_mining_income");
            aseraiDesertTradeFeat = Create("aserai_desert_trade");
            umbarNavalTradeFeat = Create("umbar_naval_trade");
            khuzaitSteppeMobilityFeat = Create("khuzait_steppe_mobility");
            battaniaSteppeRecruitmentFeat = Create("battania_steppe_recruitment");
            sturgiaTradeSecurityFeat = Create("sturgia_trade_security");
            darshiForestScoutingFeat = Create("darshi_forest_scouting");
            empireMarketplaceIncomeFeat = Create("empire_marketplace_income");

            // New feats
            elfArcherWageFeat = Create("elf_archer_wage");
            ereborEliteWageFeat = Create("erebor_elite_wage");

            InitializeAll();
        }

        private static FeatObject Create(string stringId)
        {
            return Game.Current.ObjectManager.RegisterPresumedObject(new FeatObject(stringId));
        }

        private void InitializeAll()
        {
            // Existing feats
            exampleFeat.Initialize("{=example_feat_name}Example Feat", "{=example_feat_desc}This is a placeholder feat for testing.", 0.20f, true, FeatObject.AdditionType.AddFactor);
            mordorWageMultiplierFeat.Initialize("{=mordor_wage_multiplier}Mordor Wage Efficiency", "{=mordor_wage_multiplier_desc}20% lower troop wages due to Sauron's ruthless efficiency.", 0.2f, true, FeatObject.AdditionType.AddFactor);
            mordorRecruitmentFeat.Initialize("{=mordor_recruitment}Orc Horde Recruitment", "{=mordor_recruitment_desc}20% lower recruitment cost for Mordor's vast orc armies.", 0.8f, true, FeatObject.AdditionType.AddFactor);
            mordorPartySizeFeat.Initialize("{=mordor_party_size}Mordor Swarm", "{=mordor_party_size_desc}30% larger party size to overwhelm foes.", 0.3f, true, FeatObject.AdditionType.AddFactor);
            humanPlusMilitiaProduction.Initialize("{=human_militia_production}Human Militia Training", "{=human_militia_production_desc}Towns owned by human culture rulers gain +1 militia production.", 1f, true, FeatObject.AdditionType.Add);
            gondorMoreInfluenceInArmy.Initialize("{=gondor_influence_army}Gondorian Leadership", "{=gondor_influence_army_desc}Being in an army grants 25% more influence, reflecting Gondor's disciplined command.", 0.25f, true, FeatObject.AdditionType.AddFactor);
            gondorReduceInfantryWages.Initialize("{=gondor_infantry_wage}Gondorian Infantry Efficiency", "{=gondor_infantry_wage_desc}Wages of Gondorian infantry are 15% lower due to their loyalty.", 0.15f, true, FeatObject.AdditionType.AddFactor);
            gondorReduceWagesInGarrisons.Initialize("{=gondor_reduce_wages_in_garrison}Gondorian Garrison Duty", "{=gondor_reduce_wages_in_garrison_desc}Wages of troops in garrisons are 20% lower.", 0.20f, true, FeatObject.AdditionType.AddFactor);
            rohanAnimalProductionBonus.Initialize("{=rohan_animal_production}Rohirrim Horse Breeding", "{=rohan_animal_production_desc}25% production bonus to horses, mules, cows, and sheep in villages owned by Rohan rulers.", 0.3f, true, FeatObject.AdditionType.AddFactor);
            rohanNoExtraWageForMounted.Initialize("{=rohan_reduce_mounted_cost}Rohirrim Horsemanship", "{=rohan_reduce_mounted_cost_desc}No extra wage for mounted units, reflecting Rohan's cavalry expertise.", 1f, true, FeatObject.AdditionType.Add);
            rohanSlowerBuildRate.Initialize("{=rohan_slower_build}Rohirrim Simplicity", "{=rohan_slower_build_desc}10% slower build rate for town projects due to Rohan's focus on rural life.", 0.1f, false, FeatObject.AdditionType.AddFactor);
            rohanMoreInfluenceToRecruitToArmy.Initialize("{=rohan_army_lord_influence}Rohirrim Lord Recruitment", "{=rohan_army_lord_influence_desc}Recruiting lords to armies costs 30% more influence due to their independent spirit.", 0.3f, false, FeatObject.AdditionType.AddFactor);
            elfForestProsperityFeat.Initialize("{=elf_forest_prosperity}Elven Harmony", "{=elf_forest_prosperity_desc}Villages near forests owned by Elven rulers gain 20% increased prosperity.", 0.2f, true, FeatObject.AdditionType.AddFactor);
            ereborMiningIncomeFeat.Initialize("{=erebor_mining_income}Dwarven Mining", "{=erebor_mining_income_desc}Settlements owned by Dwarven rulers gain 15% increased income from mines.", 0.15f, true, FeatObject.AdditionType.AddFactor);
            aseraiDesertTradeFeat.Initialize("{=aserai_desert_trade}Haradrim Trade Routes", "{=aserai_desert_trade_desc}Desert settlements owned by Haradrim rulers gain 10% increased trade income.", 0.1f, true, FeatObject.AdditionType.AddFactor);
            umbarNavalTradeFeat.Initialize("{=umbar_naval_trade}Corsair Trade", "{=umbar_naval_trade_desc}Coastal settlements owned by Umbar rulers gain 15% increased trade income.", 0.15f, true, FeatObject.AdditionType.AddFactor);
            khuzaitSteppeMobilityFeat.Initialize("{=khuzait_steppe_mobility}Easterling Mobility", "{=khuzait_steppe_mobility_desc}Parties on steppe terrain gain 10% increased movement speed.", 0.1f, true, FeatObject.AdditionType.AddFactor);
            battaniaSteppeRecruitmentFeat.Initialize("{=battania_steppe_recruitment}Variag Recruitment", "{=battania_steppe_recruitment_desc}20% faster recruitment in steppe settlements owned by Variag rulers.", 0.2f, true, FeatObject.AdditionType.AddFactor);
            sturgiaTradeSecurityFeat.Initialize("{=sturgia_trade_security}Barding Trade Protection", "{=sturgia_trade_security_desc}20% reduced bandit spawns near settlements owned by Barding rulers.", 0.2f, true, FeatObject.AdditionType.AddFactor);
            darshiForestScoutingFeat.Initialize("{=darshi_forest_scouting}Drúedain Tracking", "{=darshi_forest_scouting_desc}Parties owned by Drúedain rulers gain +1 scouting skill.", 1f, true, FeatObject.AdditionType.Add);
            empireMarketplaceIncomeFeat.Initialize("{=empire_marketplace_income}Dale Marketplace", "{=empire_marketplace_income_desc}Towns owned by Men of Dale gain 10% increased marketplace income.", 0.1f, true, FeatObject.AdditionType.AddFactor);

            // New feats
            elfArcherWageFeat.Initialize("{=elf_archer_wage}Elven Archery Efficiency", "{=elf_archer_wage_desc}15% lower wages for ranged units due to their elite training.", 0.15f, true, FeatObject.AdditionType.AddFactor);
            ereborEliteWageFeat.Initialize("{=erebor_elite_wage}Dwarven Elite Cost", "{=erebor_elite_wage_desc}10% higher wages for elite units (tier 4+) due to their superior craftsmanship.", 0.1f, false, FeatObject.AdditionType.AddFactor);
        }
    }
}