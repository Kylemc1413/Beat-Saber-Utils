using HarmonyLib;
//using UnityEngine;

namespace BS_Utils.Gameplay.HarmonyPatches
{
    [HarmonyPatch]
    internal class ScenesTransitionSetupDataSoFinishPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(StandardLevelScenesTransitionSetupData), nameof(StandardLevelScenesTransitionSetupData.Finish), MethodType.Normal)]
        static void Prefix(StandardLevelScenesTransitionSetupData __instance)
        {
            //Debug.Log("StandardLevelScenesTransitionSetupData.Finish: Prefix");

            if (ScoreSubmission.disabled || ScoreSubmission.prolongedDisable)
            {
                ScoreSubmission.DisableScoreSaberScoreSubmission(__instance);
            }
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MultiplayerLevelScenesTransitionSetupData), nameof(MultiplayerLevelScenesTransitionSetupData.Finish), MethodType.Normal)]
        static void Prefix(MultiplayerLevelScenesTransitionSetupData __instance)
        {
            //Debug.Log("MultiplayerLevelScenesTransitionSetupData.Finish: Prefix");

            if (ScoreSubmission.disabled || ScoreSubmission.prolongedDisable)
            {
                ScoreSubmission.DisableScoreSaberScoreSubmission(__instance);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MissionLevelScenesTransitionSetupData), nameof(MissionLevelScenesTransitionSetupData.Finish), MethodType.Normal)]
        static void Prefix(MissionLevelScenesTransitionSetupData __instance)
        {
            //Debug.Log("MissionLevelScenesTransitionSetupData.Finish: Prefix");

            if (ScoreSubmission.disabled || ScoreSubmission.prolongedDisable)
            {
                ScoreSubmission.DisableScoreSaberScoreSubmission(__instance);
            }
        }
    }
}
