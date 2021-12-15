using System.IO;
using System.Text;
using IniParser;
using IniParser.Model;
using IniParser.Model.Configuration;
using IniParser.Parser;

namespace BS_Utils.Utilities
{
    public class IniFile
    {
        private readonly string _path;

        private readonly IniParserConfiguration _config;
        private readonly FileIniDataParser _parser;
        private readonly IniDataParser _dataParser;
        internal IniData Data;

        /// <summary>
        /// INIFile Constructor.
        /// </summary>
        /// <PARAM name="INIPath"></PARAM>
        public IniFile(string iniPath)
        {
            _path = iniPath;
            
            Directory.CreateDirectory(Path.GetDirectoryName(_path));
            if (!File.Exists(_path))
                File.WriteAllText(_path, string.Empty, Encoding.Unicode);

            _config = new IniParserConfiguration
            {
                AllowCreateSectionsOnFly = true,
                AllowDuplicateKeys = true,
                AllowDuplicateSections = true,
                OverrideDuplicateKeys = true,
                SkipInvalidLines = true,
                ThrowExceptionsOnError = true,
                AllowKeysWithoutSection = true
            };

            _dataParser = new IniDataParser(_config);
            _parser = new FileIniDataParser(_dataParser);
            Data = _parser.ReadFile(_path);
        }

        public void IniWriteValue(string section, string key, string value)
        {
            try
            {
                Data[section][key] = value;
                _parser.WriteFile(_path, Data);
            }
            catch
            {
                Logger.Log("IniWriteValue doesnt want to write the stuffs");
            }
        }

        public string IniReadValue(string section, string key)
        {
            try
            {
                Data = _parser.ReadFile(_path);
                return Data[section].ContainsKey(key) ? Data[section].GetKeyData(key).Value : string.Empty;
            }
            catch
            {
                Logger.Log("IniReadValue doesn't want to read the stuffs");
                return string.Empty;
            }
        }
    }
}
