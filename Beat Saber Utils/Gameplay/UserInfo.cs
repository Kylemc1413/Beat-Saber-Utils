using Oculus.Platform;
using Oculus.Platform.Models;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BS_Utils.Utilities;
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
                if (VRPlatformHelper.instance.vrPlatformSDK == VRPlatformHelper.VRPlatformSDK.OpenVR || Environment.CommandLine.Contains("-vrmode oculus"))
                {
                    Logger.Log("Attempting to Grab Steam User");
                    GetSteamUser();
                }
                else if (VRPlatformHelper.instance.vrPlatformSDK == VRPlatformHelper.VRPlatformSDK.Oculus)
                {
                    Logger.Log("Attempting to Grab Oculus User");
                    GetOculusUser();
                }else if (Environment.CommandLine.Contains("fpfc") && VRPlatformHelper.instance.vrPlatformSDK == VRPlatformHelper.VRPlatformSDK.Unknown)
                {
                    Logger.Log("Attempting to Grab Steam User");
                    GetSteamUser();
                }
            }
        }


        internal static void GetSteamUser()
        {
            userName = SteamFriends.GetPersonaName();
            userID = SteamUser.GetSteamID().m_SteamID;
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
