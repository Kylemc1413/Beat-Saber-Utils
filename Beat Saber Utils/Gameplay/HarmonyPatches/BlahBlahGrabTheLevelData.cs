using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using TMPro;
namespace BS_Utils.Gameplay.HarmonyPatches
{
    [HarmonyPatch(typeof(StandardLevelScenesTransitionSetupDataSO),
          new Type[] {
            typeof(IDifficultyBeatmap),
                        typeof(GameplayModifiers),
                                    typeof(PlayerSpecificSettings),
            typeof(PracticeSettings)})]
    [HarmonyPatch("Init", MethodType.Normal)]
    class BlahBlahGrabTheLevelData
    {
        static void Prefix(StandardLevelScenesTransitionSetupDataSO __instance, IDifficultyBeatmap difficultyBeatmap, GameplayModifiers gameplayModifiers, PlayerSpecificSettings playerSpecificSettings, PracticeSettings practiceSettings)
        {
            Plugin.LevelData.GameplayCoreSceneSetupData = new GameplayCoreSceneSetupData(difficultyBeatmap, gameplayModifiers, playerSpecificSettings, practiceSettings);
            Plugin.LevelData.IsSet = true;
            __instance.didFinishEvent += __instance_didFinishEvent;
            
        }

        private static void __instance_didFinishEvent(StandardLevelScenesTransitionSetupDataSO levelScenesTransitionSetupDataSO, LevelCompletionResults levelCompletionResults)
        {
            Plugin.TriggerLevelFinishEvent(levelScenesTransitionSetupDataSO, levelCompletionResults);
        }
    }
}
