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

    [HarmonyPatch(typeof(SinglePlayerLevelSelectionFlowCoordinator))]
    [HarmonyPatch("HandleStandardLevelDidFinish", MethodType.Normal)]
    class ScoreSubmissionInsurance1
    {
        static void Prefix(ref StandardLevelScenesTransitionSetupDataSO standardLevelScenesTransitionSetupData, ref LevelCompletionResults levelCompletionResults)
        {
            if (ScoreSubmission.WasDisabled || ScoreSubmission.disabled || ScoreSubmission.prolongedDisable)
            {
                var gameplayCoreSceneSetupData = standardLevelScenesTransitionSetupData.Get<GameplayCoreSceneSetupData>();
                if (gameplayCoreSceneSetupData.practiceSettings == null)
                    gameplayCoreSceneSetupData.SetField("practiceSettings", new PracticeSettings());
            }
        }
    }

}
