using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;

namespace LOTRAOM.CultureFeats
{
    public class CultureFeatsCampaignStartText
    {
        public FeatObject exampleFeat;
        public FeatObject mordorWageMultiplierFeat;
        public FeatObject mordorRecruitmentFeat;
        public FeatObject mordorPartySizeFeat;
        public CultureFeatsCampaignStartText()
        {
            exampleFeat = Create("feat name");
            mordorWageMultiplierFeat = Create("mordor_wage_multiplier");
            mordorRecruitmentFeat = Create("mordor_recruitment");
            mordorPartySizeFeat = Create("mordor_party_size");
            InitializeAll();
        }
        private static FeatObject Create(string stringId)
        {
            return Game.Current.ObjectManager.RegisterPresumedObject(new FeatObject(stringId));
        }
        private void InitializeAll()
        {
            exampleFeat.Initialize("{=!}loc", "{=localisation_string_id}repeated english localisation text for no reason, but idk, TaleWorlds does it this way", 0.20f, true, FeatObject.AdditionType.AddFactor);
            mordorWageMultiplierFeat.Initialize("{=!}mordor_wage_multiplier", "{=mordor_wage_multiplier_desc}20% lower troop wages", CultureFeatsGlobals.MordorWageMultiplier, true, FeatObject.AdditionType.AddFactor);
            mordorRecruitmentFeat.Initialize("{=!}mordor_recruitment", "{=mordor_recruitment_desc}20% lower recruitment cost", CultureFeatsGlobals.MordorRecruitmentMulitpler, true, FeatObject.AdditionType.AddFactor);
            mordorPartySizeFeat.Initialize("{=!}mordor_party_size", "{=mordor_party_size_desc}30% bigger party size", CultureFeatsGlobals.MordorPartySizeMultiplier, true, FeatObject.AdditionType.AddFactor);
        }
    }
}