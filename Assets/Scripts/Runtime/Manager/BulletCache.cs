using System.Collections.Generic;
using Runtime.Interface;
using UnityEngine;

namespace Runtime.Manager
{
    public static class BulletCache
    {
        private static Dictionary<int, IBullet> _bulletCache = new();

        public static void Clear()
        {
            _bulletCache.Clear();
        }

        public static bool TryGetBulletDamage(Collider2D other, out IBullet iBullet)
        {
            var instanceId = other.gameObject.GetInstanceID();
            if (_bulletCache.TryGetValue(instanceId, out iBullet))
            {
                return true;
            }

            if (other.TryGetComponent(out iBullet))
            {
                _bulletCache.Add(instanceId, iBullet);
                return true;
            }

            return false;
        }
    }
}