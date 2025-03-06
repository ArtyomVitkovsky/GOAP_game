using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
     public abstract class PersistentDictionary<T>
    {
        private const string kPrefsStorageKey = "__dic_keys__";
        
        private readonly List<string> _existingKeys;
        private readonly string _keysPrefix;

        protected PersistentDictionary(string uniquePrefixKey)
        {
            _keysPrefix = uniquePrefixKey;
            _existingKeys = PlayerPrefs.GetString(GetKeyWithPrefix(kPrefsStorageKey)).Split(',').ToList();
        }

        public virtual T Get(string key, T defaultValue = default)
        {
            string actualKey = GetKeyWithPrefix(key);
            return GetPrefValue(actualKey, defaultValue);
        }

        public virtual void Set(string key, T value)
        {
            string actualKey = GetKeyWithPrefix(key);
            SetPrefValue(actualKey, value);
            TryAddKey(key);
        }

        public virtual bool Has(string key)
        {
            string actualKey = GetKeyWithPrefix(key);
            return PlayerPrefs.HasKey(actualKey);
        }

        public virtual void Delete(string key)
        {
            string actualKey = GetKeyWithPrefix(key);
            PlayerPrefs.DeleteKey(actualKey);
            TryRemoveKey(key);
        }

        public Dictionary<string, T> ToDictionary()
        {
            Dictionary<string, T> prefsDictionary = new Dictionary<string, T>();
            foreach (string key in _existingKeys)
            {
                prefsDictionary.Add(key, Get(key));
            }

            return prefsDictionary;
        }

        protected abstract T GetPrefValue(string key, T defaultValue = default);
        protected abstract void SetPrefValue(string key, T value);
        private string GetKeyWithPrefix(string key) => _keysPrefix + "/" + key;

        private void TryAddKey(string key)
        {
            if (!_existingKeys.Contains(key))
            {
                _existingKeys.Add(key);
                PlayerPrefs.SetString(GetKeyWithPrefix(kPrefsStorageKey), string.Join(",", _existingKeys));
            }
        }

        private void TryRemoveKey(string key)
        {
            if (_existingKeys.Contains(key))
            {
                _existingKeys.Remove(key);
                PlayerPrefs.SetString(GetKeyWithPrefix(kPrefsStorageKey), string.Join(",", _existingKeys));
            }
        }
    }
}