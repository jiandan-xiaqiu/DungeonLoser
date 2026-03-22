using UnityEngine;

namespace MyGame.Character.Systems
{
    /// <summary>
    /// 元素反应系统，管理元素反应
    /// </summary>
    public class ElementReactionSystem : MonoBehaviour
    {
        #region 方法
        /// <summary>
        /// 计算元素反应
        /// </summary>
        /// <param name="attackElement">攻击元素类型</param>
        /// <param name="targetElement">目标元素类型</param>
        /// <param name="baseDamage">基础伤害</param>
        /// <returns>反应结果</returns>
        public ElementReactionResult CalculateReaction(ElementType attackElement, ElementType targetElement, float baseDamage)
        {
            ElementReactionResult result = new ElementReactionResult();
            result.ReactionType = ElementReactionType.None;
            result.ReactionDamage = 0f;

            // 检查是否有元素反应
            if (attackElement != ElementType.None && targetElement != ElementType.None && attackElement != targetElement)
            {
                // 火元素反应
                if (attackElement == ElementType.Fire)
                {
                    switch (targetElement)
                    {
                        case ElementType.Water:
                            result.ReactionType = ElementReactionType.Vaporize;
                            result.ReactionDamage = baseDamage * 1.5f;
                            break;
                        case ElementType.Ice:
                            result.ReactionType = ElementReactionType.Melt;
                            result.ReactionDamage = baseDamage * 1.5f;
                            break;
                        case ElementType.Lightning:
                            result.ReactionType = ElementReactionType.Overload;
                            result.ReactionDamage = baseDamage * 1.2f;
                            break;
                    }
                }
                // 水元素反应
                else if (attackElement == ElementType.Water)
                {
                    switch (targetElement)
                    {
                        case ElementType.Fire:
                            result.ReactionType = ElementReactionType.Vaporize;
                            result.ReactionDamage = baseDamage * 1.5f;
                            break;
                        case ElementType.Lightning:
                            result.ReactionType = ElementReactionType.ElectroCharged;
                            result.ReactionDamage = baseDamage * 0.8f;
                            break;
                        case ElementType.Ice:
                            result.ReactionType = ElementReactionType.Freeze;
                            result.ReactionDamage = baseDamage * 1.0f;
                            break;
                    }
                }
                // 雷元素反应
                else if (attackElement == ElementType.Lightning)
                {
                    switch (targetElement)
                    {
                        case ElementType.Fire:
                            result.ReactionType = ElementReactionType.Overload;
                            result.ReactionDamage = baseDamage * 1.2f;
                            break;
                        case ElementType.Water:
                            result.ReactionType = ElementReactionType.ElectroCharged;
                            result.ReactionDamage = baseDamage * 0.8f;
                            break;
                        case ElementType.Ice:
                            result.ReactionType = ElementReactionType.Superconduct;
                            result.ReactionDamage = baseDamage * 1.0f;
                            break;
                    }
                }
                // 冰元素反应
                else if (attackElement == ElementType.Ice)
                {
                    switch (targetElement)
                    {
                        case ElementType.Fire:
                            result.ReactionType = ElementReactionType.Melt;
                            result.ReactionDamage = baseDamage * 1.5f;
                            break;
                        case ElementType.Water:
                            result.ReactionType = ElementReactionType.Freeze;
                            result.ReactionDamage = baseDamage * 1.0f;
                            break;
                        case ElementType.Lightning:
                            result.ReactionType = ElementReactionType.Superconduct;
                            result.ReactionDamage = baseDamage * 1.0f;
                            break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 获取元素反应描述
        /// </summary>
        /// <param name="reactionType">反应类型</param>
        /// <returns>反应描述</returns>
        public string GetReactionDescription(ElementReactionType reactionType)
        {
            switch (reactionType)
            {
                case ElementReactionType.Vaporize:
                    return "蒸发：造成额外50%伤害";
                case ElementReactionType.Melt:
                    return "融化：造成额外50%伤害";
                case ElementReactionType.Overload:
                    return "超载：造成额外20%伤害并击飞敌人";
                case ElementReactionType.ElectroCharged:
                    return "感电：持续造成伤害";
                case ElementReactionType.Freeze:
                    return "冻结：使敌人无法行动";
                case ElementReactionType.Superconduct:
                    return "超导：降低敌人防御力";
                default:
                    return "无反应";
            }
        }
        #endregion
    }

    /// <summary>
    /// 元素反应类型
    /// </summary>
    public enum ElementReactionType
    {
        None,
        Vaporize,    // 蒸发
        Melt,        // 融化
        Overload,    // 超载
        ElectroCharged, // 感电
        Freeze,      // 冻结
        Superconduct // 超导
    }

    /// <summary>
    /// 元素反应结果
    /// </summary>
    public class ElementReactionResult
    {
        [Tooltip("反应类型")]
        public ElementReactionType ReactionType;

        [Tooltip("反应伤害")]
        public float ReactionDamage;
    }
}
