using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoPanel : MonoBehaviour
{
    [Header("UI组件")]
    [SerializeField] private Text goldText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Text healthText;
    [SerializeField] private Slider shieldSlider;
    [SerializeField] private Text shieldText;

    private CharacterManager characterManager;

    private void Awake()
    {
        characterManager = CharacterManager.Instance;
    }

    private void Start()
    {
        UpdateGoldDisplay();
        UpdateHealthDisplay();
    }

    private void Update()
    {
        UpdateGoldDisplay();
        UpdateHealthDisplay();
        UpdateShieldDisplay();
    }

    private void UpdateGoldDisplay()
    {
        if (goldText != null && characterManager != null)
        {
            goldText.text = $"金币: {characterManager.Gold}";
        }
    }

    private void UpdateHealthDisplay()
    {
        if (characterManager == null)
            return;

        float currentHP = characterManager.CurrentHP;
        float maxHP = characterManager.HpMax;

        if (healthSlider != null)
        {
            healthSlider.value = currentHP / maxHP;
        }

        if (healthText != null)
        {
            healthText.text = $"{Mathf.Round(currentHP)}/{Mathf.Round(maxHP)}";
        }
    }

    private void UpdateShieldDisplay()
    {
        if (characterManager == null)
            return;

        float currentShield = characterManager.ShieldAmount;
        float maxShield = characterManager.ShieldMaxAmount;

        // 只有在拥有护盾时显示
        bool hasShield = currentShield > 0f;

        if (shieldSlider != null)
        {
            shieldSlider.gameObject.SetActive(hasShield);
            if (hasShield && maxShield > 0f)
            {
                shieldSlider.value = currentShield / maxShield;
            }
        }

        if (shieldText != null)
        {
            shieldText.gameObject.SetActive(hasShield);
            if (hasShield)
            {
                shieldText.text = $"{Mathf.Round(currentShield)}/{Mathf.Round(maxShield)}";
            }
        }
    }
}