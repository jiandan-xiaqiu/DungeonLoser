using UnityEngine;
using UnityEngine.UI;
using MyGame.WaveSystem;

namespace MyGame.UI
{
    /// <summary>
    /// 暂停面板
    /// </summary>
    public class PausePanel : MonoBehaviour
    {
        [Header("UI组件")]
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button quitButton;

        private void Awake()
        {
            if (resumeButton != null)
            {
                resumeButton.onClick.AddListener(OnResumeClicked);
            }

            if (restartButton != null)
            {
                restartButton.onClick.AddListener(OnRestartClicked);
            }

            if (quitButton != null)
            {
                quitButton.onClick.AddListener(OnQuitClicked);
            }
        }

        /// <summary>
        /// 继续游戏按钮点击
        /// </summary>
        private void OnResumeClicked()
        {
            WaveManager.Instance?.ResumeGame();
        }

        /// <summary>
        /// 重新开始按钮点击
        /// </summary>
        private void OnRestartClicked()
        {
            WaveManager.Instance?.ResetGame();
            WaveManager.Instance?.StartWave(1);
        }

        /// <summary>
        /// 退出按钮点击
        /// </summary>
        private void OnQuitClicked()
        {
            Application.Quit();
        }
    }
}