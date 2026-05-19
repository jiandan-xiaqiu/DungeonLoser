using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;
using MyGame.Character.Effect;
using SimpleOrbital;

public class CharacterManager : Singleton<CharacterManager>
{
    #region 字段
    private CharacterRuntimeData _characterRuntimeData;
    
    [Header("系统引用")]
    [SerializeField] private WeaponOrbitalSystem weaponOrbitalSystem;
    #endregion
    
    #region 属性
    public float MoveSpeed { get { return _characterRuntimeData.MoveSpeed; } set { _characterRuntimeData.MoveSpeed = value; } }
    public float HpMax { get { return _characterRuntimeData.HpMax; } set { _characterRuntimeData.HpMax = value; } }
    public float CurrentHP { get { return _characterRuntimeData.CurrentHP; } set { _characterRuntimeData.CurrentHP = Mathf.Clamp(value, 0, _characterRuntimeData.HpMax); } }
    public int Gold { get { return _characterRuntimeData.Gold; } set { _characterRuntimeData.Gold = value; } }
    public float AttackSpeed { get { return _characterRuntimeData.AttackSpeed; } set { _characterRuntimeData.AttackSpeed = value; } }
    
    // 护盾属性
    public float ShieldAmount { get { return _characterRuntimeData.ShieldAmount; } }
    public float ShieldMaxAmount { get { return _characterRuntimeData.ShieldMaxAmount; } }
    public float ShieldRemainingTime { get { return _characterRuntimeData.ShieldRemainingTime; } }
    public float ShieldBonusAmount { get { return _characterRuntimeData.ShieldBonusAmount; } }
    #endregion
    
    /// <summary>
    /// 初始化角色管理器
    /// </summary>
    public void Initialize()
    {
        _characterRuntimeData = new CharacterRuntimeData();
    }

    void Awake()
    {
        Initialize();
    }
    
    #region 公共方法
    /// <summary>
    /// 添加金币
    /// </summary>
    /// <param name="amount">金币数量</param>
    public void AddGold(int amount)
    {
        _characterRuntimeData.Gold += amount;
    }
    
    /// <summary>
    /// 消耗金币
    /// </summary>
    /// <param name="amount">金币数量</param>
    /// <returns>是否成功消耗</returns>
    public bool SpendGold(int amount)
    {
        if (_characterRuntimeData.Gold >= amount)
        {
            _characterRuntimeData.Gold -= amount;
            return true;
        }
        return false;
    }
    
    /// <summary>
    /// 恢复生命值
    /// </summary>
    /// <param name="amount">回血量</param>
    public void Heal(float amount)
    {
        _characterRuntimeData.CurrentHP = Mathf.Min(_characterRuntimeData.CurrentHP + amount, _characterRuntimeData.HpMax);
        //Debug.Log($"恢复{amount}点生命值，当前生命值：{_characterRuntimeData.CurrentHP}");
    }
    
    /// <summary>
    /// 受到伤害（护盾优先抵挡）
    /// </summary>
    /// <param name="damage">伤害值</param>
    public void TakeDamage(float damage)
    {
        // 如果有护盾，先消耗护盾
        if (_characterRuntimeData.ShieldAmount > 0f)
        {
            // 护盾完全抵挡伤害（可以抵挡溢出伤害）
            _characterRuntimeData.ShieldAmount -= damage;

            // 触发护盾闪烁效果
            TriggerShieldFlash();

            // 如果护盾被耗尽，重置护盾状态
            if (_characterRuntimeData.ShieldAmount <= 0f)
            {
                _characterRuntimeData.ShieldAmount = 0f;
                _characterRuntimeData.ShieldMaxAmount = 0f;
                _characterRuntimeData.ShieldDuration = 0f;
                _characterRuntimeData.ShieldRemainingTime = 0f;
            }
        }
        else
        {
            // 没有护盾，直接扣血
            _characterRuntimeData.CurrentHP = Mathf.Max(_characterRuntimeData.CurrentHP - damage, 0f);
        }
    }

