using System;
using BS_Utils.Utilities;
using HarmonyLib;
//using UnityEngine;
using Logger = BS_Utils.Utilities.Logger;

namespace BS_Utils.Gameplay.HarmonyPatches
{  
    [HarmonyPatch(
        typeof(StandardLevelScenesTransitionSetupDataSO),
        nameof(StandardLevelScenesTransitionSetupDataSO.Init),
        new Type[] { typeof(string), typeof(IBeatmapLevelData), typeof(BeatmapKey), typeof(BeatmapLevel), typeof(OverrideEnvironmentSettings), typeof(ColorScheme), typeof(ColorScheme), typeof(GameplayModifiers), typeof(PlayerSpecificSettings), typeof(PracticeSettings), typeof(EnvironmentsListModel), typeof(AudioClipAsyncLoader), typeof(BeatmapDataLoader), typeof(string), typeof(bool), typeof(bool), typeof(RecordingToolManager.SetupData?) },
        new ArgumentType[] { ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Ref, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal }
        )]
    [HarmonyPatch(
        typeof(StandardLevelScenesTransitionSetupDataSO),
        nameof(StandardLevelScenesTransitionSetupDataSO.Init),
        new Type[] { typeof(string), typeof(BeatmapKey), typeof(BeatmapLevel), typeof(OverrideEnvironmentSettings), typeof(ColorScheme), typeof(ColorScheme), typeof(GameplayModifiers), typeof(PlayerSpecificSettings), typeof(PracticeSettings), typeof(EnvironmentsListModel), typeof(AudioClipAsyncLoader), typeof(BeatmapDataLoader), typeof(string), typeof(BeatmapLevelsModel), typeof(bool), typeof(bool), typeof(RecordingToolManager.SetupData?) },
        new ArgumentType[] { ArgumentType.Normal, ArgumentType.Ref, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal }
        )]
    class BlahBlahGrabTheLevelData
    {
        static void Postfix(StandardLevelScenesTransitionSetupDataSO __instance)
        {
            //Debug.Log("StandardLevelScenesTransitionSetupDataSO.Init: Postfix");

            ScoreSubmission._wasDisabled = false;
            ScoreSubmission.LastDisablers = Array.Empty<string>();
            Plugin.scenesTransitionSetupData = __instance;
            var setupDataBase = (LevelScenesTransitionSetupDataSO)__instance;
            Plugin.LevelData.GameplayCoreSceneSetupData = setupDataBase.gameplayCoreSceneSetupData;
            Plugin.LevelData.IsSet = true;
            Plugin.LevelData.Mode = Mode.Standard;
            Logger.log.Debug("Level Data set");
            __instance.didFinishEvent -= __instance_didFinishEvent;
            __instance.didFinishEvent += __instance_didFinishEvent; // Not triggered in multiplayer
        }

        private static void __instance_didFinishEvent(StandardLevelScenesTransitionSetupDataSO levelScenesTransitionSetupDataSO, LevelCompletionResults levelCompletionResults)
        {
            Logger.log.Debug("Triggering LevelFinishEvent.");
            Plugin.TriggerLevelFinishEvent(levelScenesTransitionSetupDataSO, levelCompletionResults);
            BSEvents.TriggerLevelFinishEvent(levelScenesTransitionSetupDataSO, levelCompletionResults);
        }
    }

    [HarmonyPatch(typeof(MultiplayerLevelScenesTransitionSetupDataSO), nameof(MultiplayerLevelScenesTransitionSetupDataSO.Init))]
    class BlahBlahGrabTheMultiLevelData
    {
        static void Postfix(MultiplayerLevelScenesTransitionSetupDataSO __instance)
        {
            //Debug.Log("MultiplayerLevelScenesTransitionSetupDataSO.Init: Postfix");

            ScoreSubmission._wasDisabled = false;
            ScoreSubmission.LastDisablers = Array.Empty<string>();
            Plugin.scenesTransitionSetupData = __instance;
            var setupDataBase = (LevelScenesTransitionSetupDataSO) __instance;
            Plugin.LevelData.GameplayCoreSceneSetupData = setupDataBase.gameplayCoreSceneSetupData;
            Plugin.LevelData.IsSet = true;
            Plugin.LevelData.Mode = Mode.Multiplayer;
            Logger.log.Debug("Level Data set");
            __instance.didFinishEvent -= __instance_didFinishEvent;
            __instance.didFinishEvent += __instance_didFinishEvent;

            __instance.didDisconnectEvent -= __instance_didDisconnectEvent;
            __instance.didDisconnectEvent += __instance_didDisconnectEvent;
        }

