using Runtime.Interface;
using Runtime.Pool;
using UnityEngine;

namespace Runtime
{
    public class Bullet : MonoBehaviour, IBullet
    {
        [SerializeField] private Rigidbody2D rigid2D;

        private float _lifeTime;
        private string _key;
        
        private float _currentLifeTime;
        private bool _isDespawn;
        private IWeaponManager _manager;
        
        public void Launch(IWeaponManager iWeaponManager, string key, Vector2 velocity, float lifeTime)
        {
            _manager = iWeaponManager;
            _isDespawn = false;
            _key = key;
            _lifeTime = lifeTime;
            rigid2D.linearVelocity = velocity;
            
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

        public float GetDamage() => _manager.GetDamage();

        public void Despawn()
        {
            DeSpawn();
        }
    }
}