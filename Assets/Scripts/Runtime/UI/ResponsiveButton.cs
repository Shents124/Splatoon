using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.UI
{
    public class ResponsiveButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [Header("Scale Down")]
        [SerializeField] private float pressedScale = 0.9f;
        [SerializeField] private float scaleDownDuration = 0.1f;

        [Header("Punch Back")]
        [SerializeField] private Vector3 punchScale = new(0.15f, 0.15f, 0f);
        [SerializeField] private float punchDuration = 0.3f;
        [SerializeField] private int punchVibrato = 8;
        [SerializeField] private float punchElasticity = 1f;
        
        private RectTransform _rectTransform;
        private Vector3 _originalScale;
        private bool _pointerDown;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _originalScale = _rectTransform.localScale;
        }

        private void OnDestroy()
        {
            _rectTransform.DOKill();
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _pointerDown = true;
            _rectTransform.DOKill();
            _rectTransform.DOScale(_originalScale * pressedScale, scaleDownDuration).SetUpdate(true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_pointerDown)
            {
                _pointerDown = false;

                _rectTransform.DOKill();
                _rectTransform.localScale = _originalScale;

                _rectTransform.DOPunchScale(punchScale, punchDuration, punchVibrato, punchElasticity).SetUpdate(true);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // Nếu kéo ra ngoài nút khi vẫn đang giữ chuột/touch → reset scale
            if (_pointerDown)
            {
                _pointerDown = false;
                _rectTransform.DOKill();
                _rectTransform.DOScale(_originalScale, 0.1f).SetUpdate(true);
            }
        }
    }
}
