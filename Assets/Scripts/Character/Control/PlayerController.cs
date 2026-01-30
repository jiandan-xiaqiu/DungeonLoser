using UnityEngine;
using UnityEngine.Tilemaps;
using MyGame.Managers;
using System.Collections;
using UnityEngine.InputSystem;
using Logger;

namespace MyGame.Character.Control
{
    /// <summary>
    /// 玩家控制器，负责处理玩家的游戏玩法输入（具体逻辑调用在状态机中）
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        // 输入事件定义
        public event System.Action<Vector2> OnMoveInput;
        public event System.Action OnAttackInput;
        
        [Header("输入设置")]
        [SerializeField] private string horizontalAxis = "Horizontal";
        [SerializeField] private string verticalAxis = "Vertical";
        [SerializeField] private KeyCode attackKey = KeyCode.F;
        [SerializeField] private float inputDeadzone = 0.1f;
        
        private Vector2 lastMoveInput = Vector2.zero;
        private bool isInputEnabled = true;
        
        void Update()
        {
            if (!isInputEnabled) return;
            
            ProcessMovementInput();
            ProcessAttackInput();
        }

        #region 输入处理
        /// <summary>
        /// 处理移动输入
        /// </summary>
        private void ProcessMovementInput()
        {
            float horizontal = Input.GetAxisRaw(horizontalAxis);
            float vertical = Input.GetAxisRaw(verticalAxis);
            
            Vector2 rawInput = new Vector2(horizontal, vertical);
            Vector2 processedInput = ApplyDeadzone(rawInput);
            
            // 只有输入变化时才发布事件
            if (processedInput != lastMoveInput)
            {
                lastMoveInput = processedInput;
                OnMoveInput?.Invoke(processedInput);
            }
        }
        
        /// <summary>
        /// 处理攻击输入
        /// </summary>
        private void ProcessAttackInput()
        {
            if (Input.GetKeyDown(attackKey))
            {
                OnAttackInput?.Invoke();
            }
        }
        
        /// <summary>
        /// 应用输入死区
        /// </summary>
        private Vector2 ApplyDeadzone(Vector2 input)
        {
            if (input.magnitude < inputDeadzone)
                return Vector2.zero;
            
            return input.normalized * ((input.magnitude - inputDeadzone) / (1 - inputDeadzone));
        }
        
        /// <summary>
        /// 启用/禁用输入
        /// </summary>
        public void SetInputEnabled(bool enabled)
        {
            isInputEnabled = enabled;
            
            if (!enabled)
            {
                // 禁用时发送零输入
                lastMoveInput = Vector2.zero;
                OnMoveInput?.Invoke(Vector2.zero);
            }
        }
        
        /// <summary>
        /// 获取当前输入状态（用于调试）
        /// </summary>
        public Vector2 GetCurrentInput()
        {
            return lastMoveInput;
        }
        #endregion
    }
}