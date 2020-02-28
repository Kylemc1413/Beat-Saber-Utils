using System;
using HarmonyLib;

namespace BS_Utils.Gameplay.HarmonyPatches
{
    [HarmonyPatch(typeof(SoloFreePlayFlowCoordinator))]
    [HarmonyPatch("ProcessLevelCompletionResultsAfterLevelDidFinish", MethodType.Normal)]
    class SoloFreePlayFlowCoordinatorProcessScore
    {
        static void Prefix(LevelCompletionResults levelCompletionResults, ref bool practice)
        {
            if (ScoreSubmission.WasDisabled || ScoreSubmission.disabled || ScoreSubmission.prolongedDisable)
            {
                //Utilities.Logger.Log($"Score Submission Disabled by {string.Join("|", ScoreSubmission.LastDisablers)}");
                practice = true;
            }
        }

        static void Postfix(LevelCompletionResults levelCompletionResults, ref bool practice) { }
    }
}
