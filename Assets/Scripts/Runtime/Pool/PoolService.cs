using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Runtime.Pool
{
    public enum PoolType
    {
        Ball,
        Bullet,
    }

    public class PoolService : MonoBehaviour
    {
        private static Dictionary<PoolType, PoolBase> s_poolDictionary = new();
        
        public async UniTask Initialize()
        {
            s_poolDictionary = new() {
                { PoolType.Ball, new PoolBase() },
                { PoolType.Bullet, new PoolBase() }
            };
            await UniTask.CompletedTask;
        }

        private void OnDestroy()
        {
            foreach (var pool in s_poolDictionary.Values)
            {
                pool.ClearAll();
            }
            
            s_poolDictionary.Clear();
        }

        public static GameObject Spawn(PoolType poolType, string key)
        {
            return s_poolDictionary[poolType].Spawn(key);
        }
        
        public static T Spawn<T>(PoolType poolType, string key) where T : Component
        {
            var clone = s_poolDictionary[poolType].Spawn(key);
            return clone.GetComponent<T>();
        }
        
        public static T Spawn<T>(PoolType poolType, string key, Vector3 position) where T : Component
        {
            var clone = s_poolDictionary[poolType].Spawn(key);
            clone.transform.position = position;
            return clone.GetComponent<T>();
        }
        
        public static async UniTask<GameObject> SpawnAsync(PoolType poolType, string key)
        {
            return await s_poolDictionary[poolType].SpawnAsync(key);
        }

        public static async UniTask<T> SpawnAsync<T>(PoolType poolType, string key)
        {
            return await s_poolDictionary[poolType].SpawnAsync<T>(key);
        }

        public static async UniTask<GameObject> SpawnAsync(PoolType poolType, string key, Vector3 position)
        {
            return await s_poolDictionary[poolType].SpawnAsync(key, position);
        }

        public static async UniTask<GameObject> SpawnAsync(PoolType poolType, string key, Vector3 position, Transform parent)
        {
            return await s_poolDictionary[poolType].SpawnAsync(key, position, parent);
        }

        public static async UniTask<List<GameObject>> SpawnAsync(PoolType poolType, string key, int number)
        {
            return await s_poolDictionary[poolType].SpawnAsync(key, number);
        }

        private static async UniTask PreloadAsync(PoolType poolType, string key, int number)
        {
            await s_poolDictionary[poolType].Populate(key, number);
        }

        public static void Despawn(PoolType poolType, string key, GameObject gameObject, bool isDestroy = false)
        {
            s_poolDictionary[poolType].Despawn(key, gameObject, isDestroy);
        }

        public static void Clear(PoolType poolType, string key)
        {
            s_poolDictionary[poolType].Clear(key);
        }
    }
}