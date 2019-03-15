using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BS_Utils.Gameplay.HarmonyPatches;
using Harmony;
using UnityEngine;
namespace BS_Utils.Gameplay
{
    public class ScoreSubmission
    {
        public static bool Disabled { get { return disabled; } }
        public static bool ProlongedDisabled { get { return prolongedDisable; } }
        public static bool eventSubscribed = false;
        public static string ModString
        {
            get
            {
                string value = "";
                for (int i = 0; i < ModList.Count; i++)
                {
                    if (i == 0)
                        value += ModList[i];
                    else
                        value += ", " + ModList[i];
                }
                return value;
            }

        }
        public static string ProlongedModString
        {
            get
            {
                string value = "";
                for (int i = 0; i < ProlongedModList.Count; i++)
                {
                    if (i == 0)
                        value += ProlongedModList[i];
                    else
                        value += ", " + ProlongedModList[i];
                }
                return value;
            }

        }

        internal static bool disabled = false;
        internal static bool prolongedDisable = false;
        internal static List<String> ModList { get; set; } = new List<String>(0);
        internal static List<String> ProlongedModList { get; set; } = new List<String>(0);


        public static void DisableSubmission(string mod)
        {
            if (disabled == false)
            {
                Plugin.ApplyHarmonyPatches();
                disabled = true;
                ModList.Clear();
                if(!eventSubscribed)
                {
                Plugin.LevelDidFinishEvent += LevelData_didFinishEvent;
                    eventSubscribed = true;
                }


            }

            if (!ModList.Contains(mod))
                ModList.Add(mod);

        }

        private static void LevelData_didFinishEvent(StandardLevelScenesTransitionSetupDataSO arg1, LevelCompletionResults arg2)
        {
            switch (arg2.levelEndStateType)
            {
                case LevelCompletionResults.LevelEndStateType.Quit:
                    disabled = false;
                    ModList.Clear();
                    break;
                case LevelCompletionResults.LevelEndStateType.Failed:
                    disabled = false;
                    ModList.Clear();
                    break;
                case LevelCompletionResults.LevelEndStateType.None:
                    disabled = false;
                    ModList.Clear();
                    break;
            }
            Plugin.LevelDidFinishEvent -= LevelData_didFinishEvent;
            eventSubscribed = false;

        }

        public static void ProlongedDisableSubmission(string mod)
        {
            if (prolongedDisable == false)
            {
                Plugin.ApplyHarmonyPatches();
                prolongedDisable = true;
            }
            if (!ProlongedModList.Contains(mod))
                ProlongedModList.Add(mod);
        }

        public static void RemoveProlongedDisable(string mod)
        {
            ProlongedModList.Remove(mod);
            if (ProlongedModList.Count == 0)
                prolongedDisable = false;
        }

    }
}
