using System;
using TaleWorlds.CampaignSystem;

namespace LOTRAOM.BalanceOfPower.Pacts
{
    internal abstract class AbstractDiplomaticPact<T> where T : AbstractDiplomaticPact<T>, new()
    {
        public static void Apply(Kingdom kingdom, Kingdom otherKingdom, bool forcePlayerCharacterCosts = false, bool bypassCosts = false, float? customDurationInDays = null, bool queryPlayer = true)
        {
            Instance.TryApply(kingdom, otherKingdom, forcePlayerCharacterCosts, bypassCosts, customDurationInDays, queryPlayer);
        }

        public static void CanApply(Kingdom kingdom, Kingdom otherKingdom, bool forcePlayerCharacterCosts = false, bool bypassCosts = false)
        {
            Instance.PassesConditions(kingdom, otherKingdom, forcePlayerCharacterCosts, bypassCosts);
        }

        protected static T Instance { get; } = Activator.CreateInstance<T>();

        public abstract bool PassesConditions(Kingdom proposingKingdom, Kingdom otherKingdom, bool forcePlayerCharacterCosts, bool bypassCosts);

        protected void TryApply(Kingdom proposingKingdom, Kingdom otherKingdom, bool forcePlayerCharacterCosts, bool bypassCosts, float? customDurationInDays, bool queryPlayer)
        {
            if (otherKingdom == Clan.PlayerClan.Kingdom && otherKingdom.Leader.IsHumanPlayerCharacter && queryPlayer)
            {
                ShowPlayerInquiry(proposingKingdom, delegate
                {
                    ApplyInternalWithCosts(proposingKingdom, otherKingdom, forcePlayerCharacterCosts, bypassCosts, customDurationInDays);
                });
                return;
            }
            ApplyInternalWithCosts(proposingKingdom, otherKingdom, forcePlayerCharacterCosts, bypassCosts, customDurationInDays);
        }
        protected abstract void ShowPlayerInquiry(Kingdom proposingKingdom, Action applyAction);
        protected abstract void ApplyInternal(Kingdom proposingKingdom, Kingdom otherKingdom, float? customDurationInDays);
        protected void ApplyInternalWithCosts(Kingdom proposingKingdom, Kingdom otherKingdom, bool forcePlayerCharacterCosts, bool bypassCosts, float? customDurationInDays)
        {
            if (!bypassCosts)
            {
                AssessCosts(proposingKingdom, otherKingdom, forcePlayerCharacterCosts);
            }
            ApplyInternal(proposingKingdom, otherKingdom, customDurationInDays);
        }
        protected abstract void AssessCosts(Kingdom proposingKingdom, Kingdom otherKingdom, bool forcePlayerCharacterCosts);
    }

}
