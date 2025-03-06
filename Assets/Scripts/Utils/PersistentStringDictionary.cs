using UnityEngine;

namespace Utils
{
    public class PersistentStringDictionary : PersistentDictionary<string>
    {
        public PersistentStringDictionary(string uniquePrefixKey) : base(uniquePrefixKey) { }

        protected override string GetPrefValue(string key, string defaultValue = default)
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        protected override void SetPrefValue(string key, string value)
        {
            PlayerPrefs.SetString(key,value);
        }
    }
}