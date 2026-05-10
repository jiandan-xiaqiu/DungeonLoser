using UnityEngine;
using UnityEngine.UI;
using MyGame.WaveSystem;

namespace MyGame.UI
{
    /// <summary>
    /// 强化选择面板
    /// </summary>
    public class UpgradePanel : MonoBehaviour
    {
        [Header("UI组件")]
        [SerializeField] private Text waveNumberText;
        [SerializeField] private Button[] upgradeButtons;
        [SerializeField] private Text[] upgradeDescriptions;
        [SerializeField] private Button continueButton;

        private string[] upgradeOptions = new string[]
        {
            "增加最大生命值 +20",
            "增加攻击力 +5",
            "增加移动速度 +1",
            "增加攻击速度 +10%"
        };

        private void Awake()
        {
            if (continueButton != null)
            {
                continueButton.onClick.AddListener(OnContinueClicked);
            }

            // 设置升级按钮点击事件
            for (int i = 0; i < upgradeButtons.Length && i < upgradeOptions.Length; i++)
            {
                int index = i;
                if (upgradeButtons[i] != null)
                {
                    upgradeButtons[i].onClick.AddListener(() => OnUpgradeSelected(index));
                }
            }
        }

        private void OnEnable()
        {
            UpdateWaveInfo();
            UpdateUpgradeOptions();
        }

        /// <summary>
        /// 更新轮次信息
        /// </summary>
        private void UpdateWaveInfo()
        {
            WaveManager waveManager = WaveManager.Instance;
            if (waveManager == null)
                return;

            if (waveNumberText != null)
            {
                waveNumberText.text = $"第 {waveManager.CurrentWaveNumber} 轮完成!";
            }
        }

        /// <summary>
        /// 更新升级选项
        /// </summary>
        private void UpdateUpgradeOptions()
        {
            for (int i = 0; i < upgradeDescriptions.Length && i < upgradeOptions.Length; i++)
            {
                if (upgradeDescriptions[i] != null)
                {
                    upgradeDescriptions[i].text = upgradeOptions[i];
                }
            }
        }

        /// <summary>
        /// 选择升级选项
        /// </summary>
        private void OnUpgradeSelected(int index)
        {
            // 具体强化逻辑可以在这里实现
            Debug.Log($"选择升级: {upgradeOptions[index]}");

            // 应用强化效果
            ApplyUpgrade(index);

            // 关闭强化面板，打开轮次确认面板
            UIManager.Instance?.OpenWaveConfirmPanel();
        }

        /// <summary>
        /// 应用强化效果
        /// </summary>
        private void ApplyUpgrade(int index)
        {
            CharacterManager characterManager = CharacterManager.Instance;
            if (characterManager == null)
                return;

            switch (index)
            {
                case 0: // 增加最大生命值
                    characterManager.IncreaseHpMax(20f);
                    break;
                case 1: // 增加攻击力
                    // 攻击力可以在WeaponSystem中处理
                    break;
                case 2: // 增加移动速度
                    characterManager.IncreaseMoveSpeed(1f);
                    break;
                case 3: // 增加攻击速度
                    characterManager.IncreaseAttackSpeed(0.1f);
                    break;
            }
        }

        /// <summary>
        /// 继续按钮点击（不选择强化直接进入下一轮）
        /// </summary>
        private void OnContinueClicked()
        {
            UIManager.Instance?.OpenWaveConfirmPanel();
        }
    }
}