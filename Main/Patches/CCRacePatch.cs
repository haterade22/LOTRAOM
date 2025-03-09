using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using TaleWorlds.MountAndBlade.GauntletUI.BodyGenerator;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.Tableaus;
using TaleWorlds.MountAndBlade.View;

namespace LOTRAOM.Patches
{
    public class CCRacePatch
    {
        [HarmonyPatch(typeof(BodyGeneratorView), "RefreshCharacterEntityAux")]
        public class RefreshCharacterEntityAuxPatch
        {
            // This patch makes the created AgentVisuals use the correct action set and so the correct skeleton when it is refreshed
            // Method to avoid having to insert a bunch of instructions and instead only insert 2 (LdArg0 and Call)
            public static MBActionSet GetActionSet(BodyGeneratorView bodyGeneratorView)
            {
                var baseMonsterFromRace = TaleWorlds.Core.FaceGen.GetBaseMonsterFromRace(bodyGeneratorView.BodyGen.Race);
                return MBGlobals.GetActionSetWithSuffix(baseMonsterFromRace, bodyGeneratorView.BodyGen.IsFemale, "_facegen");
            }

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator ilGen)
            {
                var newInstructions = new List<CodeInstruction>(instructions);
                var insertionIndex = -1;
                for (int i = 0; i < newInstructions.Count - 1; i++)
                {
                    var instruction = newInstructions[i];
                    // Find where AgentVisualsData is instantiated, and insert our new instructions after it
                    if (instruction.opcode == OpCodes.Newobj && instruction.operand == AccessTools.Constructor(typeof(AgentVisualsData)))
                    {
                        insertionIndex = i + 1;
                        break;
                    }
                }
                if (insertionIndex < 0)
                {
                    throw new ArgumentException("Cannot find instruction. Patch: RefreshCharacterEntityAuxPatch");
                }
                else
                {
                    var insertedInstructions = new List<CodeInstruction>();
                    // Load "this" (The BodyGeneratorView) unto the stack
                    insertedInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    // Pass it as an argument to our static method that gets the correct action set and then puts it on the stack
                    insertedInstructions.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(RefreshCharacterEntityAuxPatch), nameof(RefreshCharacterEntityAuxPatch.GetActionSet))));
                    // equivalent to AgentVisualsData.ActionSet(RefreshCharacterEntityAuxPatch.GetActionSet(this));
                    insertedInstructions.Add(new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(AgentVisualsData), nameof(AgentVisualsData.ActionSet))));
                    newInstructions.InsertRange(insertionIndex, insertedInstructions);
                }
                return newInstructions.AsEnumerable();
            }
        }

    }
}
