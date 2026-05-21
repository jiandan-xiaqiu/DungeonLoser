using UnityEngine;
using MyGame.WaveSystem;
using SimpleOrbital.UI;

namespace MyGame.UI
{
    /// <summary>
    /// UI管理器
    /// </summary>
    public class UIManager : Singleton<UIManager>
    {
        [Header("面板引用")]
        [SerializeField] private PlayerInfoPanel playerInfoPanel;
        [SerializeField] private WaveConfirmPanel waveConfirmPanel;
        [SerializeField] private UpgradePanel upgradePanel;
        [SerializeField] private PausePanel pausePanel;
        [SerializeField] private GameObject gameOverPanel;
        
        [Header("系统引用")]
        [SerializeField] private HealthBarManager healthBarManager;

        private void Start()
        {
            // 获取血条管理器
            if (healthBarManager == null)
            {
                healthBarManager = HealthBarManager.Instance;
            }

            // 注册事件
            WaveManager waveManager = WaveManager.Instance;
            if (waveManager != null)
            {
                waveManager.OnWaveEnded += OnWaveEnded;
                waveManager.OnWaveStarted += OnWaveStarted;
                waveManager.OnGamePaused += OnGamePaused;
                waveManager.OnGameResumed += OnGameResumed;
            }

            // 注册角色死亡事件
            CharacterManager characterManager = CharacterManager.Instance;
            if (characterManager != null)
            {
                characterManager.OnCharacterDeath += OnCharacterDeath;
            }

            // 初始化面板状态
            ShowPlayerInfoPanel(true);
            ShowWaveConfirmPanel(false);
            ShowUpgradePanel(false);
            ShowPausePanel(false);
            ShowGameOverPanel(false);
        }

        private void OnDestroy()
        {
            WaveManager waveManager = WaveManager.Instance;
            if (waveManager != null)
            {
                waveManager.OnWaveEnded -= OnWaveEnded;
                waveManager.OnWaveStarted -= OnWaveStarted;
                waveManager.OnGamePaused -= OnGamePaused;
                waveManager.OnGameResumed -= OnGameResumed;
            }

            // 注销角色死亡事件
            CharacterManager characterManager = CharacterManager.Instance;
            if (characterManager != null)
            {
                characterManager.OnCharacterDeath -= OnCharacterDeath;
            }
        }

        /// <summary>
        /// 角色死亡回调
        /// </summary>
        private void OnCharacterDeath()
        {
            Debug.Log("角色死亡，触发游戏结束逻辑");
            
            // 结束当前波次
            WaveManager waveManager = WaveManager.Instance;
            if (waveManager != null)
            {
                waveManager.EndWave();
            }
            
            // 清空所有血条
            ClearAllHealthBars();
            
            // 打开游戏结束面板
            ShowGameOverPanel(true);
        }
        
        /// <summary>
        /// 清空所有血条
        /// </summary>
        public void ClearAllHealthBars()
        {
            if (healthBarManager != null)
            {
                healthBarManager.ClearAllHealthBars();
            }
        }

        /// <summary>
        /// 轮次结束回调
        /// </summary>
        private void OnWaveEnded(int waveNumber)
        {
            ShowPlayerInfoPanel(false);
            ShowWaveConfirmPanel(false);
            ShowUpgradePanel(true);
            ShowPausePanel(false);
        }

        /// <summary>
        /// 轮次开始回调
        /// </summary>
        private void OnWaveStarted(int waveNumber)
        {
            ShowPlayerInfoPanel(true);
            ShowWaveConfirmPanel(false);
            ShowUpgradePanel(false);
            ShowPausePanel(false);
        }

        /// <summary>
        /// 游戏暂停回调
        /// </summary>
        private void OnGamePaused()
        {
            ShowPausePanel(true);
        }

        /// <summary>
        /// 游戏恢复回调
        /// </summary>
        private void OnGameResumed()
        {
            ShowPausePanel(false);
        }

        #region 面板显示控制
        public void ShowPlayerInfoPanel(bool show)
        {
            if (playerInfoPanel != null)
            {
                playerInfoPanel.gameObject.SetActive(show);
            }
        }

        public void ShowWaveConfirmPanel(bool show)
        {
            if (waveConfirmPanel != null)
            {
                waveConfirmPanel.gameObject.SetActive(show);
            }
        }

        public void ShowUpgradePanel(bool show)
        {
            if (upgradePanel != null)
            {
                upgradePanel.gameObject.SetActive(show);
            }
        }

        public void ShowPausePanel(bool show)
        {
            if (pausePanel != null)
            {
                pausePanel.gameObject.SetActive(show);
            }
        }
        
        public void ShowGameOverPanel(bool show)
        {
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(show);
                // 游戏结束时暂停时间
                if (show)
                {
                    Time.timeScale = 0f;
                }
            }
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 打开强化选择面板
        /// </summary>
        public void OpenUpgradePanel()
        {
            ShowPlayerInfoPanel(false);
            ShowWaveConfirmPanel(false);
            ShowUpgradePanel(true);
            ShowPausePanel(false);
        }

        /// <summary>
        /// 打开轮次确认面板
        /// </summary>
        public void OpenWaveConfirmPanel()
        {
            ShowPlayerInfoPanel(false);
            ShowWaveConfirmPanel(true);
            ShowUpgradePanel(false);
            ShowPausePanel(false);
        }

        /// <summary>
        /// 打开暂停面板
        /// </summary>
        public void OpenPausePanel()
        {
            WaveManager.Instance?.PauseGame();
        }

        /// <summary>
        /// 关闭暂停面板
        /// </summary>
        public void ClosePausePanel()
        {
            WaveManager.Instance?.ResumeGame();
        }

        /// <summary>
        /// 返回游戏
        /// </summary>
        public void ResumeGame()
        {
            ShowPlayerInfoPanel(true);
            ShowWaveConfirmPanel(false);
            ShowUpgradePanel(false);
            ShowPausePanel(false);
            Time.timeScale = 1f;
        }
        #endregion
    }
}