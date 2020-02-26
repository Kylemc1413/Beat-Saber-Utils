using System;
using System.Collections.Generic;
using System.Linq;

namespace BS_Utils.Gameplay
{
    public class ScoreSubmission
    {
        public static bool Disabled { get { return disabled; } }
        public static bool ProlongedDisabled { get { return prolongedDisable; } }
        public static bool eventSubscribed = false;

        public static string LastDisabledModString
        {
            get
            {
                if (LastDisablers == null || LastDisablers.Length == 0)
                    return string.Empty;
                return string.Join(", ", LastDisablers);
            }
        }

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

        public static bool WasDisabled { get { return _wasDisabled; } }

        public static string[] LastDisablers
        {
            get
            {
                if (_lastDisablers == null)
                    return Array.Empty<string>();
                return _lastDisablers.ToArray();
            }
            internal set
            {
                _lastDisablers = value;
            }
        }

        internal static bool disabled = false;
        internal static bool prolongedDisable = false;
        internal static List<string> ModList { get; set; } = new List<string>(0);
        internal static List<string> ProlongedModList { get; set; } = new List<string>(0);

        internal static bool _wasDisabled = false;
        private static string[] _lastDisablers;

        public static void DisableSubmission(string mod)
        {
            if (disabled == false)
            {
                //Utilities.Logger.log.Warn($"First DisableSubmission by {mod}");
                Plugin.ApplyHarmonyPatches();

                disabled = true;
                ModList.Clear();

                if (!eventSubscribed)
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
            _wasDisabled = disabled;
            _lastDisablers = ModList.ToArray();
            disabled = false;
            ModList.Clear();
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
