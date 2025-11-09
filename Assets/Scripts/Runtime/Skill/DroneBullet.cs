using Runtime.Interface;
using Runtime.Pool;
using UnityEngine;

namespace Runtime.Skill
{
    public class DroneBullet : MonoBehaviour, IDamageable
    {
        public Rigidbody2D rigid;
        public float lifeTime = 5;
        public float speed = 10;

        private float _attack;
        private string _key;

        private float _currentLifeTime;
        private bool _isDespawn;
        
        public void Launch(string key, float attack)
        {
            _key = key;
            _attack = attack;
            rigid.linearVelocity = speed * Vector2.up;
            _isDespawn = false;
            _currentLifeTime = 0;
        }

        private void Update()
        {
            _currentLifeTime += Time.deltaTime;
            if (_currentLifeTime >= lifeTime)
            {
                Despawn();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Despawn();
        }

        private void Despawn()
        {
            if (_isDespawn)
                return;

            _isDespawn = true;
            PoolService.Despawn(PoolType.Bullet, _key, gameObject);
        }
        
        public float GetDamage()
        {
            return _attack;
        }
    }
}