#### A basic library for mods to use, does nothing on its own
##### GitHub: https://github.com/Kylemc1413/Beat-Saber-Utils
##### Feel free to look at the source or contribute if you want to add something to the library
## Changelog 1.2.1
- Added BS_Utils.Gameplay.LevelData Class and Plugin.LevelData field to contain the information StandardLevelSceneSetupDataSO used to contain
   since that is no longer easily accessible by plugins, can use the IsSet bool of BS_Utils.Plugin.LevelData to check if it is safe to access
- Updated For Beat Saber 0.13.0, Everything should be functional

## Capabilities
### Currently provides easy ways for mods to
- Easily create their own config files
- Disable Score submission when changing game play that warrant it
- Fetch User ID and name, regardless of platform
- Check whether party mode is active
- Check whether Standard, No Arrows, or One Saber is selected
- Declare when they are starting a level through their mod when they want it isolated from other mods' functionality
- Check if another mod wants the level isolated so they know when to disable functionality
## Example Usage
```cs
using BS_Utils;
using BS_Utils.Gameplay;
// For a config can simply do this on Application start
// to create a config for your mod in the UserData Folder
Utilities.Config ConfigVariable = new Utilities.Config("ModName");
//Or if you want it to be in a separate folder,
// create the folder path from UserData using your plugin if it doesn't exist, and then
Utilities.Config ConfigVariable = new Utilities.Config("FolderName/ModName");
// Then you can simply Get/Set values in the config such as
ConfigVariable.GetBool("SectionName", "SectionVariableName");
ConfigVariable.SetBool("SectionName", "SectionVariableName", value);

// When in GameCore Scene, can use the following to disable submission for the song
// All of the mods that disabled submission will be shown on the results screen
ScoreSubmission.DisableSubmission("Mod Name");

// Can also use the following to disable submission until you have your mod re-enable it
ScoreSubmission.ProlongedDisableSubmission("Mod Name");

// And to re-enable
ScoreSubmission.RemoveProlongedDisable("Mod Name");

// To fetch user information can simply use the following

// For user name
UserInfo.GetUserName();
// For User ID
UserInfo.GetUserID();

// For checking Party or Gamemode, first call this when in the Menu Scene
Gamemode.Init();
//To check If Party is active(bool) or the gamemode (string)
// you can just access these properties
Gamemode.IsPartyActive
Gamemode.GameMode

// To declare the mod is about to start a level they want isolated, call this in menu before starting the level
Gamemode.NextLevelIsIsolated("Mod Name");

// To check if the level is isolated, and which mod isolated it
Gamemode.IsIsolatedLevel // Bool value
Gamemode.IsolatingMod // String value
```
