using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS_Utils
{
    public class LevelFinishedEventArgs : EventArgs
    {
        public readonly LevelType LevelType;
        public readonly ScenesTransitionSetupDataSO ScenesTransitionSetupDataSO;
        protected LevelFinishedEventArgs(LevelType levelType, ScenesTransitionSetupDataSO scenesTransitionSetupDataSO)
        {
            LevelType = levelType;
            ScenesTransitionSetupDataSO = scenesTransitionSetupDataSO;
        }
    }

    public class SoloLevelFinishedEventArgs : LevelFinishedEventArgs
    {
        public readonly LevelCompletionResults CompletionResults;
        public SoloLevelFinishedEventArgs(StandardLevelScenesTransitionSetupDataSO levelScenesTransitionSetupDataSO, LevelCompletionResults levelCompletionResults)
            : base(LevelType.SoloParty, levelScenesTransitionSetupDataSO)
        {
            CompletionResults = levelCompletionResults;
        }
    }
    public class MultiplayerLevelFinishedEventArgs : LevelFinishedEventArgs
    {
        public readonly LevelCompletionResults CompletionResults;
        private readonly Dictionary<string, LevelCompletionResults> playersCompletionResults;

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
            if(playersCompletionResults.TryGetValue(playerId, out LevelCompletionResults value))
            {
                return value;
            }
            return null;
        }

        public IEnumerator<KeyValuePair<string, LevelCompletionResults>> PlayerResults()
        {
            return playersCompletionResults.GetEnumerator();
        }

        public MultiplayerLevelFinishedEventArgs(MultiplayerLevelScenesTransitionSetupDataSO levelScenesTransitionSetupDataSO, LevelCompletionResults levelCompletionResults, Dictionary<string, LevelCompletionResults> otherPlayersLevelCompletionResults)
            : base(LevelType.Multiplayer, levelScenesTransitionSetupDataSO)
        {
            CompletionResults = levelCompletionResults;
            playersCompletionResults = otherPlayersLevelCompletionResults;
        }

    }

    public class CampaignLevelFinishedEventArgs : LevelFinishedEventArgs
    {
        public readonly MissionCompletionResults CompletionResults;
        public CampaignLevelFinishedEventArgs(MissionLevelScenesTransitionSetupDataSO levelScenesTransitionSetupDataSO, MissionCompletionResults levelCompletionResults)
            : base(LevelType.Campaign, levelScenesTransitionSetupDataSO)
        {
            CompletionResults = levelCompletionResults;
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
