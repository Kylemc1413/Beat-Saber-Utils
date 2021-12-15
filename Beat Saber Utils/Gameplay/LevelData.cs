namespace BS_Utils.Gameplay
{
    public enum Mode { None, Standard, Multiplayer, Mission};
    public class LevelData
    {
        public GameplayCoreSceneSetupData GameplayCoreSceneSetupData { get; internal set; }
        public Mode Mode = Mode.None;
        public bool IsSet { get; internal set; }

        internal void Clear()
        {
            IsSet = false;
            GameplayCoreSceneSetupData = null;
            Mode = Mode.None;
        }
    }
}
