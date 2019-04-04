using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Harmony;
using IPA;
using IPALogger = IPA.Logging.Logger;
using Logger = BS_Utils.Utilities.Logger;

namespace BS_Utils
{
    public class Plugin : IBeatSaberPlugin
    {
        public string Name => "Beat Saber Utils";
        public string Version => "1.2.3";
        internal static bool patched = false;
        internal static HarmonyInstance harmony;
        public static Gameplay.LevelData LevelData = new Gameplay.LevelData();
        public delegate void LevelDidFinish(StandardLevelScenesTransitionSetupDataSO levelScenesTransitionSetupDataSO, LevelCompletionResults levelCompletionResults);
        public static event LevelDidFinish LevelDidFinishEvent;

        public void Init(object thisIsNull, IPALogger logger)
        {
            Logger.log = logger;
        }

        public void OnApplicationStart()
        {
            Logger.log.Debug("OnApplicationStart");
            //Create Harmony Instance
            harmony = HarmonyInstance.Create("com.kyle1413.BeatSaber.BS-Utils");
        }

        public void OnApplicationQuit()
        {
            Logger.log.Debug("OnApplicationQuit");
        }

        public void OnFixedUpdate()
        {

        }

        public void OnUpdate()
        {

        }

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
            if (nextScene.name == "MenuCore")
            {
                Logger.log.Debug("Removing Isolated Level");
                Gameplay.Gamemode.IsIsolatedLevel = false;
                Gameplay.Gamemode.IsolatingMod = "";
                LevelData.Clear();
            }
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {

        }

        public void OnSceneUnloaded(Scene scene)
        {

        }

        internal static void TriggerLevelFinishEvent(StandardLevelScenesTransitionSetupDataSO levelScenesTransitionSetupDataSO, LevelCompletionResults levelCompletionResults)
        {
            LevelDidFinishEvent?.Invoke(levelScenesTransitionSetupDataSO, levelCompletionResults);
        }
        internal static void ApplyHarmonyPatches()
        {
            if (patched) return;
            try
            {
                Logger.log.Debug("Applying Harmony Patches");
                harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
                patched = true;
            }
            catch (Exception ex)
            {
                Logger.log.Error("Exception Trying to Apply Harmony Patches");
                Logger.log.Error(ex.ToString());
            }


        }
    }
}
