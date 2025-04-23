using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace LOTRAOM.Patches
{
    [HarmonyPatch(typeof(Agent), "Die")]
    public class LOTRAOMAgentPatch
    {
        public static void Postfix(Agent? __instance, Blow b, Agent.KillInfo overrideKillInfo = Agent.KillInfo.Invalid)
        {
            try
            {
                if (__instance != null && __instance != Agent.Main && __instance.MountAgent != null && __instance.Mission.AllAgents.Count > 1000)
                {
                    // Check if the mount is already dead
                    if (__instance.MountAgent.State != AgentState.Active || __instance.MountAgent.Health <= 0)
                    {
                        Debug.Print($"[LOTRAOM] LOTRAOMAgentPatch: Despawning dead horse corpse in large battle (>1000 troops).");
                        __instance.MountAgent.FadeOut(true, true); // Despawn the dead horse corpse
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.Print($"[LOTRAOM] LOTRAOMAgentPatch Exception: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }
    }
}