using UnityEngine;
using Logger;

namespace MyGame.Character.Control
{
    /// <summary>
    /// 攻击控制器，负责角色的攻击逻辑实现
    /// 处理伤害计算、命中检测、攻击范围等具体攻击操作
    /// </summary>
    public class AttackController : MonoBehaviour
    {
        // #region 字段
        // private PlayerController _playerController;
        // private Animator _animator;
        
        // [Header("攻击设置")]
        // [Tooltip("基础攻击力")]
        // [SerializeField] private float _baseAttackPower = 10f;
        
        // [Tooltip("攻击范围")]
        // [SerializeField] private float _attackRange = 1f;
        
        // [Tooltip("攻击冷却时间")]
        // [SerializeField] private float _attackCooldown = 0.5f;
        
        // [Tooltip("当前冷却时间")]
        // private float _currentCooldown = 0f;
        
        // [Tooltip("攻击伤害倍数")]
        // private float _attackMultiplier = 1f;
        
        // [Tooltip("攻击帧时间点")]
        // [SerializeField] private float _attackFrameTime = 0.3f;
        
        // private static readonly string LOG_MODULE = LogModules.PLAYER;
        // #endregion

        // #region 事件
        // /// <summary>
        // /// 攻击命中事件
        // /// </summary>
        // public event System.Action<Collider2D, float> OnAttackHit;
        
        // /// <summary>
        // /// 攻击完成事件
        // /// </summary>
        // public event System.Action OnAttackComplete;
        // #endregion

        // #region 属性
        // /// <summary>
        // /// 是否可以攻击
        // /// </summary>
        // public bool CanAttack
        // {
        //     get { return _currentCooldown <= 0f; }
        // }
        
        // /// <summary>
        // /// 攻击范围
        // /// </summary>
        // public float AttackRange
        // {
        //     get { return _attackRange; }
        //     set { _attackRange = Mathf.Max(0f, value); }
        // }
        
        // /// <summary>
        // /// 攻击冷却时间
        // /// </summary>
        // public float AttackCooldown
        // {
        //     get { return _attackCooldown; }
        //     set { _attackCooldown = Mathf.Max(0f, value); }
        // }
        
        // /// <summary>
        // /// 攻击帧时间点
        // /// </summary>
        // public float AttackFrameTime
        // {
        //     get { return _attackFrameTime; }
        // }
        // #endregion

        // #region 初始化
        // /// <summary>
        // /// 初始化攻击控制器
        // /// </summary>
        // /// <param name="playerController">玩家控制器引用</param>
        // public void Initialize(PlayerController playerController)
        // {
        //     _playerController = playerController;
        //     _animator = playerController.GetComponent<Animator>();
            
        //     Log.Info(LOG_MODULE, "AttackController initialized", this);
        // }
        // #endregion

        // #region 更新
        // /// <summary>
        // /// 更新攻击控制器
        // /// </summary>
        // private void Update()
        // {
        //     // 更新冷却时间
        //     if (_currentCooldown > 0f)
        //     {
        //         _currentCooldown -= Time.deltaTime;
        //     }
        // }
        // #endregion

        // #region 攻击控制
        // /// <summary>
        // /// 执行攻击
        // /// </summary>
        // public void PerformAttack()
        // {
        //     if (!CanAttack || _playerController == null)
        //         return;
                
        //     // 开始冷却
        //     _currentCooldown = _attackCooldown;
            
        //     // 播放攻击动画
        //     if (_animator != null)
        //     {
        //         _animator.SetTrigger("Attack");
        //     }
            
        //     // 启动攻击帧检测协程
        //     StartCoroutine(AttackFrameDetection());
            
        //     Log.Info(LOG_MODULE, "Attack performed", this);
        // }
        
        // /// <summary>
        // /// 攻击帧检测
        // /// 在攻击动画的特定帧执行伤害检测
        // /// </summary>
        // /// <returns>协程迭代器</returns>
        // private System.Collections.IEnumerator AttackFrameDetection()
        // {
        //     // 等待到攻击帧时间点
        //     yield return new WaitForSeconds(_attackFrameTime);
            
        //     // 获取当前朝向
        //     Vector2 facingDirection = _playerController.AnimationSystem.LastFacingDirection;
            
        //     // 计算攻击检测位置
        //     Vector2 attackPosition = (Vector2)transform.position + facingDirection * (_attackRange / 2f);
            
        //     // 检测攻击范围内的敌人
        //     Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPosition, _attackRange);
            
        //     // 计算实际攻击力
        //     float actualAttackPower = _baseAttackPower * _attackMultiplier;
            
        //     // 处理命中的敌人
        //     foreach (Collider2D collider in hitColliders)
        //     {
        //         // 忽略自身
        //         if (collider.gameObject == gameObject)
        //             continue;
                    
        //         // 检查是否是可伤害对象
        //         IDamageable damageable = collider.GetComponent<IDamageable>();
        //         if (damageable != null)
        //         {
        //             // 造成伤害
        //             damageable.TakeDamage(actualAttackPower, transform);
                    
        //             // 触发攻击命中事件
        //             OnAttackHit?.Invoke(collider, actualAttackPower);
                    
        //             Log.Info(LOG_MODULE, $"Attacked {collider.gameObject.name} for {actualAttackPower} damage", this);
        //         }
        //     }
            
        //     // 攻击完成
        //     OnAttackComplete?.Invoke();
        // }
        // #endregion

        // #region 攻击设置
        // /// <summary>
        // /// 设置攻击伤害倍数
        // /// </summary>
        // /// <param name="multiplier">伤害倍数</param>
        // public void SetAttackMultiplier(float multiplier)
        // {
        //     _attackMultiplier = Mathf.Max(0f, multiplier);
        // }
        
        // /// <summary>
        // /// 设置基础攻击力
        // /// </summary>
        // /// <param name="power">基础攻击力</param>
        // public void SetBaseAttackPower(float power)
        // {
        //     _baseAttackPower = Mathf.Max(0f, power);
        // }
        
        // /// <summary>
        // /// 重置冷却时间
        // /// </summary>
        // public void ResetCooldown()
        // {
        //     _currentCooldown = 0f;
        // }
        // #endregion

        // #region 调试
        // /// <summary>
        // /// 调试绘制攻击范围
        // /// </summary>
        // private void OnDrawGizmosSelected()
        // {
        //     // 获取当前朝向
        //     Vector2 facingDirection = Vector2.down; // 默认朝下
            
        //     if (Application.isPlaying && _playerController != null)
        //     {
        //         facingDirection = _playerController.AnimationSystem.LastFacingDirection;
        //     }
            
        //     // 计算攻击检测位置
        //     Vector2 attackPosition = (Vector2)transform.position + facingDirection * (_attackRange / 2f);
            
        //     // 绘制攻击范围
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawWireSphere(attackPosition, _attackRange);
        // }
        //#endregion
    }
}