using HarmonyLib;

namespace BS_Utils.Gameplay.HarmonyPatches
{
    [HarmonyPatch(typeof(StandardLevelScenesTransitionSetupDataSO))]
    [HarmonyPatch(nameof(StandardLevelScenesTransitionSetupDataSO.Finish), MethodType.Normal)]
    class StandardLevelScenesTransitionSetupDataSOFinishPatch
    {
        static void Prefix(LevelCompletionResults levelCompletionResults)
        {
            if (ScoreSubmission.disabled || ScoreSubmission.prolongedDisable)
            {
                ScoreSubmission.DisableScoreSaberScoreSubmission();
            }
        }
    }
}
