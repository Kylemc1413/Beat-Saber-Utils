using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace BS_Utils.Gameplay
{
    public class Gamemode
    {
        internal static BeatmapCharacteristicSelectionViewController CharacteristicSelectionViewController;
        internal static string GameMode = "";

        public static void Init()
        {
            if (CharacteristicSelectionViewController != null) return;
            CharacteristicSelectionViewController = Resources.FindObjectsOfTypeAll<BeatmapCharacteristicSelectionViewController>().First();
            if (CharacteristicSelectionViewController == null)
            {
                Utilities.Logger.Log("Characteristic View Controller null");
                return;
            }
            CharacteristicSelectionViewController.didSelectBeatmapCharacteristicEvent += CharacteristicSelectionViewController_didSelectBeatmapCharacteristicEvent;



        }

        private static void CharacteristicSelectionViewController_didSelectBeatmapCharacteristicEvent(BeatmapCharacteristicSelectionViewController arg1, BeatmapCharacteristicSO arg2)
        {
            GameMode = arg2.characteristicName;
        }

        public static string GetCurrentGameplayMode()
        {
            return GameMode;
        }


    }
}
