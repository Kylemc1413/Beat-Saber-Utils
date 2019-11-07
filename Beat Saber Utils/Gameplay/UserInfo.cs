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
namespace BS_Utils.Gameplay
{
    public static class GetUserInfo
    {
        static string userName = null;
        static ulong userID = 0;
        static Texture2D userAvatar = null;
        public static VRPlatformHelper vRPlatformHelper
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
        static GetUserInfo()
        {
            UpdateUserInfo();
        }
        
        public static void UpdateUserInfo()
        {
            if (userID == 0 || userName == null)
            {
                try
                {
                    if (vRPlatformHelper.vrPlatformSDK == VRPlatformHelper.VRPlatformSDK.OpenVR || Environment.CommandLine.Contains("-vrmode oculus"))
                    {
                        Logger.Log("Attempting to Grab Steam User", LogLevel.Debug);
                        GetSteamUser();
                    }
                    else if (vRPlatformHelper.vrPlatformSDK == VRPlatformHelper.VRPlatformSDK.Oculus)
                    {
                        Logger.Log("Attempting to Grab Oculus User", LogLevel.Debug);
                        GetOculusUser();
                    }
                    else if (Environment.CommandLine.Contains("fpfc") && vRPlatformHelper.vrPlatformSDK == VRPlatformHelper.VRPlatformSDK.Unknown)
                    {
                        Logger.Log("Attempting to Grab Steam User", LogLevel.Debug);
                        GetSteamUser();
                    }
                }
                catch (Exception e)
                {

                    Logger.Log("Unable to grab user! Exception: "+e, LogLevel.Error);
                }
            }
        }

        internal static void GetSteamUser()
        {
            if (SteamManager.Initialized)
            {
                var steamUser = SteamUser.GetSteamID();

                userName = SteamFriends.GetPersonaName();
                userID = steamUser.m_SteamID;
                userAvatar = GetAvatar(steamUser);
            }
            else
            {
                Logger.Log("Steam is not initialized!", LogLevel.Warning);
            }
        }

        internal static void GetOculusUser()
        {
            Users.GetLoggedInUser().OnComplete((Message<User> msg) =>
            {
                if (!msg.IsError)
                {
                    userID = msg.Data.ID;
                    userName = msg.Data.OculusID;
                    userAvatar = LoadTextureFromResources("BS_Utils.Resources.oculus.png");
                }
            });
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
}
