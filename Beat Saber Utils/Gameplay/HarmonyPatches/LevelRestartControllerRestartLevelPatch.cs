using HarmonyLib;
//using UnityEngine;

namespace BS_Utils.Gameplay.HarmonyPatches
{
    [HarmonyPatch(typeof(StandardLevelRestartController), nameof(StandardLevelRestartController.RestartLevel))]
    internal class StandardLevelRestartControllerPatch
    {
        static void Postfix()
        {
            //Debug.Log("StandardLevelRestartController: Postfix");
            ScoreSubmission.PauseMenuRestart();
        }
    }


    [HarmonyPatch(typeof(MissionLevelRestartController), nameof(MissionLevelRestartController.RestartLevel))]
    internal class MissionLevelRestartControllerPatch
    {
        static void Postfix()
        {
            //Debug.Log("MissionLevelRestartController: Postfix");
            ScoreSubmission.PauseMenuRestart();
        }
    }


    [HarmonyPatch(typeof(TutorialRestartController), nameof(TutorialRestartController.RestartLevel))]
    internal class TutorialRestartControllerPatch
    {
        static void Postfix()
        {
            //Debug.Log("TutorialRestartController: Postfix");
            ScoreSubmission.PauseMenuRestart();
        }
    }
}
