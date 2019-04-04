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
        internal static BeatmapCharacteristicSegmentedControlController CharacteristicSelectionViewController;
        internal static SoloFreePlayFlowCoordinator SoloFreePlayFlowCoordinator;
        internal static PartyFreePlayFlowCoordinator PartyFreePlayFlowCoordinator;
        internal static MainMenuViewController MainMenuViewController;
        public static bool IsPartyActive { get; private set; } = false;
        public static string GameMode { get; private set; } = "Standard";
        public static bool IsIsolatedLevel { get; internal set; } = false;
        public static string IsolatingMod { get; internal set; } = "";
        public static void Init()
        {
            Plugin.ApplyHarmonyPatches();
            if (MainMenuViewController == null)
            {
                SoloFreePlayFlowCoordinator = Resources.FindObjectsOfTypeAll<SoloFreePlayFlowCoordinator>().FirstOrDefault();
                PartyFreePlayFlowCoordinator = Resources.FindObjectsOfTypeAll<PartyFreePlayFlowCoordinator>().FirstOrDefault();
                MainMenuViewController = Resources.FindObjectsOfTypeAll<MainMenuViewController>().FirstOrDefault();
                if (MainMenuViewController == null) return;
                MainMenuViewController.didFinishEvent += MainMenuViewController_didFinishEvent;
                
                if (CharacteristicSelectionViewController == null)
                {
                    CharacteristicSelectionViewController = Resources.FindObjectsOfTypeAll<BeatmapCharacteristicSegmentedControlController>().FirstOrDefault();
                    if (CharacteristicSelectionViewController != null)
                        CharacteristicSelectionViewController.didSelectBeatmapCharacteristicEvent += CharacteristicSelectionViewController_didSelectBeatmapCharacteristicEvent;
                }
                
            }
        }

        private static void MainMenuViewController_didFinishEvent(MainMenuViewController arg1, MainMenuViewController.MenuButton arg2)
        {
            if (arg2 == MainMenuViewController.MenuButton.Party)
                IsPartyActive = true;
            else
                IsPartyActive = false;
        }

        internal static void CharacteristicSelectionViewController_didSelectBeatmapCharacteristicEvent(BeatmapCharacteristicSegmentedControlController arg1, BeatmapCharacteristicSO arg2)
        {
        //    Utilities.Logger.Log("Prev: " + GameMode + "    New: " + arg2.characteristicName);
            GameMode = arg2.characteristicName;
        }

        internal static void ResetGameMode()
        {
            GameMode = "Standard";
        }
        public static void NextLevelIsIsolated(string modName)
        {
            Plugin.ApplyHarmonyPatches();

            IsIsolatedLevel = true;
            Utilities.Logger.log.Debug($"Isolated level being started by {modName}");
            IsolatingMod = modName;

        }

        private static void SceneManager_activeSceneChanged(UnityEngine.SceneManagement.Scene oldScene, UnityEngine.SceneManagement.Scene newScene)
        {

        }
    }
}
