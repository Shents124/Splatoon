using Runtime.Interface;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.UI;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Runtime.Manager
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private InputSystemUIInputModule uiInputModule;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float boundMove;

        private Rigidbody2D _rb;
        private Vector2 _touchPosition;

        private float _maxHeath;
        private float _currentHeath;
        
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

        public void Initialize(float maxHeath)
        {
            _maxHeath = maxHeath;
            _currentHeath = _maxHeath;
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                _currentHeath -= damageable.GetDamage();
                if (_currentHeath <= 0)
                {
                    Debug.LogError("Player died");
                }
            }
        }
    }
}