        private static void __instance_didFinishEvent(MultiplayerLevelScenesTransitionSetupDataSO levelScenesTransitionSetupDataSO, MultiplayerResultsData resultsData)
        {
            Logger.log.Debug("Triggering Multiplayer LevelFinishEvent.");
            Plugin.TriggerMultiplayerLevelDidFinish(levelScenesTransitionSetupDataSO, resultsData.localPlayerResultData.multiplayerLevelCompletionResults.levelCompletionResults, resultsData.otherPlayersData);
            BSEvents.TriggerMultiplayerLevelDidFinish(levelScenesTransitionSetupDataSO, resultsData.localPlayerResultData.multiplayerLevelCompletionResults.levelCompletionResults, resultsData.otherPlayersData);
        }

        private static void __instance_didDisconnectEvent(MultiplayerLevelScenesTransitionSetupDataSO levelScenesTransitionSetupDataSO, DisconnectedReason disconnectedReason)
        {
            Logger.log.Debug("Triggering Multiplayer DidDisconnectEvent.");
            Plugin.TriggerMultiplayerDidDisconnect(levelScenesTransitionSetupDataSO, disconnectedReason);
            BSEvents.TriggerMultiplayerDidDisconnect(levelScenesTransitionSetupDataSO, disconnectedReason);
        }
    }

    [HarmonyPatch(
        typeof(MissionLevelScenesTransitionSetupDataSO), 
        nameof(MissionLevelScenesTransitionSetupDataSO.Init),
        new Type[] { typeof(string), typeof(IBeatmapLevelData), typeof(BeatmapKey), typeof(BeatmapLevel), typeof(MissionObjective[]), typeof(ColorScheme), typeof(GameplayModifiers), typeof(PlayerSpecificSettings), typeof(EnvironmentsListModel), typeof(AudioClipAsyncLoader), typeof(BeatmapDataLoader), typeof(string) },
        new ArgumentType[] { ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Ref, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal }
        )]
    [HarmonyPatch(
        typeof(MissionLevelScenesTransitionSetupDataSO), 
        nameof(MissionLevelScenesTransitionSetupDataSO.Init),
        new Type[] { typeof(string), typeof(BeatmapKey), typeof(BeatmapLevel), typeof(MissionObjective[]), typeof(ColorScheme), typeof(GameplayModifiers), typeof(PlayerSpecificSettings), typeof(EnvironmentsListModel), typeof(BeatmapLevelsModel), typeof(AudioClipAsyncLoader), typeof(BeatmapDataLoader), typeof(string) },
        new ArgumentType[] { ArgumentType.Normal, ArgumentType.Ref, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal }
        )]
    class BlahBlahGrabTheMissionLevelData
    {
        static void Postfix(MissionLevelScenesTransitionSetupDataSO __instance)
        {
            ScoreSubmission._wasDisabled = false;
            ScoreSubmission.LastDisablers = Array.Empty<string>();
            Plugin.scenesTransitionSetupData = __instance;
            var setupDataBase = (LevelScenesTransitionSetupDataSO) __instance;
            Plugin.LevelData.GameplayCoreSceneSetupData = setupDataBase.gameplayCoreSceneSetupData;
            Plugin.LevelData.IsSet = true;
            Plugin.LevelData.Mode = Mode.Mission;
            Logger.log.Debug("Level Data set");
            __instance.didFinishEvent -= __instance_didFinishEvent;
            __instance.didFinishEvent += __instance_didFinishEvent;

        }

        private static void __instance_didFinishEvent(MissionLevelScenesTransitionSetupDataSO missionLevelScenesTransitionSetupDataSO, MissionCompletionResults missionCompletionResults)
        {
            Plugin.TriggerMissionFinishEvent(missionLevelScenesTransitionSetupDataSO, missionCompletionResults);
            BSEvents.TriggerMissionFinishEvent(missionLevelScenesTransitionSetupDataSO, missionCompletionResults);
        }
    }

    [HarmonyPatch(typeof(TutorialScenesTransitionSetupDataSO), nameof(TutorialScenesTransitionSetupDataSO.Init))]
    class BlahBlahSetTutorialEvent
    {
        static void Postfix(TutorialScenesTransitionSetupDataSO __instance)
        {
            ScoreSubmission._wasDisabled = false;
            ScoreSubmission.LastDisablers = Array.Empty<string>();

            __instance.didFinishEvent -= __instance_didFinishEvent;
            __instance.didFinishEvent += __instance_didFinishEvent;

        }

        private static void __instance_didFinishEvent(TutorialScenesTransitionSetupDataSO missionLevelScenesTransitionSetupDataSO, TutorialScenesTransitionSetupDataSO.TutorialEndStateType endState)
        {
            Plugin.TriggerTutorialFinishEvent(missionLevelScenesTransitionSetupDataSO, endState);
            BSEvents.TriggerTutorialFinishEvent(missionLevelScenesTransitionSetupDataSO, endState);
        }
    }
}
