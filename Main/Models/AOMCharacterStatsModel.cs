using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace LOTRAOM.Models
{
    internal class AOMCharacterStatsModel : CharacterStatsModel
    {
        private CharacterStatsModel baseModel;

        public AOMCharacterStatsModel(CharacterStatsModel characterStatsModel)
        {
            baseModel = characterStatsModel;
        }

        public override int MaxCharacterTier => 10;

        public override int GetTier(CharacterObject character)
        {
            return baseModel.GetTier(character);
        }

        public override ExplainedNumber MaxHitpoints(CharacterObject character, bool includeDescriptions = false)
        {
            return baseModel.MaxHitpoints(character, includeDescriptions);
        }
    }
}