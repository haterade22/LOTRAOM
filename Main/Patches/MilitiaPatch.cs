using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using LOTRAOM.Models;
using TaleWorlds.Library;
using System.Reflection;
using System.Collections.Generic;
using System;

namespace LOTRAOM.Patches
{
    [HarmonyPatch]
    public class MilitiaPatch
    {
        private static readonly MethodInfo TargetMethod = typeof(Settlement).GetMethod("AddMilitiasToParty", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(MobileParty), typeof(int) }, null);

        [HarmonyTargetMethods]
        public static IEnumerable<MethodBase> TargetMethods()
        {
            if (TargetMethod != null)
            {
                yield return TargetMethod;
            }
            else
            {
                Debug.Print("[LOTRAOM] MilitiaPatch: Failed to find AddMilitiasToParty with expected signature. Falling back to alternative method.");
                // Fallback to find any AddMilitiasToParty method (less strict)
                MethodInfo fallback = typeof(Settlement).GetMethod("AddMilitiasToParty", BindingFlags.NonPublic | BindingFlags.Instance);
                if (fallback != null)
                {
                    yield return fallback;
                }
                else
                {
                    Debug.Print("[LOTRAOM] MilitiaPatch: No AddMilitiasToParty method found in Settlement class.");
                }
            }
        }

        public static bool Prefix(Settlement __instance, MobileParty __0, int __1)
        {
            try
            {
                if (__instance == null || __0 == null)
                {
                    Debug.Print($"[LOTRAOM] MilitiaPatch: Null settlement or MobileParty for AddMilitiasToParty (settlement: {__instance?.StringId ?? "null"})");
                    return true; // Fallback to vanilla
                }

                if (Campaign.Current?.Models?.SettlementMilitiaModel is LOTRAOMSettlementMilitiaModel model)
                {
                    model.AssignMilitiaTroops(__instance, __0, __1);
                    return false; // Skip vanilla method
                }

                Debug.Print($"[LOTRAOM] MilitiaPatch: SettlementMilitiaModel is not LOTRAOMSettlementMilitiaModel for settlement {__instance.StringId}");
                return true; // Fallback to vanilla
            }
            catch (Exception ex)
            {
                Debug.Print($"[LOTRAOM] MilitiaPatch Exception for settlement {__instance?.StringId ?? "null"}: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return true; // Fallback to vanilla to prevent crash
            }
        }
    }
}