using HarmonyLib;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace LOTRAOM.Patches
{
    [HarmonyPatch]
    public class LOTRAOMSallyOutPatch
    {
        private static readonly int[] SallyOutSizes = new int[] { 300, 400, 480, 560, 640, 720, 800 };

        [HarmonyPostfix]
        [HarmonyPatch(typeof(BannerlordConfig), "GetRealBattleSizeForSallyOut")]
        public static void GetRealBattleSizeForSallyOutPostfix(ref int __result)
        {
            int battleSizeIndex = BannerlordConfig.BattleSize; // 0 to 6, from Very Low to Engine Max
            int selectedSize = SallyOutSizes[(int)MathF.Clamp(battleSizeIndex, 0, SallyOutSizes.Length - 1)];
            __result = selectedSize;
            Debug.Print($"[LOTRAOM] LOTRAOMSallyOutPatch: Set GetRealBattleSizeForSallyOut to {__result}.");
        }
    }
}