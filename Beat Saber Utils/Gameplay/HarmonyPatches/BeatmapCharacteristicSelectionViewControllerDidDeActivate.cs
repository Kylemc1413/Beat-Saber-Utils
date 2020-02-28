using System;
using HarmonyLib;

namespace BS_Utils.Gameplay.Harmony_Patches
{
    [HarmonyPatch(typeof(BeatmapCharacteristicSegmentedControlController))]
    [HarmonyPatch("SetData", MethodType.Normal)]
    class BeatmapCharacteristicSelectionViewControllerDidDeactivate
    {
        static void Postfix(BeatmapCharacteristicSegmentedControlController __instance, IDifficultyBeatmapSet[] difficultyBeatmapSets, BeatmapCharacteristicSO selectedBeatmapCharacteristic)
        {
            Gamemode.CharacteristicSelectionViewController_didSelectBeatmapCharacteristicEvent(__instance, __instance.selectedBeatmapCharacteristic);
        }
    }
}
