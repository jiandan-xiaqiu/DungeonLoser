using SimpleOrbital;
using UnityEngine;

/// <summary>
/// 简单敌人生成器，使用现有的怪物预制体
/// </summary>
public class SimpleEnemySpawner : MonoBehaviour
{
    [Header("生成设置")]
    [SerializeField] private GameObject enemyPrefab; // 敌人预制体
    [SerializeField] private int maxEnemies = 10; // 最大敌人数量
    [SerializeField] private float spawnInterval = 2f; // 生成间隔
    [SerializeField] private float spawnRadius = 10f; // 生成半径
    [SerializeField] private float spawnAreaMin = 5f; // 最小生成距离
    
    [Header("系统引用")]
    [SerializeField] private GameObject player; // 玩家引用
    [SerializeField] private ItemDropSystem itemDropSystem; // 物品掉落系统
    
    private int currentEnemyCount = 0;
    private float spawnTimer = 0f;
    
    #region 初始化
    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        
        // 初始生成一些敌人
        SpawnInitialEnemies();
    }
    
    private void Update()
    {
        // 检查是否需要生成敌人
        if (currentEnemyCount < maxEnemies)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                SpawnEnemy();
                spawnTimer = 0f;
            }
        }
    }
    #endregion
    
    #region 敌人生成
    /// <summary>
    /// 初始生成敌人
    /// </summary>
    private void SpawnInitialEnemies()
    {
        int initialEnemies = Mathf.Min(5, maxEnemies);
        for (int i = 0; i < initialEnemies; i++)
        {
            SpawnEnemy();
        }
    }
    
    /// <summary>
    /// 生成单个敌人
    /// </summary>
    private void SpawnEnemy()
    {
        if (player == null || enemyPrefab == null)
            return;
        
        // 计算生成位置
        Vector3 spawnPosition = GetRandomSpawnPosition();
        
        // 实例化敌人
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        
        if (enemy != null)
        {
            currentEnemyCount++;
            
            // 注册物品掉落
            if (itemDropSystem != null)
            {
                itemDropSystem.RegisterEnemy(enemy);
            }
            
            // 添加死亡监听
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.OnEnemyDeath += OnEnemyDeath;
            }
        }
    }
    
    /// <summary>
    /// 获取随机生成位置
    /// </summary>
    /// <returns>生成位置</returns>
    private Vector3 GetRandomSpawnPosition()
    {
        if (player == null)
            return Vector3.zero;
        
        // 生成一个在指定范围内的随机位置
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float distance = Random.Range(spawnAreaMin, spawnRadius);
        
        float x = Mathf.Cos(angle) * distance;
        float y = Mathf.Sin(angle) * distance;
        
        return player.transform.position + new Vector3(x, y, -2f);
    }
    #endregion
    
    #region 事件处理
    /// <summary>
    /// 处理敌人死亡
    /// </summary>
    /// <param name="enemy">敌人游戏对象</param>
    private void OnEnemyDeath(GameObject enemy)
    {
        currentEnemyCount = Mathf.Max(0, currentEnemyCount - 1);
        
        // 移除事件监听
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.OnEnemyDeath -= OnEnemyDeath;
        }
    }
    #endregion
    
    #region 公共方法
    /// <summary>
    /// 设置最大敌人数量
    /// </summary>
    /// <param name="max">最大数量</param>
    public void SetMaxEnemies(int max)
    {
        maxEnemies = Mathf.Max(1, max);
    }
    
    /// <summary>
    /// 设置生成间隔
    /// </summary>
    /// <param name="interval">间隔时间</param>
    public void SetSpawnInterval(float interval)
    {
        spawnInterval = Mathf.Max(0.1f, interval);
    }
    
    /// <summary>
    /// 立即生成一个敌人
    /// </summary>
    public void SpawnEnemyImmediately()
    {
        if (currentEnemyCount < maxEnemies)
        {
            SpawnEnemy();
        }
    }
    
    /// <summary>
    /// 清除所有敌人
    /// </summary>
    public void ClearAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        currentEnemyCount = 0;
    }
    #endregion
}
