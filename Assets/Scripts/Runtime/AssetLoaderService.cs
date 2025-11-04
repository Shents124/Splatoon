using Cysharp.Threading.Tasks;
using UnityEngine;
using ZBase.UnityScreenNavigator.Foundation.AssetLoaders;

namespace AssetLoader
{
    public static partial class AssetLoaderService
    {
        private static AddressableAssetLoader s_loader = new();

#if UNITY_EDITOR
        // Hỗ trợ khi tắt Domain Reload trên editor
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            s_loader = new();
        }
#endif
        
        public static async UniTask<AssetLoadHandle<T>> LoadAsync<T>(string key) where T : Object
        {
            var assetLoadHandle = s_loader.LoadAsync<T>(key);
            
            while (assetLoadHandle.IsDone == false)
            {
                await UniTask.NextFrame();
            }

            if (assetLoadHandle.Status == AssetLoadStatus.Failed)
            {
                throw assetLoadHandle.OperationException;
            }

            return assetLoadHandle;
        }

        public static async UniTask<T> LoadAssetAsync<T>(string key) where T : Object
        {
            var handle = await LoadAsync<T>(key);
            return handle.Result;
        }
        
        public static async UniTask<GameObject> LoadObject(string key)
        {
            var handle = await LoadAsync<GameObject>(key);
            return handle.Result;
        }

        public static async UniTask<T> LoadCsvAsync<T>() where T : ScriptableObject
        {
            var path = typeof(T).FullName;
            var handle = await LoadAsync<T>(path);
            return handle.Result;
        }
        
        public static async UniTask<T> LoadCsvAsync<T>(string key) where T : ScriptableObject
        {
            var handle = await LoadAsync<T>(key);
            return handle.Result;
        }
        
        public static T LoadCsv<T>() where T : ScriptableObject
        {
            var path = typeof(T).FullName;
            return Load<T>(path);
        }
        
        public static T LoadCsvWithSimpleName<T>() where T : ScriptableObject
        {
            var path = typeof(T).Name;
            return Load<T>(path);
        }
        
        public static T Load<T>(string key) where T : Object
        {
            return s_loader.Load<T>(key).Result;
        }

        public static void Release(AssetLoadHandleId handleId)
        {
            s_loader.Release(handleId);
        }
        
        public static async UniTask<AudioClip> LoadSound(string key)
        {
            var handle = await LoadAsync<AudioClip>(key);
            return handle.Result;
        }
    }
}

