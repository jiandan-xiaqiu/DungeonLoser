using System;using UnityEngine;
using Logger;

namespace MyGame.Character.Systems
{
    /// <summary>
    /// 健康系统，负责处理角色的生命值管理
    /// </summary>
    public class HealthSystem : MonoBehaviour
    {
        #region 字段
        private CharacterRuntimeData _characterRuntimeData;
        
        // 休息相关设置
        // [Tooltip("休息恢复速率")]
        // [SerializeField] private float _restRecoveryRate = 5f;
        
        // [Tooltip("休息最大持续时间")]
        // [SerializeField] private float _maxRestDuration = 10f;
        
        // [Tooltip("当前休息时间")]
        // private float _currentRestTime = 0f;
        
        //private static readonly string LOG_MODULE = LogModules.PLAYER;
        #endregion

        #region 事件
        /// <summary>
        /// 生命值改变事件
        /// </summary>
        // public event Action<float, float> OnHealthChanged;
        
        // /// <summary>
        // /// 角色死亡事件
        // /// </summary>
        // public event Action OnPlayerDeath;
        
        // /// <summary>
        // /// 休息开始事件
        // /// </summary>
        // public event Action OnRestStarted;
        
        // /// <summary>
        // /// 休息结束事件
        // /// </summary>
        // public event Action OnRestEnded;
        #endregion

        #region 属性
        /// <summary>
        /// 当前生命值
        /// </summary>
        // public float Health
        // {
        //     get { return _health; }
        //     set
        //     {
        //         float oldHealth = _health;
        //         _health = Mathf.Clamp(value, 0f, _maxHealth);
                
        //         // 如果生命值发生变化，触发事件
        //         if (Mathf.Abs(oldHealth - _health) > 0.01f)
        //         {
        //             OnHealthChanged?.Invoke(_health, _maxHealth);
                    
        //             // 检查是否死亡
        //             if (_health <= 0f)
        //             {
        //                 OnPlayerDeath?.Invoke();
        //             }
        //         }
        //     }
        // }
        
        // /// <summary>
        // /// 最大生命值
        // /// </summary>
        // public float MaxHealth
        // {
        //     get { return _characterRuntimeData.CurrentHpMax; }
        // }
        
        // /// <summary>
        // /// 休息恢复速率
        // /// </summary>
        // public float RestRecoveryRate
        // {
        //     get { return _restRecoveryRate; }
        //     set { _restRecoveryRate = Mathf.Max(0f, value); }
        // }
        
        // /// <summary>
        // /// 休息最大持续时间
        // /// </summary>
        // public float MaxRestDuration
        // {
        //     get { return _maxRestDuration; }
        //     set { _maxRestDuration = Mathf.Max(0f, value); }
        // }
        
        // /// <summary>
        // /// 当前休息时间
        // /// </summary>
        // public float CurrentRestTime
        // {
        //     get { return _currentRestTime; }
        // }
        #endregion

        #region 初始化
        /// <summary>
        /// 初始化健康系统
        /// </summary>
        /// <param name="characterRuntimeData">角色运行时数据</param>
        public HealthSystem(CharacterRuntimeData characterRuntimeData)
        {
            _characterRuntimeData = characterRuntimeData;            
            //Log.Info(LOG_MODULE, "HealthSystem initialized", this);
        }
        #endregion

        #region 生命值管理
        /// <summary>
        /// 造成伤害
        /// </summary>
        /// <param name="damage">伤害值</param>
        // public void TakeDamage(float damage)
        // {
        //     if (damage <= 0f)
        //         return;
                
        //     Health -= damage;
        //     Log.Info(LOG_MODULE, $"Player took {damage} damage. Health: {_health}/{_maxHealth}", this);
        // }
        
        // /// <summary>
        // /// 恢复生命值
        // /// </summary>
        // /// <param name="healAmount">恢复量</param>
        // public void Heal(float healAmount)
        // {
        //     if (healAmount <= 0f)
        //         return;
                
        //     Health += healAmount;
        //     Log.Info(LOG_MODULE, $"Player healed {healAmount}. Health: {_health}/{_maxHealth}", this);
        // }
        
        // /// <summary>
        // /// 设置最大生命值
        // /// </summary>
        // /// <param name="newMaxHealth">新的最大生命值</param>
        // public void SetMaxHealth(float newMaxHealth)
        // {
        //     _maxHealth = Mathf.Max(0f, newMaxHealth);
        //     Health = Mathf.Min(_health, _maxHealth); // 确保当前生命值不超过新的最大值
        // }
        #endregion

        #region 更新生命值到控制器
        /// <summary>
        /// 将当前生命值同步到玩家控制器
        /// </summary>
        public void SyncHealthToController()
        {
            //_playerController.Health = _health;
        }
        #endregion
    }
}