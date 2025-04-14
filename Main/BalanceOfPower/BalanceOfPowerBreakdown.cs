using LOTRAOM.BalanceOfPower.ViewModel;
using System.Collections.Generic;
using System.Runtime;

namespace LOTRAOM.BalanceOfPower
{
    public struct BalanceOfPowerBreakdown
    {
        public BalanceOfPowerActionType Type { init; get; }
        public int ValueFaction1 { init; get; }
        public int ValueFaction2 { init; get; }
        public float BalanceOfPowerFraction1 { init; get; }
        public float BalanceOfPowerFraction2 { init; get; }
    }
    public enum BalanceOfPowerActionType
    {
        Casualty,
        Raid,
        Siege,
        Daily,
        Occupied
    }
}