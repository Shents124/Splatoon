using Runtime.Interface;
using Runtime.Pool;
using UnityEngine;

namespace Runtime.Ball
{
    public class BossBullet : MonoBehaviour, IDamageable
    {
        [SerializeField] private Rigidbody2D rigid2D;
        
        private float _lifeTime;
        private string _key;
        
        private float _currentLifeTime;
        private bool _isDespawn;
        private float _damage;
        
        public void Launch( string key, Vector2 velocity, float lifeTime, float damage)
        {
            _isDespawn = false;
            _key = key;
            _lifeTime = lifeTime;
            rigid2D.linearVelocity = velocity;
            _currentLifeTime = 0;
            _damage = damage;
        }
        
        private void Update()
        {
            _currentLifeTime += Time.deltaTime;
            if (_currentLifeTime >= _lifeTime)
            {
                _currentLifeTime = 0;
                DeSpawn();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            DeSpawn();
        }

        private void DeSpawn()
        {
            if (_isDespawn)
                return;

            _isDespawn = true;
            PoolService.Despawn(PoolType.Bullet, _key, gameObject);
        }
        
        public float GetDamage()
        {
            return _damage;
        }
    }
}