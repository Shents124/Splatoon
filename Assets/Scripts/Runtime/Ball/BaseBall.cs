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
    public struct BallData
    {
        public int id;
        public float attack;
        public float health;
        public float scale;
        public Vector2 force;
        public Vector2 position;
        public int sortOrder;
    }
    
    public class BaseBall : MonoBehaviour, IDamageable
    {
        [SerializeField] private Rigidbody2D rigid2D;
        [SerializeField] private SortingGroup sortingGroup;
        [SerializeField] private TextMeshPro heathTxt;
        public int ballId => _ballData.id;
        public float maxHealth => _ballData.health;
        
        private float _currentHealth;
        private BallData _ballData;
        
        private bool _isDead;
        private Action<BaseBall> _onDead;
        
        public void Initialize(BallData ballData, Action<BaseBall> onDead)
        {
            _ballData = ballData;
            _onDead = onDead;
            _isDead = false;
            _currentHealth = ballData.health;
            var scale = ballData.scale;
            transform.localScale = new Vector3(scale, scale, scale);
            transform.position = ballData.position;
            sortingGroup.sortingOrder = ballData.sortOrder;
            UpdateHeath();
            rigid2D.AddForce(ballData.force, ForceMode2D.Impulse);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other == null)
                return;

            if (!BulletCache.TryGetBulletDamage(other, out var iBullet)) return;
            
            _currentHealth -= iBullet.GetDamage();
            UpdateHeath();
            iBullet.Despawn();
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
            PoolService.Despawn(PoolType.Ball, PrefabName.BALL_PREFAB, gameObject);
        }
    }
}