using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // 角色死亡逻辑
    }

public void Attack(IDamageable target)
    {
        // 攻击目标逻辑
    }
}