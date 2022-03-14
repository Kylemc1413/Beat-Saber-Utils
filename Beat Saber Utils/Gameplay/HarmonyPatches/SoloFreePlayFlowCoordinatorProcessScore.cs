using System.Linq;
using HarmonyLib;
using IPA.Utilities;

namespace BS_Utils.Gameplay.HarmonyPatches
{
    [HarmonyPatch(typeof(LevelCompletionResultsHelper))]
    [HarmonyPatch(nameof(LevelCompletionResultsHelper.ProcessScore), MethodType.Normal)]
    class SoloFreePlayFlowCoordinatorProcessScore
    {
        static bool Prefix()
        {
            return !ScoreSubmission.WasDisabled && !ScoreSubmission.disabled && !ScoreSubmission.prolongedDisable;
        }
    }

    [HarmonyPatch(typeof(PrepareLevelCompletionResults))]
    [HarmonyPatch(nameof(PrepareLevelCompletionResults.FillLevelCompletionResults), MethodType.Normal)]
    class ScoreSubmissionInsurance1
    {
        private static readonly FieldAccessor<ScenesTransitionSetupDataSO, SceneSetupData[]>.Accessor GameplayCoreSceneSetupDataAccessor = FieldAccessor<ScenesTransitionSetupDataSO, SceneSetupData[]>.GetAccessor("_sceneSetupDataArray");
        private static readonly FieldAccessor<GameplayCoreSceneSetupData, PracticeSettings>.Accessor PracticeSettingsAccessor = FieldAccessor<GameplayCoreSceneSetupData, PracticeSettings>.GetAccessor("practiceSettings");
        static void Postfix(ref LevelCompletionResults __result, LevelCompletionResults.LevelEndStateType levelEndStateType)
        {
            if ((ScoreSubmission.WasDisabled || ScoreSubmission.disabled || ScoreSubmission.prolongedDisable) && levelEndStateType == LevelCompletionResults.LevelEndStateType.Cleared && Plugin.scenesTransitionSetupData != null)
            {
                var gameplayCoreSceneSetupData = GameplayCoreSceneSetupDataAccessor(ref Plugin.scenesTransitionSetupData).OfType<GameplayCoreSceneSetupData>().FirstOrDefault();
                if (gameplayCoreSceneSetupData != null)
                {
                    PracticeSettingsAccessor(ref gameplayCoreSceneSetupData) = new PracticeSettings();
                }
                
                Plugin.scenesTransitionSetupData = null;
                __result.SetField("multipliedScore", -__result.multipliedScore);
            }
        }
    }

}
