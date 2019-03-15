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
          new Type[] {   typeof(bool),
          typeof(VRUI.VRUIViewController.ActivationType) }
          )]
    [HarmonyPatch("DidActivate", MethodType.Normal)]

    class BeatmapCharacteristicSelectionViewControllerDidActivate
    {
        static void Prefix(BeatmapCharacteristicSelectionViewController __instance, bool firstActivation, VRUI.VRUIViewController.ActivationType activationType)
        {
            Utilities.Logger.Log("Activation");
                __instance.didSelectBeatmapCharacteristicEvent += Gamemode.CharacteristicSelectionViewController_didSelectBeatmapCharacteristicEvent;
           

        }
    }
}

