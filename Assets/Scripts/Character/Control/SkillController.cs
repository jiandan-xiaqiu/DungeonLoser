using UnityEngine;
using System.Collections.Generic;
using Logger;

namespace MyGame.Character.Control
{
    /// <summary>
    /// 技能控制器，负责角色的技能逻辑实现
    /// 处理技能冷却、资源消耗、技能效果等具体技能操作
    /// </summary>
    // public class SkillController : MonoBehaviour
    // {
    //     #region 技能数据结构
    //     /// <summary>
    //     /// 技能数据类
    //     /// </summary>
    //     [System.Serializable]
    //     public class SkillData
    //     {
    //         [Tooltip("技能名称")]
    //         public string SkillName;
            
    //         [Tooltip("技能描述")]
    //         public string SkillDescription;
            
    //         [Tooltip("技能ID")]
    //         public int SkillID;
            
    //         [Tooltip("技能冷却时间")]
    //         public float CooldownTime;
            
    //         [Tooltip("当前冷却时间")]
    //         public float CurrentCooldown;
            
    //         [Tooltip("技能消耗")]
    //         public int ManaCost;
            
    //         [Tooltip("技能等级")]
    //         public int SkillLevel;
            
    //         [Tooltip("技能伤害")]
    //         public float SkillDamage;
            
    //         [Tooltip("技能范围")]
    //         public float SkillRange;
            
    //         [Tooltip("技能是否可用")]
    //         public bool IsUnlocked;
    //     }
    //     #endregion

    //     #region 字段
    //     private PlayerController _playerController;
    //     private Animator _animator;
        
    //     [Header("技能设置")]
    //     [Tooltip("技能列表")]
    //     [SerializeField] private List<SkillData> _skills = new();
        
    //     [Tooltip("当前选中的技能")]
    //     private int _currentSelectedSkill = 0;
        
    //     [Tooltip("技能动画参数前缀")]
    //     private const string _skillAnimationPrefix = "Skill";
        
    //     private static readonly string LOG_MODULE = LogModules.PLAYER;
    //     #endregion

    //     #region 事件
    //     /// <summary>
    //     /// 技能释放事件
    //     /// </summary>
    //     public event System.Action<int, Vector3> OnSkillCast;
        
    //     /// <summary>
    //     /// 技能命中事件
    //     /// </summary>
    //     public event System.Action<int, Collider2D[], float> OnSkillHit;
        
    //     /// <summary>
    //     /// 技能冷却完成事件
    //     /// </summary>
    //     public event System.Action<int> OnSkillCooldownComplete;
        
    //     /// <summary>
    //     /// 技能释放失败事件（如资源不足、冷却中）
    //     /// </summary>
    //     public event System.Action<int, string> OnSkillCastFailed;
    //     #endregion

    //     #region 属性
    //     /// <summary>
    //     /// 技能列表
    //     /// </summary>
    //     public List<SkillData> Skills
    //     {
    //         get { return _skills; }
    //     }
        
    //     /// <summary>
    //     /// 当前选中的技能
    //     /// </summary>
    //     public int CurrentSelectedSkill
    //     {
    //         get { return _currentSelectedSkill; }
    //         set { _currentSelectedSkill = Mathf.Clamp(value, 0, _skills.Count - 1); }
    //     }
    //     #endregion

    //     #region 初始化
    //     /// <summary>
    //     /// 初始化技能控制器
    //     /// </summary>
    //     /// <param name="playerController">玩家控制器引用</param>
    //     public void Initialize(PlayerController playerController)
    //     {
    //         _playerController = playerController;
    //         _animator = playerController.GetComponent<Animator>();
            
    //         // 初始化技能
    //         InitializeSkills();
            
    //         Log.Info(LOG_MODULE, "SkillController initialized", this);
    //     }
        
    //     /// <summary>
    //     /// 初始化技能
    //     /// </summary>
    //     private void InitializeSkills()
    //     {
    //         foreach (SkillData skill in _skills)
    //         {
    //             // 确保技能数据的初始状态正确
    //             skill.CurrentCooldown = 0f;
    //         }
    //     }
    //     #endregion

    //     #region 更新
    //     /// <summary>
    //     /// 更新技能控制器
    //     /// </summary>
    //     private void Update()
    //     {
    //         // 更新所有技能的冷却时间
    //         UpdateSkillCooldowns();
    //     }
        
