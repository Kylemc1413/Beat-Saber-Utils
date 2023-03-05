using HarmonyLib;

namespace BS_Utils.Gameplay.HarmonyPatches
{
    [HarmonyPatch]
    internal class ScenesTransitionSetupDataSoFinishPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(StandardLevelScenesTransitionSetupDataSO), nameof(StandardLevelScenesTransitionSetupDataSO.Finish), MethodType.Normal)]
        static void Prefix(StandardLevelScenesTransitionSetupDataSO __instance)
        {
            if (ScoreSubmission.disabled || ScoreSubmission.prolongedDisable)
            {
                ScoreSubmission.DisableScoreSaberScoreSubmission(__instance);
            }
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MultiplayerLevelScenesTransitionSetupDataSO), nameof(MultiplayerLevelScenesTransitionSetupDataSO.Finish), MethodType.Normal)]
        static void Prefix(MultiplayerLevelScenesTransitionSetupDataSO __instance)
        {
            if (ScoreSubmission.disabled || ScoreSubmission.prolongedDisable)
            {
                ScoreSubmission.DisableScoreSaberScoreSubmission(__instance);
            }
        }
    }
}
