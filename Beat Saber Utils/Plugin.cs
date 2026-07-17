using IPA;
using System;
using System.Collections.Generic;
using HarmonyLib;
using BS_Utils.Gameplay;
using BS_Utils.Utilities;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;
using LogLevel = IPA.Logging.Logger.Level;
using IPA.Loader;
using System.Threading.Tasks;
using Logger = BS_Utils.Utilities.Logger;
using BS_Utils.Utilities.Events;
using Object = UnityEngine.Object;

namespace BS_Utils
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static bool patched = false;
        internal static Harmony harmony;
        internal static ScenesTransitionSetupData scenesTransitionSetupData;

        public static LevelData LevelData = new LevelData();

        internal static event EventHandler<LevelFinishedEventArgs> LevelFinished; // Raised before the BSEvents version.
        internal static event EventHandler<(MultiplayerLevelScenesTransitionSetupData, DisconnectedReason)> MultiplayerDidDisconnect;

        [OnStart]
        public void OnApplicationStart()
        {
            //Create Harmony Instance
            harmony = new Harmony("com.kyle1413.BeatSaber.BS-Utils");
            BSEvents.OnLoad();
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            PluginManager.OnPluginsStateChanged += PluginManager_OnPluginsStateChanged;
            ApplyHarmonyPatches();
        }

        private void PluginManager_OnPluginsStateChanged(Task task)
        {
            var fadeOutHelper = Object.FindAnyObjectByType<FadeInOutController>();
            if (fadeOutHelper != null)
            {
                fadeOutHelper.FadeOut();
            }

            var mainFlowCoordinator = Object.FindAnyObjectByType<MainFlowCoordinator>();
            if (mainFlowCoordinator != null)
            {
                task.ContinueWith(_ => mainFlowCoordinator._menuTransitionsHelper.RestartGame(), TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        [Init]
        public void Init(IPALogger logger)
        {
            Logger.log = logger;
            GetUserInfo.UpdateUserInfo();
        }

        [OnExit]
        public void Exit()
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
            PluginManager.OnPluginsStateChanged -= PluginManager_OnPluginsStateChanged;
        }

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
            if (nextScene.name == SceneNames.Menu)
                GetUserInfo.TriggerReady();
            if (nextScene.name == SceneNames.PostSongMenu)
            {
                GetUserInfo.TriggerReady();
                if (Gamemode.IsIsolatedLevel) // Only remove is necessary.
                    Logger.Log("Removing Isolated Level");
                Gamemode.IsIsolatedLevel = false;
                Gamemode.IsolatingMod = "";
                scenesTransitionSetupData = null;
                LevelData.Clear();
            }
        }

        internal static void TriggerLevelFinishEvent(StandardLevelScenesTransitionSetupData levelScenesTransitionSetupData, LevelCompletionResults levelCompletionResults)
        {
            Logger.log.Debug("Solo/Party mode level finished.");
            LevelFinished?.RaiseEventSafe(levelScenesTransitionSetupData, new SoloLevelFinishedEventArgs(levelScenesTransitionSetupData, levelCompletionResults), nameof(LevelFinished));
        }

        internal static void TriggerMultiplayerLevelDidFinish(MultiplayerLevelScenesTransitionSetupData levelScenesTransitionSetupData, LevelCompletionResults levelCompletionResults,
            IReadOnlyList<MultiplayerPlayerResultsData> otherPlayersLevelCompletionResults)
        {
            Logger.log.Debug("Multiplayer level finished.");
            LevelFinished?.RaiseEventSafe(levelScenesTransitionSetupData, new MultiplayerLevelFinishedEventArgs(levelScenesTransitionSetupData, levelCompletionResults, otherPlayersLevelCompletionResults), nameof(LevelFinished));
        }

        internal static void TriggerMissionFinishEvent(MissionLevelScenesTransitionSetupData missionLevelScenesTransitionSetupData, MissionCompletionResults missionCompletionResults)
        {
            Logger.log.Debug("Campaign level finished.");
            LevelFinished?.RaiseEventSafe(missionLevelScenesTransitionSetupData, new CampaignLevelFinishedEventArgs(missionLevelScenesTransitionSetupData, missionCompletionResults), nameof(LevelFinished));
        }

        internal static void TriggerTutorialFinishEvent(TutorialScenesTransitionSetupData tutorialLevelScenesTransitionSetupData, TutorialScenesTransitionSetupData.TutorialEndStateType endState)
        {
            Logger.log.Debug("Tutorial level finished.");
            LevelFinished?.RaiseEventSafe(tutorialLevelScenesTransitionSetupData, new TutorialLevelFinishedEventArgs(tutorialLevelScenesTransitionSetupData, endState), nameof(LevelFinished));
        }

        internal static void TriggerMultiplayerDidDisconnect(MultiplayerLevelScenesTransitionSetupData levelScenesTransitionSetupData, DisconnectedReason resultsData)
        {
            Logger.log.Debug("Multiplayer did disconnect.");
            MultiplayerDidDisconnect?.RaiseEventSafe(levelScenesTransitionSetupData, (levelScenesTransitionSetupData, resultsData), nameof(MultiplayerDidDisconnect));
        }

        internal static void ApplyHarmonyPatches()
        {
            if (patched) return;
            try
            {
                Logger.Log("Applying Harmony Patches", LogLevel.Debug);
                harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
                patched = true;
            }
            catch (Exception ex)
            {
                Logger.Log("Exception Trying to Apply Harmony Patches", LogLevel.Error);
                Logger.Log(ex.ToString(), LogLevel.Error);
            }
        }
    }
}
