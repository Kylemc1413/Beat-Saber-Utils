using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
namespace BS_Utils.Gameplay.HarmonyPatches
{
    [HarmonyPatch(typeof(SoloFreePlayFlowCoordinator),
          new Type[] {
            typeof(LevelCompletionResults),
            typeof(bool)})]
    [HarmonyPatch("ProcessScore", MethodType.Normal)]
    class SoloFreePlayFlowCoordinatorProcessScore
    {
        public static bool disabled = false;
        public static bool prolongedDisable = false;
        static void Prefix(LevelCompletionResults levelCompletionResults, ref bool practice)
        {
            if(disabled || prolongedDisable)
            practice = true;
        }

        static void Postfix(LevelCompletionResults levelCompletionResults, ref bool practice)
        {
            if(disabled)
            disabled = false;

        }
    }
}
