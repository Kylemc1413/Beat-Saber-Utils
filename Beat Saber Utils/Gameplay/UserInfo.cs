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
                if (SteamVR.instance != null || Environment.CommandLine.Contains("-vrmode oculus"))
                {
                    Logger.Log("BS-Utils", "Attempting to Grab Steam User");
                    GetSteamUser();
                }
                else
                {
                    Logger.Log("BS-Utils","Attempting to Grab Oculus User");
                    GetOculusUser();
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
