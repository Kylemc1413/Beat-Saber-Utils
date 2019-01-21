using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

namespace BS_Utils.Utilities
{
    public class Config
    {
        private IniFile _instance;

        #region Bool Alternatives

        public enum BoolSavingMode
        {
            TrueFalse,
            OneZero,
            YesNo,
            EnabledDisabled,
            OnOff,
        };

        private List<string> yesAlts = new List<string>() { "True", "1", "Yes", "Enabled", "On" };
        private List<string> noAlts = new List<string>() { "False", "0", "No", "Disabled", "Off" };
        #endregion

        public Config(string name)
        {
            _instance = new IniFile(Path.Combine(Environment.CurrentDirectory, $"UserData/{name}.ini"));
        }

        /// <summary>
        /// Gets a string from the ini.
        /// </summary>
        /// <param name="section">Section of the key.</param>
        /// <param name="name">Name of the key.</param>
        /// <param name="defaultValue">Value that should be used when no value is found.</param>
        /// <param name="autoSave">Whether or not the default value should be written if no value is found.</param>
        /// <returns></returns>
        public string GetString(string section, string name, string defaultValue = "", bool autoSave = false)
        {
            string value = _instance.IniReadValue(section, name);
            if (value != "")
                return value;
            else if (autoSave)
                SetString(section, name, defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// Gets an int from the ini.
        /// </summary>
        /// <param name="section">Section of the key.</param>
        /// <param name="name">Name of the key.</param>
        /// <param name="defaultValue">Value that should be used when no value is found.</param>
        /// <param name="autoSave">Whether or not the default value should be written if no value is found.</param>
        /// <returns></returns>
        public int GetInt(string section, string name, int defaultValue = 0, bool autoSave = false)
        {
            if (int.TryParse(_instance.IniReadValue(section, name), out int value))
                return value;
            else if (autoSave)
                SetInt(section, name, defaultValue);

            return defaultValue;
        }


        /// <summary>
        /// Gets a float from the ini.
        /// </summary>
        /// <param name="section">Section of the key.</param>
        /// <param name="name">Name of the key.</param>
        /// <param name="defaultValue">Value that should be used when no value is found.</param>
        /// <param name="autoSave">Whether or not the default value should be written if no value is found.</param>
        /// <returns></returns>
        public float GetFloat(string section, string name, float defaultValue = 0f, bool autoSave = false)
        {
            if (float.TryParse(_instance.IniReadValue(section, name), out float value))
                return value;
            else if (autoSave)
                SetFloat(section, name, defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// Gets a bool from the ini.
        /// </summary>
        /// <param name="section">Section of the key.</param>
        /// <param name="name">Name of the key.</param>
        /// <param name="defaultValue">Value that should be used when no value is found.</param>
        /// <param name="autoSave">Whether or not the default value should be written if no value is found.</param>
        /// <returns></returns>
        public bool GetBool(string section, string name, bool defaultValue = false, bool autoSave = false)
        {
            return GetBool(section, name, BoolSavingMode.TrueFalse, defaultValue, autoSave);
        }

        /// <summary>
        /// Gets a bool from the ini.
        /// </summary>
        /// <param name="section">Section of the key.</param>
        /// <param name="name">Name of the key.</param>
        /// <param name="mode">Yes/No alternative we should look for.</param>
        /// <param name="defaultValue">Value that should be used when no value is found.</param>
        /// <param name="autoSave">Whether or not the default value should be written if no value is found.</param>
        /// <returns></returns>
        public bool GetBool(string section, string name, BoolSavingMode mode, bool defaultValue = false, bool autoSave = false)
        {
            string sVal = GetString(section, name);
            try{
                int yesIndex = yesAlts.IndexOf(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(sVal));
                int noIndex = noAlts.IndexOf(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(sVal));
                if (yesIndex != -1 && yesIndex == (int)mode) return true;
                else if (noIndex != -1 && noIndex == (int)mode) return false;
                else if (autoSave) SetBool(section, name, defaultValue);
            }catch{SetBool(section, name, defaultValue);}
            return defaultValue;
        }

        /// <summary>
        /// Checks whether or not a key exists in the ini.
        /// </summary>
        /// <param name="section">Section of the key.</param>
        /// <param name="name">Name of the key.</param>
        /// <returns></returns>

        public bool HasKey(string section, string name)
        {
            if (_instance.data.Sections.ContainsSection(section))
                return _instance.data[section].ContainsKey(name);
            else
                return false;
        }

        /// <summary>
        /// Sets a float in the ini.
        /// </summary>
        /// <param name="section">Section of the key.</param>
        /// <param name="name">Name of the key.</param>
        /// <param name="value">Value that should be written.</param>
        public void SetFloat(string section, string name, float value)
        {
            _instance.IniWriteValue(section, name, value.ToString());
        }

        /// <summary>
        /// Sets an int in the ini.
        /// </summary>
        /// <param name="section">Section of the key.</param>
        /// <param name="name">Name of the key.</param>
        /// <param name="value">Value that should be written.</param>
        public void SetInt(string section, string name, int value)
        {
            _instance.IniWriteValue(section, name, value.ToString());
        }

        /// <summary>
        /// Sets a string in the ini.
        /// </summary>
        /// <param name="section">Section of the key.</param>
        /// <param name="name">Name of the key.</param>
        /// <param name="value">Value that should be written.</param>
        public void SetString(string section, string name, string value)
        {
            _instance.IniWriteValue(section, name, value);
        }

        /// <summary>
        /// Sets a bool in the ini.
        /// </summary>
        /// <param name="section">Section of the key.</param>
        /// <param name="name">Name of the key.</param>
        /// <param name="value">Value that should be written.</param>
        public void SetBool(string section, string name, bool value)
        {
            SetBool(section, name, BoolSavingMode.TrueFalse, value);
        }

        /// <summary>
        /// Sets a bool in the ini.
        /// </summary>
        /// <param name="section">Section of the key.</param>
        /// <param name="name">Name of the key.</param>
        /// <param name="mode">What common yes/no alternative should we use.</param>
        /// <param name="value">Value that should be written.</param>
        public void SetBool(string section, string name, BoolSavingMode mode, bool value)
        {
            _instance.IniWriteValue(section, name, value ? yesAlts[(int)mode] : noAlts[(int)mode]);

        }


    }
}
