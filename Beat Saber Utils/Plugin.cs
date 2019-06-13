using System;
using UnityEngine.SceneManagement;
using IPA;
using Harmony;
using IPALogger = IPA.Logging.Logger;
using LogLevel = IPA.Logging.Logger.Level;
namespace BS_Utils
{
    public class Plugin : IBeatSaberPlugin
    {
        
        internal static bool patched = false;
        internal static HarmonyInstance harmony;

        public static Gameplay.LevelData LevelData = new Gameplay.LevelData();

        public delegate void LevelDidFinish(StandardLevelScenesTransitionSetupDataSO levelScenesTransitionSetupDataSO, LevelCompletionResults levelCompletionResults);
        public static event LevelDidFinish LevelDidFinishEvent;
        
        public delegate void MissionDidFinish(MissionLevelScenesTransitionSetupDataSO missionLevelScenesTransitionSetupDataSO, MissionCompletionResults missionCompletionResults);
        public static event MissionDidFinish MissionDidFinishEvent;

        public void OnApplicationStart()
        {
            

            //Create Harmony Instance
            harmony = HarmonyInstance.Create("com.kyle1413.BeatSaber.BS-Utils");
        }

        public void Init(object thisIsNull, IPALogger logger)
        {
            
                Utilities.Logger.log = logger;

            
        }
        
        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {

        }

        public void OnSceneUnloaded(Scene scene)
        {

        }

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
            if (nextScene.name == "MenuCore")
            {
                Utilities.Logger.Log("Removing Isolated Level");
                Gameplay.Gamemode.IsIsolatedLevel = false;
                Gameplay.Gamemode.IsolatingMod = "";
                LevelData.Clear();
            }
        }

       
        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
        {

        }

        public void OnApplicationQuit()
        {

        }


        public void OnUpdate() { }

        public void OnFixedUpdate() { }

        internal static void TriggerLevelFinishEvent(StandardLevelScenesTransitionSetupDataSO levelScenesTransitionSetupDataSO, LevelCompletionResults levelCompletionResults)
        {
            LevelDidFinishEvent?.Invoke(levelScenesTransitionSetupDataSO, levelCompletionResults);
        }
        internal static void TriggerMissionFinishEvent(MissionLevelScenesTransitionSetupDataSO missionLevelScenesTransitionSetupDataSO, MissionCompletionResults missionCompletionResults)
        {
            MissionDidFinishEvent?.Invoke(missionLevelScenesTransitionSetupDataSO, missionCompletionResults);
        }

        internal static void ApplyHarmonyPatches()
        {
            if (patched) return;
            try
            {
                Utilities.Logger.Log("Applying Harmony Patches", LogLevel.Debug);
                harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
                patched = true;
            }
            catch (Exception ex)
            {
                Utilities.Logger.Log("Exception Trying to Apply Harmony Patches", LogLevel.Error);
                Utilities.Logger.Log(ex.ToString(), LogLevel.Error);
            }
        }

    }
}
