using System;
using DG.Tweening;
using Runtime.Constant;
using Runtime.Interface;
using Runtime.Manager;
using Runtime.Pool;
using Runtime.PubSub;
using Runtime.PubSub.CommonMessage;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace Runtime.Ball
{
    public class BaseEnemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private SortingGroup sortingGroup;
        [SerializeField] private TextMeshPro heathTxt;
        public int ballId => ballData.id;
        public float maxHealth => ballData.health;
        
        public BallType ballType => ballData.ballType;
        
        protected SpawnManager spawnManager;
        
        private float _currentHealth;
        public BallData ballData { get; private set; }
        
        private bool _isDead;
        private Action<BaseEnemy> _onDead;
        private string _key;
        private Tweener _tweener;
        
        public virtual void Initialize(SpawnManager spawnManager, BallData ballData, string key, Action<BaseEnemy> onDead)
        {
            this.spawnManager = spawnManager;
            this.ballData = ballData;
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
        
        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other == null)
                return;

            if (BulletCache.TryGetIBullet(other, out var iBullet))
            {
                WorldMessenger.Pub(new BulletHitEnemy(iBullet.position));
                _currentHealth -= iBullet.GetDamage();
            }
            else if (BulletCache.TryGetDamageable(other, out var damageable))
            {
                _currentHealth -= damageable.GetDamage();
            }
            
            UpdateHeath();
            if (_currentHealth <= 0)
            {
                DeSpawn();
            }
            else
            {
                DoScale();
            }
        }

        private void DoScale()
        {
            var scale = ballData.scale;
            transform.localScale = new Vector3(scale, scale, scale);
            _tweener?.Kill();
            _tweener = transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 40);
        }
        
        private void UpdateHeath()
        {
            heathTxt.text = $"{(int)_currentHealth}";
        }
        
        public float GetDamage()
        {
            return ballData.attack;
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