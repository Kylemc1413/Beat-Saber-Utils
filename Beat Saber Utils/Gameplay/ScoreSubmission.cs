using BS_Utils.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IPA.Loader;
using Logger = BS_Utils.Utilities.Logger;
using LogLevel = IPA.Logging.Logger.Level;
//using UnityEngine;

namespace BS_Utils.Gameplay
{
    public class ScoreSubmission
    {
        public static bool Disabled => disabled;
        public static bool ProlongedDisabled => prolongedDisable;
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

        public static string ModString => string.Join(", ", ModList);
        public static string ProlongedModString => string.Join(", ", ProlongedModList);

        public static bool WasDisabled => _wasDisabled;

        public static string[] LastDisablers
        {
            get
            {
                if (_lastDisablers == null)
                    return Array.Empty<string>();
                return _lastDisablers.ToArray();
            }
            internal set => _lastDisablers = value;
        }

        internal static bool disabled = false;
        internal static bool prolongedDisable = false;
        internal static List<string> ModList { get; set; } = new List<string>(0);
        internal static List<string> ProlongedModList { get; set; } = new List<string>(0);

        internal static bool _wasDisabled = false;
        private static string[] _lastDisablers;

        public static void DisableSubmission(string mod)
        {
            //Debug.Log("DisableSubmission(): " + mod);

            if (disabled == false)
            {
                //Utilities.Logger.log.Warn($"First DisableSubmission by {mod}");
                Plugin.ApplyHarmonyPatches();

                disabled = true;
                ModList.Clear();

                if (!eventSubscribed)
                {
                    Plugin.LevelFinished += LevelData_didFinishEvent;
                    Plugin.MultiplayerDidDisconnect += Plugin_MultiplayerDidDisconnect;
                    eventSubscribed = true;
                }
            }

            if (!ModList.Contains(mod))
                ModList.Add(mod);
        }

        private static PropertyInfo _scoreSaberSubmissionProperty;
        private static PropertyInfo ScoreSaberSubmissionProperty
        {
            get
            {
                if (_scoreSaberSubmissionProperty == null)
                {
                    PluginMetadata scoreSaberMetaData = PluginManager.GetPluginFromId("ScoreSaber");
                    if (scoreSaberMetaData != null)
                    {
                        foreach (Type type in scoreSaberMetaData.Assembly.GetTypes())
                        {
                            if (type.Namespace == "ScoreSaber" && type.Name == "Plugin")
                            {
                                _scoreSaberSubmissionProperty = type.GetProperty("ScoreSubmission", BindingFlags.Public | BindingFlags.Static);
                                return _scoreSaberSubmissionProperty;
                            }
                        }
                    }
                }

                return _scoreSaberSubmissionProperty;
            }
        }

        internal static void DisableScoreSaberScoreSubmission(LevelScenesTransitionSetupDataSO setupDataSO)
        {
            //Debug.Log("DisableScoreSaberScoreSubmission(): " + setupDataSO.ToString()); ;

            if (ScoreSaberSubmissionProperty != null)
            {
                ScoreSaberSubmissionProperty.SetValue(null, false);
            }
            else
            {
                DisableEvent(setupDataSO, "didFinishEvent", "Five");
            }
        }

        private static void LevelData_didFinishEvent(object sender, LevelFinishedEventArgs args)
        {
            //Debug.Log("LevelData_didFinishEvent(): " + args.LevelType.ToString()); ;

            _wasDisabled = disabled;
            _lastDisablers = ModList.ToArray();
            disabled = false;
            ModList.Clear();

            Plugin.LevelFinished -= LevelData_didFinishEvent;
            Plugin.MultiplayerDidDisconnect -= Plugin_MultiplayerDidDisconnect;
            ScoreSaberSubmissionProperty?.SetValue(null, true);

            switch (args.ScenesTransitionSetupDataSO)
            {
                case StandardLevelScenesTransitionSetupDataSO standardLevelScenesTransitionSetupDataSO:
                    if (RemovedSoloFive != null)
                    {
                        standardLevelScenesTransitionSetupDataSO.didFinishEvent -= RemovedSoloFive;
                        standardLevelScenesTransitionSetupDataSO.didFinishEvent += RemovedSoloFive;
                        RemovedSoloFive = null;
                    }
                    break;
                case MultiplayerLevelScenesTransitionSetupDataSO multiplayerLevelScenesTransitionSetupDataSO:
                    if (RemovedMultiFive != null)
                    {
                        multiplayerLevelScenesTransitionSetupDataSO.didFinishEvent -= RemovedMultiFive;
                        multiplayerLevelScenesTransitionSetupDataSO.didFinishEvent += RemovedMultiFive;
                        RemovedMultiFive = null;
                    }
                    break;
            }

            eventSubscribed = false;
        }

