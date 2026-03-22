using UnityEngine;
using System.Collections.Generic;

namespace MyGame.Character.Systems
{
    /// <summary>
    /// 技能系统，管理技能冷却和技能效果
    /// </summary>
    public class SkillSystem : MonoBehaviour
    {
        #region 字段
        private List<Skill> _skills = new List<Skill>();
        #endregion

        #region 初始化
        /// <summary>
        /// 初始化技能系统
        /// </summary>
        public void Initialize()
        {
            // 初始化技能列表
            _skills.Clear();
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新技能系统
        /// </summary>
        private void Update()
        {
            // 更新所有技能的冷却时间
            foreach (var skill in _skills)
            {
                if (skill.CurrentCooldown > 0)
                {
                    skill.CurrentCooldown -= Time.deltaTime;
                    if (skill.CurrentCooldown < 0)
                    {
                        skill.CurrentCooldown = 0;
                    }
                }
            }
        }
        #endregion

        #region 技能管理
        /// <summary>
        /// 添加技能
        /// </summary>
        /// <param name="skill">要添加的技能</param>
        public void AddSkill(Skill skill)
        {
            if (!_skills.Contains(skill))
            {
                _skills.Add(skill);
            }
        }

        /// <summary>
        /// 移除技能
        /// </summary>
        /// <param name="skill">要移除的技能</param>
        public void RemoveSkill(Skill skill)
        {
            _skills.Remove(skill);
        }

        /// <summary>
        /// 检查技能是否可以释放
        /// </summary>
        /// <param name="skill">要检查的技能</param>
        /// <returns>是否可以释放</returns>
        public bool CanCastSkill(Skill skill)
        {
            return skill.CurrentCooldown <= 0;
        }

        /// <summary>
        /// 释放技能
        /// </summary>
        /// <param name="skill">要释放的技能</param>
        /// <param name="caster">技能释放者</param>
        /// <param name="target">技能目标</param>
        public void CastSkill(Skill skill, GameObject caster, GameObject target = null)
        {
            if (CanCastSkill(skill))
            {
                skill.CurrentCooldown = skill.CooldownTime;
                skill.Execute(caster, target);
            }
        }
        #endregion
    }

    /// <summary>
    /// 技能基类
    /// </summary>
    [System.Serializable]
    public abstract class Skill
    {
        #region 字段
        [Tooltip("技能名称")]
        public string SkillName = "Skill";

        [Tooltip("技能冷却时间")]
        public float CooldownTime = 5f;

        [Tooltip("当前冷却时间")]
        public float CurrentCooldown = 0f;

        [Tooltip("技能消耗")]
        public int ManaCost = 10;
        #endregion

        #region 方法
        /// <summary>
        /// 执行技能
        /// </summary>
        /// <param name="caster">技能释放者</param>
        /// <param name="target">技能目标</param>
        public abstract void Execute(GameObject caster, GameObject target = null);
        #endregion
    }

    /// <summary>
    /// 武器技能类
    /// </summary>
    [System.Serializable]
    public class WeaponSkill : Skill
    {
        #region 字段
        [Tooltip("技能伤害")]
        public float SkillDamage = 50f;

        [Tooltip("技能范围")]
        public float SkillRange = 3f;

        [Tooltip("技能动画名称")]
        public string AnimationName = "Skill";
        #endregion

        #region 方法
        /// <summary>
        /// 执行技能
        /// </summary>
        /// <param name="caster">技能释放者</param>
        /// <param name="target">技能目标</param>
        public override void Execute(GameObject caster, GameObject target = null)
        {
            // 播放技能动画
            Animator animator = caster.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger(AnimationName);
            }

            // 这里可以添加具体的技能效果实现
            // 例如：造成伤害、施加Buff等
        }
        #endregion
    }
}
