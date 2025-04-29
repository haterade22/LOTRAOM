using HarmonyLib;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Core;
using System.Reflection;
using TaleWorlds.Engine;

namespace LOTRAOM
{
    [HarmonyPatch(typeof(Mission))]
    public static class HarmonyPatches
    {
        private static readonly LOTRAOMAgentApplyDamageModel _customDamageModel = new LOTRAOMAgentApplyDamageModel();
        private static bool _isPatched = false;

        [HarmonyPostfix]
        [HarmonyPatch("GetAttackCollisionResults")]
        public static void GetAttackCollisionResultsPostfix(
            Mission __instance,
            Agent attackerAgent,
            Agent victimAgent,
            GameEntity hitObject,
            float momentumRemaining,
            MissionWeapon attackerWeapon,
            bool crushedThrough,
            bool cancelDamage,
            bool crushedThroughWithoutAgentCollision,
            ref AttackCollisionData attackCollisionData,
            ref WeaponComponentData shieldOnBack,
            ref CombatLogData combatLog)
        {
            // Only apply in single-player missions
            if (GameNetwork.IsMultiplayer)
            {
                return;
            }

            // Determine if the hit object is a siege engine and set the property
            _customDamageModel.IsSiegeEngineHit = hitObject != null && hitObject.HasTag("siege_engine");

            // Recalculate damage using the custom damage model
            AttackInformation attackInformation = new AttackInformation(attackerAgent, victimAgent, hitObject, attackCollisionData, attackerWeapon);
            float baseDamage = attackCollisionData.BaseMagnitude;
            float newDamage = _customDamageModel.CalculateDamage(attackInformation, attackCollisionData, attackerWeapon, baseDamage);

            // Update the attack collision data with the new damage
            combatLog.ModifiedDamage = (int)(newDamage - attackCollisionData.InflictedDamage);
            attackCollisionData.InflictedDamage = (int)newDamage;
            attackCollisionData.AbsorbedByArmor = (int)(baseDamage - newDamage); // Simplified absorption calculation
        }

        public static void ApplyPatches()
        {
            if (!_isPatched)
            {
                var harmony = new Harmony("com.lotraom.mod");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                _isPatched = true;
            }
        }

        public static void UnapplyPatches()
        {
            if (_isPatched)
            {
                var harmony = new Harmony("com.lotraom.mod");
                harmony.UnpatchAll("com.lotraom.mod");
                _isPatched = false;
            }
        }
    }
}