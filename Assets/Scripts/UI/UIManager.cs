using UnityEngine;
using MyGame.WaveSystem;

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

        private void Start()
        {
            // 注册事件
            WaveManager waveManager = WaveManager.Instance;
            if (waveManager != null)
            {
                waveManager.OnWaveEnded += OnWaveEnded;
                waveManager.OnWaveStarted += OnWaveStarted;
                waveManager.OnGamePaused += OnGamePaused;
                waveManager.OnGameResumed += OnGameResumed;
            }

            // 初始化面板状态
            ShowPlayerInfoPanel(true);
            ShowWaveConfirmPanel(false);
            ShowUpgradePanel(false);
            ShowPausePanel(false);
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