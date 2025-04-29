using TaleWorlds.SaveSystem;

namespace LOTRAOM.Momentum
{
    public class MomentumFactionTotalStats
    {
        [SaveableField(0)] int totalKills;
        [SaveableField(1)] int totalVillagesRaided;
        [SaveableField(2)] int totalSettlementsCaptured;
        public int TotalKills { get { return totalKills; } }
        public int TotalVillagesRaided { get { return totalVillagesRaided; } }
        public int TotalSettlementsCaptured { get { return totalSettlementsCaptured; } }
        public void AddSettlementCaptured()
        {
            totalSettlementsCaptured++;
        }
        public void AddKills(int kills)
        {
            totalKills += kills;
        }
        public void AddRaid()
        {
            totalVillagesRaided++;
        }
    }
}