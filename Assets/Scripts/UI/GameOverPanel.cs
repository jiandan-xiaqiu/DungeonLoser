using UnityEngine;
using UnityEngine.UI;
using MyGame.WaveSystem;
using SimpleOrbital.UI;

namespace MyGame.UI
{
    /// <summary>
    /// 游戏结束面板
    /// </summary>
    public class GameOverPanel : MonoBehaviour
    {
        [Header("UI组件")]

        [SerializeField] private Button restartButton;
        //[SerializeField] private Button mainMenuButton;

        private void Awake()
        {
            if (restartButton != null)
            {
                restartButton.onClick.AddListener(OnRestartClicked);
            }

            // if (mainMenuButton != null)
            // {
            //     mainMenuButton.onClick.AddListener(OnMainMenuClicked);
            // }
        }

        private void OnEnable()
        {
            UpdateGameOverInfo();
        }

        /// <summary>
        /// 更新游戏结束信息
        /// </summary>
        private void UpdateGameOverInfo()
        {
            // 更新波次信息

        }

        /// <summary>
        /// 重新开始按钮点击
        /// </summary>
        private void OnRestartClicked()
        {
            Debug.Log("重新开始游戏");
            
            // 重置游戏状态
            WaveManager.Instance?.ResetGame();
            CharacterManager.Instance?.Initialize();
            
            // 清空所有血条
            HealthBarManager.Instance?.ClearAllHealthBars();
            
            // 恢复时间
            Time.timeScale = 1f;
            
            // 关闭游戏结束面板，开始新一轮
            gameObject.SetActive(false);
            WaveManager.Instance?.StartWave(1);
        }

        /// <summary>
        /// 返回主菜单按钮点击
        /// </summary>
        private void OnMainMenuClicked()
        {
            Debug.Log("返回主菜单");
            
            // 恢复时间
            Time.timeScale = 1f;
            
            // 这里可以添加返回主菜单的逻辑
            // 例如加载主菜单场景
            // SceneManager.LoadScene("MainMenu");
        }
    }
}
