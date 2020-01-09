using System;
using Harmony;

namespace BS_Utils.Gameplay.HarmonyPatches
{
    [HarmonyPatch(typeof(SoloFreePlayFlowCoordinator))]
    [HarmonyPatch("ProcessLevelCompletionResultsAfterLevelDidFinish", MethodType.Normal)]
    class SoloFreePlayFlowCoordinatorProcessScore
    {
        static void Prefix(LevelCompletionResults levelCompletionResults, ref bool practice)
        {
            if (ScoreSubmission.disabled || ScoreSubmission.prolongedDisable)
            {
                //Utilities.Logger.Log("Score Submission Disabled");
                practice = true;
            }
        }

        static void Postfix(LevelCompletionResults levelCompletionResults, ref bool practice) { }
    }

    [HarmonyPatch(typeof(StandardLevelScenesTransitionSetupDataSO))]
    [HarmonyPatch("Finish", MethodType.Normal)]
    class FinishPatch
    {
        static void Prefix(LevelCompletionResults levelCompletionResults)
        {
            if (ScoreSubmission.disabled || ScoreSubmission.prolongedDisable)
            {
                //Utilities.Logger.Log("Score Submission Disabled");
                ScoreSubmission.DisableScoreSaberScoreSubmission();
            }
        }
    }
}
