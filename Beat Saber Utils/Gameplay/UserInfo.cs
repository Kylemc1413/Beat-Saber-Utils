using Oculus.Platform;
using Oculus.Platform.Models;
using Steamworks;
using System;
using UnityEngine;
using Logger = BS_Utils.Utilities.Logger;
using LogLevel = IPA.Logging.Logger.Level;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Collections;

namespace BS_Utils.Gameplay
{
    /*
    public static class GetUserInfo
    {
        static PlatformInfo platformInfo;
        static string userName = null;
        static ulong userID = 0;
        static Texture2D userAvatar = null;
        public static IVRPlatformHelper vRPlatformHelper
        {
            get
            {
                if (_vRPlatformHelper == null)
                    _vRPlatformHelper = Resources.FindObjectsOfTypeAll<VRPlatformHelper>().First();
                return _vRPlatformHelper;

            }
            internal set
            {
                _vRPlatformHelper = value;
            }
        }
        private static VRPlatformHelper _vRPlatformHelper;
        private static IPlatformUserModel _platformUserModel;

        public static PlatformUserModelSO PlatformUserModelSO
        {
            get
            {
                if (_platformUserModel == null)
                    _platformUserModel = ScriptableObject.CreateInstance<PlatformUserModelSO>();
                return _platformUserModel;
            }
            internal set { _platformUserModel = value; }
        }

        static GetUserInfo()
        {
            UpdateUserInfo();
        }
        
        public static void UpdateUserInfo()
        {
            if (getUserActive)
                return; // Already retrieving user info.
            if (userID == 0 || userName == null)
            {
                try
                {
                    SharedCoroutineStarter.instance.StartCoroutine(GetUserCoroutine());
                }
                catch (Exception e)
                {

                    Logger.Log("Unable to grab user! Exception: "+e, LogLevel.Error);
                }
            }
        }
        private static bool getUserActive = false;
        private static bool foundUser = false;
        private static bool waitingForCompletion = false;
        private static IEnumerator GetUserCoroutine()
        {
            getUserActive = true;
            WaitForSeconds wait = new WaitForSeconds(5f);
            int tries = 1;
            try
            {
                while (!foundUser && tries < 10)
                {
                    if (!waitingForCompletion)
                    {
                        if (!foundUser)
                        {
                            Logger.log.Debug($"Detected platform: {PlatformUserModelSO.platformInfo.platform}");
                            try
                            {
                                waitingForCompletion = true;
                                PlatformUserModelSO.GetUserInfo(UserInfoCompletionHandler);
                            }
                            catch (Exception ex)
                            {
                                waitingForCompletion = false;
                                Logger.log.Error($"Error retrieving user info: {ex.Message}");
                                Logger.log.Debug(ex);
                            }
                            tries++;
                        }
                    }
                    yield return wait;
                }
            }
            finally
            {
                getUserActive = false;
            }
        }

        private static void UserInfoCompletionHandler(PlatformUserModelSO.GetUserInfoResult result, PlatformUserModelSO.UserInfo userInfo)
        {
            if (result == PlatformUserModelSO.GetUserInfoResult.OK)
            {
                Logger.log.Debug($"UserInfo found: {userInfo.userId}: {userInfo.userName}");
                platformInfo = PlatformUserModelSO.platformInfo;
                if (ulong.TryParse(userInfo.userId, out ulong id))
                    userID = id;
                else
                {
                    Logger.log.Warn($"Unable to parse {userInfo.userId} as a ulong.");
                    userID = 0;
                }
                userName = userInfo.userName;
                if (PlatformUserModelSO.platformInfo.platform == PlatformInfo.Platform.Steam)
                {
                    GetSteamAvatar();
                }
                else if(PlatformUserModelSO.platformInfo.platform == PlatformInfo.Platform.Oculus)
                    userAvatar = LoadTextureFromResources("BS_Utils.Resources.oculus.png");
                foundUser = true;
            }
            else
                Logger.log.Error("Failed to retrieve user info.");
            waitingForCompletion = false;
        }

        //internal static void GetSteamUser()
        //{
        //    if (SteamManager.Initialized)
        //    {
        //        var steamUser = SteamUser.GetSteamID();

        //        userName = SteamFriends.GetPersonaName();
        //        userID = steamUser.m_SteamID;
        //        userAvatar = GetAvatar(steamUser);
        //    }
        //    else
        //    {
        //        Logger.Log("Steam is not initialized!", LogLevel.Warning);
        //    }
        //}

        //internal static void GetOculusUser()
        //{
        //    Users.GetLoggedInUser().OnComplete((Message<User> msg) =>
        //    {
        //        if (!msg.IsError)
        //        {
        //            userID = msg.Data.ID;
        //            userName = msg.Data.OculusID;
        //            userAvatar = LoadTextureFromResources("BS_Utils.Resources.oculus.png");
        //        }
        //    });
        //}
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
        public static PlatformInfo GetPlatformInfo()
        {
            return platformInfo;
        }

        public static string GetUserName()
        {
            return userName;
        }

        public static ulong GetUserID()
        {
            return userID;
        }

        public static Texture2D GetUserAvatar()
        {
            return userAvatar;
        }


        internal static Texture2D LoadTextureFromResources(string resourcePath)
        {
            return LoadTextureRaw(GetResource(Assembly.GetCallingAssembly(), resourcePath));
        }
        internal static Texture2D LoadTextureRaw(byte[] file)
        {
            if (file.Count() > 0)
            {
                Texture2D Tex2D = new Texture2D(2, 2);
                if (Tex2D.LoadImage(file))
                    return Tex2D;
            }
            return null;
        }
        internal static byte[] GetResource(Assembly asm, string ResourceName)
        {
            System.IO.Stream stream = asm.GetManifestResourceStream(ResourceName);
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, (int)stream.Length);
            return data;
        }

    }
    */
}
