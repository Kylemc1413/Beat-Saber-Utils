using System;
using HarmonyLib;
using IPA.Utilities;
namespace BS_Utils.Gameplay.HarmonyPatches
{
    [HarmonyPatch(typeof(LevelCompletionResultsHelper))]
    [HarmonyPatch("ProcessScore", MethodType.Normal)]
    class SoloFreePlayFlowCoordinatorProcessScore
    {
        static bool Prefix()
        {
            if (ScoreSubmission.WasDisabled || ScoreSubmission.disabled || ScoreSubmission.prolongedDisable)
            {
                //Utilities.Logger.Log($"Score Submission Disabled by {string.Join("|", ScoreSubmission.LastDisablers)}");
                return false;
            }
            return true;
        }

      //  static void Postfix(LevelCompletionResults levelCompletionResults, ref bool practice) { }
    }

    [HarmonyPatch(typeof(PrepareLevelCompletionResults))]
    [HarmonyPatch("FillLevelCompletionResults", MethodType.Normal)]
    class ScoreSubmissionInsurance1
    {
        static void Postfix(ref LevelCompletionResults __result, LevelCompletionResults.LevelEndStateType levelEndStateType)
        {
            if ((ScoreSubmission.WasDisabled || ScoreSubmission.disabled || ScoreSubmission.prolongedDisable)
                && levelEndStateType == LevelCompletionResults.LevelEndStateType.Cleared)
            {
                Plugin.scenesTransitionSetupData.Get<GameplayCoreSceneSetupData>().SetField("practiceSettings", new PracticeSettings());
                Plugin.scenesTransitionSetupData = null;
                __result.SetField("rawScore", -__result.rawScore);
            }
        }
    }

}
