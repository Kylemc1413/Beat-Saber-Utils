using System;
using BS_Utils.Utilities;
using HarmonyLib;
using IPA.Logging;

namespace BS_Utils.Gameplay.HarmonyPatches
{

    [HarmonyPatch(typeof(StandardLevelScenesTransitionSetupDataSO), "Init")]
    /*, new Type[] 
    {typeof(string), typeof(IDifficultyBeatmap) ,
        typeof(OverrideEnvironmentSettings), 
        typeof(ColorScheme),

            typeof(GameplayModifiers) ,
        typeof(PlayerSpecificSettings) ,
        typeof(PracticeSettings) ,
        typeof(string) ,
        typeof(bool)})]
    */
    class BlahBlahGrabTheLevelData
    {
        static void Postfix(StandardLevelScenesTransitionSetupDataSO __instance, string gameMode, IDifficultyBeatmap difficultyBeatmap, IPreviewBeatmapLevel previewBeatmapLevel, OverrideEnvironmentSettings overrideEnvironmentSettings,
            GameplayModifiers gameplayModifiers, ColorScheme overrideColorScheme, PlayerSpecificSettings playerSpecificSettings, ref PracticeSettings practiceSettings, string backButtonText, bool useTestNoteCutSoundEffects)
        {
            EnvironmentInfoSO environmentInfoSO = difficultyBeatmap.GetEnvironmentInfo();
            if (overrideEnvironmentSettings != null && overrideEnvironmentSettings.overrideEnvironments)
            {
                environmentInfoSO = overrideEnvironmentSettings.GetOverrideEnvironmentInfoForType(environmentInfoSO.environmentType);
            }
            ScoreSubmission._wasDisabled = false;
            ScoreSubmission.LastDisablers = Array.Empty<string>();
            Plugin.LevelData.GameplayCoreSceneSetupData = new GameplayCoreSceneSetupData(difficultyBeatmap, previewBeatmapLevel, gameplayModifiers, playerSpecificSettings, practiceSettings, useTestNoteCutSoundEffects, environmentInfoSO, overrideColorScheme);
            Plugin.LevelData.IsSet = true;
            Plugin.LevelData.Mode = Mode.Standard;
            Utilities.Logger.log.Debug("Level Data set");
            __instance.didFinishEvent -= __instance_didFinishEvent;
            __instance.didFinishEvent += __instance_didFinishEvent; // Not triggered in multiplayer

        }

        private static void __instance_didFinishEvent(StandardLevelScenesTransitionSetupDataSO levelScenesTransitionSetupDataSO, LevelCompletionResults levelCompletionResults)
        {
            Utilities.Logger.log.Debug("Triggering LevelFinishEvent.");
            Plugin.TriggerLevelFinishEvent(levelScenesTransitionSetupDataSO, levelCompletionResults);
            BSEvents.TriggerLevelFinishEvent(levelScenesTransitionSetupDataSO, levelCompletionResults);

        }
    }

    [HarmonyPatch(typeof(MultiplayerLevelScenesTransitionSetupDataSO), "Init")]
    /*, new Type[] 
    {typeof(string), typeof(IDifficultyBeatmap) ,
        typeof(OverrideEnvironmentSettings), 
        typeof(ColorScheme),

            typeof(GameplayModifiers) ,
        typeof(PlayerSpecificSettings) ,
        typeof(PracticeSettings) ,
        typeof(string) ,
        typeof(bool)})]
    */
    class BlahBlahGrabTheMultiLevelData
    {
        static void Postfix(MultiplayerLevelScenesTransitionSetupDataSO __instance, ref EnvironmentInfoSO ____multiplayerEnvironmentInfo, string gameMode,
            IPreviewBeatmapLevel previewBeatmapLevel, BeatmapDifficulty beatmapDifficulty,
            BeatmapCharacteristicSO beatmapCharacteristic, IDifficultyBeatmap difficultyBeatmap,
            ColorScheme overrideColorScheme, GameplayModifiers gameplayModifiers, PlayerSpecificSettings playerSpecificSettings,
            ref PracticeSettings practiceSettings, bool useTestNoteCutSoundEffects = false)
        {

            ScoreSubmission._wasDisabled = false;
            ScoreSubmission.LastDisablers = Array.Empty<string>();
            Plugin.LevelData.GameplayCoreSceneSetupData = new GameplayCoreSceneSetupData(difficultyBeatmap, previewBeatmapLevel, gameplayModifiers, playerSpecificSettings, practiceSettings, useTestNoteCutSoundEffects, ____multiplayerEnvironmentInfo, overrideColorScheme);
            Plugin.LevelData.IsSet = true;
            Plugin.LevelData.Mode = Mode.Multiplayer;
            Utilities.Logger.log.Debug("Level Data set");
            __instance.didFinishEvent -= __instance_didFinishEvent;
            __instance.didFinishEvent += __instance_didFinishEvent;
        }

