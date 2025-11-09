using Runtime.Constant;
using Runtime.Pool;
using UnityEngine;

namespace Runtime.Skill
{
    public class Drone : MonoBehaviour
    {
        public Transform firePoint;

        private float _fireRate;
        private float _damage;

        private float _currentTime;

        public void Initialize(float fireRate, float damage)
        {
            _fireRate = fireRate;
            _damage = damage;
            _currentTime = 0;
            Fire();
            transform.localPosition = Vector3.zero;
        }

        private void Update()
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= 1 / _fireRate)
            {
                Fire();
                _currentTime = 0;
            }
        }

        private void Fire()
        {
            var bullet = PoolService.Spawn<DroneBullet>(PoolType.Bullet, PrefabName.drone_bullet);
            bullet.transform.position = firePoint.position;
            bullet.Launch(PrefabName.drone_bullet, _damage);
        }

        public void Despawn()
        {
            transform.SetParent(null);
            PoolService.Despawn(PoolType.Bullet, PrefabName.drone, gameObject);
        }
    }
}