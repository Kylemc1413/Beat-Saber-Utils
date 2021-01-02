using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using System.Collections;
using System.Collections.Generic;
namespace BS_Utils.Utilities
{
    public class BSEvents : MonoBehaviour
    {
        static BSEvents Instance;

        //Scene Events
        public static event Action menuSceneActive;
        public static event Action menuSceneLoaded;
        /// <summary>
        /// Raised after the game's menu is loaded fresh. This event should be used for cloning game objects. Do NOT modify base game objects during this event.
        /// </summary>
        public static event Action<ScenesTransitionSetupDataSO> earlyMenuSceneLoadedFresh;
        [Obsolete("Use earlyMenuSceneLoadedFresh or lateMenuSceneLoadedFresh.")]
        public static event Action menuSceneLoadedFresh;
        /// <summary>
        /// Raised after the game's menu is loaded fresh and <see cref="earlyMenuSceneLoadedFresh"/> and <see cref="menuSceneLoadedFresh"/> have run.
        /// Base game objects are not guaranteed to be unmodified during this event.
        /// </summary>
        public static event Action<ScenesTransitionSetupDataSO> lateMenuSceneLoadedFresh;
        public static event Action gameSceneActive;
        public static event Action gameSceneLoaded;

        // Menu Events
        public static event Action<StandardLevelDetailViewController, IDifficultyBeatmap> difficultySelected;
        public static event Action<BeatmapCharacteristicSegmentedControlController, BeatmapCharacteristicSO> characteristicSelected;
        public static event Action<LevelSelectionNavigationController, IBeatmapLevelPack> levelPackSelected;
        public static event Action<LevelCollectionViewController, IPreviewBeatmapLevel> levelSelected;

        // Game Events
        public static event Action songPaused;
        public static event Action songUnpaused;
        public static event Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults> levelCleared;
        public static event Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults> levelQuit;
        public static event Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults> levelFailed;
        public static event Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults> levelRestarted;

        public static event Action<NoteData, NoteCutInfo, int> noteWasCut;
        public static event Action<NoteData, int> noteWasMissed;
        public static event Action<int, float> multiplierDidChange;
        public static event Action<int> multiplierDidIncrease;
        public static event Action<int> comboDidChange;
        public static event Action comboDidBreak;
        public static event Action<int> scoreDidChange;
        public static event Action<float> energyDidChange;
        public static event Action energyReachedZero;

        public static event Action<BeatmapEventData> beatmapEvent;

        public static event Action<SaberType> sabersStartCollide;
        public static event Action<SaberType> sabersEndCollide;

        readonly string[] MainSceneNames = { SceneNames.Game, SceneNames.Credits, SceneNames.BeatmapEditor };
        private bool lastMainSceneWasNotMenu = false;
        GameScenesManager gameScenesManager;

        public static void OnLoad()
        {
            if (Instance != null) return;
            GameObject go = new GameObject("BSEvents");
            go.AddComponent<BSEvents>();
        }

        private void Awake()
        {
            if (Instance != null) return;
            Instance = this;

            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;

            DontDestroyOnLoad(gameObject);
        }

        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene arg1)
        {
            //    Utilities.Logger.log.Info(arg1.name);
            try
            {
                if (arg1.name == SceneNames.Game)
                {

                    InvokeAll(gameSceneActive);

                    gameScenesManager = Resources.FindObjectsOfTypeAll<GameScenesManager>().FirstOrDefault();

                    if (gameScenesManager != null)
                    {
                        gameScenesManager.transitionDidFinishEvent -= GameSceneLoadedCallback;
                        gameScenesManager.transitionDidFinishEvent += GameSceneLoadedCallback;
                    }
                }
                else if (arg1.name == SceneNames.Menu)
                {
                    gameScenesManager = Resources.FindObjectsOfTypeAll<GameScenesManager>().FirstOrDefault();

                    InvokeAll(menuSceneActive);

                    if (gameScenesManager != null)
                    {

                        if (arg0.name == SceneNames.EmptyTransition && !lastMainSceneWasNotMenu)
                        {
                            //     Utilities.Logger.log.Info("Fresh");

                            gameScenesManager.transitionDidFinishEvent -= OnMenuSceneWasLoadedFresh;
                            gameScenesManager.transitionDidFinishEvent += OnMenuSceneWasLoadedFresh;
                        }
                        else
                        {
                            gameScenesManager.transitionDidFinishEvent -= OnMenuSceneWasLoaded;
                            gameScenesManager.transitionDidFinishEvent += OnMenuSceneWasLoaded;
                        }
                    }
                    lastMainSceneWasNotMenu = false;
                }
                if (MainSceneNames.Contains(arg1.name))
                    lastMainSceneWasNotMenu = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("[BSEvents] " + e);
            }
        }

        private void OnMenuSceneWasLoaded(ScenesTransitionSetupDataSO transitionSetupData, DiContainer diContainer)
        {
            gameScenesManager.transitionDidFinishEvent -= OnMenuSceneWasLoaded;
            InvokeAll(menuSceneLoaded);
        }

