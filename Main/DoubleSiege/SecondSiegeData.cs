using System.Collections.Generic;

namespace LOTRAOM.DoubleSiege
{
    public class SecondSiegeData
    {
        public string SecondSiegeSceneId { get; }
        public string PartyTemplateId { get; }
        public string BackgroundMeshId { get; }
        public SecondSiegeData(string secondSiegeSceneId, string memberRosterId, string backgroundMeshId)
        {
            SecondSiegeSceneId = secondSiegeSceneId;
            PartyTemplateId = memberRosterId;
            BackgroundMeshId = backgroundMeshId;
        }

        public static SecondSiegeData? GetSecondSiegeData(string settlementId)
        {
            All.TryGetValue(settlementId, out SecondSiegeData? val);
            return val;
        }
        public static readonly Dictionary<string, SecondSiegeData> All = new()
        {
            ["town_V1"] = new("vlandia_town_e", "crazyman_party_template", "wait_besieging")
        };
    }
}
