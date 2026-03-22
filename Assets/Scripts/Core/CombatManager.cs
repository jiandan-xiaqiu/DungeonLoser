using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;
using MyGame.Character.Systems;

public class CombatManager : Singleton<CombatManager>
{
    #region 字段
    private CombatManager _combatManager;
    #endregion

    // 子系统
    [SerializeField] private WeaponSystem weaponSystem;
    [SerializeField] private SkillSystem skillSystem;
    [SerializeField] private StatSystem statSystem;
    [SerializeField] private BuffSystem buffSystem;
    
    // 事件系统
    public CombatEvents events;
    
    void InitializeSystems()
    {
        // 确保所有系统都存在
        if (weaponSystem == null) weaponSystem = GetComponent<WeaponSystem>();
        if (skillSystem == null) skillSystem = GetComponent<SkillSystem>();
        if (statSystem == null) statSystem = GetComponent<StatSystem>();
        if (buffSystem == null) buffSystem = GetComponent<BuffSystem>();
        
        // 初始化事件系统
        events = new CombatEvents();
    }
    
    // 发起攻击
    // public void PerformAttack(CharacterStats attacker, AttackData attackData)
    // {
    //     // 验证攻击者
    //     if (attacker == null || attackData == null) return;
        
    //     // 触发攻击前事件
    //     events.OnAttackStart?.Invoke(attacker, attackData);
        
    //     // 执行攻击逻辑
    //     // 这里可以添加攻击前检查（如是否被眩晕等）
        
    //     // 触发攻击后事件
    //     events.OnAttackComplete?.Invoke(attacker, attackData);
    // }
    
    // // 施放技能
    // public void CastSkill(CharacterStats caster, string skillId)
    // {
    //     SkillBase skill = skillSystem.GetSkill(skillId);
    //     if (skill == null) return;
        
    //     // 触发技能施放事件
    //     events.OnSkillCastStart?.Invoke(caster, skill);
        
    //     // 执行技能逻辑
    //     // ...
        
    //     // 触发技能完成事件
    //     events.OnSkillCastComplete?.Invoke(caster, skill);
    // }
}

// 战斗事件系统
public class CombatEvents
{
    // 攻击事件
    // public event System.Action<CharacterStats, AttackData> OnAttackStart;
    // public event System.Action<CharacterStats, AttackData> OnAttackComplete;
    // public event System.Action<DamageResult> OnDamageDealt;
    // public event System.Action<CharacterStats, float> OnHealthChanged;
    // public event System.Action<CharacterStats> OnCharacterDeath;
    
    // // 技能事件
    // public event System.Action<CharacterStats, SkillBase> OnSkillCastStart;
    // public event System.Action<CharacterStats, SkillBase> OnSkillCastComplete;
    
    // // 元素事件
    // public event System.Action<ElementReaction> OnElementReaction;
    
    // // Buff事件
    // public event System.Action<CharacterStats, BuffBase> OnBuffApplied;
    // public event System.Action<CharacterStats, string> OnBuffRemoved;
}
