using TaleWorlds.SaveSystem;

namespace LOTRAOM.Momentum
{
    public class MomentumFactionTotalStats
    {
        [SaveableField(0)] public int totalKills;
        [SaveableField(1)] public int totalVillagesRaided;
        [SaveableField(2)] public int totalSettlementsCaptured;
        public int TotalKills { get {return totalKills; } }
        public int TotalVillagesRaided { get { return totalVillagesRaided; } }
        public int TotalSettlementsCaptured { get { return TotalSettlementsCaptured; } }
    }
    public class MomentumTotalStats
    {
        [SaveableField(0)] MomentumFactionTotalStats goodKingdoms;
        [SaveableField(1)] MomentumFactionTotalStats evilKingdoms;
        public MomentumTotalStats()
        {
            goodKingdoms = new();
            evilKingdoms = new();
        }
        public MomentumFactionTotalStats GoodKingdoms { get { return goodKingdoms; } }
        public MomentumFactionTotalStats EvilKingdoms { get { return evilKingdoms; } }
    }
}