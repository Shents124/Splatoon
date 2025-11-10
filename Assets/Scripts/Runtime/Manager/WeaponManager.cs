using Runtime.ConfigData;
using Runtime.Constant;
using Runtime.Interface;
using Runtime.Pool;
using Runtime.Stat;
using UnityEngine;

namespace Runtime.Manager
{
    public class WeaponManager : MonoBehaviour, IWeaponManager
    {
        [SerializeField] private Transform firePoint;
        [SerializeField] private ShotgunAngleConfig shotgunAngleConfig;

        private WeaponStat _weaponStat;
        
        private WeaponConfig _weaponConfig;
        
        private float _currentTimeFireInterval;
        private bool _isInit;
        
        public void Initialize(WeaponStat weaponStat, WeaponConfig weaponConfig)
        {
            _weaponConfig = weaponConfig;
            _weaponStat = weaponStat;
            _weaponStat.Initialize(_weaponConfig);
            _isInit = true;
        }
        
        private void Update()
        {
            if (_isInit == false)
                return;
            
            _currentTimeFireInterval += Time.deltaTime;
            if (_currentTimeFireInterval >= 1 / fireRate)
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
            var x = ((float)(numberBullet - 1) * offset) / 2;
            var minX = firePoint.position.x - x;
            var startPos = new Vector2(minX, firePoint.position.y);
            for (var i = 0; i < numberBullet; i++)
            {
                var bullet = SpawnBullet(PrefabName.BulletRiffe);
                bullet.transform.position = startPos;
                bullet.transform.rotation = Quaternion.identity;
                bullet.Launch(this, PrefabName.BulletRiffe, _weaponConfig.bulletSpeed * Vector2.up,
                    _weaponConfig.bulletLifeTime, _weaponConfig.bulletSpeed, sizeScale);
                startPos += new Vector2(offset, 0);
            }
        }

        private void FireShotgun()
        {
            var aimDir = Vector2.up;
            var coneAngle = shotgunAngleConfig.GetAngle(numberBullet);
            
            float baseAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
            float half = coneAngle * 0.5f;

            // Nếu chỉ 1 viên, không cần offset
            if (numberBullet == 1)
            {
                FireOneBulletShotgun(baseAngle, bulletSpeed, _weaponConfig.bulletLifeTime,
                    _weaponConfig.bulletRange);
                return;
            }

            // Chia đều các góc giữa -half và +half
            for (int i = 0; i < numberBullet; i++)
            {
                float t = (float)i / (numberBullet - 1); // 0 → 1
                float offset = Mathf.Lerp(-half, half, t);
                FireOneBulletShotgun(baseAngle + offset, bulletSpeed, _weaponConfig.bulletLifeTime,
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
            bullet.Launch(this, PrefabName.BulletShotGun, dir * speed, lifeTime, _weaponConfig.bulletSpeed, sizeScale);
        }
        
        private Bullet SpawnBullet(string key)
        {
            var clone = PoolService.Spawn<Bullet>(PoolType.Bullet, key);
            return clone;
        }

        private float fireRate => _weaponStat.fireRate.value;
        
        private float bulletSpeed => _weaponStat.bulletSpeed.value;

        private int numberBullet => (int)_weaponStat.bulletPerShot.value;

        private float sizeScale => _weaponStat.bulletSize.value;

        public int NumberBounce() => (int) _weaponStat.numberBounce.value;
        
        public float GetDamage()
        {
            var damage = _weaponStat.attack.value * _weaponStat.dmg.value;
            if (_weaponStat.IsCrit())
            {
                return damage * _weaponStat.critDamage.value;
            }
            
            return damage;
        }

      
    }
}