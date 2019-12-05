using System;
using Harmony;

namespace BS_Utils.Gameplay.HarmonyPatches
{
    [HarmonyPatch(typeof(SoloFreePlayFlowCoordinator))]
    [HarmonyPatch("ProcessScore", MethodType.Normal)]
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
}
