using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Pool
{
    public class PoolHolder
    {
        private readonly GameObject _poolKey;

        private readonly Queue<GameObject> _queue;
        public int PoolCount => _queue.Count;

        public PoolHolder(GameObject prefab)
        {
            _poolKey = prefab;
            _queue = new();
        }

        #region API

        public GameObject Spawn()
        {
            if (!_poolKey)
            {
                Clear();
                return null;
            }

            GameObject clone = _queue.Count <= 0 ? Object.Instantiate(_poolKey) : _queue.Dequeue();

            while (!clone)
            {
                clone = _queue.Count <= 0 ? Object.Instantiate(_poolKey) : _queue.Dequeue();
            }

            clone.SetActive(true);
            return clone;
        }

        public void Populate(int numberOfObjects)
        {
            if (numberOfObjects <= 0)
            {
                Debug.LogError("Number of objects must be greater than 0");
            }

            var spawnNumber = numberOfObjects - PoolCount;
            if (spawnNumber <= 0) return;

            for (int i = 0; i < spawnNumber; i++)
            {
                var clone = Object.Instantiate(_poolKey);
                clone.SetActive(false);
                _queue.Enqueue(clone);
            }
        }

        public void DeSpawn(GameObject instance, bool isDestroy = false)
        {
            if (isDestroy)
            {
                Clear();
                Object.Destroy(instance);
            }
            else
            {
                instance.SetActive(false);
                if (!_queue.Contains(instance))
                {
                    _queue.Enqueue(instance);
                }
                else
                {
                    Debug.LogWarning("Pool had item : " + instance.name);
                }
            }
        }

        public void Clear()
        {
            foreach (var gameObject in _queue)
            {
                Object.Destroy(gameObject);
            }

            _queue.Clear();
        }

        #endregion
    }
}