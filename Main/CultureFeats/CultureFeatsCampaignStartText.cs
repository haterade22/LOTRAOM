using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;

namespace LOTRAOM.CultureFeats
{
    public class CultureFeatsCampaignStartText
    {

        public FeatObject exampleFeat;
        public CultureFeatsCampaignStartText()
        {
            exampleFeat = Create("feat name");
            InitializeAll();
        }
        private FeatObject Create(string stringId)
        {
            return Game.Current.ObjectManager.RegisterPresumedObject(new FeatObject(stringId));
        }
        private void InitializeAll()
        {
            exampleFeat.Initialize("{=!}loc", "{=localisation_string_id}repeated english localisation text for no reason, but idk, TaleWorlds does it this way", 0.20f, true, FeatObject.AdditionType.AddFactor);
        }
    }
}
