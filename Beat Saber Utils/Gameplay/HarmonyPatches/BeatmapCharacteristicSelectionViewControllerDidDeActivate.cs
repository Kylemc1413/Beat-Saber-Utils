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
    [HarmonyPatch(typeof(BeatmapCharacteristicSelectionViewController),
          new Type[] {   typeof(VRUI.VRUIViewController.DeactivationType) })]
    [HarmonyPatch("DidDeactivate", MethodType.Normal)]

    class BeatmapCharacteristicSelectionViewControllerDidDeactivate
    {
        static void Postfix(VRUI.VRUIViewController.DeactivationType deactivationType)
        {
            if (deactivationType == VRUI.VRUIViewController.DeactivationType.RemovedFromHierarchy)
                Gamemode.ResetGameMode();      
        }
    }
}

