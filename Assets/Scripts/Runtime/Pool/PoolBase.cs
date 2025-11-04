using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using ZBase.UnityScreenNavigator.Foundation.AssetLoaders;
using AssetLoaderService = AssetLoader.AssetLoaderService;
using Object = UnityEngine.Object;

namespace Runtime.Pool
{
    public class PoolBase
    {
        private readonly Dictionary<string, PoolHolder> _pools = new();
        private readonly Dictionary<string, AssetLoadHandle<GameObject>> _assetLoadHandles = new();

        public GameObject Spawn(string key)
        {
            if (_pools.TryGetValue(key, out var poolHolder))
            {
                var result = poolHolder.Spawn();
                if (result)
                {
                    return result;
                }
            }

            return CreateNewGameObject(key);
        }
        
        public async UniTask<GameObject> SpawnAsync(string key)
        {
            if (_pools.TryGetValue(key, out var poolHolder))
            {
                var result = poolHolder.Spawn();
                if (result)
                {
                    return result;
                }
            }

            return await CreateNewGameObjectAsync(key);
        }

        public async UniTask<T> SpawnAsync<T>(string key)
        {
            GameObject clone = await SpawnAsync(key);
            return clone.GetComponent<T>();
        }

        public async UniTask<GameObject> SpawnAsync(string key, Vector3 position)
        {
            GameObject clone = await SpawnAsync(key);
            clone.transform.position = position;
            return clone;
        }

        public async UniTask<GameObject> SpawnAsync(string key, Vector3 position, Transform parent)
        {
            GameObject clone = await SpawnAsync(key, position);
            clone.transform.SetParent(parent);
            return clone;
        }

        public async UniTask<List<GameObject>> SpawnAsync(string key, int number)
        {
            var result = new List<GameObject>();

            for (int i = 0; i < number; i++)
            {
                result.Add(await SpawnAsync(key));
            }

            return result;
        }

        public void Despawn(string key, GameObject gameObject, bool isDestroy = false)
        {
            if (_pools.TryGetValue(key, out var pool))
            {
                pool.DeSpawn(gameObject, isDestroy);
            }
            else
            {
                var newPool = new PoolHolder(gameObject);
                newPool.DeSpawn(gameObject, isDestroy);
                if (!isDestroy)
                {
                    _pools.TryAdd(key, newPool);
                }
            }
        }

        private async UniTask<GameObject> CreateNewGameObjectAsync(string key)
        {
            GameObject clone;
            if (_assetLoadHandles.TryGetValue(key, out var existingHandle))
            {
                clone = Object.Instantiate(existingHandle.Result);
            }
            else
            {
                var assetLoadHandle = await AssetLoaderService.LoadAsync<GameObject>(key);
                _assetLoadHandles.TryAdd(key, assetLoadHandle);
                clone = Object.Instantiate(assetLoadHandle.Result);
            }
            
            var newPool = new PoolHolder(_assetLoadHandles[key].Result);
            _pools.TryAdd(key, newPool);
            clone.SetActive(true);
            return clone;
        }
        
        private GameObject CreateNewGameObject(string key)
        {
            GameObject clone;
            if (_assetLoadHandles.TryGetValue(key, out var existingHandle))
            {
                clone = Object.Instantiate(existingHandle.Result);
            }
            else
            {
                var assetLoadHandle = Addressables.LoadAssetAsync<GameObject>(key);
                clone = Object.Instantiate(assetLoadHandle.WaitForCompletion());
            }
            var newPool = new PoolHolder(clone);
            _pools.TryAdd(key, newPool);
            clone.SetActive(true);
            return clone;
        }

        public async UniTask Populate(string key, int numberOfObject)
        {
            if (!_pools.ContainsKey(key))
            {
                var assetLoadHandle = await AssetLoaderService.LoadAsync<GameObject>(key);
                _pools.Add(key, new PoolHolder(assetLoadHandle.Result));
            }

            _pools[key].Populate(numberOfObject);
        }

        public void Clear(string key)
        {
            if (_pools.TryGetValue(key, out var pool))
            {
                pool.Clear();
            }
        }

        public void ClearAll()
        {
            foreach (var pool in _pools.Values)
            {
                pool.Clear();
            }
            
            _pools.Clear();

            foreach (var handle in _assetLoadHandles.Values)
            {
                AssetLoaderService.Release(handle.Id);
            }
            
            _assetLoadHandles.Clear();
        }
        
        public void Release(string addressablePath)
        {
            if (!_pools.TryGetValue(addressablePath, out var pool))
                return;

            pool.Clear();

            if (_assetLoadHandles.TryGetValue(addressablePath, out var handle))
            {
                AssetLoaderService.Release(handle.Id);
                _assetLoadHandles.Remove(addressablePath);
            }
        }
    }
}