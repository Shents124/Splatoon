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
        private float _speed;
        private int _numberBounce;
        
        public void Launch(IWeaponManager iWeaponManager, string key, Vector2 velocity, float lifeTime, float speed, float sizeScale)
        {
            _numberBounce = 0;
            _manager = iWeaponManager;
            _speed = speed;
            _isDespawn = false;
            _key = key;
            _lifeTime = lifeTime;
            rigid2D.linearVelocity = velocity;
            transform.localScale = Vector3.one * (sizeScale * 1.5f);
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
            if (other.gameObject.CompareTag("LeftWall"))
            {
                var incoming = rigid2D.linearVelocity.normalized;
                var normal = Vector2.right;          // mặt tường bên phải
                Vector2 reflected = Vector2.Reflect(incoming, normal);
                    
                rigid2D.linearVelocity = reflected * _speed;
                transform.up = reflected;
                _numberBounce++;
            }
            else if (other.gameObject.CompareTag("RightWall"))
            {
                var incoming = rigid2D.linearVelocity.normalized;
                var normal = Vector2.right;          // mặt tường bên phải
                Vector2 reflected = Vector2.Reflect(incoming, normal);
                    
                rigid2D.linearVelocity = reflected * _speed;
                transform.up = reflected;
                _numberBounce++;
            }
            else if (other.gameObject.CompareTag("TopWall"))
            {
                var incoming = rigid2D.linearVelocity.normalized;
                var normal = Vector2.down;         
                Vector2 reflected = Vector2.Reflect(incoming, normal);
                    
                rigid2D.linearVelocity = reflected * _speed;
                transform.up = reflected;
                _numberBounce++;
            }
            else if (other.gameObject.CompareTag("Ground"))
            {
                var incoming = rigid2D.linearVelocity.normalized;
                var normal = Vector2.up;         
                Vector2 reflected = Vector2.Reflect(incoming, normal);
                    
                rigid2D.linearVelocity = reflected * _speed;
                transform.up = reflected;
                _numberBounce++;
            }
            else
            {
                Despawn();
            }
        }

        private bool CanBounce()
        {
            return _numberBounce < _manager.NumberBounce();
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

        public Vector2 position => transform.position;
    }
}