using System;
using HarmonyLib;

namespace BS_Utils.Gameplay.HarmonyPatches
{
    
    [HarmonyPatch(typeof(StandardLevelScenesTransitionSetupDataSO), "Init", new Type[] 
    {typeof(string), typeof(IDifficultyBeatmap) ,
        typeof(OverrideEnvironmentSettings), 
        typeof(ColorScheme),

            typeof(GameplayModifiers) ,
        typeof(PlayerSpecificSettings) ,
        typeof(PracticeSettings) ,
        typeof(string) ,
        typeof(bool)})]
    
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
            __instance.didFinishEvent -= __instance_didFinishEvent;
            __instance.didFinishEvent += __instance_didFinishEvent;

        }

        private static void __instance_didFinishEvent(StandardLevelScenesTransitionSetupDataSO levelScenesTransitionSetupDataSO, LevelCompletionResults levelCompletionResults)
        {
            Plugin.TriggerLevelFinishEvent(levelScenesTransitionSetupDataSO, levelCompletionResults);
        }
    }
    
    [HarmonyPatch(typeof(MissionLevelScenesTransitionSetupDataSO), "Init", new Type[] {typeof(string), typeof(IDifficultyBeatmap) , typeof(MissionObjective[]) ,typeof(ColorScheme),
            typeof(GameplayModifiers) , typeof(PlayerSpecificSettings) , typeof(string)})]
    class BlahBlahGrabTheMissionLevelData
    {
        static void Prefix(MissionLevelScenesTransitionSetupDataSO __instance, IDifficultyBeatmap difficultyBeatmap,OverrideEnvironmentSettings overrideColorScheme,
            MissionObjective[] missionObjectives, GameplayModifiers gameplayModifiers, PlayerSpecificSettings playerSpecificSettings)
        {
            EnvironmentInfoSO environmentInfoSO = difficultyBeatmap.GetEnvironmentInfo();
            if (overrideColorScheme != null && overrideColorScheme.overrideEnvironments)
            {
                environmentInfoSO = overrideColorScheme.GetOverrideEnvironmentInfoForType(environmentInfoSO.environmentType);
            }
            ScoreSubmission._wasDisabled = false;
            ScoreSubmission.LastDisablers = Array.Empty<string>();
            Plugin.LevelData.GameplayCoreSceneSetupData = new GameplayCoreSceneSetupData(difficultyBeatmap, gameplayModifiers, playerSpecificSettings, PracticeSettings.defaultPracticeSettings, false, environmentInfoSO);
            Plugin.LevelData.IsSet = true;
            __instance.didFinishEvent -= __instance_didFinishEvent;
            __instance.didFinishEvent += __instance_didFinishEvent;

        }

        private static void __instance_didFinishEvent(MissionLevelScenesTransitionSetupDataSO missionLevelScenesTransitionSetupDataSO, MissionCompletionResults missionCompletionResults)
        {
            Plugin.TriggerMissionFinishEvent(missionLevelScenesTransitionSetupDataSO, missionCompletionResults);
        }
    }
}
