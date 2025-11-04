using System.Collections.Generic;
using Runtime.ConfigData;
using Runtime.Constant;
using Runtime.Pool;
using UnityEngine;

namespace Runtime.Manager
{
    public class WeaponManager : MonoBehaviour
    {
        [SerializeField] private Transform firePoint;
        
        private WeaponConfig _weaponConfig;

        private float _fireInterval;
        private float _currentTimeFireInterval;

        private readonly Dictionary<int, int> _angleShotgun = new()
        {
            { 3, 30 }, { 4, 90 }, { 5, 25 }, { 6, 100 }, { 7, 120 }
        };
        
        public void Initialize(WeaponConfig weaponConfig)
        {
            _weaponConfig = weaponConfig;
            _fireInterval = 1 / _weaponConfig.fireRate;
        }
        
        private void Update()
        {
            _currentTimeFireInterval += Time.deltaTime;
            if (_currentTimeFireInterval >= _fireInterval)
            {
                _currentTimeFireInterval = 0;
                Fire();
            }
        }

        private void Fire()
        {
            switch (_weaponConfig.weaponType)
            {
                case WeaponType.Riffe:
                    FireRiffe();
                    break;
                case WeaponType.Shotgun:
                    FireShotgun();
                    break;
            }
        }

        private void FireRiffe()
        {
            var offset = 0.5f;
            var numberBullet = _weaponConfig.bulletPerShot;
            var x = ((float)(numberBullet - 1) * offset) / 2;
            var minX = firePoint.position.x - x;
            var startPos = new Vector2(minX, firePoint.position.y);
            for (var i = 0; i < numberBullet; i++)
            {
                var bullet = SpawnBullet(PrefabName.BulletRiffe);
                bullet.transform.position = startPos;
                bullet.transform.rotation = Quaternion.identity;
                bullet.Launch(PrefabName.BulletRiffe, _weaponConfig.bulletSpeed * Vector2.up, _weaponConfig.bulletLifeTime, _weaponConfig.bulletRange);
                startPos += new Vector2(offset, 0);
            }
        }

        private void FireShotgun()
        {
            var aimDir = Vector2.up;
            var numberBullet = _weaponConfig.bulletPerShot;
            var coneAngle = _angleShotgun[numberBullet];
            
            float baseAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
            float half = coneAngle * 0.5f;

            // Nếu chỉ 1 viên, không cần offset
            if (numberBullet == 1)
            {
                FireOneBulletShotgun(baseAngle, _weaponConfig.bulletSpeed, _weaponConfig.bulletLifeTime,
                    _weaponConfig.bulletRange);
                return;
            }

            // Chia đều các góc giữa -half và +half
            for (int i = 0; i < numberBullet; i++)
            {
                float t = (float)i / (numberBullet - 1); // 0 → 1
                float offset = Mathf.Lerp(-half, half, t);
                FireOneBulletShotgun(baseAngle + offset, _weaponConfig.bulletSpeed, _weaponConfig.bulletLifeTime,
                    _weaponConfig.bulletRange);
            }
        }

        private void FireOneBulletShotgun(float angleDeg, float speed, float lifeTime, float maxDistance)
        {
            float rad = angleDeg * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            Vector3 spawnPos = firePoint ? firePoint.position : transform.position;

            // Sprite hướng lên → trục up
            Quaternion rot = Quaternion.Euler(0f, 0f, angleDeg - 90f);

            var bullet = SpawnBullet(PrefabName.BulletShotGun);
            bullet.transform.position = spawnPos;
            bullet.transform.rotation = rot;
            bullet.Launch(PrefabName.BulletShotGun, dir * speed, lifeTime, maxDistance);
        }
        
        private Bullet SpawnBullet(string key)
        {
            var clone = PoolService.Spawn<Bullet>(PoolType.Bullet, key);
            return clone;
        }
    }
}