    /// <summary>
    /// 触发护盾闪烁效果
    /// </summary>
    private void TriggerShieldFlash()
    {
        ShieldEffect shieldEffect = GetComponentInChildren<ShieldEffect>();
        if (shieldEffect != null)
        {
            shieldEffect.TriggerHitFlash();
        }
    }
    
    /// <summary>
    /// 增加血量上限
    /// </summary>
    /// <param name="amount">增加量</param>
    public void IncreaseHpMax(float amount)
    {
        _characterRuntimeData.HpMax += amount;
        _characterRuntimeData.CurrentHP = Mathf.Min(_characterRuntimeData.CurrentHP, _characterRuntimeData.HpMax);
    }
    
    /// <summary>
    /// 增加移速
    /// </summary>
    /// <param name="amount">增加量</param>
    public void IncreaseMoveSpeed(float amount)
    {
        _characterRuntimeData.MoveSpeed += amount;
    }
    
    /// <summary>
    /// 增加攻击速度
    /// </summary>
    /// <param name="amount">增加量</param>
    public void IncreaseAttackSpeed(float amount)
    {
        _characterRuntimeData.AttackSpeed += amount;
    }
    
    /// <summary>
    /// 增加护盾
    /// </summary>
    /// <param name="amount">护盾值</param>
    /// <param name="duration">持续时间（秒）</param>
    public void AddShield(float amount, float duration)
    {
        // 设置护盾最大值和当前值（加上额外获取量）
        float totalAmount = amount + _characterRuntimeData.ShieldBonusAmount;
        _characterRuntimeData.ShieldAmount = totalAmount;
        _characterRuntimeData.ShieldMaxAmount = totalAmount;
        _characterRuntimeData.ShieldDuration = duration;
        _characterRuntimeData.ShieldRemainingTime = duration;
    }
    
    /// <summary>
    /// 增加武器数量
    /// </summary>
    public void IncreaseWeaponCount()
    {
        if (weaponOrbitalSystem != null)
        {
            weaponOrbitalSystem.AddWeaponCopy();
        }
        else
        {
            Debug.LogWarning("WeaponOrbitalSystem 未设置，无法增加武器数量");
        }
    }
    
    /// <summary>
    /// 增加护盾额外获取量
    /// </summary>
    /// <param name="amount">增加量</param>
    public void IncreaseShieldBonus(float amount)
    {
        _characterRuntimeData.ShieldBonusAmount += amount;
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        // 更新护盾衰减
        UpdateShield();
    }
    
    /// <summary>
    /// 更新护盾衰减逻辑
    /// </summary>
    private void UpdateShield()
    {
        // 如果有护盾且时间未结束
        if (_characterRuntimeData.ShieldAmount > 0f && _characterRuntimeData.ShieldRemainingTime > 0f)
        {
            // 减少剩余时间
            _characterRuntimeData.ShieldRemainingTime -= Time.deltaTime;
            
            // 计算衰减后的护盾值（线性衰减）
            if (_characterRuntimeData.ShieldDuration > 0f)
            {
                float decayRate = _characterRuntimeData.ShieldMaxAmount / _characterRuntimeData.ShieldDuration;
                _characterRuntimeData.ShieldAmount = Mathf.Max(0f, _characterRuntimeData.ShieldAmount - decayRate * Time.deltaTime);
            }
            
            // 如果时间结束或护盾值为0，重置护盾状态
            if (_characterRuntimeData.ShieldRemainingTime <= 0f || _characterRuntimeData.ShieldAmount <= 0f)
            {
                _characterRuntimeData.ShieldAmount = 0f;
                _characterRuntimeData.ShieldMaxAmount = 0f;
                _characterRuntimeData.ShieldDuration = 0f;
                _characterRuntimeData.ShieldRemainingTime = 0f;
            }
        }
    }
}
