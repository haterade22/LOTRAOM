using LOTRAOM.Extensions;
using LOTRAOM.Momentum;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace LOTRAOM.Models
{
    internal class AOMKingdomDecisionPermissionModel : KingdomDecisionPermissionModel
    {

        public AOMKingdomDecisionPermissionModel() {}

        public override bool IsAnnexationDecisionAllowed(Settlement annexedSettlement)
        {
            return true;
        }

        public override bool IsExpulsionDecisionAllowed(Clan expelledClan)
        {
            return true;
        }

        public override bool IsKingSelectionDecisionAllowed(Kingdom kingdom)
        {
            return true;
        }

        public override bool IsPeaceDecisionAllowedBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, out TextObject reason)
        {

            WarOfTheRingData data = MomentumCampaignBehavior.Instance.WarOfTheRingdata;
            if (data.HasWarStarted() && data.DoesFactionTakePartInWar(kingdom1) && data.DoesFactionTakePartInWar(kingdom2))
            {
                reason = new TextObject("There can be no peace between the forces of good and evil!");
                return false;
            }
            reason = TextObject.Empty;
            return true;
        }

        public override bool IsPolicyDecisionAllowed(PolicyObject policy)
        {
            return true;
        }

        public override bool IsWarDecisionAllowedBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, out TextObject reason)
        {
            if (kingdom1.Culture.IsGoodCulture() && kingdom2.Culture.IsGoodCulture())
            {
                //@todo localization
                reason = new TextObject("We can not attack our friends against the forces of darkness!");
                return false;
            }
            if (kingdom1.Culture.IsEvilCulture() && kingdom2.Culture.IsEvilCulture())
            {
                reason = new TextObject("It's time to stop the infighting, we know who our enemies are!");
                return false;
            }
            if (kingdom1.StringId == Globals.MordorKingdom?.StringId && !MomentumCampaignBehavior.Instance.hasIsengardAttacked)
            {
                reason = new TextObject("We are still preparing for our conquest");
                return false;
            }
            if ((kingdom1.Culture.StringId == Globals.RohanCulture || kingdom1.Culture.StringId == Globals.Gondorculture) && kingdom2.Culture.StringId == Globals.IsengardCulture)
            {
                reason = new TextObject("The tower of Orthanoc belongs to the keeper of peace, Saruman the White. We will not attack our ally.");
                return false;
            }
            if (kingdom2.Culture.StringId == Globals.RivendellCulture || ((kingdom2.Culture.StringId == Globals.MirkwoodCulture || kingdom2.Culture.StringId == Globals.LothlorienCulture) && kingdom1.Culture.StringId != Globals.DolguldurCulture && kingdom1.Culture.StringId != Globals.GundabadCulture))
            {
                reason = new TextObject("The elves are stagnant, and unworthy of our attention, we need to defeat the kingdom of man first!");
                return false;
            }

            reason = TextObject.Empty;
            return true;
        }
    }
}