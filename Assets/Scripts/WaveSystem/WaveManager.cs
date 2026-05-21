using UnityEngine;
using System;
using System.Collections.Generic;

namespace MyGame.WaveSystem
{
    /// <summary>
    /// 关卡/轮次管理器
    /// </summary>
    public class WaveManager : Singleton<WaveManager>
    {
        [Header("轮次配置列表")]
        [SerializeField] private List<WaveConfig> waveConfigs = new List<WaveConfig>();

        [Header("默认轮次配置（当配置的轮次不存在时使用）")]
        [SerializeField] private WaveConfig defaultWaveConfig;

        [Header("自动开始第一轮")]
        [SerializeField] private bool autoStartWave = true;

        private int _currentWaveIndex = 0;
        private int _enemiesSpawnedThisWave = 0;
        private int _enemiesKilledThisWave = 0;
        private bool _isWaveActive = false;
        private bool _isPaused = false;
        private float _waveEndTimer = 0f;
        private List<GameObject> _activeEnemies = new List<GameObject>();

        public event Action<int> OnWaveStarted;
        public event Action<int> OnWaveEnded;
        public event Action OnGamePaused;
        public event Action OnGameResumed;

        public int CurrentWaveNumber => _currentWaveIndex + 1;
        public bool IsPaused => _isPaused;
        public bool IsWaveActive => _isWaveActive;
        public int EnemiesRemainingThisWave => (_enemiesSpawnedThisWave - _enemiesKilledThisWave);
        public int EnemiesSpawnedThisWave => _enemiesSpawnedThisWave;

        private void Start()
        {
            if (autoStartWave)
            {
                StartWave(1);
            }
        }

        private void Update()
        {
            if (_isPaused)
                return;

            if (_isWaveActive)
            {
                CheckWaveEndCondition();
            }
        }

        /// <summary>
        /// 开始指定轮次
        /// </summary>
        public void StartWave(int waveNumber)
        {
            if (waveNumber < 1)
            {
                Debug.LogWarning($"Invalid wave number: {waveNumber}");
                return;
            }

            _currentWaveIndex = waveNumber - 1;
            WaveConfig config = GetWaveConfig(_currentWaveIndex);

            _enemiesSpawnedThisWave = 0;
            _enemiesKilledThisWave = 0;
            _isWaveActive = true;
            _waveEndTimer = 0f;

            OnWaveStarted?.Invoke(waveNumber);

            Debug.Log($"Wave {waveNumber} started. Total enemies: {config.totalEnemyCount}");
        }

        /// <summary>
        /// 获取当前轮次配置
        /// </summary>
        public WaveConfig GetCurrentWaveConfig()
        {
            return GetWaveConfig(_currentWaveIndex);
        }

        /// <summary>
        /// 获取指定轮次配置
        /// </summary>
        private WaveConfig GetWaveConfig(int index)
        {
            if (index >= 0 && index < waveConfigs.Count)
            {
                return waveConfigs[index];
            }
            return GenerateDefaultWaveConfig(index);
        }

        /// <summary>
        /// 生成默认轮次配置（当配置的轮次不存在时使用）
        /// </summary>
        private WaveConfig GenerateDefaultWaveConfig(int index)
        {
            if (defaultWaveConfig != null)
            {
                return defaultWaveConfig;
            }

            WaveConfig config = new WaveConfig
            {
                waveNumber = index + 1,
                totalEnemyCount = 10 + index * 5,
                spawnInterval = Mathf.Max(0.5f, 2f - index * 0.1f),
                spawnRadius = 10f,
                spawnAreaMin = 5f,
                nextWaveDelay = 5f,
                enemyTypes = new EnemyTypeConfig[0]
            };
            return config;
        }

        /// <summary>
        /// 检查轮次结束条件
        /// </summary>
        private void CheckWaveEndCondition()
        {
            if (!_isWaveActive)
                return;

            WaveConfig config = GetWaveConfig(_currentWaveIndex);

            if (_enemiesSpawnedThisWave >= config.totalEnemyCount && EnemiesRemainingThisWave <= 0)
            {
                EndWave();
            }
        }

        /// <summary>
        /// 结束当前轮次
        /// </summary>
        public void EndWave()
        {
            if (!_isWaveActive)
                return;
                
            _isWaveActive = false;
            int completedWave = _currentWaveIndex + 1;
            OnWaveEnded?.Invoke(completedWave);

            Debug.Log($"Wave {completedWave} ended.");
        }

        /// <summary>
        /// 开始下一轮（由玩家手动触发）
        /// </summary>
        public void StartNextWave()
        {
            if (_isPaused)
                return;

            StartWave(_currentWaveIndex + 2);
        }

        /// <summary>
        /// 注册生成的敌人
        /// </summary>
        public void RegisterEnemy(GameObject enemy)
        {
            if (enemy != null && !_activeEnemies.Contains(enemy))
            {
                _activeEnemies.Add(enemy);
                _enemiesSpawnedThisWave++;
            }
        }

        /// <summary>
        /// 注销死亡的敌人
        /// </summary>
        public void UnregisterEnemy(GameObject enemy)
        {
            if (enemy != null)
            {
                _activeEnemies.Remove(enemy);
                _enemiesKilledThisWave++;
            }
        }

        /// <summary>
        /// 暂停游戏
        /// </summary>
        public void PauseGame()
        {
            if (_isPaused)
                return;

            _isPaused = true;
            Time.timeScale = 0f;
            OnGamePaused?.Invoke();

            Debug.Log("Game Paused");
        }

        /// <summary>
        /// 恢复游戏
        /// </summary>
        public void ResumeGame()
        {
            if (!_isPaused)
                return;

            _isPaused = false;
            Time.timeScale = 1f;
            OnGameResumed?.Invoke();

            Debug.Log("Game Resumed");
        }

        /// <summary>
        /// 切换暂停状态
        /// </summary>
        public void TogglePause()
        {
            if (_isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        /// <summary>
        /// 重置游戏
        /// </summary>
        public void ResetGame()
        {
            _currentWaveIndex = 0;
            _enemiesSpawnedThisWave = 0;
            _enemiesKilledThisWave = 0;
            _isWaveActive = false;
            _isPaused = false;
            Time.timeScale = 1f;
            
            foreach (var enemy in _activeEnemies)
            {
                if (enemy != null)
                {
                    Destroy(enemy);
                }
            }
            _activeEnemies.Clear();
        }

        /// <summary>
        /// 跳过当前轮次（进入下一轮）
        /// </summary>
        public void SkipWave()
        {
            CancelInvoke(nameof(StartNextWave));
            StartNextWave();
        }
    }
}