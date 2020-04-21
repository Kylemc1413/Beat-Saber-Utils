using System;
using System.IO;
using System.Text;
using IniParser;
using IniParser.Model;
using IniParser.Parser;

namespace BS_Utils.Utilities
{
    public class IniFile
    {
        private string _path = "";
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(value));
                if (!File.Exists(value))
                    File.WriteAllText(value, "", Encoding.Unicode);

                _path = value;
            }
        }

        internal IniParser.Model.Configuration.IniParserConfiguration config = new IniParser.Model.Configuration.IniParserConfiguration();
        internal FileIniDataParser parser;
        internal IniDataParser dataParser;
        internal IniData data;

        /// <summary>
        /// INIFile Constructor.
        /// </summary>
        /// <PARAM name="INIPath"></PARAM>
        public IniFile(string INIPath)
        {
            this.Path = INIPath;
            config.AllowCreateSectionsOnFly = true;
            config.AllowDuplicateKeys = true;
            config.AllowDuplicateSections = true;
            config.OverrideDuplicateKeys = true;
            config.SkipInvalidLines = true;
            config.ThrowExceptionsOnError = true;
            config.AllowKeysWithoutSection = true;
            dataParser = new IniDataParser(config);
            parser = new FileIniDataParser(dataParser);
            data = parser.ReadFile(Path);
        }

        public void IniWriteValue(string Section, string Key, string Value)
        {
            try
            {
                data[Section][Key] = Value;
                parser.WriteFile(Path, data);
            }
            catch
            {
                Logger.Log("IniWriteValue doesnt want to write the stuffs");
            }
        }

        public string IniReadValue(string Section, string Key)
        {
            try
            {
                data = parser.ReadFile(Path);
                string result;

                if (!data[Section].ContainsKey(Key))
                {
                    return "";
                }
                else
                {
                    result = data[Section].GetKeyData(Key).Value;
                    return result;
                }
            }
            catch
            {
                Logger.Log("IniReadValue doesn't want to read the stuffs");
                return "";
            }
        }
    }
}
