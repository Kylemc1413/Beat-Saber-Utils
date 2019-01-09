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
        internal static bool disabled = false;
        internal static bool prolongedDisable = false;
        internal static List<String> ModList { get;  set; } = new List<String>(0);
        internal static string ModString
        {
            get
            {
                string value = "";
                for(int i = 0; i < ModList.Count; i++)
                {
                    if (i == 0)
                        value += ModList[i];
                    else
                        value += ", " + ModList[i];
                }
                return value;
            }
            set
            {
            }

        }
        internal static List<String> ProlongedModList { get;  set; } = new List<String>(0);
        internal static string ProlongedModString
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
            set
            {
            }

        }

        public static void DisableSubmission(string mod)
        {
            if (disabled == false)
            {
                Plugin.ApplyHarmonyPatches();
                disabled = true;
            }

            if(!ModList.Contains(mod))
            ModList.Add(mod);

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
