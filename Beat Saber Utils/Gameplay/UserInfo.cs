using Oculus.Platform;
using Oculus.Platform.Models;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BS_Utils.Utilities;
using LogLevel = IPA.Logging.Logger.Level;

namespace BS_Utils.Gameplay
{
    public static class GetUserInfo
    {
        static string userName = null;
        static ulong userID = 0;

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
                    if (VRPlatformHelper.instance.vrPlatformSDK == VRPlatformHelper.VRPlatformSDK.OpenVR || Environment.CommandLine.Contains("-vrmode oculus"))
                    {
                        Logger.Log("Attempting to Grab Steam User", LogLevel.Debug);
                        GetSteamUser();
                    }
                    else if (VRPlatformHelper.instance.vrPlatformSDK == VRPlatformHelper.VRPlatformSDK.Oculus)
                    {
                        Logger.Log("Attempting to Grab Oculus User", LogLevel.Debug);
                        GetOculusUser();
                    }
                    else if (Environment.CommandLine.Contains("fpfc") && VRPlatformHelper.instance.vrPlatformSDK == VRPlatformHelper.VRPlatformSDK.Unknown)
                    {
                        Logger.Log("Attempting to Grab Steam User", LogLevel.Debug);
                        GetSteamUser();
                    }
                }catch(Exception e)
                {
                    Logger.Log("Unable to grab user! Exception: "+e, LogLevel.Error);
                }
            }
        }


        internal static void GetSteamUser()
        {
            if (SteamManager.Initialized)
            {
                userName = SteamFriends.GetPersonaName();
                userID = SteamUser.GetSteamID().m_SteamID;
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
                }
            });
        }
        public static string GetUserName()
        {
            return userName;
        }

        public static ulong GetUserID()
        {
            return userID;
        }

    }
}
