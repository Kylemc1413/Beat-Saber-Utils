using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS_Utils.Utilities
{
    public abstract class LevelFinishedEventArgs : EventArgs
    {
        /// <summary>
        /// The type of level that finished.
        /// </summary>
        public readonly LevelType LevelType;
        public readonly ScenesTransitionSetupDataSO ScenesTransitionSetupDataSO;
        protected LevelFinishedEventArgs(LevelType levelType, ScenesTransitionSetupDataSO scenesTransitionSetupDataSO)
        {
            LevelType = levelType;
            ScenesTransitionSetupDataSO = scenesTransitionSetupDataSO;
        }
    }

    /// <summary>
    /// <see cref="LevelFinishedEventArgs"/> that contains <see cref="LevelCompletionResults"/>.
    /// </summary>
    public abstract class LevelFinishedWithResultsEventArgs : LevelFinishedEventArgs
    {
        public readonly LevelCompletionResults CompletionResults;

        protected LevelFinishedWithResultsEventArgs(LevelType levelType, ScenesTransitionSetupDataSO scenesTransitionSetupDataSO, LevelCompletionResults completionResults) 
            : base(levelType, scenesTransitionSetupDataSO)
        {
            CompletionResults = completionResults;
        }
    }

    public class SoloLevelFinishedEventArgs : LevelFinishedWithResultsEventArgs
    {
        public SoloLevelFinishedEventArgs(StandardLevelScenesTransitionSetupDataSO levelScenesTransitionSetupDataSO, LevelCompletionResults levelCompletionResults)
            : base(LevelType.SoloParty, levelScenesTransitionSetupDataSO, levelCompletionResults)
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
            if (playersCompletionResults == null)
                return null;
            var player = playersCompletionResults.FirstOrDefault(x => x.connectedPlayer.userId == playerId);

            return player.levelCompletionResults;
        }

        public IEnumerator<MultiplayerPlayerResultsData> PlayerResults()
        {
            return playersCompletionResults.GetEnumerator();
        }

        public MultiplayerLevelFinishedEventArgs(MultiplayerLevelScenesTransitionSetupDataSO levelScenesTransitionSetupDataSO, LevelCompletionResults levelCompletionResults, IReadOnlyList<MultiplayerPlayerResultsData> otherPlayersLevelCompletionResults)
            : base(LevelType.Multiplayer, levelScenesTransitionSetupDataSO, levelCompletionResults)
        {
            playersCompletionResults = otherPlayersLevelCompletionResults;
        }

    }

    public class CampaignLevelFinishedEventArgs : LevelFinishedWithResultsEventArgs
    {
        public readonly MissionCompletionResults MissionCompletionResults;
        public CampaignLevelFinishedEventArgs(MissionLevelScenesTransitionSetupDataSO levelScenesTransitionSetupDataSO, MissionCompletionResults missionCompletionResults)
            : base(LevelType.Campaign, levelScenesTransitionSetupDataSO, missionCompletionResults?.levelCompletionResults)
        {
            MissionCompletionResults = missionCompletionResults;
        }
    }
    public class TutorialLevelFinishedEventArgs : LevelFinishedEventArgs
    {
        public readonly TutorialScenesTransitionSetupDataSO.TutorialEndStateType EndState;
        public TutorialLevelFinishedEventArgs(TutorialScenesTransitionSetupDataSO levelScenesTransitionSetupDataSO, TutorialScenesTransitionSetupDataSO.TutorialEndStateType endState)
            : base(LevelType.Tutorial, levelScenesTransitionSetupDataSO)
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
