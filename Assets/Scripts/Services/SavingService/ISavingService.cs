using System;
using Cysharp.Threading.Tasks;

namespace Services.SavingService
{
    public interface ISaveData<T>
    {
        T Item { get; set; }
    }
    
    public interface ISavingService
    {
        ISaveData<T> GetPackage<T>(string packageKey, T initialValue = default);
        
        ISaveData<T> ResetPackage<T>(string packageKey, T value = default);
        
        event Action OnBeforeSave;
        
        UniTask Bootstrap();
    }
}