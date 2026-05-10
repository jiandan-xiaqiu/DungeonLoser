using UnityEngine;
using MyGame.WaveSystem;

namespace SimpleOrbital
{
    /// <summary>
    /// 敌人生成器，支持轮次管理
    /// </summary>
    public class SimpleEnemySpawner : MonoBehaviour
    {
        [Header("系统引用")]
        [SerializeField] private GameObject player;
        [SerializeField] private ItemDropSystem itemDropSystem;
        [SerializeField] private WaveManager waveManager;

        [Header("备用敌人预制体（当轮次配置没有指定时使用）")]
        [SerializeField] private GameObject defaultEnemyPrefab;

        private float _spawnTimer = 0f;

        #region 初始化
        private void Start()
        {
            if (player == null)
            {
                player = GameObject.FindWithTag("Player");
            }

            if (waveManager == null)
            {
                waveManager = WaveManager.Instance;
            }

            if (waveManager != null)
            {
                waveManager.OnWaveStarted += OnWaveStarted;
            }
        }

        private void OnDestroy()
        {
            if (waveManager != null)
            {
                waveManager.OnWaveStarted -= OnWaveStarted;
            }
        }

        private void Update()
        {
            if (waveManager == null || waveManager.IsPaused)
                return;

            UpdateSpawning();
        }
        #endregion

        /// <summary>
        /// 轮次开始回调
        /// </summary>
        private void OnWaveStarted(int waveNumber)
        {
            _spawnTimer = 0f;
        }

        /// <summary>
        /// 更新生成逻辑
        /// </summary>
        private void UpdateSpawning()
        {
            if (waveManager == null || !waveManager.IsWaveActive)
                return;

            WaveConfig config = waveManager.GetCurrentWaveConfig();
            if (config == null)
                return;

            if (waveManager.EnemiesSpawnedThisWave < config.totalEnemyCount)
            {
                _spawnTimer += Time.deltaTime;
                if (_spawnTimer >= config.spawnInterval)
                {
                    SpawnEnemy(config);
                    _spawnTimer = 0f;
                }
            }
        }

        /// <summary>
        /// 生成敌人
        /// </summary>
        private void SpawnEnemy(WaveConfig config)
        {
            if (player == null)
                return;

            GameObject enemyPrefab = SelectEnemyPrefab(config);
            if (enemyPrefab == null)
                return;

            Vector3 spawnPosition = GetRandomSpawnPosition(config);
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            if (enemy != null)
            {
                waveManager.RegisterEnemy(enemy);

                if (itemDropSystem != null)
                {
                    itemDropSystem.RegisterEnemy(enemy);
                }

                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.OnEnemyDeath += OnEnemyDeath;
                }
            }
        }

        /// <summary>
        /// 根据权重随机选择敌人预制体
        /// </summary>
        private GameObject SelectEnemyPrefab(WaveConfig config)
        {
            if (config.enemyTypes != null && config.enemyTypes.Length > 0)
            {
                float totalWeight = 0f;
                foreach (var type in config.enemyTypes)
                {
                    if (type.enemyPrefab != null)
                    {
                        totalWeight += type.weight;
                    }
                }

                if (totalWeight > 0f)
                {
                    float randomValue = Random.Range(0f, totalWeight);
                    float currentWeight = 0f;

                    foreach (var type in config.enemyTypes)
                    {
                        if (type.enemyPrefab != null)
                        {
                            currentWeight += type.weight;
                            if (randomValue <= currentWeight)
                            {
                                return type.enemyPrefab;
                            }
                        }
                    }
                }
            }

            return defaultEnemyPrefab;
        }

        /// <summary>
        /// 获取随机生成位置
        /// </summary>
        private Vector3 GetRandomSpawnPosition(WaveConfig config)
        {
            float radius = config != null ? config.spawnRadius : 3f;
            float minDist = config != null ? config.spawnAreaMin : 1f;

            float angle = Random.Range(0f, Mathf.PI * 2f);
            float distance = Random.Range(minDist, radius);

            float x = Mathf.Cos(angle) * distance;
            float y = Mathf.Sin(angle) * distance;

            return player.transform.position + new Vector3(x, y, -2f);
        }

        #region 事件处理
        /// <summary>
        /// 处理敌人死亡
        /// </summary>
        private void OnEnemyDeath(GameObject enemy)
        {
            if (enemy != null)
            {
                waveManager?.UnregisterEnemy(enemy);

                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.OnEnemyDeath -= OnEnemyDeath;
                }
            }
        }
        #endregion
    }
}