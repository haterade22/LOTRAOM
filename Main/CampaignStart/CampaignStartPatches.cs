using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.ViewModelCollection.FaceGenerator;
using System.Reflection;

namespace LOTRAOM.CampaignStart
{
    //          PATCHES FOR THE RACE WIDGET IN CHARACTER CREATION
    [HarmonyPatch(typeof(FaceGenVM), "Refresh")]
    public class FaceGenVMRefreshPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator ilGen)
        {
            var codes = instructions.ToList();
            var insertionIndex = -1;
            for (var index = 0; index < codes.Count; index++)
            {
                if (codes[index].opcode == OpCodes.Call && (MethodInfo)codes[index].operand == AccessTools.Method("TaleWorlds.Core.FaceGen:GetRaceNames"))
                    insertionIndex = index;
            }
            codes.Insert(insertionIndex, new CodeInstruction(OpCodes.Ldarg_0));
            codes[insertionIndex + 1] = new(OpCodes.Call, AccessTools.Method("LOTRAOM.CampaignStart.FaceGenVMRefreshPatch:GetAvailableRacesNames"));
            codes[insertionIndex + 3] = new(OpCodes.Call, AccessTools.Method("LOTRAOM.CampaignStart.FaceGenVMRefreshPatch:ChangeIndexToCorrectNumber"));
            return codes;
        }
        private static int ChangeIndexToCorrectNumber(FaceGenVM vm)
        {
            if (CharacterCreationCultureStageVMPatch.HasJustSwitchedCulture) // selected race is treated as which list element to start on
            {
                CharacterCreationCultureStageVMPatch.HasJustSwitchedCulture = false;
                return 0;
            }

            CampaignStartGlobals.CCCultureData playerRaceData = CampaignStartGlobals.CCCulturesRaceData.TryGetValue(CharacterCreationCultureStageVMPatch.CurrentlySelectedCulture, out var value) ? value : CampaignStartGlobals.CCCulturesRaceData["default"];
            List<string> possibleCultureRaces = playerRaceData.PossibleRaces;
            return possibleCultureRaces.IndexOf(FaceGenVMOnSelectRacePatch.RaceSelected);
        }
        private static string[] GetAvailableRacesNames(FaceGenVM vm) // sets RaceSelector so it only contains available races 
        {
            CampaignStartGlobals.CCCultureData playerRaceData = CampaignStartGlobals.CCCulturesRaceData.TryGetValue(CharacterCreationCultureStageVMPatch.CurrentlySelectedCulture, out var value) ? value : CampaignStartGlobals.CCCulturesRaceData["default"];
            List<string> possibleCultureRaces = playerRaceData.PossibleRaces;
            return possibleCultureRaces.ToArray();
        }
    }
    [HarmonyPatch(typeof(FaceGenVM), "OnSelectRace")]
    public class FaceGenVMOnSelectRacePatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator ilGen)
        {
            var codes = instructions.ToList();
            var insertionIndex = -1;
            for (var index = 0; index < codes.Count; index++)
            {
                if (codes[index].opcode == OpCodes.Ldarg_0
                    && codes[index + 1].opcode == OpCodes.Ldfld
                    && codes[index + 2].opcode == OpCodes.Ldc_I4_M1)
                    insertionIndex = index;
            }

            var instr = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldarg_1),
                new(OpCodes.Call, AccessTools.Method("LOTRAOM.CampaignStart.FaceGenVMOnSelectRacePatch:SetRaceToRaceFromSelector")),
            };
            codes.InsertRange(insertionIndex, instr);

            return codes;
        }
        public static void SetRaceToRaceFromSelector(FaceGenVM vm, SelectorVM<SelectorItemVM> selector) // sets current race based on race id, rather than id of the list element
        {
            if (selector.SelectedItem == null)
            {
                selector.SelectedItem = selector.ItemList[0];
                selector.SelectedItem.IsSelected = true;
            }
            var field = AccessTools.Field(typeof(FaceGenVM), "_selectedRace");
            int value = FaceGen.GetRaceOrDefault(selector.SelectedItem.StringItem);
            RaceSelected = selector.SelectedItem.StringItem;
            field.SetValue(vm, value);
        }
        public static string RaceSelected { get; set; } = "human";
    }

    [HarmonyPatch(typeof(CharacterCreationCultureStageVM), "OnCultureSelection")]
    public class CharacterCreationCultureStageVMPatch
    {
        public static string? CurrentlySelectedCulture { get; set; }
        public static bool HasJustSwitchedCulture { get; set; } = false;
        public static void Postfix(CharacterCreationCultureStageVM __instance, CharacterCreationCultureVM selectedCulture)
        {
            CurrentlySelectedCulture = selectedCulture.Culture.StringId;
            HasJustSwitchedCulture = true;
        }
    }
    //          END OF PATCHES FOR THE RACE WIDGET IN CHARACTER CREATION
}