        private void OnMenuSceneWasLoadedFresh(ScenesTransitionSetupDataSO transitionSetupData, DiContainer diContainer)
        {
            gameScenesManager.transitionDidFinishEvent -= OnMenuSceneWasLoadedFresh;

            var levelDetailViewController = Resources.FindObjectsOfTypeAll<StandardLevelDetailViewController>().FirstOrDefault();
            levelDetailViewController.didChangeDifficultyBeatmapEvent += delegate (StandardLevelDetailViewController vc, IDifficultyBeatmap beatmap) { InvokeAll(difficultySelected, vc, beatmap); };

            var characteristicSelect = Resources.FindObjectsOfTypeAll<BeatmapCharacteristicSegmentedControlController>().FirstOrDefault();
            characteristicSelect.didSelectBeatmapCharacteristicEvent += delegate (BeatmapCharacteristicSegmentedControlController controller, BeatmapCharacteristicSO characteristic) { InvokeAll(characteristicSelected, controller, characteristic); };

            var packSelectViewController = Resources.FindObjectsOfTypeAll<LevelSelectionNavigationController>().FirstOrDefault();
            packSelectViewController.didSelectLevelPackEvent += delegate (LevelSelectionNavigationController controller, IBeatmapLevelPack pack) { InvokeAll(levelPackSelected, controller, pack); };
            var levelSelectViewController = Resources.FindObjectsOfTypeAll<LevelCollectionViewController>().FirstOrDefault();
            levelSelectViewController.didSelectLevelEvent += delegate (LevelCollectionViewController controller, IPreviewBeatmapLevel level) { InvokeAll(levelSelected, controller, level); };

            InvokeAll(earlyMenuSceneLoadedFresh, transitionSetupData);
            InvokeAll(menuSceneLoadedFresh);
            InvokeAll(lateMenuSceneLoadedFresh, transitionSetupData);
        }

        private void GameSceneLoadedCallback(ScenesTransitionSetupDataSO transitionSetupData, DiContainer diContainer)
        {
            // Prevent firing this event when returning to menu
            var gameScenesManager = Resources.FindObjectsOfTypeAll<GameScenesManager>().FirstOrDefault();
            gameScenesManager.transitionDidFinishEvent -= GameSceneLoadedCallback;
            if (Plugin.LevelData.Mode == Gameplay.Mode.Multiplayer)
            {
                MultiplayerController sync = Resources.FindObjectsOfTypeAll<MultiplayerController>().LastOrDefault(x => x.isActiveAndEnabled);
                if (sync != null)
                {
                    sync.stateChangedEvent += (state) => { MultiControllerStateChanged(state, transitionSetupData, diContainer, sync); };

                }

            }
            else
            {
                GameSceneSceneWasLoaded(transitionSetupData, diContainer);
            }
        }

        private void MultiControllerStateChanged(MultiplayerController.State newState, ScenesTransitionSetupDataSO transitionSetupData, DiContainer diContainer, MultiplayerController sync = null)
        {
            if(newState == MultiplayerController.State.Gameplay)
            {
                sync.stateChangedEvent -= (state) => { MultiControllerStateChanged(state, transitionSetupData, diContainer, sync); };
                GameSceneSceneWasLoaded(transitionSetupData, diContainer, sync);
            }
        }

        private void GameSceneSceneWasLoaded(ScenesTransitionSetupDataSO transitionSetupData, DiContainer diContainer, MultiplayerController sync = null)
        {


            var pauseManager = Resources.FindObjectsOfTypeAll<PauseController>().LastOrDefault();
            if (pauseManager != null)
            {
                pauseManager.didResumeEvent += delegate () { InvokeAll(songUnpaused); };
                pauseManager.didPauseEvent += delegate () { InvokeAll(songPaused); };
            }

            var scoreController = Resources.FindObjectsOfTypeAll<ScoreController>().LastOrDefault(x => x.isActiveAndEnabled);
            if (scoreController != null)
            {
                scoreController.noteWasCutEvent += delegate (NoteData noteData, NoteCutInfo noteCutInfo, int multiplier) { InvokeAll(noteWasCut, noteData, noteCutInfo, multiplier); };
                scoreController.noteWasMissedEvent += delegate (NoteData noteData, int multiplier) { InvokeAll(noteWasMissed, noteData, multiplier); }; ;
                scoreController.multiplierDidChangeEvent += delegate (int multiplier, float progress) { InvokeAll(multiplierDidChange, multiplier, progress); if (multiplier > 1 && progress < 0.1f) InvokeAll(multiplierDidIncrease, multiplier); };
                scoreController.comboDidChangeEvent += delegate (int combo) { InvokeAll(comboDidChange, combo); };
                scoreController.comboBreakingEventHappenedEvent += delegate () { InvokeAll(comboDidBreak); };
                scoreController.scoreDidChangeEvent += delegate (int score, int scoreAfterModifier) { InvokeAll(scoreDidChange); };

            }

            var saberCollisionManager = Resources.FindObjectsOfTypeAll<ObstacleSaberSparkleEffectManager>().LastOrDefault(x => x.isActiveAndEnabled);
            if (saberCollisionManager != null)
            {
                saberCollisionManager.sparkleEffectDidStartEvent += delegate (SaberType saber) { InvokeAll(sabersStartCollide, saber); };
                saberCollisionManager.sparkleEffectDidEndEvent += delegate (SaberType saber) { InvokeAll(sabersEndCollide, saber); };
            }


            var gameEnergyCounter = Resources.FindObjectsOfTypeAll<GameEnergyCounter>().LastOrDefault(x => x.isActiveAndEnabled);
            if (gameEnergyCounter != null)
            {
                gameEnergyCounter.gameEnergyDidReach0Event += delegate () { InvokeAll(energyReachedZero); };
                gameEnergyCounter.gameEnergyDidChangeEvent += delegate (float energy) { InvokeAll(energyDidChange, energy); };

            }

            var beatmapObjectCallbackController = Resources.FindObjectsOfTypeAll<BeatmapObjectCallbackController>().LastOrDefault(x => x.isActiveAndEnabled);
            if (beatmapObjectCallbackController != null)
            {
                beatmapObjectCallbackController.beatmapEventDidTriggerEvent += delegate (BeatmapEventData songEvent) { InvokeAll(beatmapEvent, songEvent); };

            }

            var transitionSetup = Resources.FindObjectsOfTypeAll<StandardLevelScenesTransitionSetupDataSO>().FirstOrDefault();
            if (transitionSetup)
            {
                transitionSetup.didFinishEvent -= OnTransitionSetupOnDidFinishEvent;
                transitionSetup.didFinishEvent += OnTransitionSetupOnDidFinishEvent;
            }

            InvokeAll(gameSceneLoaded);
        }

