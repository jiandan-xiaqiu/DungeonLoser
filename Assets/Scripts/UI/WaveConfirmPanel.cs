using UnityEngine;
using UnityEngine.UI;
using MyGame.WaveSystem;

namespace MyGame.UI
{
    /// <summary>
    /// 轮次确认面板
    /// </summary>
    public class WaveConfirmPanel : MonoBehaviour
    {
        [Header("UI组件")]
        [SerializeField] private Text waveNumberText;
        [SerializeField] private Text enemyCountText;
        [SerializeField] private Button startWaveButton;

        private void Awake()
        {
            if (startWaveButton != null)
            {
                startWaveButton.onClick.AddListener(OnStartWaveClicked);
            }
        }

        private void OnEnable()
        {
            UpdateWaveInfo();
        }

        /// <summary>
        /// 更新轮次信息
        /// </summary>
        private void UpdateWaveInfo()
        {
            WaveManager waveManager = WaveManager.Instance;
            if (waveManager == null)
                return;

            int nextWave = waveManager.CurrentWaveNumber + 1;
            WaveConfig config = waveManager.GetCurrentWaveConfig();

            if (waveNumberText != null)
            {
                waveNumberText.text = $"第 {nextWave} 轮";
            }

            if (enemyCountText != null && config != null)
            {
                enemyCountText.text = $"本关敌人数量: {config.totalEnemyCount}";
            }
        }

        /// <summary>
        /// 开始下一轮按钮点击
        /// </summary>
        private void OnStartWaveClicked()
        {
            WaveManager waveManager = WaveManager.Instance;
            if (waveManager != null)
            {
                waveManager.StartWave(waveManager.CurrentWaveNumber + 1);
            }
        }
    }
}