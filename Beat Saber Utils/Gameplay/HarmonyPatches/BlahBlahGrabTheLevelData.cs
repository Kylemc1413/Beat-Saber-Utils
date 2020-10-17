using System;
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
        static void Prefix(StandardLevelScenesTransitionSetupDataSO __instance, string gameMode, IDifficultyBeatmap difficultyBeatmap, OverrideEnvironmentSettings overrideEnvironmentSettings,
            GameplayModifiers gameplayModifiers, PlayerSpecificSettings playerSpecificSettings, PracticeSettings practiceSettings, string backButtonText, bool useTestNoteCutSoundEffects)
        {
            EnvironmentInfoSO environmentInfoSO = difficultyBeatmap.GetEnvironmentInfo();
            if (overrideEnvironmentSettings != null && overrideEnvironmentSettings.overrideEnvironments)
            {
                environmentInfoSO = overrideEnvironmentSettings.GetOverrideEnvironmentInfoForType(environmentInfoSO.environmentType);
            }

            ScoreSubmission._wasDisabled = false;
            ScoreSubmission.LastDisablers = Array.Empty<string>();
            Plugin.LevelData.GameplayCoreSceneSetupData = new GameplayCoreSceneSetupData(difficultyBeatmap, gameplayModifiers, playerSpecificSettings, practiceSettings, useTestNoteCutSoundEffects, environmentInfoSO);
            Plugin.LevelData.IsSet = true;
            Plugin.LevelData.Mode = Mode.Standard;
            Utilities.Logger.log.Debug("Level Data set");
            __instance.didFinishEvent -= __instance_didFinishEvent;
            __instance.didFinishEvent += __instance_didFinishEvent;

        }

        private static void __instance_didFinishEvent(StandardLevelScenesTransitionSetupDataSO levelScenesTransitionSetupDataSO, LevelCompletionResults levelCompletionResults)
        {
            Plugin.TriggerLevelFinishEvent(levelScenesTransitionSetupDataSO, levelCompletionResults);
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
        static void Prefix(MultiplayerLevelScenesTransitionSetupDataSO __instance, ref EnvironmentInfoSO ____multiplayerEnvironmentInfo, string gameMode,
            IPreviewBeatmapLevel previewBeatmapLevel, BeatmapDifficulty beatmapDifficulty,
            BeatmapCharacteristicSO beatmapCharacteristic, IDifficultyBeatmap difficultyBeatmap,
            ColorScheme overrideColorScheme, GameplayModifiers gameplayModifiers, PlayerSpecificSettings playerSpecificSettings,
            PracticeSettings practiceSettings, bool useTestNoteCutSoundEffects = false)
        {

            ScoreSubmission._wasDisabled = false;
            ScoreSubmission.LastDisablers = Array.Empty<string>();
            Plugin.LevelData.GameplayCoreSceneSetupData = new GameplayCoreSceneSetupData(difficultyBeatmap, gameplayModifiers, playerSpecificSettings, practiceSettings, useTestNoteCutSoundEffects, ____multiplayerEnvironmentInfo);
            Plugin.LevelData.IsSet = true;
            Plugin.LevelData.Mode = Mode.Multiplayer;
            Utilities.Logger.log.Debug("Level Data set");
            __instance.didFinishEvent -= __instance_didFinishEvent;
            __instance.didFinishEvent += __instance_didFinishEvent;
        }

        private static void __instance_didFinishEvent(MultiplayerLevelScenesTransitionSetupDataSO levelScenesTransitionSetupDataSO, LevelCompletionResults levelCompletionResults, System.Collections.Generic.Dictionary<string, LevelCompletionResults> otherPlayersLevelCompletionResults)
        {
            Plugin.TriggerMultiplayerLevelDidFinish(levelScenesTransitionSetupDataSO, levelCompletionResults, otherPlayersLevelCompletionResults);
        }
    }
    [HarmonyPatch(typeof(MissionLevelScenesTransitionSetupDataSO), "Init")]
    /*
        ,new Type[] {typeof(string), typeof(IDifficultyBeatmap) , typeof(MissionObjective[]) ,typeof(ColorScheme),
            typeof(GameplayModifiers) , typeof(PlayerSpecificSettings) , typeof(string)})]*/
    class BlahBlahGrabTheMissionLevelData
    {
        static void Prefix(MissionLevelScenesTransitionSetupDataSO __instance, string missionId, IDifficultyBeatmap difficultyBeatmap, MissionObjective[] missionObjectives, ColorScheme overrideColorScheme, GameplayModifiers gameplayModifiers, PlayerSpecificSettings playerSpecificSettings, string backButtonText)
        {
            EnvironmentInfoSO environmentInfoSO = difficultyBeatmap.GetEnvironmentInfo();

            ScoreSubmission._wasDisabled = false;
            ScoreSubmission.LastDisablers = Array.Empty<string>();
            Plugin.LevelData.GameplayCoreSceneSetupData = new GameplayCoreSceneSetupData(difficultyBeatmap, gameplayModifiers, playerSpecificSettings, PracticeSettings.defaultPracticeSettings, false, environmentInfoSO);
            Plugin.LevelData.IsSet = true;
            Plugin.LevelData.Mode = Mode.Mission;
            Utilities.Logger.log.Debug("Level Data set");
            __instance.didFinishEvent -= __instance_didFinishEvent;
            __instance.didFinishEvent += __instance_didFinishEvent;

        }

        private static void __instance_didFinishEvent(MissionLevelScenesTransitionSetupDataSO missionLevelScenesTransitionSetupDataSO, MissionCompletionResults missionCompletionResults)
        {
            Plugin.TriggerMissionFinishEvent(missionLevelScenesTransitionSetupDataSO, missionCompletionResults);
        }
    }
}