    //     /// <summary>
    //     /// 更新技能冷却时间
    //     /// </summary>
    //     private void UpdateSkillCooldowns()
    //     {
    //         for (int i = 0; i < _skills.Count; i++)
    //         {
    //             SkillData skill = _skills[i];
    //             if (skill.CurrentCooldown > 0f)
    //             {
    //                 skill.CurrentCooldown -= Time.deltaTime;
                    
    //                 // 检查冷却是否完成
    //                 if (skill.CurrentCooldown <= 0f)
    //                 {
    //                     skill.CurrentCooldown = 0f;
    //                     OnSkillCooldownComplete?.Invoke(i);
    //                     Log.Info(LOG_MODULE, $"Skill {skill.SkillName} cooldown completed", this);
    //                 }
    //             }
    //         }
    //     }
    //     #endregion

    //     #region 技能控制
    //     /// <summary>
    //     /// 释放技能
    //     /// </summary>
    //     /// <param name="skillIndex">技能索引</param>
    //     /// <param name="targetPosition">技能目标位置</param>
    //     public void CastSkill(int skillIndex, Vector3 targetPosition = default)
    //     {
    //         // 检查技能索引是否有效
    //         if (skillIndex < 0 || skillIndex >= _skills.Count)
    //         {
    //             Log.Error(LOG_MODULE, $"Invalid skill index: {skillIndex}", this);
    //             return;
    //         }
            
    //         SkillData skill = _skills[skillIndex];
            
    //         // 检查技能是否可用
    //         if (!skill.IsUnlocked)
    //         {
    //             OnSkillCastFailed?.Invoke(skillIndex, "技能未解锁");
    //             Log.Warning(LOG_MODULE, $"Skill {skill.SkillName} is not unlocked", this);
    //             return;
    //         }
            
    //         // 检查技能冷却
    //         if (skill.CurrentCooldown > 0f)
    //         {
    //             OnSkillCastFailed?.Invoke(skillIndex, "技能冷却中");
    //             Log.Warning(LOG_MODULE, $"Skill {skill.SkillName} is on cooldown", this);
    //             return;
    //         }
            
    //         // 检查资源消耗（这里假设消耗MP）
    //         if (_playerController.HealthSystem != null)
    //         {
    //             // TODO: 需要在HealthSystem中添加MP管理
    //             // if (_playerController.HealthSystem.MP < skill.ManaCost)
    //             // {
    //             //     OnSkillCastFailed?.Invoke(skillIndex, "魔法值不足");
    //             //     Log.Warning(LOG_MODULE, $"Not enough MP for skill {skill.SkillName}", this);
    //             //     return;
    //             // }
    //         }
            
    //         // 设置技能冷却
    //         skill.CurrentCooldown = skill.CooldownTime;
            
    //         // 消耗资源
    //         // _playerController.HealthSystem.SpendMP(skill.ManaCost);
            
    //         // 播放技能动画
    //         if (_animator != null)
    //         {
    //             string animationTriggerName = $"{_skillAnimationPrefix}{skillIndex + 1}";
    //             _animator.SetTrigger(animationTriggerName);
    //         }
            
    //         // 触发技能释放事件
    //         OnSkillCast?.Invoke(skillIndex, targetPosition);
            
    //         Log.Info(LOG_MODULE, $"Skill {skill.SkillName} cast", this);
            
    //         // 启动技能效果协程
    //         StartCoroutine(ExecuteSkillEffect(skillIndex, targetPosition));
    //     }
        
    //     /// <summary>
    //     /// 执行技能效果
    //     /// </summary>
    //     /// <param name="skillIndex">技能索引</param>
    //     /// <param name="targetPosition">目标位置</param>
    //     /// <returns>协程迭代器</returns>
    //     private System.Collections.IEnumerator ExecuteSkillEffect(int skillIndex, Vector3 targetPosition)
    //     {
    //         SkillData skill = _skills[skillIndex];
            
    //         // 根据技能索引执行不同的技能效果
    //         switch (skillIndex)
    //         {
    //             case 0:
    //                 // 示例：扇形范围攻击
    //                 yield return new WaitForSeconds(0.3f); // 等待技能动画的攻击帧
    //                 yield return ExecuteFanAttack(skill);
    //                 break;
                    
