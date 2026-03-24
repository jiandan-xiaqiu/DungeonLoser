using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace SimpleOrbital.UI
{
    /// <summary>
    /// 血条管理器，管理所有怪物的血条
    /// </summary>
    public class HealthBarManager : MonoBehaviour
    {
        [Header("血条设置")]
        [SerializeField] private GameObject healthBarPrefab; // 血条预制体
        [SerializeField] private Canvas canvas; // UI画布
        
        // 血条字典，键为怪物GameObject，值为对应的血条组件
        private Dictionary<GameObject, HealthBar> healthBars = new Dictionary<GameObject, HealthBar>();
        
        #region 单例模式
        private static HealthBarManager instance;
        public static HealthBarManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<HealthBarManager>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject("HealthBarManager");
                        instance = obj.AddComponent<HealthBarManager>();
                    }
                }
                return instance;
            }
        }
        #endregion
        
        #region 初始化
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            // 确保有画布
            if (canvas == null)
            {
                canvas = FindObjectOfType<Canvas>();
                if (canvas == null)
                {
                    // 创建默认画布
                    GameObject canvasObj = new GameObject("HealthBarCanvas");
                    canvas = canvasObj.AddComponent<Canvas>();
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    canvasObj.AddComponent<CanvasScaler>();
                    canvasObj.AddComponent<GraphicRaycaster>();
                }
            }
        }
        #endregion
        
        #region 血条管理
        /// <summary>
        /// 为怪物创建血条
        /// </summary>
        /// <param name="monster">怪物GameObject</param>
        /// <param name="initialHealth">初始生命值</param>
        /// <param name="maxHealth">最大生命值</param>
        public void CreateHealthBar(GameObject monster, float initialHealth, float maxHealth)
        {
            if (healthBars.ContainsKey(monster))
                return;
            
            if (healthBarPrefab == null)
            {
                Debug.LogError("HealthBar prefab is not assigned!");
                return;
            }
            
            // 实例化血条
            GameObject healthBarObj = Instantiate(healthBarPrefab, canvas.transform);
            HealthBar healthBar = healthBarObj.GetComponent<HealthBar>();
            
            if (healthBar != null)
            {
                // 设置血条目标和初始生命值
                healthBar.SetTarget(monster.transform);
                healthBar.SetHealth(initialHealth, maxHealth);
                
                // 添加到字典
                healthBars.Add(monster, healthBar);
            }
        }
        
        /// <summary>
        /// 更新怪物血条
        /// </summary>
        /// <param name="monster">怪物GameObject</param>
        /// <param name="currentHealth">当前生命值</param>
        /// <param name="maxHealth">最大生命值</param>
        public void UpdateHealthBar(GameObject monster, float currentHealth, float maxHealth)
        {
            if (healthBars.TryGetValue(monster, out HealthBar healthBar))
            {
                healthBar.SetHealth(currentHealth, maxHealth);
            }
        }
        
        /// <summary>
        /// 移除怪物血条
        /// </summary>
        /// <param name="monster">怪物GameObject</param>
        public void RemoveHealthBar(GameObject monster)
        {
            if (healthBars.TryGetValue(monster, out HealthBar healthBar))
            {
                Destroy(healthBar.gameObject);
                healthBars.Remove(monster);
            }
        }
        
        /// <summary>
        /// 显示怪物血条
        /// </summary>
        /// <param name="monster">怪物GameObject</param>
        public void ShowHealthBar(GameObject monster)
        {
            if (healthBars.TryGetValue(monster, out HealthBar healthBar))
            {
                healthBar.Show();
            }
        }
        
        /// <summary>
        /// 隐藏怪物血条
        /// </summary>
        /// <param name="monster">怪物GameObject</param>
        public void HideHealthBar(GameObject monster)
        {
            if (healthBars.TryGetValue(monster, out HealthBar healthBar))
            {
                healthBar.Hide();
            }
        }
        #endregion
    }
}
