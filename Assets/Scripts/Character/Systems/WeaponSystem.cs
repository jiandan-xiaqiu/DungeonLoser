using UnityEngine;

namespace MyGame.Character.Systems
{
    /// <summary>
    /// 武器系统，管理武器和武器相关的功能
    /// </summary>
    public class WeaponSystem : MonoBehaviour
    {
        #region 字段
        private Weapon _currentWeapon;
        #endregion

        #region 属性
        /// <summary>
        /// 当前武器
        /// </summary>
        public Weapon CurrentWeapon
        {
            get { return _currentWeapon; }
            set { _currentWeapon = value; }
        }
        #endregion

        #region 初始化
        /// <summary>
        /// 初始化武器系统
        /// </summary>
        public void Initialize()
        {
            // 初始化为默认武器
            _currentWeapon = new Weapon();
        }
        #endregion

        #region 武器方法
        /// <summary>
        /// 获取普通攻击信息
        /// </summary>
        /// <returns>普通攻击信息</returns>
        public AttackInfo GetNormalAttack()
        {
            return _currentWeapon?.NormalAttack ?? new AttackInfo();
        }

        /// <summary>
        /// 获取武器技能
        /// </summary>
        /// <returns>武器技能</returns>
        public Skill GetWeaponSkill()
        {
            return _currentWeapon?.WeaponSkill;
        }
        #endregion
    }

    /// <summary>
    /// 武器类
    /// </summary>
    [System.Serializable]
    public class Weapon
    {
        #region 字段
        [Tooltip("武器基础伤害")]
        public float BaseDamage = 10f;

        [Tooltip("武器元素类型")]
        public ElementType ElementType = ElementType.None;

        [Tooltip("普通攻击信息")]
        public AttackInfo NormalAttack = new AttackInfo();

        [Tooltip("武器技能")]
        public Skill WeaponSkill;
        #endregion
    }

    /// <summary>
    /// 普通攻击信息
    /// </summary>
    [System.Serializable]
    public class AttackInfo
    {
        [Tooltip("攻击动画名称")]
        public string AnimationName = "Attack";

        [Tooltip("攻击范围")]
        public float AttackRange = 1f;

        [Tooltip("攻击速度")]
        public float AttackSpeed = 1f;

        [Tooltip("攻击帧时间点")]
        public float AttackFrameTime = 0.3f;
    }

    /// <summary>
    /// 元素类型
    /// </summary>
    public enum ElementType
    {
        None,
        Fire,
        Water,
        Lightning,
        Ice,
        Wind,
        Earth,
        Light,
        Dark
    }
}
