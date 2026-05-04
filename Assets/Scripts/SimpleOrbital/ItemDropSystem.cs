using UnityEngine;
using System.Collections.Generic;

namespace SimpleOrbital
{
    /// <summary>
    /// 物品掉落系统，处理敌人死亡时的掉落
    /// </summary>
    public class ItemDropSystem : MonoBehaviour
    {
        [Header("掉落设置")]
        [SerializeField] private float dropChance = 0.7f; // 掉落概率
        [SerializeField] private float weaponDropChance = 0f; // 武器掉落概率
        [SerializeField] private float weaponUpgradeChance = 0f; // 武器升级概率
        [SerializeField] private float goldDropChance = 0.4f; // 金币掉落概率
        [SerializeField] private float healthDropChance = 0.15f; // 回血道具掉落概率
        [SerializeField] private float shieldDropChance = 0.1f; // 护盾道具掉落概率
        [SerializeField] private float itemDropForce = 5f; // 掉落力度
        
        [Header("道具预制体")]
        [SerializeField] private GameObject weaponPickupPrefab; // 武器拾取预制体
        [SerializeField] private GameObject goldPickupPrefab; // 金币拾取预制体
        [SerializeField] private GameObject healthPickupPrefab; // 回血道具拾取预制体
        [SerializeField] private GameObject shieldPickupPrefab; // 护盾道具拾取预制体
        
        // 系统引用
        private WeaponOrbitalSystem weaponSystem;
        
        // 武器列表
        public List<Weapon> possibleWeapons = new List<Weapon>();
        
        #region 初始化
        private void Awake()
        {
            // 获取武器系统
            weaponSystem = GetComponent<WeaponOrbitalSystem>();
        }
        
