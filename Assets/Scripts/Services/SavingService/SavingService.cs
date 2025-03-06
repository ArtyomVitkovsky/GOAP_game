using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using Utils;
using Zenject;

namespace Services.SavingService
{
    public class SavingService : ISavingService
    {
        public event Action OnBeforeSave;
        
        private readonly SignalBus _signalBus;
        
        private Dictionary<string, object> _items;
        private bool _isBootstrapComplete;
        private PersistentStringDictionary _persistantJsons;

        private CancellationTokenSource intervalSavingCts;

        [Inject]
        protected SavingService(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public UniTask Bootstrap()
        {
            if (_isBootstrapComplete)
            {
                return UniTask.CompletedTask;
            }

            _isBootstrapComplete = true;
            _items = new Dictionary<string, object>();
            _persistantJsons = new PersistentStringDictionary("savedKeys");
            Load();

            IntervalSaving();
            
            return UniTask.CompletedTask;
        }


        private async UniTask IntervalSaving()
        {
            intervalSavingCts = new CancellationTokenSource();
            
            while (!intervalSavingCts.IsCancellationRequested)
            {
                await UniTask.Delay(5000);
                Save();
            }
        }

        private void Load()
        {
            var save = new Dictionary<string, object>();

            foreach (var keyJsonPair in _persistantJsons.ToDictionary())
            {
                try
                {
                    var value = JsonConvert.DeserializeObject<object>(keyJsonPair.Value,
                        new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All
                        });

                    save.Add(keyJsonPair.Key, value);
                }
                catch (Exception e)
                {
                    save.Add(keyJsonPair.Key, null);
                    Debug.LogWarning(e);
                }
            }

            _items = save;
        }

        private void Save()
        {
            if (!_isBootstrapComplete) Bootstrap();

            foreach (var saveItem in _items)
            {
                try
                {
                    var json = JsonConvert.SerializeObject(saveItem.Value, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });


                    _persistantJsons.Set(saveItem.Key, json);
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e);
                    _persistantJsons.Set(saveItem.Key, "");
                }
            }

            PlayerPrefs.Save();
        }

        public ISaveData<T> GetPackage<T>(string key, T defaultValue = default)
        {
            if (!_isBootstrapComplete) Bootstrap();

            if (!_items.ContainsKey(key))
            {
                var result = new SaveContainer<T>
                {
                    Item = defaultValue
                };
                
                _items.Add(key, result);
                return result;
            }

            try
            {
                return _items[key] as SaveContainer<T>;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error key: {key}:" + e.ToString());
            }

            return null;
        }
        
        public ISaveData<T> ResetPackage<T>(string packageKey, T value = default)
        {
            if (_items.ContainsKey(packageKey) && _items[packageKey] != null)
            {
                var package = _items[packageKey] as ISaveData<T>;
                package.Item = value;
                return package;
            }
            else
            {
                var item = new SaveContainer<T>()
                {
                    Item = value
                };

                _items[packageKey] = item;
                
                return item;
            }
        }
        
        public class SaveContainer<T> : ISaveData<T>
        {
            [JsonProperty("item")] public T Item { get; set; }
        }
    }
}