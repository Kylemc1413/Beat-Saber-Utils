using System;
using Harmony;

namespace BS_Utils.Gameplay.HarmonyPatches
{
    [HarmonyPatch(typeof(StandardLevelScenesTransitionSetupDataSO), "Init", new Type[] {typeof(IDifficultyBeatmap) , typeof(OverrideEnvironmentSettings) ,typeof(ColorScheme),
            typeof(GameplayModifiers) , typeof(PlayerSpecificSettings) , typeof(PracticeSettings) , typeof(string) , typeof(bool)})]
  //  [HarmonyPatch("Init", MethodType.Normal)]
    class BlahBlahGrabTheLevelData
    {
        static void Prefix(StandardLevelScenesTransitionSetupDataSO __instance, IDifficultyBeatmap difficultyBeatmap, OverrideEnvironmentSettings overrideEnvironmentSettings, ColorScheme overrideColorScheme,
            GameplayModifiers gameplayModifiers, PlayerSpecificSettings playerSpecificSettings, PracticeSettings practiceSettings, string backButtonText, bool useTestNoteCutSoundEffects)
        {
            Plugin.LevelData.GameplayCoreSceneSetupData = new GameplayCoreSceneSetupData(difficultyBeatmap, gameplayModifiers, playerSpecificSettings, practiceSettings, useTestNoteCutSoundEffects);
            Plugin.LevelData.IsSet = true;
            __instance.didFinishEvent -= __instance_didFinishEvent;
            __instance.didFinishEvent += __instance_didFinishEvent;

        }

        private static void __instance_didFinishEvent(StandardLevelScenesTransitionSetupDataSO levelScenesTransitionSetupDataSO, LevelCompletionResults levelCompletionResults)
        {
            Plugin.TriggerLevelFinishEvent(levelScenesTransitionSetupDataSO, levelCompletionResults);
        }
    }
    
    [HarmonyPatch(typeof(MissionLevelScenesTransitionSetupDataSO), "Init", new Type[] {typeof(IDifficultyBeatmap) , typeof(MissionObjective[]) ,typeof(OverrideEnvironmentSettings),
            typeof(ColorScheme) , typeof(GameplayModifiers) , typeof(PlayerSpecificSettings) , typeof(string)})]
    class BlahBlahGrabTheMissionLevelData
    {
        static void Prefix(MissionLevelScenesTransitionSetupDataSO __instance, IDifficultyBeatmap difficultyBeatmap,OverrideEnvironmentSettings overrideEnvironmentSettings,
            MissionObjective[] missionObjectives, GameplayModifiers gameplayModifiers, PlayerSpecificSettings playerSpecificSettings)
        {
            Plugin.LevelData.GameplayCoreSceneSetupData = new GameplayCoreSceneSetupData(difficultyBeatmap, gameplayModifiers, playerSpecificSettings, PracticeSettings.defaultPracticeSettings, false);
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