    //             case 1:
    //                 // 示例：直线范围攻击
    //                 yield return new WaitForSeconds(0.3f);
    //                 yield return ExecuteLineAttack(skill);
    //                 break;
                    
    //             case 2:
    //                 // 示例：圆形范围攻击
    //                 yield return new WaitForSeconds(0.3f);
    //                 yield return ExecuteCircleAttack(skill);
    //                 break;
                    
    //             default:
    //                 // 默认技能效果
    //                 yield return new WaitForSeconds(0.3f);
    //                 yield return ExecuteDefaultAttack(skill);
    //                 break;
    //         }
    //     }
        
    //     /// <summary>
    //     /// 执行扇形范围攻击
    //     /// </summary>
    //     /// <param name="skill">技能数据</param>
    //     /// <returns>协程迭代器</returns>
    //     private System.Collections.IEnumerator ExecuteFanAttack(SkillData skill)
    //     {
    //         // 获取当前朝向
    //         Vector2 facingDirection = _playerController.AnimationSystem.LastFacingDirection;
            
    //         // 计算扇形攻击参数
    //         int rayCount = 8;
    //         float angle = 45f;
    //         float stepAngle = angle / (rayCount - 1);
            
    //         // 存储命中的敌人
    //         List<Collider2D> hitEnemies = new();
            
    //         // 执行扇形射线检测
    //         for (int i = 0; i < rayCount; i++)
    //         {
    //             float currentAngle = (-angle / 2f) + (stepAngle * i);
    //             Vector2 rayDirection = Quaternion.Euler(0, 0, currentAngle) * facingDirection;
                
    //             RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, skill.SkillRange);
                
    //             // 绘制射线（调试用）
    //             Debug.DrawRay(transform.position, rayDirection * skill.SkillRange, Color.red, 0.5f);
                
    //             if (hit.collider != null)
    //             {
    //                 // 检查是否是敌人
    //                 if (hit.collider.CompareTag("Enemy"))
    //                 {
    //                     // 避免重复添加同一敌人
    //                     if (!hitEnemies.Contains(hit.collider))
    //                     {
    //                         hitEnemies.Add(hit.collider);
    //                     }
    //                 }
    //             }
                
    //             yield return null;
    //         }
            
    //         // 处理命中的敌人
    //         if (hitEnemies.Count > 0)
    //         {
    //             OnSkillHit?.Invoke(skill.SkillID, hitEnemies.ToArray(), skill.SkillDamage);
    //         }
    //     }
        
    //     /// <summary>
    //     /// 执行直线范围攻击
    //     /// </summary>
    //     /// <param name="skill">技能数据</param>
    //     /// <returns>协程迭代器</returns>
    //     private System.Collections.IEnumerator ExecuteLineAttack(SkillData skill)
    //     {
    //         // 获取当前朝向
    //         Vector2 facingDirection = _playerController.AnimationSystem.LastFacingDirection;
            
    //         // 计算直线攻击的矩形
    //         Vector2 lineDirection = facingDirection;
    //         Vector2 lineCenter = (Vector2)transform.position + lineDirection * (skill.SkillRange / 2f);
    //         Vector2 lineSize = new Vector2(skill.SkillRange, 0.5f);
            
    //         // 绘制攻击范围（调试用）
    //         Debug.DrawLine(lineCenter - lineDirection * (skill.SkillRange / 2f), lineCenter + lineDirection * (skill.SkillRange / 2f), Color.red, 0.5f);
            
    //         // 检测直线范围内的敌人
    //         Collider2D[] hitColliders = Physics2D.OverlapBoxAll(lineCenter, lineSize, 0f);
            
    //         // 处理命中的敌人
    //         if (hitColliders.Length > 0)
    //         {
    //             OnSkillHit?.Invoke(skill.SkillID, hitColliders, skill.SkillDamage);
    //         }
            
    //         yield return null;
    //     }
        
    //     /// <summary>
    //     /// 执行圆形范围攻击
    //     /// </summary>
    //     /// <param name="skill">技能数据</param>
    //     /// <returns>协程迭代器</returns>
    //     private System.Collections.IEnumerator ExecuteCircleAttack(SkillData skill)
    //     {
    //         // 计算圆形攻击的中心
    //         Vector2 circleCenter = (Vector2)transform.position;
            
