using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS_Utils.Gameplay
{
    public class LevelData
    {
        public GameplayCoreSceneSetupData GameplayCoreSceneSetupData { get; internal set; }
        public bool IsSet { get; internal set; }


        internal void Clear()
        {
            IsSet = false;
            GameplayCoreSceneSetupData = null;
        }
    }
}
