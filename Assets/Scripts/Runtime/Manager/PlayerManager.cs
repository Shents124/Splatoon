using System.Collections.Generic;
using Runtime.Constant;
using Runtime.Interface;
using Runtime.Pool;
using Runtime.PubSub;
using Runtime.PubSub.CommonMessage;
using Runtime.Skill;
using Runtime.Stat;
using Runtime.UI;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.UI;
using ZBase.Foundation.PubSub;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Runtime.Manager
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private PlayerStatUI statUI;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private InputSystemUIInputModule uiInputModule;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float boundMove;

        private Rigidbody2D _rb;
        private Vector2 _touchPosition;

        private float _maxHeath;
        private float _currentHeath;
        private float _shield;

        private List<ISubscription> _subscriptions = new();

        private void Awake()
        {
           WorldMessenger.Sub<AddShieldMessage>(msg =>
            {
                _shield = msg.value;
            }).AddTo(_subscriptions);
           
           WorldMessenger.Sub<SpawnSpikeShieldMessage>(SpawnSpikeShield).AddTo(_subscriptions);
        }

        private void OnDestroy()
        {
            _subscriptions?.UnsubscribeAll();
        }

        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
            TouchSimulation.Enable();
        }

        private void OnDisable()
        {
            EnhancedTouchSupport.Disable();
            TouchSimulation.Disable();
        }

        public void Initialize(WeaponStat weaponStat)
        {
            _maxHeath = weaponStat.health.value;
            _currentHeath = _maxHeath;
            _shield = 0;
            
            statUI.UpdateHealth(_currentHeath);
            statUI.UpdateShield(_shield);
        }
        
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            HandleTouchInput();
        }

        void FixedUpdate()
        {
            float targetX = Mathf.Clamp(_touchPosition.x, -boundMove, boundMove);
            float newX = Mathf.Lerp(transform.position.x, targetX, Time.fixedDeltaTime * moveSpeed);

            // Di chuyển bằng MovePosition để an toàn vật lý
            _rb.MovePosition(new Vector2(newX, _rb.position.y));
        }

        void HandleTouchInput()
        {
            var touchCount = Touch.activeTouches.Count;
            if (touchCount == 0)
                return;

            var touch = Touch.activeTouches[0];

            if (touch.valid == false)
            {
                return;
            }

            if (IsPointerOverUI(touch.touchId))
            {
                return;
            }

            // Chuyển sang world-space
            _touchPosition = mainCamera.ScreenToWorldPoint(touch.screenPosition);
        }

        bool IsPointerOverUI(int touchId)
        {
            return uiInputModule.IsPointerOverGameObject(touchId);
        }

        private void SpawnSpikeShield(SpawnSpikeShieldMessage msg)
        {
            _shield += msg.shield;
            var clone = PoolService.Spawn<SpikeShield>(PoolType.Bullet, PrefabName.Spike_Shield);
            clone.transform.SetParent(gameObject.transform);
            clone.transform.localPosition = Vector3.zero;
            clone.Initialize(msg.damage);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                var damage = damageable.GetDamage();
                if (damage <= _shield)
                {
                    _shield -= damage;
                    statUI.UpdateShield(_shield);
                    return;
                }
                
                var damageRemain = damage - _shield;
                _shield = 0;

                _currentHeath -= damageRemain;
                _currentHeath = Mathf.Clamp(_currentHeath, 0, _maxHeath);
                
                statUI.UpdateHealth(_currentHeath);
                statUI.UpdateShield(_shield);
                if (_currentHeath <= 0)
                {
                    Debug.LogError("Player died");
                }
                else if (_currentHeath / _maxHeath <= 0.3f)
                {
                    WorldMessenger.Pub(new HealthUnder30());
                }
                
                
            }
        }
    }
}