        private static void __instance_didFinishEvent(MultiplayerLevelScenesTransitionSetupDataSO levelScenesTransitionSetupDataSO, MultiplayerResultsData resultsData)
        {
            Utilities.Logger.log.Debug("Triggering Multiplayer LevelFinishEvent.");
            Plugin.TriggerMultiplayerLevelDidFinish(levelScenesTransitionSetupDataSO, resultsData.localPlayerResultData.levelCompletionResults, resultsData.otherPlayersData);
            BSEvents.TriggerMultiplayerLevelDidFinish(levelScenesTransitionSetupDataSO, resultsData.localPlayerResultData.levelCompletionResults, resultsData.otherPlayersData);
        }
    }
    [HarmonyPatch(typeof(MissionLevelScenesTransitionSetupDataSO), "Init")]
    /*
        ,new Type[] {typeof(string), typeof(IDifficultyBeatmap) , typeof(MissionObjective[]) ,typeof(ColorScheme),
            typeof(GameplayModifiers) , typeof(PlayerSpecificSettings) , typeof(string)})]*/
    class BlahBlahGrabTheMissionLevelData
    {
        static void Postfix(MissionLevelScenesTransitionSetupDataSO __instance, IPreviewBeatmapLevel previewBeatmapLevel, string missionId, IDifficultyBeatmap difficultyBeatmap, MissionObjective[] missionObjectives, ColorScheme overrideColorScheme, GameplayModifiers gameplayModifiers, PlayerSpecificSettings playerSpecificSettings, string backButtonText)
        {
            EnvironmentInfoSO environmentInfoSO = difficultyBeatmap.GetEnvironmentInfo();

            ScoreSubmission._wasDisabled = false;
            ScoreSubmission.LastDisablers = Array.Empty<string>();
            Plugin.LevelData.GameplayCoreSceneSetupData = new GameplayCoreSceneSetupData(difficultyBeatmap, previewBeatmapLevel, gameplayModifiers, playerSpecificSettings, PracticeSettings.defaultPracticeSettings, false, environmentInfoSO, overrideColorScheme);
            Plugin.LevelData.IsSet = true;
            Plugin.LevelData.Mode = Mode.Mission;
            Utilities.Logger.log.Debug("Level Data set");
            __instance.didFinishEvent -= __instance_didFinishEvent;
            __instance.didFinishEvent += __instance_didFinishEvent;

        }

        private static void __instance_didFinishEvent(MissionLevelScenesTransitionSetupDataSO missionLevelScenesTransitionSetupDataSO, MissionCompletionResults missionCompletionResults)
        {
            Plugin.TriggerMissionFinishEvent(missionLevelScenesTransitionSetupDataSO, missionCompletionResults);
            BSEvents.TriggerMissionFinishEvent(missionLevelScenesTransitionSetupDataSO, missionCompletionResults);
        }
    }

    [HarmonyPatch(typeof(TutorialScenesTransitionSetupDataSO), "Init")]
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
