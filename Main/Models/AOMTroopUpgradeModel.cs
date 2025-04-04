using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;

namespace LOTRAOM.Models
{
    internal class AOMTroopUpgradeModel : PartyTroopUpgradeModel
    {
        private PartyTroopUpgradeModel baseModel;

        public AOMTroopUpgradeModel(PartyTroopUpgradeModel partyTroopUpgradeModel)
        {
            baseModel = partyTroopUpgradeModel;
        }

        public override bool CanPartyUpgradeTroopToTarget(PartyBase party, CharacterObject character, CharacterObject target)
        {
            return baseModel.CanPartyUpgradeTroopToTarget(party, character, target);
        }

        public override bool DoesPartyHaveRequiredItemsForUpgrade(PartyBase party, CharacterObject upgradeTarget)
        {
            return baseModel.DoesPartyHaveRequiredItemsForUpgrade(party, upgradeTarget);
        }

        public override bool DoesPartyHaveRequiredPerksForUpgrade(PartyBase party, CharacterObject character, CharacterObject upgradeTarget, out PerkObject requiredPerk)
        {
            return baseModel.DoesPartyHaveRequiredPerksForUpgrade(party, character, upgradeTarget, out requiredPerk);
        }

        public override int GetGoldCostForUpgrade(PartyBase party, CharacterObject characterObject, CharacterObject upgradeTarget)
        {
            return baseModel.GetGoldCostForUpgrade(party, characterObject, upgradeTarget);
        }

        public override int GetSkillXpFromUpgradingTroops(PartyBase party, CharacterObject troop, int numberOfTroops)
        {
            return baseModel.GetSkillXpFromUpgradingTroops(party, troop, numberOfTroops);
        }

        public override float GetUpgradeChanceForTroopUpgrade(PartyBase party, CharacterObject troop, int upgradeTargetIndex)
        {
            return baseModel.GetUpgradeChanceForTroopUpgrade(party, troop, upgradeTargetIndex);
        }

        private static readonly Dictionary<int, int> XpPerTier = new()
        {
            { 1, 100 },
            { 2, 300 },
            { 3, 550 },
            { 4, 900 },
            { 5, 1300 },
            { 6, 1700 },
            { 7, 2100 },
            { 8, 2500 },
            { 9, 2900 },
            { 10, 3300 }
        };
        public override int GetXpCostForUpgrade(PartyBase party, CharacterObject characterObject, CharacterObject upgradeTarget)
        {
            if (upgradeTarget != null && characterObject.UpgradeTargets.Contains(upgradeTarget))
            {
                int xpNeeded = 0;
                for (int i = characterObject.Tier + 1; i <= upgradeTarget.Tier; i++)
                {
                    if (XpPerTier.ContainsKey(i))
                        xpNeeded += XpPerTier[i];
                    else
                        xpNeeded += XpPerTier[9];
                }
                return xpNeeded;
            }
            return 100000000;
        }

        public override bool IsTroopUpgradeable(PartyBase party, CharacterObject character)
        {
            return baseModel.IsTroopUpgradeable(party, character);
        }
    }
}