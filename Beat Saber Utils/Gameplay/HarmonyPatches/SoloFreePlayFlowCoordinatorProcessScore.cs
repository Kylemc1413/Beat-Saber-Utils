using System;
using HarmonyLib;

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
}