        /// <summary>
        /// 注册敌人死亡事件
        /// </summary>
        /// <param name="enemy">敌人游戏对象</param>
        public void RegisterEnemy(GameObject enemy)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.OnEnemyDeath += HandleEnemyDeath;
            }
        }
        #endregion
        
        #region 掉落处理
        /// <summary>
        /// 处理敌人死亡
        /// </summary>
        /// <param name="enemy">敌人游戏对象</param>
        private void HandleEnemyDeath(GameObject enemy)
        {
            // 检查是否掉落
            if (Random.value <= dropChance)
            {
                // 决定掉落类型
                float rand = Random.value;
                float cumulativeChance = 0f;
                
                cumulativeChance += weaponDropChance;
                if (rand <= cumulativeChance && possibleWeapons.Count > 0)
                {
                    DropWeapon(enemy.transform.position);
                    return;
                }
                
                cumulativeChance += weaponUpgradeChance;
                if (rand <= cumulativeChance && weaponSystem != null)
                {
                    UpgradeExistingWeapon();
                    return;
                }
                
                cumulativeChance += goldDropChance;
                if (rand <= cumulativeChance)
                {
                    DropGold(enemy.transform.position);
                    return;
                }
                
                cumulativeChance += healthDropChance;
                if (rand <= cumulativeChance)
                {
                    DropHealthPotion(enemy.transform.position);
                    return;
                }
                
                cumulativeChance += shieldDropChance;
                if (rand <= cumulativeChance)
                {
                    DropShieldPotion(enemy.transform.position);
                    return;
                }
            }
        }
        
        /// <summary>
        /// 掉落武器
        /// </summary>
        /// <param name="position">掉落位置</param>
        private void DropWeapon(Vector3 position)
        {
            if (possibleWeapons.Count == 0 || weaponSystem == null)
                return;
            
            // 随机选择武器
            Weapon weapon = possibleWeapons[Random.Range(0, possibleWeapons.Count)];
            
            // 创建武器掉落物品
            CreateWeaponPickup(weapon, position);
        }
        
        /// <summary>
        /// 掉落金币
        /// </summary>
        /// <param name="position">掉落位置</param>
        private void DropGold(Vector3 position)
        {
            CreateGoldPickup(position);
        }

        /// <summary>
        /// 掉落回血道具
        /// </summary>
        /// <param name="position">掉落位置</param>
        private void DropHealthPotion(Vector3 position)
        {
            CreateHealthPickup(position);
        }

        /// <summary>
        /// 掉落护盾道具
        /// </summary>
        /// <param name="position">掉落位置</param>
        private void DropShieldPotion(Vector3 position)
        {
            CreateShieldPickup(position);
        }
        
        /// <summary>
        /// 升级现有武器
        /// </summary>
        private void UpgradeExistingWeapon()
        {
            if (weaponSystem == null)
                return;
            
            List<Weapon> weapons = weaponSystem.GetWeapons();
            if (weapons.Count > 0)
            {
                // 随机选择一个武器升级
                Weapon weapon = weapons[Random.Range(0, weapons.Count)];
                weapon.damage *= 1.2f; // 伤害提升20%
                weapon.orbitSpeed *= 1.1f; // 速度提升10%
            }
        }
        
        /// <summary>
        /// 创建武器拾取物品
        /// </summary>
        /// <param name="weapon">武器数据</param>
        /// <param name="position">生成位置</param>
        private void CreateWeaponPickup(Weapon weapon, Vector3 position)
        {
            if (weaponPickupPrefab != null)
            {
                GameObject pickup = Instantiate(weaponPickupPrefab, position, Quaternion.identity);
                pickup.name = "Weapon Pickup: " + weapon.weaponName;

                var weaponPickup = pickup.GetComponent("WeaponPickup");
                if (weaponPickup != null)
                {
                    var weaponField = weaponPickup.GetType().GetField("weapon");
                    if (weaponField != null) weaponField.SetValue(weaponPickup, weapon);
                }

                ApplyDropForce(pickup);
            }
        }

        /// <summary>
        /// 创建金币拾取物品
        /// </summary>
        /// <param name="position">生成位置</param>
        private void CreateGoldPickup(Vector3 position)
        {
            if (goldPickupPrefab != null)
            {
                GameObject pickup = Instantiate(goldPickupPrefab, position, Quaternion.identity);
                pickup.name = "Gold Pickup";
                ApplyDropForce(pickup);
            }
        }

        /// <summary>
        /// 创建回血道具拾取物品
        /// </summary>
        /// <param name="position">生成位置</param>
        private void CreateHealthPickup(Vector3 position)
        {
            if (healthPickupPrefab != null)
            {
                GameObject pickup = Instantiate(healthPickupPrefab, position, Quaternion.identity);
                pickup.name = "Health Pickup";
                ApplyDropForce(pickup);
            }
        }

        /// <summary>
        /// 创建护盾道具拾取物品
        /// </summary>
        /// <param name="position">生成位置</param>
        private void CreateShieldPickup(Vector3 position)
        {
            if (shieldPickupPrefab != null)
            {
                GameObject pickup = Instantiate(shieldPickupPrefab, position, Quaternion.identity);
                pickup.name = "Shield Pickup";
                ApplyDropForce(pickup);
            }
        }

        /// <summary>
        /// 应用掉落力
        /// </summary>
        private void ApplyDropForce(GameObject pickup)
        {
            Rigidbody2D rb = pickup.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 force = new Vector2(
                    Random.Range(-1f, 1f),
                    Random.Range(0.5f, 1f)
                ) * itemDropForce;
                rb.AddForce(force, ForceMode2D.Impulse);
            }
        }
        #endregion
        
        #region 系统集成
        /// <summary>
        /// 设置武器系统
        /// </summary>
        /// <param name="system">武器系统</param>
        public void SetWeaponSystem(WeaponOrbitalSystem system)
        {
            weaponSystem = system;
        }
        
        /// <summary>
        /// 添加可能的武器掉落
        /// </summary>
        /// <param name="weapon">武器数据</param>
        public void AddPossibleWeapon(Weapon weapon)
        {
            if (!possibleWeapons.Contains(weapon))
            {
                possibleWeapons.Add(weapon);
            }
        }
        #endregion
    }
}