        private void OnTransitionSetupOnDidFinishEvent(StandardLevelScenesTransitionSetupDataSO data, LevelCompletionResults results)
        {
            switch (results.levelEndStateType)
            {
                case LevelCompletionResults.LevelEndStateType.Cleared:
                    InvokeAll(levelCleared, data, results);
                    break;
                case LevelCompletionResults.LevelEndStateType.Failed:
                    if (results.levelEndAction == LevelCompletionResults.LevelEndAction.Restart)
                        InvokeAll(levelRestarted, data, results);
                    else
                        InvokeAll(levelFailed, data, results);
                    break;
            }

            switch (results.levelEndAction)
            {
                case LevelCompletionResults.LevelEndAction.Quit:
                    InvokeAll(levelQuit, data, results);
                    break;
                case LevelCompletionResults.LevelEndAction.Restart:
                    InvokeAll(levelRestarted, data, results);
                    break;
            }
        }

        public void InvokeAll<T1, T2, T3>(Action<T1, T2, T3> action, params object[] args)
        {
            Delegate[] actions = action?.GetInvocationList();
            if (actions == null) return;
            foreach (Delegate invoc in actions)
            {
                string name = "";
                try
                {
                    name = invoc?.Method.DeclaringType.Assembly.FullName;
                    invoc?.DynamicInvoke(args);
                }
                catch (Exception e)
                {
                    Utilities.Logger.log.Error($"Caught Exception when executing event: {e.Message}\n In Assembly: {name}");
                    Utilities.Logger.log.Debug(e);
                }
            }
        }
        public void InvokeAll<T1, T2>(Action<T1, T2> action, params object[] args)
        {
            Delegate[] actions = action?.GetInvocationList();
            if (actions == null) return;
            foreach (Delegate invoc in actions)
            {
                string name = "";
                try
                {
                    name = invoc?.Method.DeclaringType.Assembly.FullName;
                    invoc?.DynamicInvoke(args);
                }
                catch (Exception e)
                {
                    Utilities.Logger.log.Error($"Caught Exception when executing event: {e.Message}\n In Assembly: {name}");
                    Utilities.Logger.log.Debug(e);
                }
            }
        }

        public void InvokeAll<T>(Action<T> action, params object[] args)
        {
            Delegate[] actions = action?.GetInvocationList();
            if (actions == null) return;
            foreach (Delegate invoc in actions)
            {
                string name = "";
                try
                {
                    name = invoc?.Method.DeclaringType.Assembly.FullName;
                    invoc?.DynamicInvoke(args);
                }
                catch (Exception e)
                {
                    Utilities.Logger.log.Error($"Caught Exception when executing event: {e.Message}\n In Assembly: {name}");
                    Utilities.Logger.log.Debug(e);
                }
            }
        }
        public void InvokeAll(Action action, params object[] args)
        {
            Delegate[] actions = action?.GetInvocationList();
            if (actions == null) return;
            foreach (Delegate invoc in actions)
            {
                string name = "";
                try
                {
                    name = invoc?.Method.DeclaringType.Assembly.FullName;
                    invoc?.DynamicInvoke(args);
                }
                catch (Exception e)
                {
                    Utilities.Logger.log.Error($"Caught Exception when executing event: {e.Message}\n In Assembly: {name}");
                    Utilities.Logger.log.Debug(e);
                }
            }
        }
    }
}