    //         // 绘制攻击范围（调试用）
    //         Debug.DrawWireSphere(circleCenter, skill.SkillRange, Color.red, 0.5f);
            
    //         // 检测圆形范围内的敌人
    //         Collider2D[] hitColliders = Physics2D.OverlapCircleAll(circleCenter, skill.SkillRange);
            
    //         // 处理命中的敌人
    //         if (hitColliders.Length > 0)
    //         {
    //             OnSkillHit?.Invoke(skill.SkillID, hitColliders, skill.SkillDamage);
    //         }
            
    //         yield return null;
    //     }
        
    //     /// <summary>
    //     /// 执行默认攻击
    //     /// </summary>
    //     /// <param name="skill">技能数据</param>
    //     /// <returns>协程迭代器</returns>
    //     private System.Collections.IEnumerator ExecuteDefaultAttack(SkillData skill)
    //     {
    //         // 获取当前朝向
    //         Vector2 facingDirection = _playerController.AnimationSystem.LastFacingDirection;
            
    //         // 计算攻击位置
    //         Vector2 attackPosition = (Vector2)transform.position + facingDirection * (skill.SkillRange / 2f);
            
    //         // 绘制攻击范围（调试用）
    //         Debug.DrawWireSphere(attackPosition, skill.SkillRange, Color.red, 0.5f);
            
    //         // 检测攻击范围内的敌人
    //         Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPosition, skill.SkillRange);
            
    //         // 处理命中的敌人
    //         if (hitColliders.Length > 0)
    //         {
    //             OnSkillHit?.Invoke(skill.SkillID, hitColliders, skill.SkillDamage);
    //         }
            
    //         yield return null;
    //     }
        
    //     /// <summary>
    //     /// 解锁技能
    //     /// </summary>
    //     /// <param name="skillIndex">技能索引</param>
    //     public void UnlockSkill(int skillIndex)
    //     {
    //         if (skillIndex < 0 || skillIndex >= _skills.Count)
    //         {
    //             Log.Error(LOG_MODULE, $"Invalid skill index: {skillIndex}", this);
    //             return;
    //         }
            
    //         _skills[skillIndex].IsUnlocked = true;
    //         Log.Info(LOG_MODULE, $"Skill {_skills[skillIndex].SkillName} unlocked", this);
    //     }
        
    //     /// <summary>
    //     /// 升级技能
    //     /// </summary>
    //     /// <param name="skillIndex">技能索引</param>
    //     public void UpgradeSkill(int skillIndex)
    //     {
    //         if (skillIndex < 0 || skillIndex >= _skills.Count)
    //         {
    //             Log.Error(LOG_MODULE, $"Invalid skill index: {skillIndex}", this);
    //             return;
    //         }
            
    //         SkillData skill = _skills[skillIndex];
    //         skill.SkillLevel++;
            
    //         // 提升技能属性
    //         skill.SkillDamage *= 1.2f; // 伤害提升20%
    //         skill.SkillRange *= 1.1f; // 范围提升10%
            
    //         Log.Info(LOG_MODULE, $"Skill {skill.SkillName} upgraded to level {skill.SkillLevel}", this);
    //     }
    //     #endregion

    //     #region 辅助方法
    //     /// <summary>
    //     /// 获取技能剩余冷却时间
    //     /// </summary>
    //     /// <param name="skillIndex">技能索引</param>
    //     /// <returns>剩余冷却时间</returns>
    //     public float GetSkillRemainingCooldown(int skillIndex)
    //     {
    //         if (skillIndex < 0 || skillIndex >= _skills.Count)
    //         {
    //             Log.Error(LOG_MODULE, $"Invalid skill index: {skillIndex}", this);
    //             return 0f;
    //         }
            
    //         return _skills[skillIndex].CurrentCooldown;
    //     }
        
    //     /// <summary>
    //     /// 检查技能是否可以释放
    //     /// </summary>
    //     /// <param name="skillIndex">技能索引</param>
    //     /// <returns>是否可以释放</returns>
    //     public bool CanCastSkill(int skillIndex)
    //     {
    //         if (skillIndex < 0 || skillIndex >= _skills.Count)
    //         {
    //             return false;
    //         }
            
    //         SkillData skill = _skills[skillIndex];
            
    //         return skill.IsUnlocked && skill.CurrentCooldown <= 0f;
    //     }
    //     #endregion
    // }
}