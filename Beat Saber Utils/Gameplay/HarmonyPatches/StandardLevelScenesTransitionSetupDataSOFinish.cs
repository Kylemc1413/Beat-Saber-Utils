using HarmonyLib;
using System;

namespace BS_Utils.Gameplay.HarmonyPatches
{
    [HarmonyPatch(typeof(StandardLevelScenesTransitionSetupDataSO))]
    [HarmonyPatch("Finish", MethodType.Normal)]
    class StandardLevelScenesTransitionSetupDataSOFinishPatch
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
