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
        [SerializeField] private float weaponDropChance = 0.3f; // 武器掉落概率
        [SerializeField] private float weaponUpgradeChance = 0.2f; // 武器升级概率
        [SerializeField] private float itemDropForce = 5f; // 掉落力度
        [SerializeField] private float pickupRange = 1f; // 拾取范围
        [SerializeField] private float attractSpeed = 5f; // 吸引速度
        
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
                
                if (rand <= weaponDropChance && possibleWeapons.Count > 0)
                {
                    DropWeapon(enemy.transform.position);
                }
                else if (rand <= weaponDropChance + weaponUpgradeChance && weaponSystem != null)
                {
                    UpgradeExistingWeapon();
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
            // 创建拾取物品
            GameObject pickup = new GameObject("Weapon Pickup: " + weapon.weaponName);
            pickup.transform.position = position;
            
            // 添加碰撞器
            CircleCollider2D collider = pickup.AddComponent<CircleCollider2D>();
            collider.radius = 0.5f;
            collider.isTrigger = true;
            
            // 添加刚体
            Rigidbody2D rb = pickup.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            
            // 应用掉落力
            Vector2 force = new Vector2(
                Random.Range(-1f, 1f),
                Random.Range(0.5f, 1f)
            ) * itemDropForce;
            rb.AddForce(force, ForceMode2D.Impulse);
            
            // 添加拾取组件
            WeaponPickup weaponPickup = pickup.AddComponent<WeaponPickup>();
            weaponPickup.weapon = weapon;
            weaponPickup.pickupRange = pickupRange;
            weaponPickup.attractSpeed = attractSpeed;
            weaponPickup.weaponSystem = weaponSystem;
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
    
    /// <summary>
    /// 武器拾取组件
    /// </summary>
    public class WeaponPickup : MonoBehaviour
    {
        public Weapon weapon;
        public float pickupRange = 1f;
        public float attractSpeed = 5f;
        public WeaponOrbitalSystem weaponSystem;
        
        private Transform playerTransform;
        private bool isAttracting = false;
        
        private void Update()
        {
            if (playerTransform == null)
            {
                FindPlayer();
                return;
            }
            
            // 检查是否在拾取范围内
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            
            if (distance <= pickupRange)
            {
                isAttracting = true;
            }
            
            if (isAttracting)
            {
                // 吸引到玩家
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    playerTransform.position,
                    attractSpeed * Time.deltaTime
                );
                
                // 到达玩家
                if (Vector3.Distance(transform.position, playerTransform.position) < 0.1f)
                {
                    Pickup();
                }
            }
        }
        
        /// <summary>
        /// 寻找玩家
        /// </summary>
        private void FindPlayer()
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }
        
        /// <summary>
        /// 拾取武器
        /// </summary>
        private void Pickup()
        {
            if (weapon != null && weaponSystem != null)
            {
                weaponSystem.AddWeapon(weapon);
            }
            
            Destroy(gameObject);
        }
    }
}
