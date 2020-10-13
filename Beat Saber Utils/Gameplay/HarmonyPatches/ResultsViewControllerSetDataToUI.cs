using System;
using HarmonyLib;
using TMPro;

namespace BS_Utils.Gameplay.HarmonyPatches
{
    [HarmonyPatch(typeof(ResultsViewController))]
    [HarmonyPatch("SetDataToUI", MethodType.Normal)]
    class ResultsViewControllerSetDataToUI
    {
        static void Postfix(ref TextMeshProUGUI ____rankText)
        {
            ____rankText.overflowMode = TextOverflowModes.Overflow;
            ____rankText.richText = true;
         //   ____failedDifficultyText.overflowMode = TextOverflowModes.Overflow;
         //   ____failedDifficultyText.richText = true;

            if (ScoreSubmission.WasDisabled || ScoreSubmission.disabled || ScoreSubmission.prolongedDisable)
            {
                ____rankText.text += "  \r\n<color=#ff0000ff><size=60%><b>Score Submission Disabled by: " +
                    ScoreSubmission.LastDisabledModString +
                    " | " +
                    ScoreSubmission.ProlongedModString;
                /*
               ____failedDifficultyText.text += "  \r\n<color=#ff0000ff><size=60%><b>Score Submission Disabled by: " +
                    ScoreSubmission.LastDisabledModString +
                    " | " +
                    ScoreSubmission.ProlongedModString;
                */
            }


            ScoreSubmission.ModList.Clear();
            ScoreSubmission.disabled = false;
        }
    }
}
