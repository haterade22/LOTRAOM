using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using LOTRAOM.CultureFeats;

namespace LOTRAOM.Models
{
    public class LOTRAOMVillageProductionCalculatorModel : VillageProductionCalculatorModel
    {
        private VillageProductionCalculatorModel defaultVillageProductionCalculatorModel;
        public LOTRAOMVillageProductionCalculatorModel(VillageProductionCalculatorModel baseModel)
        {
            defaultVillageProductionCalculatorModel = baseModel;
        }

        public override float CalculateDailyFoodProductionAmount(Village village)
        {
            return defaultVillageProductionCalculatorModel.CalculateDailyFoodProductionAmount(village);
        }

        public override float CalculateDailyProductionAmount(Village village, ItemObject item)
        {
            float value = defaultVillageProductionCalculatorModel.CalculateDailyProductionAmount(village, item);
            if (village.Settlement.OwnerClan.Culture.HasFeat(DefaultCulturalFeats.KhuzaitAnimalProductionFeat) && (item.ItemCategory == DefaultItemCategories.Sheep || item.ItemCategory == DefaultItemCategories.Cow || item.ItemCategory == DefaultItemCategories.WarHorse || item.ItemCategory == DefaultItemCategories.Horse || item.ItemCategory == DefaultItemCategories.PackAnimal))
            {
                value += value * LOTRAOMCultureFeats.Instance.rohanAnimalProductionBonus.EffectBonus;
            }
            return value;
        }

        public override float CalculateProductionSpeedOfItemCategory(ItemCategory item)
        {
            return defaultVillageProductionCalculatorModel.CalculateProductionSpeedOfItemCategory(item);
        }
    }
}
