using System.Collections.Generic;
using Runtime.Interface;
using UnityEngine;

namespace Runtime.Manager
{
    public static class BulletCache
    {
        private static Dictionary<int, IBullet> _bulletCache = new();
        private static Dictionary<int, IDamageable> _damageableCache = new();

        public static void Clear()
        {
            _bulletCache.Clear();
        }

        public static bool TryGetIBullet(Collider2D other, out IBullet iBullet)
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

        public static bool TryGetDamageable(Collider2D other, out IDamageable damageable)
        {
            var instanceId = other.gameObject.GetInstanceID();
            if (_damageableCache.TryGetValue(instanceId, out damageable))
            {
                return true;
            }

            if (other.gameObject.TryGetComponent(out damageable))
            {
                _damageableCache.Add(instanceId, damageable);
                return true;
            }

            return false;
        }
    }
}