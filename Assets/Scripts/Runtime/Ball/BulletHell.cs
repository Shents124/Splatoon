using System.Collections.Generic;
using DG.Tweening;
using Runtime.Pool;
using UnityEngine;

namespace Runtime.Ball
{
    public class BulletHell : MonoBehaviour
    {
        public float bulletLifeTime = 4;
        public float bulletSpeed = 10;
        public float damage = 1;
        public float fireRate = 5;
        public List<Transform> firePoints = new List<Transform>();
        public float rotateSpeed = 60f;
        public float timeRotate = 3f;

        private float _currentTime;
        private float _currentTimeRotate;
        
        private void Start()
        {
            transform.DORotate(new Vector3(0,0,rotateSpeed), timeRotate, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Incremental)
                .SetEase(Ease.Linear);
        }

        private void Update()
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= 1 / fireRate)
            {
                Spawn();
                _currentTime = 0;
            }
        }

        private void Spawn()
        {
            for (int i = 0; i < firePoints.Count; i++)
            {
                var clone = PoolService.Spawn<BossBullet>(PoolType.Bullet, "boss_bullet");
                clone.transform.position = firePoints[i].position;
                var direction = (firePoints[i].position - transform.position).normalized;
                clone.Launch("boss_bullet", direction * bulletSpeed, bulletLifeTime, damage);
            }
        }
    }
}