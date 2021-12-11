using HarmonyLib;
using HMUI;
using TMPro;
using UnityEngine;

namespace BS_Utils.Gameplay.HarmonyPatches
{
    [HarmonyPatch(typeof(ResultsViewController))]
    [HarmonyPatch(nameof(ResultsViewController.SetDataToUI), MethodType.Normal)]
    class ResultsViewControllerSetDataToUI
    {
        static void Postfix(ref TextMeshProUGUI ____rankText, ref GameObject ____clearedBannerGo, ref GameObject ____failedBannerGo)
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
            if (ScoreSubmission.WasDisabled || ScoreSubmission.disabled || ScoreSubmission.prolongedDisable)
            {
                string color = "<color=#ff0000ff>";
                string scoresubmissiontext = $"  \r\n{color}<size=50%><b>Score Submission Disabled by: " +
                    ScoreSubmission.LastDisabledModString +
                    " | " +
                    ScoreSubmission.ProlongedModString;
                tmp.color = Color.white;
                tmp.text += scoresubmissiontext;
                tmp.enableWordWrapping = false;
                if(bgGo != null)
                    bgGo.SetActive(false);
            }

            ScoreSubmission.ModList.Clear();
            ScoreSubmission.disabled = false;
        }
    }
}
