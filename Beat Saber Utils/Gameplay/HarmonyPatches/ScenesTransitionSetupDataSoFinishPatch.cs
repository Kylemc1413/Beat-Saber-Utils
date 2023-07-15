using HarmonyLib;
//using UnityEngine;

namespace BS_Utils.Gameplay.HarmonyPatches
{
    [HarmonyPatch]
    internal class ScenesTransitionSetupDataSoFinishPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(StandardLevelScenesTransitionSetupDataSO), nameof(StandardLevelScenesTransitionSetupDataSO.Finish), MethodType.Normal)]
        static void Prefix(StandardLevelScenesTransitionSetupDataSO __instance)
        {
            //Debug.Log("StandardLevelScenesTransitionSetupDataSO.Finish: Prefix");

            if (ScoreSubmission.disabled || ScoreSubmission.prolongedDisable)
            {
                ScoreSubmission.DisableScoreSaberScoreSubmission(__instance);
            }
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MultiplayerLevelScenesTransitionSetupDataSO), nameof(MultiplayerLevelScenesTransitionSetupDataSO.Finish), MethodType.Normal)]
        static void Prefix(MultiplayerLevelScenesTransitionSetupDataSO __instance)
        {
            //Debug.Log("MultiplayerLevelScenesTransitionSetupDataSO.Finish: Prefix");

            if (ScoreSubmission.disabled || ScoreSubmission.prolongedDisable)
            {
                ScoreSubmission.DisableScoreSaberScoreSubmission(__instance);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MissionLevelScenesTransitionSetupDataSO), nameof(MissionLevelScenesTransitionSetupDataSO.Finish), MethodType.Normal)]
        static void Prefix(MissionLevelScenesTransitionSetupDataSO __instance)
        {
            //Debug.Log("MissionLevelScenesTransitionSetupDataSO.Finish: Prefix");

            if (ScoreSubmission.disabled || ScoreSubmission.prolongedDisable)
            {
                ScoreSubmission.DisableScoreSaberScoreSubmission(__instance);
            }
        }
    }
}
