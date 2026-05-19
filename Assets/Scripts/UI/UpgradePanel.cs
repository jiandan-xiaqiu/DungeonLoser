using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
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

        // 升级选项数据结构
        private struct UpgradeOption
        {
            public string Description;
            public int OptionIndex;
        }

        // 当前显示的3个升级选项
        private UpgradeOption[] currentOptions;

        // 所有升级选项描述
        private readonly string[] upgradeDescriptionsAll = new string[]
        {
            "增加最大生命值 +20",
            "增加武器数量 +1",
            "增加移动速度 +1",
            "增加攻击速度 +10%",
            "增加护盾额外获取量 +10"
        };

        private void Awake()
        {
            if (continueButton != null)
            {
                continueButton.onClick.AddListener(OnContinueClicked);
            }

            // 设置升级按钮点击事件
            for (int i = 0; i < upgradeButtons.Length; i++)
            {
                int index = i;
                if (upgradeButtons[i] != null)
                {
                    upgradeButtons[i].onClick.AddListener(() => OnUpgradeSelected(index));
                }
            }

            // 初始化选项数组
            currentOptions = new UpgradeOption[3];
        }

        private void OnEnable()
        {
            UpdateWaveInfo();
            GenerateRandomOptions();
            UpdateUpgradeUI();
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
        /// 随机生成3个升级选项
        /// </summary>
        private void GenerateRandomOptions()
        {
            // 创建索引列表
            List<int> indices = new List<int>();
            for (int i = 0; i < upgradeDescriptionsAll.Length; i++)
            {
                indices.Add(i);
            }

            // 打乱顺序
            for (int i = 0; i < indices.Count; i++)
            {
                int temp = indices[i];
                int randomIndex = Random.Range(i, indices.Count);
                indices[i] = indices[randomIndex];
                indices[randomIndex] = temp;
            }

            // 取前3个
            for (int i = 0; i < 3 && i < indices.Count; i++)
            {
                currentOptions[i] = new UpgradeOption
                {
                    Description = upgradeDescriptionsAll[indices[i]],
                    OptionIndex = indices[i]
                };
            }
        }

        /// <summary>
        /// 更新升级UI显示
        /// </summary>
        private void UpdateUpgradeUI()
        {
            for (int i = 0; i < upgradeDescriptions.Length; i++)
            {
                if (upgradeDescriptions[i] != null)
                {
                    if (i < 3)
                    {
                        upgradeDescriptions[i].text = currentOptions[i].Description;
                    }
                }

                if (i < upgradeButtons.Length && upgradeButtons[i] != null)
                {
                    upgradeButtons[i].gameObject.SetActive(i < 3);
                    
                    // 设置按钮的Text子物体
                    if (i < 3)
                    {
                        Text buttonText = upgradeButtons[i].GetComponentInChildren<Text>();
                        if (buttonText != null)
                        {
                            buttonText.text = currentOptions[i].Description;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 选择升级选项
        /// </summary>
        private void OnUpgradeSelected(int index)
        {
            if (index < 0 || index >= 3)
                return;

            Debug.Log($"选择升级: {currentOptions[index].Description}");

            // 应用强化效果
            ApplyUpgrade(currentOptions[index].OptionIndex);

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
                case 1: // 增加武器数量
                    characterManager.IncreaseWeaponCount();
                    break;
                case 2: // 增加移动速度
                    characterManager.IncreaseMoveSpeed(1f);
                    break;
                case 3: // 增加攻击速度
                    characterManager.IncreaseAttackSpeed(0.1f);
                    break;
                case 4: // 增加护盾额外获取量
                    characterManager.IncreaseShieldBonus(10f);
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