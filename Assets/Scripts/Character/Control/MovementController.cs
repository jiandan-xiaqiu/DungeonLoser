using UnityEngine;
using UnityEngine.Tilemaps;
using Logger;
using System.Collections;

namespace MyGame.Character.Control
{
    /// <summary>
    /// 移动控制器，负责角色的移动逻辑实现
    /// 处理物理、碰撞、路径计算等具体移动操作
    /// </summary>
    public class MovementController : MonoBehaviour
    {
        [Header("移动设置")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float acceleration = 10f;
        [SerializeField] private float deceleration = 10f;
        [SerializeField] private bool flipSpriteByDirection = true;
        
        [Header("组件引用")]
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        private Vector2 targetVelocity = Vector2.zero;
        private Vector2 currentVelocity = Vector2.zero;
        private bool isMovementEnabled = true;
        
        void Awake()
        {
            // 获取组件引用
            if (rb == null) rb = GetComponent<Rigidbody2D>();
            if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            
            if (rb == null)
                Debug.LogError("Rigidbody2D not found on MovementController!");
        }
        
        void FixedUpdate()
        {
            if (!isMovementEnabled) return;
            
            ApplyMovement();
        }
        
        /// <summary>
        /// 执行移动
        /// </summary>
        public void Move(Vector2 direction)
        {
            if (!isMovementEnabled) return;
            
            // 设置目标速度
            targetVelocity = direction * moveSpeed;
            
            // 更新面向方向
            UpdateFacingDirection(direction);
        }
        
        /// <summary>
        /// 停止移动
        /// </summary>
        public void StopMovement()
        {
            targetVelocity = Vector2.zero;
            currentVelocity = Vector2.zero;
            
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
            }
        }
        
        /// <summary>
        /// 启用移动
        /// </summary>
        public void EnableMovement()
        {
            isMovementEnabled = true;
        }
        
        /// <summary>
        /// 禁用移动
        /// </summary>
        public void DisableMovement()
        {
            isMovementEnabled = false;
            StopMovement();
        }
        
        /// <summary>
        /// 应用移动（使用平滑加速/减速）
        /// </summary>
        private void ApplyMovement()
        {
            if (rb == null) return;
            
            // 平滑插值到目标速度
            float accelerationRate = (targetVelocity.magnitude > 0.1f) ? acceleration : deceleration;
            currentVelocity = Vector2.Lerp(currentVelocity, targetVelocity, accelerationRate * Time.fixedDeltaTime);
            
            // 应用速度
            rb.velocity = currentVelocity;
        }
        
        /// <summary>
        /// 更新面向方向
        /// </summary>
        private void UpdateFacingDirection(Vector2 direction)
        {
            if (!flipSpriteByDirection || spriteRenderer == null) return;
            
            // 只在水平方向有明显输入时翻转
            if (Mathf.Abs(direction.x) > 0.1f)
            {
                spriteRenderer.flipX = direction.x < 0;
            }
        }
        
        /// <summary>
        /// 获取当前速度
        /// </summary>
        public Vector2 GetCurrentVelocity()
        {
            return currentVelocity;
        }
        
        /// <summary>
        /// 获取移动速度
        /// </summary>
        public float GetMoveSpeed()
        {
            return moveSpeed;
        }
        
        /// <summary>
        /// 设置移动速度
        /// </summary>
        public void SetMoveSpeed(float speed)
        {
            moveSpeed = Mathf.Max(0, speed);
        }
    }
}