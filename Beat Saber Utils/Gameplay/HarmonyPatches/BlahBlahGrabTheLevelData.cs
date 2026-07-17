using System;
using BS_Utils.Utilities;
using HarmonyLib;
//using UnityEngine;
using Logger = BS_Utils.Utilities.Logger;

namespace BS_Utils.Gameplay.HarmonyPatches
{
    [HarmonyPatch(typeof(StandardLevelScenesTransitionSetupData), nameof(StandardLevelScenesTransitionSetupData.Init))]
    class BlahBlahGrabTheLevelData
    {
        static void Postfix(StandardLevelScenesTransitionSetupData __instance)
        {
            //Debug.Log("StandardLevelScenesTransitionSetupData.Init: Postfix");

            ScoreSubmission._wasDisabled = false;
            ScoreSubmission.LastDisablers = Array.Empty<string>();
            Plugin.scenesTransitionSetupData = __instance;
            var setupDataBase = (LevelScenesTransitionSetupData)__instance;
            Plugin.LevelData.GameplayCoreSceneSetupData = setupDataBase.gameplayCoreSceneSetupData;
            Plugin.LevelData.IsSet = true;
            Plugin.LevelData.Mode = Mode.Standard;
            Logger.log.Debug("Level Data set");
            __instance.didFinishEvent -= __instance_didFinishEvent;
            __instance.didFinishEvent += __instance_didFinishEvent; // Not triggered in multiplayer
        }

        private static void __instance_didFinishEvent(StandardLevelScenesTransitionSetupData levelScenesTransitionSetupData, LevelCompletionResults levelCompletionResults)
        {
            Logger.log.Debug("Triggering LevelFinishEvent.");
            Plugin.TriggerLevelFinishEvent(levelScenesTransitionSetupData, levelCompletionResults);
            BSEvents.TriggerLevelFinishEvent(levelScenesTransitionSetupData, levelCompletionResults);
        }
    }

    [HarmonyPatch(typeof(MultiplayerLevelScenesTransitionSetupData), nameof(MultiplayerLevelScenesTransitionSetupData.Init))]
    class BlahBlahGrabTheMultiLevelData
    {
        static void Postfix(MultiplayerLevelScenesTransitionSetupData __instance)
        {
            //Debug.Log("MultiplayerLevelScenesTransitionSetupData.Init: Postfix");

            ScoreSubmission._wasDisabled = false;
            ScoreSubmission.LastDisablers = Array.Empty<string>();
            Plugin.scenesTransitionSetupData = __instance;
            var setupDataBase = (LevelScenesTransitionSetupData) __instance;
            Plugin.LevelData.GameplayCoreSceneSetupData = setupDataBase.gameplayCoreSceneSetupData;
            Plugin.LevelData.IsSet = true;
            Plugin.LevelData.Mode = Mode.Multiplayer;
            Logger.log.Debug("Level Data set");
            __instance.didFinishEvent -= __instance_didFinishEvent;
            __instance.didFinishEvent += __instance_didFinishEvent;

            __instance.didDisconnectEvent -= __instance_didDisconnectEvent;
            __instance.didDisconnectEvent += __instance_didDisconnectEvent;
        }

        private static void __instance_didFinishEvent(MultiplayerLevelScenesTransitionSetupData levelScenesTransitionSetupData, MultiplayerResultsData resultsData)
        {
            Logger.log.Debug("Triggering Multiplayer LevelFinishEvent.");
            Plugin.TriggerMultiplayerLevelDidFinish(levelScenesTransitionSetupData, resultsData.localPlayerResultData.multiplayerLevelCompletionResults.levelCompletionResults, resultsData.otherPlayersData);
            BSEvents.TriggerMultiplayerLevelDidFinish(levelScenesTransitionSetupData, resultsData.localPlayerResultData.multiplayerLevelCompletionResults.levelCompletionResults, resultsData.otherPlayersData);
        }

        private static void __instance_didDisconnectEvent(MultiplayerLevelScenesTransitionSetupData levelScenesTransitionSetupData, DisconnectedReason disconnectedReason)
        {
            Logger.log.Debug("Triggering Multiplayer DidDisconnectEvent.");
            Plugin.TriggerMultiplayerDidDisconnect(levelScenesTransitionSetupData, disconnectedReason);
            BSEvents.TriggerMultiplayerDidDisconnect(levelScenesTransitionSetupData, disconnectedReason);
        }
    }

    [HarmonyPatch]
    [HarmonyPatch(typeof(MissionLevelScenesTransitionSetupData), nameof(MissionLevelScenesTransitionSetupData.Init))]
    class BlahBlahGrabTheMissionLevelData
    {
        static void Postfix(MissionLevelScenesTransitionSetupData __instance)
        {
            ScoreSubmission._wasDisabled = false;
            ScoreSubmission.LastDisablers = Array.Empty<string>();
            Plugin.scenesTransitionSetupData = __instance;
            var setupDataBase = (LevelScenesTransitionSetupData) __instance;
            Plugin.LevelData.GameplayCoreSceneSetupData = setupDataBase.gameplayCoreSceneSetupData;
            Plugin.LevelData.IsSet = true;
            Plugin.LevelData.Mode = Mode.Mission;
            Logger.log.Debug("Level Data set");
            __instance.didFinishEvent -= __instance_didFinishEvent;
            __instance.didFinishEvent += __instance_didFinishEvent;

        }

        private static void __instance_didFinishEvent(MissionLevelScenesTransitionSetupData missionLevelScenesTransitionSetupData, MissionCompletionResults missionCompletionResults)
        {
            Plugin.TriggerMissionFinishEvent(missionLevelScenesTransitionSetupData, missionCompletionResults);
            BSEvents.TriggerMissionFinishEvent(missionLevelScenesTransitionSetupData, missionCompletionResults);
        }
    }

    [HarmonyPatch(typeof(TutorialScenesTransitionSetupData), nameof(TutorialScenesTransitionSetupData.Init))]
    class BlahBlahSetTutorialEvent
    {
        static void Postfix(TutorialScenesTransitionSetupData __instance)
        {
            ScoreSubmission._wasDisabled = false;
            ScoreSubmission.LastDisablers = Array.Empty<string>();

            __instance.didFinishEvent -= __instance_didFinishEvent;
            __instance.didFinishEvent += __instance_didFinishEvent;

        }

        private static void __instance_didFinishEvent(TutorialScenesTransitionSetupData tutorialScenesTransitionSetupData, TutorialScenesTransitionSetupData.TutorialEndStateType endState)
        {
            Plugin.TriggerTutorialFinishEvent(tutorialScenesTransitionSetupData, endState);
            BSEvents.TriggerTutorialFinishEvent(tutorialScenesTransitionSetupData, endState);
        }
    }
}
