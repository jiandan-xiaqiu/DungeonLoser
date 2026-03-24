using UnityEngine;
using MyGame.AI;
using SimpleOrbital.UI;

namespace SimpleOrbital
{
    /// <summary>
    /// 敌人健康组件，处理敌人的生命值和死亡
    /// </summary>
    public class EnemyHealth : MonoBehaviour
    {
        [Header("健康设置")]
        public float maxHealth = 100f;
        public float currentHealth;
        public bool isDead = false;
        
        [Header("血条设置")]
        public bool showHealthBar = true; // 是否显示血条
        
        // 事件
        public System.Action<GameObject> OnEnemyDeath;
        
        // AI组件引用
        private MonsterAI monsterAI;
        
        #region 初始化
        private void Awake()
        {
            currentHealth = maxHealth;
            
            // 获取AI组件
            monsterAI = GetComponent<MonsterAI>();
            
            // 创建血条
            if (showHealthBar)
            {
                HealthBarManager.Instance.CreateHealthBar(gameObject, currentHealth, maxHealth);
            }
        }
        #endregion
        
        #region 伤害处理
        /// <summary>
        /// 受到伤害
        /// </summary>
        /// <param name="damage">伤害值</param>
        public void TakeDamage(float damage)
        {
            if (isDead)
                return;
            
            currentHealth -= damage;
            
            // 更新血条
            if (showHealthBar)
            {
                HealthBarManager.Instance.UpdateHealthBar(gameObject, currentHealth, maxHealth);
            }
            
            // 触发受击状态
            if (monsterAI != null)
            {
                monsterAI.OnHit();
            }
            
            if (currentHealth <= 0f)
            {
                Die();
            }
        }
        
        /// <summary>
        /// 死亡
        /// </summary>
        private void Die()
        {
            isDead = true;
            
            // 触发死亡状态
            if (monsterAI != null)
            {
                monsterAI.OnDeath();
            }
            
            // 移除血条
            if (showHealthBar)
            {
                HealthBarManager.Instance.RemoveHealthBar(gameObject);
            }
            
            OnEnemyDeath?.Invoke(gameObject);
            
            // 触发死亡效果
            // 这里可以添加死亡动画、粒子效果等
            
            // 延迟销毁敌人
            Destroy(gameObject, 0.5f);
        }
        #endregion
    }
}
