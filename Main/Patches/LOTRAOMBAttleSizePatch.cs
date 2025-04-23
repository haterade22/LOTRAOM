using HarmonyLib;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace LOTRAOM.Patches
{
    [HarmonyPatch]
    public class LOTRAOMBattleSizePatch
    {
        private static readonly int[] BattleSizes = new int[] { 500, 800, 1000, 1200, 1400, 1600, 1700, 1800, 1900, 2000 };

        [HarmonyPostfix]
        [HarmonyPatch(typeof(BannerlordConfig), "MaxBattleSize", MethodType.Getter)]
        public static void MaxBattleSizePostfix(ref int __result)
        {
            __result = 10000; // Set high to allow large battles
            Debug.Print($"[LOTRAOM] LOTRAOMBattleSizePatch: Set MaxBattleSize to {__result}.");
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(BannerlordConfig), "GetRealBattleSize")]
        public static void GetRealBattleSizePostfix(ref int __result)
        {
            int battleSizeIndex = BannerlordConfig.BattleSize; // 0 to 6, from Very Low to Engine Max
            int selectedSize = BattleSizes[(int)MathF.Clamp(battleSizeIndex, 0, BattleSizes.Length - 1)];
            __result = selectedSize > 1999 ? 2040 : selectedSize; // Slightly higher for engine max
            Debug.Print($"[LOTRAOM] LOTRAOMBattleSizePatch: Set GetRealBattleSize to {__result}.");
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(BannerlordConfig), "GetRealBattleSizeForSiege")]
        public static void GetRealBattleSizeForSiegePostfix(ref int __result)
        {
            int battleSizeIndex = BannerlordConfig.BattleSize;
            int selectedSize = BattleSizes[(int)MathF.Clamp(battleSizeIndex, 0, BattleSizes.Length - 1)];
            __result = selectedSize > 1999 ? 2040 : selectedSize;
            Debug.Print($"[LOTRAOM] LOTRAOMBattleSizePatch: Set GetRealBattleSizeForSiege to {__result}.");
        }
    }
}