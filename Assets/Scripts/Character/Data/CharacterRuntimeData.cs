using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 角色运行时数据
/// </summary>
public class CharacterRuntimeData
{
    // 角色移速
    public float MoveSpeed;
    // 血量上限
    public float HpMax;
    // 当前血量
    public float CurrentHP;
    // 金币数量
    public int Gold;
    // 攻击速度（武器转速）
    public float AttackSpeed;
    
    // 护盾相关
    public float ShieldAmount;      // 当前护盾值
    public float ShieldMaxAmount;   // 护盾最大值
    public float ShieldDuration;    // 护盾持续时间（秒）
    public float ShieldRemainingTime; // 护盾剩余时间
    public float ShieldBonusAmount; // 护盾额外获取量
    
    /// <summary>
    /// 初始化角色运行时数据
    /// </summary>
    public CharacterRuntimeData()
    {
        MoveSpeed = 5f;       // 默认移速
        HpMax = 100f;         // 默认血量上限
        CurrentHP = 100f;     // 默认当前血量
        Gold = 0;             // 默认金币数量
        AttackSpeed = 1f;     // 默认攻击速度
        
        ShieldAmount = 0f;        // 默认无护盾
        ShieldMaxAmount = 0f;     // 默认无护盾上限
        ShieldDuration = 0f;      // 默认无持续时间
        ShieldRemainingTime = 0f; // 默认无剩余时间
        ShieldBonusAmount = 0f;   // 默认护盾额外获取量
    }
}