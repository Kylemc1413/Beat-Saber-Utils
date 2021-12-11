using Steamworks;
using System;
using UnityEngine;
using Logger = BS_Utils.Utilities.Logger;
using LogLevel = IPA.Logging.Logger.Level;
using System.Linq;
using IPA.Utilities;
using System.Threading.Tasks;
using BS_Utils.Utilities;

namespace BS_Utils.Gameplay
{
    public static class GetUserInfo
    {
        private static readonly FieldAccessor<PlatformLeaderboardsModel, IPlatformUserModel>.Accessor AccessPlatformUserModel;
        private static readonly TaskCompletionSource<bool> shouldBeReadyTask = new TaskCompletionSource<bool>();
        private static readonly object getUserLock = new object();

        static string userName = null;
        static string userID = null;
        static UserInfo.Platform platform;
        static Texture2D userAvatar = null;
        private static Task<UserInfo> getUserTask;
        private static bool isReady => shouldBeReadyTask.Task.IsCompleted;
        private static IPlatformUserModel _platformUserModel;

        static GetUserInfo()
        {
            try
            {
                AccessPlatformUserModel = FieldAccessor<PlatformLeaderboardsModel, IPlatformUserModel>.GetAccessor("_platformUserModel");
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error getting PlatformUserModel, GetUserInfo is unavailable: {ex.Message}");
                Logger.log.Debug(ex);
            }
        }

        internal static void TriggerReady()
        {
            shouldBeReadyTask.TrySetResult(true);
        }

        public static IPlatformUserModel GetPlatformUserModel()
        {
            return _platformUserModel ?? SetPlatformUserModel();
        }

        internal static IPlatformUserModel SetPlatformUserModel()
        {
            if (_platformUserModel != null)
                return _platformUserModel;
            try
            {
                // Need to check for null because there's multiple PlatformLeaderboardsModels (at least sometimes), and one has a null IPlatformUserModel with 'vrmode oculus'
                var leaderboardsModel = Resources.FindObjectsOfTypeAll<PlatformLeaderboardsModel>().Where(p => AccessPlatformUserModel(ref p) != null).LastOrDefault();
                IPlatformUserModel platformUserModel = null;
                if (leaderboardsModel == null)
                {
                    Logger.log.Error("Could not find a 'PlatformLeaderboardsModel', GetUserInfo unavailable.");
                    return null;
                }
                if (AccessPlatformUserModel == null)
                {
                    Logger.log.Error("Accessor for 'PlatformLeaderboardsModel._platformUserModel' is null, GetUserInfo unavailable.");
                    return null;
                }

                platformUserModel = AccessPlatformUserModel(ref leaderboardsModel);
                _platformUserModel = platformUserModel;
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error getting 'IPlatformUserModel', GetUserInfo unavailable: {ex.Message}");
                Logger.log.Debug(ex);
            }
            return _platformUserModel;
        }

        public static async void UpdateUserInfo()
        {
            try
            {
                await GetUserAsync();
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error retrieving UserInfo: {ex.Message}.");
                Logger.log.Debug(ex);
            }
        }

        /// <summary>
        /// Attempts to retrieve the UserInfo. Returns null if <see cref="IPlatformUserModel"/> is unavailable.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="IPlatformUserModel"/> returns null for the <see cref="UserInfo"/>.</exception>
        public static async Task<UserInfo> GetUserAsync()
        {
            try
            {
                if (!isReady)
                    await shouldBeReadyTask.Task;
                await Task.Delay(200);
                lock (getUserLock)
                {

                    IPlatformUserModel platformUserModel = GetPlatformUserModel();
                    if (platformUserModel == null)
                    {
                        Logger.log.Error("IPlatformUserModel not found, cannot update user info.");
                        return null;
                    }
                    if (getUserTask == null || getUserTask.Status == TaskStatus.Faulted)
                        getUserTask = InternalGetUserAsync();
                }
                return await getUserTask;
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error retrieving UserInfo: {ex.Message}.");
                Logger.log.Debug(ex);
                throw;
            }
        }

        private static async Task<UserInfo> InternalGetUserAsync()
        {
            UserInfo userInfo = await _platformUserModel.GetUserInfo();
            if (userInfo != null)
            {
                Logger.log.Debug($"UserInfo found: {userInfo.platformUserId}: {userInfo.userName} on {userInfo.platform}");
                userName = userInfo.userName;
                userID = userInfo.platformUserId;
                platform = userInfo.platform;
                if (userInfo.platform == UserInfo.Platform.Steam)
                    GetSteamAvatar();
                else if (userInfo.platform == UserInfo.Platform.Oculus)
                    userAvatar = UIUtilities.LoadTextureFromResources("BS_Utils.Resources.oculus.png");
            }
            else
                throw new InvalidOperationException("UserInfo is null.");
            return userInfo;
        }

        private static void GetSteamAvatar()
        {
            if (SteamManager.Initialized)
            {
                var steamUser = SteamUser.GetSteamID();
                userAvatar = GetAvatar(steamUser);
            }
        }
        private static Texture2D GetAvatar(CSteamID steamUser)
        {
            int avatarInt = SteamFriends.GetLargeFriendAvatar(steamUser);
            bool success = SteamUtils.GetImageSize(avatarInt, out uint imageWidth, out uint imageHeight);

            if (success && imageWidth > 0 && imageHeight > 0)
            {
                byte[] Image = new byte[imageWidth * imageHeight * 4];
                Texture2D returnTexture = new Texture2D((int)imageWidth, (int)imageHeight, TextureFormat.RGBA32, false, true);
                success = SteamUtils.GetImageRGBA(avatarInt, Image, (int)(imageWidth * imageHeight * 4));

                if (success)
                {
                    returnTexture.LoadRawTextureData(Image);
                    returnTexture.Apply();
                }

                return returnTexture;
            }
            else
            {
                Debug.LogError("Couldn't get avatar.");
                return new Texture2D(0, 0);
            }
        }

        [Obsolete("This will not be valid unless BS_Utils has finished retrieving the UserInfo. Use 'GetUserAsync()' instead.")]
        public static UserInfo.Platform GetPlatformInfo()
        {
            return platform;
        }

        [Obsolete("This will return null until BS_Utils has finished retrieving the UserInfo. Use 'GetUserAsync()' instead.")]
        public static string GetUserName()
        {
            return userName;
        }

        [Obsolete("This will return null until BS_Utils has finished retrieving the UserInfo. Use 'GetUserAsync()' instead.")]
        public static string GetUserID()
        {
            return userID;
        }

        public static Texture2D GetUserAvatar()
        {
            return userAvatar;
        }
    }
}