        private static void Plugin_MultiplayerDidDisconnect(object sender, (MultiplayerLevelScenesTransitionSetupDataSO levelScenesTransitionSetupDataSO, DisconnectedReason) eventDetails)
        {
            LevelData_didFinishEvent(sender, new MultiplayerLevelFinishedEventArgs(eventDetails.levelScenesTransitionSetupDataSO, null, null));
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
            {
                prolongedDisable = false;
            }

        }

        private static Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults> RemovedSoloFive;
        private static Action<MultiplayerLevelScenesTransitionSetupDataSO, MultiplayerResultsData> RemovedMultiFive;
        private static bool DisableEvent(object target, string eventName, string delegateName)
        {
            FieldInfo fieldInfo = target.GetType().GetField(eventName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            var eventDelegate = fieldInfo.GetValue(target) as MulticastDelegate;
            bool eventDisabled = false;
            if (eventDelegate != null)
            {
                var delegates = eventDelegate.GetInvocationList();
                foreach (var item in delegates)
                {
                    if (item.Method.Name == delegateName)
                    {
                        switch (target)
                        {
                            case StandardLevelScenesTransitionSetupDataSO _:
                                RemovedSoloFive = (Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults>)item;
                                break;
                            case MultiplayerLevelScenesTransitionSetupDataSO _:
                                RemovedMultiFive = (Action<MultiplayerLevelScenesTransitionSetupDataSO, MultiplayerResultsData>)item;
                                break;
                            default:
                                throw new NotImplementedException();
                        }

                        target.GetType().GetEvent(eventName).RemoveEventHandler(target, item);
                        eventDisabled = true;
                    }
                }
            }
            return eventDisabled;
        }

        // Use ONLY for restarts from pause menu
        internal static void PauseMenuRestart()
        {
            Logger.Log("RestartLevel from Pause Menu");

            // These are reset in GrabTheLevelData, which doesnt run when restarted from Pause menu (that only runs when restarting from ResultsView)
            _wasDisabled = false;
            LastDisablers = Array.Empty<string>();

            // These must be called in ResultsView to actually reset, which doesnt run if restarting from Pause menu
            ModList.Clear();    // Removes text from ResultsView
            disabled = false;   // Stops DisableScoreSubmission() from being called. This was otherwise reset to false in LevelData_didFinishEvent and ResultsViewControllerSetDataToUI

            eventSubscribed = false;
            Plugin.LevelFinished -= LevelData_didFinishEvent;
            Plugin.MultiplayerDidDisconnect -= Plugin_MultiplayerDidDisconnect;

            RemovedSoloFive = null;
            RemovedMultiFive = null;
        }

        // Used for debugging purposes
        private static void LogEvents(object target, string eventName)
        {
            FieldInfo fieldInfo = target.GetType().GetField(eventName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            var eventDelegate = fieldInfo.GetValue(target) as MulticastDelegate;
            if (eventDelegate == null)
            {
                Logger.Log("ScoreSubmission: Unable to get eventDelegate from StandardLevelScenesTransitionSetupDataSO - exiting...", LogLevel.Debug);
            }

            var delegates = eventDelegate.GetInvocationList();
            Logger.Log("ScoreSubmission: Getting list of delegates for didFinish event...", LogLevel.Debug);
            foreach (var item in delegates)
            {
                Logger.Log(String.Format("ScoreSubmission: Found delegate named '{0}' by Module '{1}', part of Assembly '{2}'", item.Method.Name, item.Method.Module.Name, item.Method.Module.Assembly.FullName), LogLevel.Debug);
            }
        }
    }
}
