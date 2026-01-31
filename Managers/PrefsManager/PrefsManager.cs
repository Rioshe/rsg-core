using UnityEngine;

namespace RSG
{
    public static class PrefsManager
    {
        private static string FormatKey(string key, string subKey)
        {
            if (string.IsNullOrEmpty(key))
                throw new System.ArgumentException("Key cannot be null or empty", nameof(key));
            return string.IsNullOrEmpty(subKey) ? key : $"{key}_{subKey}";
        }

        public static bool GetBool(string key, bool defaultValue = false, string subKey = "")
        {
            return PlayerPrefs.GetInt(FormatKey(key, subKey), defaultValue ? 1 : 0) == 1;
        }
        
        public static void SetBool(string key, bool value, string subKey = "")
        {
            PlayerPrefs.SetInt(FormatKey(key, subKey), value ? 1 : 0);
        }
        
        public static int GetInt(string key, int defaultValue = -1, string subKey = "")
        {
            return PlayerPrefs.GetInt(FormatKey(key, subKey), defaultValue);
        }

        public static void SetInt(string key, int value, string subKey = "")
        {
            PlayerPrefs.SetInt(FormatKey(key, subKey), value);
        }

        public static float GetFloat(string key, float defaultValue = -1, string subKey = "")
        {
            return PlayerPrefs.GetFloat(FormatKey(key, subKey), defaultValue);
        }

        public static void SetFloat(string key, float value, string subKey = "")
        {
            PlayerPrefs.SetFloat(FormatKey(key, subKey), value);
        }

        public static string GetString(string key, string defaultValue = "", string subKey = "")
        {
            return PlayerPrefs.GetString(FormatKey(key, subKey), defaultValue);
        }

        public static void SetString(string key, string value, string subKey = "")
        {
            PlayerPrefs.SetString(FormatKey(key, subKey), value);
        }

        public static void DeleteKey(string key, string subKey = "")
        {
            PlayerPrefs.DeleteKey(FormatKey(key, subKey));
        }

        public static bool HasKey(string key, string subKey = "")
        {
            return PlayerPrefs.HasKey(FormatKey(key, subKey));
        }

        public static void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }

        public static void Save()
        {
            PlayerPrefs.Save();
        }
    }
}