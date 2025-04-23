using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace LOTRAOM.Patches
{
    [HarmonyPatch(typeof(GameTextManager), "FindText")]
    public class LOTRAOMUITextPatch
    {
        private static readonly string[] BattleSizeLabels = new string[]
        {
            "LOTR Very Low (1200)", "LOTR Low (1400)", "LOTR Medium (1600)", "LOTR High (1700)",
            "LOTR Very High (1800)", "LOTR Ultra (1900)", "LOTR Epic (2000)"
        };

        public static void Postfix(ref TextObject __result, string id, string variation = null)
        {
            try
            {
                if (id != null && id.StartsWith("str_options_type_BattleSize_"))
                {
                    // Extract the index from the string ID (e.g., "str_options_type_BattleSize_0")
                    string indexStr = id.Replace("str_options_type_BattleSize_", "");
                    if (int.TryParse(indexStr, out int battleSizeIndex))
                    {
                        int clampedIndex = (int)MathF.Clamp(battleSizeIndex, 0, BattleSizeLabels.Length - 1);
                        string label = BattleSizeLabels[clampedIndex];
                        __result = new TextObject(label, null);
                        Debug.Print($"[LOTRAOM] LOTRAOMUITextPatch: ID='{id}', Parsed Index={battleSizeIndex}, Clamped Index={clampedIndex}, Updated UI text to '{label}'.");
                    }
                    else
                    {
                        // Fallback to BannerlordConfig.BattleSize if parsing fails
                        int battleSizeIndexFallback = BannerlordConfig.BattleSize;
                        int clampedIndex = (int)MathF.Clamp(battleSizeIndexFallback, 0, BattleSizeLabels.Length - 1);
                        string label = BattleSizeLabels[clampedIndex];
                        __result = new TextObject(label, null);
                        Debug.Print($"[LOTRAOM] LOTRAOMUITextPatch: ID='{id}', Failed to parse index, using fallback Index={battleSizeIndexFallback}, Clamped Index={clampedIndex}, Updated UI text to '{label}'.");
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.Print($"[LOTRAOM] LOTRAOMUITextPatch Exception: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }
    }
}