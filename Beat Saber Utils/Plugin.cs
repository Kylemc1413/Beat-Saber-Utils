using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using IllusionPlugin;
using Harmony;

namespace BS_Utils
{
    public class Plugin : IPlugin
    {
        public string Name => "Beat Saber Utils";
        public string Version => "1.1.10";
        internal static bool patched = false;
        internal static HarmonyInstance harmony;
        internal static StandardLevelSceneSetupDataSO LevelData;
        public void OnApplicationStart()
        {
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;

            //Create Harmony Instance
            harmony = HarmonyInstance.Create("com.kyle1413.BeatSaber.BS-Utils");

        }

        private void SceneManagerOnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            if (newScene.name == "Menu")
            {
                Utilities.Logger.Log("Removing Isolated Level");
                Gameplay.Gamemode.IsIsolatedLevel = false;
                Gameplay.Gamemode.IsolatingMod = "";
            }
        }

        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
        {

        }

        public void OnApplicationQuit()
        {
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }

        public void OnLevelWasLoaded(int level)
        {

        }

        public void OnLevelWasInitialized(int level)
        {
        }

        public void OnUpdate()
        {


        }

        public void OnFixedUpdate()
        {
        }


        internal static void ApplyHarmonyPatches()
        {
            if (patched) return;
            try
            {
                harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
                patched = true;
            }
            catch (Exception ex)
            {
                Utilities.Logger.Log("Exception Trying to Apply Harmony Patches");
                Utilities.Logger.Log(ex.ToString());
            }


        }
    }
}
