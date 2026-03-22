using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Character.Systems
{
    /// <summary>
    /// 数值系统，管理角色的属性
    /// </summary>
    public class StatSystem : MonoBehaviour                                    
    {
        #region 字段
        [Tooltip("基础属性")]
        private BaseStats _baseStats = new BaseStats();

        [Tooltip("战斗属性")]
        private CombatStats _combatStats = new CombatStats();

        [Tooltip("装备加成")]
        private StatModifiers _equipmentModifiers = new StatModifiers();

        [Tooltip("Buff加成")]
        private StatModifiers _buffModifiers = new StatModifiers();
        #endregion

        #region 属性
        /// <summary>
        /// 基础属性
        /// </summary>
        public BaseStats BaseStats
        {
            get { return _baseStats; }
            set { _baseStats = value; UpdateCombatStats(); }
        }

        /// <summary>
        /// 战斗属性
        /// </summary>
        public CombatStats CombatStats
        {
            get { return _combatStats; }
        }
        #endregion

        #region 初始化
        /// <summary>
        /// 初始化数值系统
        /// </summary>
        public void Initialize()
        {
            UpdateCombatStats();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 更新战斗属性
        /// </summary>
        public void UpdateCombatStats()
        {
            // 计算基础战斗属性
            _combatStats.AttackPower = _baseStats.Strength * 1.5f + _baseStats.Agility * 0.5f;
            _combatStats.Defense = _baseStats.Vitality * 1.2f + _baseStats.Strength * 0.3f;
            _combatStats.MagicPower = _baseStats.Intelligence * 1.5f + _baseStats.Wisdom * 0.5f;
            _combatStats.Speed = _baseStats.Agility * 0.8f + _baseStats.Strength * 0.2f;

            // 应用装备加成
            _combatStats.AttackPower += _equipmentModifiers.AttackPower;
            _combatStats.Defense += _equipmentModifiers.Defense;
            _combatStats.MagicPower += _equipmentModifiers.MagicPower;
            _combatStats.Speed += _equipmentModifiers.Speed;

            // 应用Buff加成
            _combatStats.AttackPower *= (1 + _buffModifiers.AttackPowerMultiplier);
            _combatStats.Defense *= (1 + _buffModifiers.DefenseMultiplier);
            _combatStats.MagicPower *= (1 + _buffModifiers.MagicPowerMultiplier);
            _combatStats.Speed *= (1 + _buffModifiers.SpeedMultiplier);

            // 计算元素伤害加成
            _combatStats.ElementalDamageBonus = _baseStats.Intelligence * 0.1f + _equipmentModifiers.ElementalDamageBonus + _buffModifiers.ElementalDamageBonus;
        }

        /// <summary>
        /// 设置装备加成
        /// </summary>
        /// <param name="modifiers">装备加成</param>
        public void SetEquipmentModifiers(StatModifiers modifiers)
        {
            _equipmentModifiers = modifiers;
            UpdateCombatStats();
        }

        /// <summary>
        /// 添加Buff加成
        /// </summary>
        /// <param name="modifiers">Buff加成</param>
        public void AddBuffModifiers(StatModifiers modifiers)
        {
            _buffModifiers.AttackPower += modifiers.AttackPower;
            _buffModifiers.Defense += modifiers.Defense;
            _buffModifiers.MagicPower += modifiers.MagicPower;
            _buffModifiers.Speed += modifiers.Speed;
            _buffModifiers.AttackPowerMultiplier += modifiers.AttackPowerMultiplier;
            _buffModifiers.DefenseMultiplier += modifiers.DefenseMultiplier;
            _buffModifiers.MagicPowerMultiplier += modifiers.MagicPowerMultiplier;
            _buffModifiers.SpeedMultiplier += modifiers.SpeedMultiplier;
            _buffModifiers.ElementalDamageBonus += modifiers.ElementalDamageBonus;
            UpdateCombatStats();
        }

        /// <summary>
        /// 移除Buff加成
        /// </summary>
        /// <param name="modifiers">Buff加成</param>
        public void RemoveBuffModifiers(StatModifiers modifiers)
        {
            _buffModifiers.AttackPower -= modifiers.AttackPower;
            _buffModifiers.Defense -= modifiers.Defense;
            _buffModifiers.MagicPower -= modifiers.MagicPower;
            _buffModifiers.Speed -= modifiers.Speed;
            _buffModifiers.AttackPowerMultiplier -= modifiers.AttackPowerMultiplier;
            _buffModifiers.DefenseMultiplier -= modifiers.DefenseMultiplier;
            _buffModifiers.MagicPowerMultiplier -= modifiers.MagicPowerMultiplier;
            _buffModifiers.SpeedMultiplier -= modifiers.SpeedMultiplier;
            _buffModifiers.ElementalDamageBonus -= modifiers.ElementalDamageBonus;
            UpdateCombatStats();
        }
        #endregion
    }

    /// <summary>
    /// 基础属性
    /// </summary>
    [System.Serializable]
    public class BaseStats
    {
        [Tooltip("力量")]
        public float Strength = 10f;

        [Tooltip("敏捷")]
        public float Agility = 10f;

        [Tooltip("体力")]
        public float Vitality = 10f;

        [Tooltip("智力")]
        public float Intelligence = 10f;

        [Tooltip("智慧")]
        public float Wisdom = 10f;
    }

    /// <summary>
    /// 战斗属性
    /// </summary>
    [System.Serializable]
    public class CombatStats
    {
        [Tooltip("攻击力")]
        public float AttackPower = 0f;

        [Tooltip("防御力")]
        public float Defense = 0f;

        [Tooltip("魔法力")]
        public float MagicPower = 0f;

        [Tooltip("速度")]
        public float Speed = 0f;

        [Tooltip("元素伤害加成")]
        public float ElementalDamageBonus = 0f;
    }

    /// <summary>
    /// 属性加成
    /// </summary>
    [System.Serializable]
    public class StatModifiers
    {
        [Tooltip("攻击力加成")]
        public float AttackPower = 0f;

        [Tooltip("防御力加成")]
        public float Defense = 0f;

        [Tooltip("魔法力加成")]
        public float MagicPower = 0f;

        [Tooltip("速度加成")]
        public float Speed = 0f;

        [Tooltip("攻击力倍数加成")]
        public float AttackPowerMultiplier = 0f;

        [Tooltip("防御力倍数加成")]
        public float DefenseMultiplier = 0f;

        [Tooltip("魔法力倍数加成")]
        public float MagicPowerMultiplier = 0f;

        [Tooltip("速度倍数加成")]
        public float SpeedMultiplier = 0f;

        [Tooltip("元素伤害加成")]
        public float ElementalDamageBonus = 0f;
    }
}