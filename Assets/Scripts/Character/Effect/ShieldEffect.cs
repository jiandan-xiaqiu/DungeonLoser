using UnityEngine;
using MyGame;

namespace MyGame.Character.Effect
{
    /// <summary>
    /// 护盾视觉效果组件
    /// </summary>
    public class ShieldEffect : MonoBehaviour
    {
        [Header("护盾光圈引用")]
        [SerializeField] private GameObject shieldVisual;

        [Header("受击闪烁设置")]
        [SerializeField] private Color hitColor = Color.cyan;
        [SerializeField] private float hitFlashDuration = 0.15f;

        [Header("消失提醒设置")]
        [SerializeField] private float disappearThreshold = 0.1f;
        [SerializeField] private float fadeOutDuration = 0.5f;
        [SerializeField] private float fadeOutFlashInterval = 0.1f;

        private SpriteRenderer _shieldSpriteRenderer;
        private CharacterManager _characterManager;
        private Color _originalColor;
        private Coroutine _flashCoroutine;
        private bool _isFadingOut;

        private void Awake()
        {
            _characterManager = CharacterManager.Instance;
        }

        private void Start()
        {
            if (shieldVisual != null)
            {
                _shieldSpriteRenderer = shieldVisual.GetComponent<SpriteRenderer>();
                if (_shieldSpriteRenderer != null)
                {
                    _originalColor = _shieldSpriteRenderer.color;
                }
                shieldVisual.SetActive(false);
            }
        }

        private void Update()
        {
            UpdateShieldVisibility();
            UpdateFadeOut();
        }

        /// <summary>
        /// 更新护盾可见性
        /// </summary>
        private void UpdateShieldVisibility()
        {
            if (shieldVisual == null || _characterManager == null)
                return;

            bool hasShield = _characterManager.ShieldAmount > 0f;

            if (hasShield && !shieldVisual.activeSelf)
            {
                shieldVisual.SetActive(true);
                _isFadingOut = false;
                if (_shieldSpriteRenderer != null)
                {
                    _shieldSpriteRenderer.color = _originalColor;
                }
            }
            else if (!hasShield && shieldVisual.activeSelf)
            {
                shieldVisual.SetActive(false);
            }
        }

        /// <summary>
        /// 更新消失淡出逻辑
        /// </summary>
        private void UpdateFadeOut()
        {
            if (_characterManager == null || _shieldSpriteRenderer == null || !shieldVisual.activeSelf)
                return;

            float shieldRatio = _characterManager.ShieldAmount / _characterManager.ShieldMaxAmount;

            if (shieldRatio <= disappearThreshold && shieldRatio > 0f && !_isFadingOut)
            {
                _isFadingOut = true;
                if (_flashCoroutine != null)
                {
                    StopCoroutine(_flashCoroutine);
                }
                _flashCoroutine = StartCoroutine(FadeOutSequence());
            }
        }

        /// <summary>
        /// 护盾受击闪烁效果
        /// </summary>
        public void TriggerHitFlash()
        {
            if (_flashCoroutine != null)
            {
                StopCoroutine(_flashCoroutine);
            }
            _flashCoroutine = StartCoroutine(HitFlashSequence());
        }

        /// <summary>
        /// 受击闪烁序列
        /// </summary>
        private System.Collections.IEnumerator HitFlashSequence()
        {
            if (_shieldSpriteRenderer == null)
                yield break;

            _shieldSpriteRenderer.color = hitColor;
            yield return new WaitForSeconds(hitFlashDuration);
            _shieldSpriteRenderer.color = _originalColor;
        }

        /// <summary>
        /// 消失淡出闪烁序列
        /// </summary>
        private System.Collections.IEnumerator FadeOutSequence()
        {
            if (_shieldSpriteRenderer == null)
                yield break;

            float elapsed = 0f;
            float halfInterval = fadeOutFlashInterval / 2f;
            Color fadedColor = _originalColor;
            fadedColor.a = 0f;

            while (elapsed < fadeOutDuration)
            {
                _shieldSpriteRenderer.color = _originalColor;
                yield return new WaitForSeconds(halfInterval);

                _shieldSpriteRenderer.color = fadedColor;
                yield return new WaitForSeconds(halfInterval);

                elapsed += fadeOutFlashInterval;
            }

            _shieldSpriteRenderer.color = _originalColor;
            _isFadingOut = false;
        }
    }
}