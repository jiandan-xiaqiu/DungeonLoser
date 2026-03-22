using UnityEngine;
using Logger;
using MyGame.Character.Control;

namespace MyGame.Character.Systems
{
    /// <summary>
    /// 动画系统，负责管理所有动画相关逻辑
    /// 控制Animator、动画事件、混合树
    /// 响应状态和行为控制器的指令
    /// </summary>
    public class AnimationSystem : MonoBehaviour
    {
        [Header("动画参数")]
        [SerializeField] private string moveSpeedParam = "MoveSpeed";
        [SerializeField] private string moveXParam = "MoveX";
        [SerializeField] private string moveYParam = "MoveY";
        [SerializeField] private string isMovingParam = "IsMoving";
        [SerializeField] private string isAttackParam = "IsAttack";
        
        [Header("组件引用")]
        [SerializeField] private Animator animator;
        [SerializeField] private MovementController movementController;
        
        private Vector2 lastMoveInput = Vector2.zero;
        
        void Awake()
        {
            // 获取组件引用
            if (animator == null) animator = GetComponent<Animator>();
            if (movementController == null) movementController = GetComponent<MovementController>();
            
            if (animator == null)
                Debug.LogError("Animator not found on AnimationSystem!");
        }
        
        void Update()
        {
            // 每帧更新动画参数
            UpdateAnimationParameters();
        }
        
        /// <summary>
        /// 播放空闲动画
        /// </summary>
        public void PlayIdleAnimation()
        {
            if (animator == null) return;
            
            SetAnimatorBool(isMovingParam, false);
            SetAnimatorFloat(moveSpeedParam, 0f);
        }
        
        /// <summary>
        /// 播放移动动画
        /// </summary>
        public void PlayMoveAnimation(Vector2 moveInput)
        {
            if (animator == null) return;
            
            SetAnimatorBool(isMovingParam, true);
            UpdateMoveAnimation(moveInput);
        }
        
        /// <summary>
        /// 更新移动动画参数
        /// </summary>
        public void UpdateMoveAnimation(Vector2 moveInput)
        {
            if (animator == null) return;
            
            lastMoveInput = moveInput;
            
            // 设置移动方向和速度
            SetAnimatorFloat(moveXParam, moveInput.x);
            SetAnimatorFloat(moveYParam, moveInput.y);
            
            // 使用实际速度或输入大小
            float speed = movementController != null ? 
                movementController.GetCurrentVelocity().magnitude : 
                moveInput.magnitude;
            
            SetAnimatorFloat(moveSpeedParam, speed);
        }
        
        /// <summary>
        /// 更新动画参数
        /// </summary>
        private void UpdateAnimationParameters()
        {
            if (animator == null || movementController == null) return;
            
            // 更新速度参数
            float currentSpeed = movementController.GetCurrentVelocity().magnitude;
            SetAnimatorFloat(moveSpeedParam, currentSpeed);
            
            // 根据速度更新是否移动状态
            bool isMoving = currentSpeed > 0.1f;
            SetAnimatorBool(isMovingParam, isMoving);
        }
        
        /// <summary>
        /// 设置Animator浮点参数
        /// </summary>
        private void SetAnimatorFloat(string paramName, float value)
        {
            if (animator == null) return;
            
            animator.SetFloat(paramName, value);
        }
        
        /// <summary>
        /// 设置Animator布尔参数
        /// </summary>
        private void SetAnimatorBool(string paramName, bool value)
        {
            if (animator == null) return;
            
            animator.SetBool(paramName, value);
        }
        
        /// <summary>
        /// 设置Animator触发器
        /// </summary>
        public void SetAnimatorTrigger(string triggerName)
        {
            if (animator == null) return;
            
            animator.SetTrigger(triggerName);
        }
        
        /// <summary>
        /// 获取Animator引用
        /// </summary>
        public Animator GetAnimator()
        {
            return animator;
        }
        
        /// <summary>
        /// 设置动画速度乘数
        /// </summary>
        public void SetAnimationSpeedMultiplier(float multiplier)
        {
            if (animator == null) return;
            
            animator.speed = Mathf.Max(0, multiplier);
        }
    }
}