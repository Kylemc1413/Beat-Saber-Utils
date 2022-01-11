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
        static void Postfix(ref LevelCompletionResults __result, LevelCompletionResults.LevelEndStateType levelEndStateType)
        {
            if ((ScoreSubmission.WasDisabled || ScoreSubmission.disabled || ScoreSubmission.prolongedDisable) && levelEndStateType == LevelCompletionResults.LevelEndStateType.Cleared && Plugin.scenesTransitionSetupData != null)
            {
                Plugin.scenesTransitionSetupData.Get<GameplayCoreSceneSetupData>().SetField("practiceSettings", new PracticeSettings());
                Plugin.scenesTransitionSetupData = null;
                __result.SetField("rawScore", -__result.rawScore);
            }
        }
    }

}
