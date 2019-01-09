using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BS_Utils.Gameplay.HarmonyPatches;
using Harmony;
namespace BS_Utils.Gameplay
{
    public class ScoreSubmission
    {
        internal static List<String> ModList { get;  set; }
        internal static string ModString
        {
            get
            {
                ModString = "";
                for(int i = 0; i < ModList.Count; i++)
                {
                    if (i == 0)
                        ModString += ModList[i];
                    else
                        ModString += ", " + ModList[i];
                }
                return ModString;
            }
            set
            {
            }

        }
        internal static List<String> ProlongedModList { get;  set; }
        internal static string ProlongedModString
        {
            get
            {
                ProlongedModString = "";
                for (int i = 0; i < ProlongedModList.Count; i++)
                {
                    if (i == 0)
                        ProlongedModString += ProlongedModList[i];
                    else
                        ProlongedModString += ", " + ProlongedModList[i];
                }
                return ProlongedModString;
            }
            set
            {
            }

        }

        public static void DisableSubmission(string mod)
        {
            if (SoloFreePlayFlowCoordinatorProcessScore.disabled == false)
            {
                Plugin.ApplyHarmonyPatches();
              SoloFreePlayFlowCoordinatorProcessScore.disabled = true;
            }

            if(!ModList.Contains(mod))
            ModList.Add(mod);

        }

        public static void ProlongedDisableSubmission(string mod)
        {
            if (SoloFreePlayFlowCoordinatorProcessScore.prolongedDisable == false)
            {
                Plugin.ApplyHarmonyPatches();
                SoloFreePlayFlowCoordinatorProcessScore.prolongedDisable = true;
            }
            if (!ProlongedModList.Contains(mod))
                ProlongedModList.Add(mod);
        }

        public static void RemoveProlongedDisable(string mod)
        {
            ProlongedModList.Remove(mod);
            if (ProlongedModList.Count == 0)
                SoloFreePlayFlowCoordinatorProcessScore.prolongedDisable = false;
        }

    }
}
