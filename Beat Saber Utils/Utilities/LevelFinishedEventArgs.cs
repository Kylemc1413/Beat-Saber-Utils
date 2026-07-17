using System;
using System.Collections.Generic;
using System.Linq;

namespace BS_Utils.Utilities
{
    public abstract class LevelFinishedEventArgs : EventArgs
    {
        /// <summary>
        /// The type of level that finished.
        /// </summary>
        public readonly LevelType LevelType;

        public readonly ScenesTransitionSetupData ScenesTransitionSetupDataSO;

        protected LevelFinishedEventArgs(LevelType levelType, ScenesTransitionSetupData scenesTransitionSetupData)
        {
            LevelType = levelType;
            ScenesTransitionSetupDataSO = scenesTransitionSetupData;
        }
    }

    /// <summary>
    /// <see cref="LevelFinishedEventArgs"/> that contains <see cref="LevelCompletionResults"/>.
    /// </summary>
    public abstract class LevelFinishedWithResultsEventArgs : LevelFinishedEventArgs
    {
        public readonly LevelCompletionResults CompletionResults;

        protected LevelFinishedWithResultsEventArgs(LevelType levelType, ScenesTransitionSetupData scenesTransitionSetupData, LevelCompletionResults completionResults)
            : base(levelType, scenesTransitionSetupData)
        {
            CompletionResults = completionResults;
        }
    }

    public class SoloLevelFinishedEventArgs : LevelFinishedWithResultsEventArgs
    {
        public SoloLevelFinishedEventArgs(StandardLevelScenesTransitionSetupData levelScenesTransitionSetupData, LevelCompletionResults levelCompletionResults)
            : base(LevelType.SoloParty, levelScenesTransitionSetupData, levelCompletionResults)
        {
        }
    }

    public class MultiplayerLevelFinishedEventArgs : LevelFinishedWithResultsEventArgs
    {
        private IReadOnlyList<MultiplayerPlayerResultsData> playersCompletionResults;

        /// <summary>
        /// Gets the <see cref="LevelCompletionResults"/> for the specified player ID.
        /// Returns null if there is no matching player ID.
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public LevelCompletionResults GetPlayerResults(string playerId)
        {
            var player = playersCompletionResults?.FirstOrDefault(x => x.connectedPlayer.userId == playerId);
            return player?.multiplayerLevelCompletionResults.levelCompletionResults;
        }

        public IEnumerator<MultiplayerPlayerResultsData> PlayerResults()
        {
            return playersCompletionResults.GetEnumerator();
        }

        public MultiplayerLevelFinishedEventArgs(MultiplayerLevelScenesTransitionSetupData levelScenesTransitionSetupData, LevelCompletionResults levelCompletionResults, IReadOnlyList<MultiplayerPlayerResultsData> otherPlayersLevelCompletionResults)
            : base(LevelType.Multiplayer, levelScenesTransitionSetupData, levelCompletionResults)
        {
            playersCompletionResults = otherPlayersLevelCompletionResults;
        }
    }

    public class CampaignLevelFinishedEventArgs : LevelFinishedWithResultsEventArgs
    {
        public readonly MissionCompletionResults MissionCompletionResults;

        public CampaignLevelFinishedEventArgs(MissionLevelScenesTransitionSetupData levelScenesTransitionSetupData, MissionCompletionResults missionCompletionResults)
            : base(LevelType.Campaign, levelScenesTransitionSetupData, missionCompletionResults?.levelCompletionResults)
        {
            MissionCompletionResults = missionCompletionResults;
        }
    }

    public class TutorialLevelFinishedEventArgs : LevelFinishedEventArgs
    {
        public readonly TutorialScenesTransitionSetupData.TutorialEndStateType EndState;

        public TutorialLevelFinishedEventArgs(TutorialScenesTransitionSetupData levelScenesTransitionSetupData, TutorialScenesTransitionSetupData.TutorialEndStateType endState)
            : base(LevelType.Tutorial, levelScenesTransitionSetupData)
        {
            EndState = endState;
        }
    }

    public enum LevelType
    {
        SoloParty,
        Multiplayer,
        Campaign,
        Tutorial
    }
}
