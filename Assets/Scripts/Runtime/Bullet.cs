using Runtime.Interface;
using Runtime.Pool;
using UnityEngine;

namespace Runtime
{
    public class Bullet : MonoBehaviour, IBullet
    {
        [SerializeField] private Rigidbody2D rigid2D;

        private float _lifeTime;
        private float _maxDistance;
        private string _key;
        
        private float _currentLifeTime;
        private Vector2 _startPosition;
        private bool _isDespawn;
        
        public void Launch(string key, Vector2 velocity, float lifeTime, float maxDistance)
        {
            _isDespawn = false;
            _key = key;
            _lifeTime = lifeTime;
            _maxDistance = maxDistance;
            rigid2D.linearVelocity = velocity;
            
            _startPosition = transform.position;
            _currentLifeTime = 0;
        }

        private void Update()
        {
            _currentLifeTime += Time.deltaTime;
            if (_currentLifeTime >= _lifeTime)
            {
                _currentLifeTime = 0;
                DeSpawn();
            }

            // if (Vector2.Distance(transform.position, _startPosition) > _maxDistance)
            // {
            //     transform.position = _startPosition;
            //     DeSpawn();
            // }
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
            return 1;
        }

        public void Despawn()
        {
            DeSpawn();
        }
    }
}