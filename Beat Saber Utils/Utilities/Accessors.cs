using IPA.Utilities;

namespace BS_Utils.Utilities
{
    internal static class Accessors
    {
        internal static readonly PropertyAccessor<LevelScenesTransitionSetupDataSO, GameplayCoreSceneSetupData>.Getter SceneSetupDataGetter =
            PropertyAccessor<LevelScenesTransitionSetupDataSO, GameplayCoreSceneSetupData>.GetGetter("gameplayCoreSceneSetupData");
    }
}