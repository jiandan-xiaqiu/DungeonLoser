using UnityEngine;

namespace SimpleOrbital
{
    /// <summary>
    /// 武器类，定义武器的基本属性
    /// </summary>
    [System.Serializable]
    public class Weapon
    {
        [Header("基本属性")]
        public string weaponName = "Sword";
        public GameObject weaponPrefab; // 武器的视觉预制体
        public float damage = 10f; // 基础伤害
        public float attackRange = 0.5f; // 攻击范围
        public float orbitRadius = 1f; // 环绕半径
        public float orbitSpeed = 0.5f; // 环绕速度
        
        [Header("运行时属性")]
        [System.NonSerialized]
        public GameObject weaponInstance; // 武器实例
        [System.NonSerialized]
        public float currentAngle = 0f; // 当前角度
        [System.NonSerialized]
        public float lastAttackTime = 0f; // 上次攻击时间
        
        /// <summary>
        /// 检查是否可以攻击
        /// </summary>
        /// <returns>是否可以攻击</returns>
        public bool CanAttack()
        {
            return Time.time - lastAttackTime >= 0.5f; // 简单的攻击间隔
        }
        
        /// <summary>
        /// 执行攻击
        /// </summary>
        public void PerformAttack()
        {
            lastAttackTime = Time.time;
        }
    }
}
