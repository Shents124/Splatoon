using Runtime.Interface;
using Runtime.Pool;
using UnityEngine;

namespace Runtime
{
    public class HorizontalBullet : MonoBehaviour, IDamageable
    {
        [SerializeField] private float lifeTime;
        [SerializeField] private float speed;
        [SerializeField] private Rigidbody2D rigid2D;
        
        private string _key;
        
        private float _currentLifeTime;
        private bool _isDespawn;
        private float _damage;
        
        public void Launch(float damage, string key, Vector2 direction)
        {
            _key = key;
            _damage = damage;
            _currentLifeTime = 0;
            rigid2D.linearVelocity = direction.normalized * speed;
            _isDespawn = false;
        }

        private void Update()
        {
            _currentLifeTime += Time.deltaTime;
            if (_currentLifeTime >= lifeTime)
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