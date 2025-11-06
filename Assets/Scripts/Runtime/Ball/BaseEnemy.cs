using System;
using Runtime.Constant;
using Runtime.Interface;
using Runtime.Manager;
using Runtime.Pool;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace Runtime.Ball
{
    public class BaseEnemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private SortingGroup sortingGroup;
        [SerializeField] private TextMeshPro heathTxt;
        public int ballId => _ballData.id;
        public float maxHealth => _ballData.health;
        
        public BallType ballType => _ballData.ballType;
        
        protected SpawnManager spawnManager;
        
        private float _currentHealth;
        private BallData _ballData;
        
        private bool _isDead;
        private Action<BaseEnemy> _onDead;
        private string _key;
        
        public virtual void Initialize(SpawnManager spawnManager, BallData ballData, string key, Action<BaseEnemy> onDead)
        {
            this.spawnManager = spawnManager;
            _ballData = ballData;
            _key = key;
            _onDead = onDead;
            _isDead = false;
            _currentHealth = ballData.health;
            var scale = ballData.scale;
            transform.localScale = new Vector3(scale, scale, scale);
            transform.position = ballData.position;
            sortingGroup.sortingOrder = ballData.sortOrder;
            UpdateHeath();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other == null)
                return;

            if (!BulletCache.TryGetBulletDamage(other, out var iBullet)) return;
            
            _currentHealth -= iBullet.GetDamage();
            UpdateHeath();
            if (_currentHealth <= 0)
            {
                DeSpawn();
            }
        }

        private void UpdateHeath()
        {
            heathTxt.text = $"{(int)_currentHealth}";
        }
        
        public float GetDamage()
        {
            return _ballData.attack;
        }

        private void DeSpawn()
        {
            if (_isDead)
                return;
            
            _isDead = true;
            _onDead?.Invoke(this);
            PoolService.Despawn(PoolType.Ball, _key, gameObject);
        }
    }
}