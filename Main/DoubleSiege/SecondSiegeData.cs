using System.Collections.Generic;

namespace LOTRAOM.DoubleSiege
{
    public class SecondSiegeData
    {
        public string SecondSiegeSceneId { get; }
        public string PartyTemplateId { get; }
        public string BackgroundMeshId { get; }
        public string TextDefender { get; }
        public string TextAttacker { get; }
        public string StartTextDefender { get; }
        public string StartTextAttacker { get; }

        public static SecondSiegeData? GetSecondSiegeData(string settlementId)
        {
            All.TryGetValue(settlementId, out SecondSiegeData? val);
            return val;
        }
        public static readonly Dictionary<string, SecondSiegeData> All = new()
        {
            ["town_V1"] = new("vlandia_town_e", "crazyman_party_template", "wait_besieging", "We have to defend", "We do be attacking", "Start the defence", "Start the attack")
        };

        public SecondSiegeData(string secondSiegeSceneId, string partyTemplateId, string backgroundMeshId, string textDefender, string textAttacker, string startTextDefender, string startTextAttacker)
        {
            SecondSiegeSceneId = secondSiegeSceneId;
            PartyTemplateId = partyTemplateId;
            BackgroundMeshId = backgroundMeshId;
            TextDefender = textDefender;
            TextAttacker = textAttacker;
            StartTextDefender = startTextDefender;
            StartTextAttacker = startTextAttacker;
        }
    }
}
