using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Library;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System;

namespace LOTRAOM.Patches
{
    [HarmonyPatch]
    public class MilitiaSurrenderPatch
    {
        [HarmonyTargetMethods]
        public static IEnumerable<MethodBase> TargetMethods()
        {
            // Primary target: SiegeEvent.Tick
            MethodInfo targetMethod = AccessTools.DeclaredMethod(typeof(SiegeEvent), "Tick");
            if (targetMethod != null)
            {
                Debug.Print($"[LOTRAOM] MilitiaSurrenderPatch: Found SiegeEvent.Tick with expected signature.");
                yield return targetMethod;
            }
            else
            {
                Debug.Print("[LOTRAOM] MilitiaSurrenderPatch: Failed to find SiegeEvent.Tick. Listing all SiegeEvent methods:");
                foreach (var method in AccessTools.GetDeclaredMethods(typeof(SiegeEvent)))
                {
                    Debug.Print($"[LOTRAOM] SiegeEvent method: {method.Name} (Return: {method.ReturnType.Name}, Parameters: {string.Join(", ", method.GetParameters().Select(p => p.ParameterType.Name))})");
                }
            }
        }

        public static bool Prefix(SiegeEvent __instance)
        {
            try
            {
                if (__instance != null && __instance.BesiegedSettlement != null && __instance.BesiegedSettlement.MilitiaPartyComponent != null)
                {
                    MobileParty militiaParty = __instance.BesiegedSettlement.MilitiaPartyComponent.MobileParty;
                    if (militiaParty != null && militiaParty.IsMilitia)
                    {
                        // Only prevent surrender if settlement walls are intact or militia is from Isengard
                        bool preventSurrender = militiaParty.CurrentSettlement?.StringId == "town_SWAN_ISENGARD1" ||
                                               __instance.BesiegedSettlement.SettlementHitPoints > 0.1f;
                        if (preventSurrender)
                        {
                            Debug.Print($"[LOTRAOM] MilitiaSurrenderPatch: Preventing surrender for militia party in settlement {militiaParty.CurrentSettlement?.StringId ?? "null"} (Wall HP: {__instance.BesiegedSettlement.SettlementHitPoints})");
                            return false; // Skip vanilla method for this tick
                        }
                    }
                }

                return true; // Proceed with vanilla method if no militia parties need protection
            }
            catch (Exception ex)
            {
                Debug.Print($"[LOTRAOM] MilitiaSurrenderPatch Exception: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return true; // Fallback to vanilla to prevent crash
            }
        }
    }
}