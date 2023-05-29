using System.Text;
using HarmonyLib;
using HMUI;
using TMPro;
using UnityEngine;

namespace BS_Utils.Gameplay.HarmonyPatches
{
    [HarmonyPatch(typeof(ResultsViewController))]
    [HarmonyPatch("SetDataToUI", MethodType.Normal)]
    internal class ResultsViewControllerSetDataToUI
    {
        private static void Postfix(ref TextMeshProUGUI ____rankText, ref GameObject ____clearedBannerGo, ref GameObject ____failedBannerGo)
        {
            ____rankText.overflowMode = TextOverflowModes.Overflow;
            ____rankText.richText = true;

            GameObject bannerGo = ____clearedBannerGo.activeInHierarchy ? ____clearedBannerGo : ____failedBannerGo;

            GameObject bgGo = bannerGo.transform.Find("BG").gameObject;
            CurvedTextMeshPro tmp = bannerGo.GetComponentInChildren<CurvedTextMeshPro>();
            if (bannerGo == ____failedBannerGo)
                tmp.color = Color.red;
            if (bgGo != null)
                bgGo.SetActive(true);
            if (ScoreSubmission.WasDisabled || ScoreSubmission.prolongedDisable)
            {
                string color = "<color=#ff0000ff>";
                StringBuilder scoreSubmissionTextBuilder = new StringBuilder("  \r\n").Append(color).Append("<size=50%><b>Score Submission Disabled by: ");
                if (ScoreSubmission.WasDisabled)
                {
                    scoreSubmissionTextBuilder.Append(ScoreSubmission.LastDisabledModString);

                    if (ScoreSubmission.prolongedDisable)
                        scoreSubmissionTextBuilder.Append(" | ");
                }

                if (ScoreSubmission.prolongedDisable)
                {
                    scoreSubmissionTextBuilder.Append(ScoreSubmission.ProlongedModString);
                }

                tmp.color = Color.white;
                tmp.text += scoreSubmissionTextBuilder.ToString();
                tmp.enableWordWrapping = false;
                if(bgGo != null)
                    bgGo.SetActive(false);
            }

            ScoreSubmission.ModList.Clear();
            ScoreSubmission.disabled = false;
        }
    }
}
