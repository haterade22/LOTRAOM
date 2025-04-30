using LOTRAOM.CultureFeats;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace LOTRAOM.Models
{
    public class LOTRAOMVillageProductionCalculatorModel : VillageProductionCalculatorModel
    {
        private readonly VillageProductionCalculatorModel _defaultVillageProductionCalculatorModel;

        public LOTRAOMVillageProductionCalculatorModel(VillageProductionCalculatorModel baseModel)
        {
            _defaultVillageProductionCalculatorModel = baseModel;
        }

        public override float CalculateDailyFoodProductionAmount(Village village)
        {
            return _defaultVillageProductionCalculatorModel.CalculateDailyFoodProductionAmount(village);
        }

        public override float CalculateDailyProductionAmount(Village village, ItemObject item)
        {
            if (village == null || item == null) return 0f;

            ExplainedNumber production = new ExplainedNumber(_defaultVillageProductionCalculatorModel.CalculateDailyProductionAmount(village, item), false);

            if (village.Settlement?.OwnerClan?.Culture != null)
            {
                // Rohan animal production bonus (horses, sheep, cows, etc.)
                if (village.Settlement.OwnerClan.Culture.HasFeat(LOTRAOMCultureFeats.Instance.rohanAnimalProductionBonus) &&
                    item.Type == ItemObject.ItemTypeEnum.Animal)
                {
                    production.AddFactor(LOTRAOMCultureFeats.Instance.rohanAnimalProductionBonus.EffectBonus,
                        new TextObject("{=rohan_animal_production}Rohirrim Horse Breeding"));
                }

                // Elven forest goods bonus (e.g., wood, hides, potentially food)
                if (village.Settlement.OwnerClan.Culture.HasFeat(LOTRAOMCultureFeats.Instance.elfForestProsperityFeat) &&
                    (item.ItemCategory == DefaultItemCategories.Wood || item.ItemCategory == DefaultItemCategories.Hides || item.IsFood))
                {
                    production.AddFactor(LOTRAOMCultureFeats.Instance.elfForestProsperityFeat.EffectBonus * 0.5f,
                        new TextObject("{=elf_forest_production}Elven Forest Harmony"));
                }

                // Dwarven mining bonus (e.g., iron, clay)
                if (village.Settlement.OwnerClan.Culture.HasFeat(LOTRAOMCultureFeats.Instance.ereborMiningIncomeFeat) &&
                    (item.ItemCategory == DefaultItemCategories.Iron || item.ItemCategory == DefaultItemCategories.Clay))
                {
                    production.AddFactor(LOTRAOMCultureFeats.Instance.ereborMiningIncomeFeat.EffectBonus,
                        new TextObject("{=erebor_mining_production}Dwarven Mining Efficiency"));
                }
            }

            return production.ResultNumber;
        }

        public override float CalculateProductionSpeedOfItemCategory(ItemCategory item)
        {
            return _defaultVillageProductionCalculatorModel.CalculateProductionSpeedOfItemCategory(item);
        }
    }
}