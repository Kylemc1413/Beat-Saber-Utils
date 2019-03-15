using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using CustomUI.BeatSaber;
namespace BS_Utils.Gameplay.Harmony_Patches
{
    [HarmonyPatch(typeof(BeatmapCharacteristicSegmentedControlController),
            new Type[] {
            typeof(IDifficultyBeatmapSet[]),
                        typeof(BeatmapCharacteristicSO)
                                    })]
        
    [HarmonyPatch("SetData", MethodType.Normal)]

    class BeatmapCharacteristicSelectionViewControllerDidDeactivate
    {
        static void Postfix(BeatmapCharacteristicSegmentedControlController __instance, IDifficultyBeatmapSet[] difficultyBeatmapSets, BeatmapCharacteristicSO selectedBeatmapCharacteristic)
        {
            Gamemode.CharacteristicSelectionViewController_didSelectBeatmapCharacteristicEvent(__instance, __instance.selectedBeatmapCharacteristic);
        }
    }
}

