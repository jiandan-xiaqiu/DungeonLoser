using UnityEngine;
using System.Collections.Generic;

namespace SimpleOrbital
{
    /// <summary>
    /// 武器环绕系统，管理武器的环绕行为和碰撞检测
    /// </summary>
    public class WeaponOrbitalSystem : MonoBehaviour
    {
        [Header("系统设置")]
        [SerializeField] private Transform characterTransform; // 角色transform
        [SerializeField] private float damageCheckInterval = 0.1f; // 伤害检测间隔
        [SerializeField] private LayerMask enemyLayer; // 敌人图层
        [SerializeField] private bool autoEquipDefaultWeapon = true; // 是否自动装备默认武器
        [SerializeField] private Weapon defaultWeapon; // 默认武器
        
        // 武器列表
        private List<Weapon> weapons = new List<Weapon>();
        
        // 碰撞检测缓存
        private Dictionary<Collider2D, float> lastDamageTime = new Dictionary<Collider2D, float>();
        private float lastCheckTime = 0f;
        
        #region 初始化
        private void Awake()
        {
            if (characterTransform == null)
            {
                characterTransform = transform;
            }
            
            // 自动装备默认武器
            if (autoEquipDefaultWeapon && defaultWeapon != null)
            {
                AddWeapon(defaultWeapon);
            }
        }
        
        private void Update()
        {
            UpdateWeapons();
            
            // 检查碰撞
            if (Time.time - lastCheckTime >= damageCheckInterval)
            {
                CheckWeaponCollisions();
                lastCheckTime = Time.time;
            }
        }
        #endregion
        
        #region 武器管理
        /// <summary>
        /// 添加武器
        /// </summary>
        /// <param name="weapon">武器数据</param>
        public void AddWeapon(Weapon weapon)
        {
            // 克隆武器数据
            Weapon newWeapon = new Weapon();
            CopyWeaponData(weapon, newWeapon);
            
            // 计算初始角度
            float angleStep = 360f / (weapons.Count + 1);
            newWeapon.currentAngle = weapons.Count * angleStep;
            
            // 创建武器实例
            if (weapon.weaponPrefab != null)
            {
                newWeapon.weaponInstance = Instantiate(weapon.weaponPrefab, characterTransform.position, Quaternion.identity);
                newWeapon.weaponInstance.transform.SetParent(transform);
            }
            
            weapons.Add(newWeapon);
        }
        
        /// <summary>
        /// 移除武器
        /// </summary>
        /// <param name="index">武器索引</param>
        public void RemoveWeapon(int index)
        {
            if (index >= 0 && index < weapons.Count)
            {
                // 销毁武器实例
                if (weapons[index].weaponInstance != null)
                {
                    Destroy(weapons[index].weaponInstance);
                }
                
                weapons.RemoveAt(index);
            }
        }
        
        /// <summary>
        /// 移除所有武器
        /// </summary>
        public void ClearWeapons()
        {
            foreach (var weapon in weapons)
            {
                if (weapon.weaponInstance != null)
                {
                    Destroy(weapon.weaponInstance);
                }
            }
            weapons.Clear();
        }
        
        /// <summary>
        /// 获取武器数量
        /// </summary>
        /// <returns>武器数量</returns>
        public int GetWeaponCount()
        {
            return weapons.Count;
        }
        
        /// <summary>
        /// 获取武器列表
        /// </summary>
        /// <returns>武器列表</returns>
        public List<Weapon> GetWeapons()
        {
            return weapons;
        }
        
        /// <summary>
        /// 复制武器数据
        /// </summary>
        /// <param name="source">源武器</param>
        /// <param name="target">目标武器</param>
        private void CopyWeaponData(Weapon source, Weapon target)
        {
            target.weaponName = source.weaponName;
            target.weaponPrefab = source.weaponPrefab;
            target.damage = source.damage;
            target.attackRange = source.attackRange;
            target.orbitRadius = source.orbitRadius;
            target.orbitSpeed = source.orbitSpeed;
        }
        #endregion
        
        #region 武器更新
        /// <summary>
        /// 更新所有武器
        /// </summary>
        private void UpdateWeapons()
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                UpdateWeaponPosition(weapons[i], i);
                UpdateWeaponRotation(weapons[i]);
            }
        }
        
        /// <summary>
        /// 更新武器位置
        /// </summary>
        /// <param name="weapon">武器</param>
        /// <param name="index">武器索引</param>
        private void UpdateWeaponPosition(Weapon weapon, int index)
        {
            if (weapon == null || characterTransform == null)
                return;
            
            // 更新角度
            weapon.currentAngle += weapon.orbitSpeed * Time.deltaTime * 360f;
            if (weapon.currentAngle >= 360f)
            {
                weapon.currentAngle -= 360f;
            }
            
            // 计算位置
            float angleRad = weapon.currentAngle * Mathf.Deg2Rad;
            float x = Mathf.Cos(angleRad) * weapon.orbitRadius;
            float y = Mathf.Sin(angleRad) * weapon.orbitRadius;
            Vector3 newPosition = characterTransform.position + new Vector3(x, y, 0f);
            
            // 更新武器实例位置
            if (weapon.weaponInstance != null)
            {
                weapon.weaponInstance.transform.position = newPosition;
            }
        }
        
        /// <summary>
        /// 更新武器旋转
        /// </summary>
        /// <param name="weapon">武器</param>
        private void UpdateWeaponRotation(Weapon weapon)
        {
            if (weapon == null || weapon.weaponInstance == null)
                return;
            
            // 计算旋转方向（朝向移动方向）
            float angleRad = weapon.currentAngle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
            float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            
            weapon.weaponInstance.transform.rotation = Quaternion.Euler(0f, 0f, rotation);
        }
        #endregion
        
        #region 碰撞检测
        /// <summary>
        /// 检查武器碰撞
        /// </summary>
        private void CheckWeaponCollisions()
        {
            foreach (var weapon in weapons)
            {
                if (weapon == null || weapon.weaponInstance == null)
                    continue;
                
                // 检查武器是否可以攻击
                if (!weapon.CanAttack())
                    continue;
                
                // 检测碰撞
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(
                    weapon.weaponInstance.transform.position,
                    weapon.attackRange,
                    enemyLayer
                );
                
                foreach (var hitCollider in hitColliders)
                {
                    // 检查是否是敌人
                    if (hitCollider.CompareTag("Enemy"))
                    {
                        // 检查是否在冷却时间内
                        if (lastDamageTime.TryGetValue(hitCollider, out float lastTime))
                        {
                            if (Time.time - lastTime < 0.5f)
                                continue;
                        }
                        
                        // 应用伤害
                        EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
                        if (enemyHealth != null)
                        {
                            enemyHealth.TakeDamage(weapon.damage);
                        }
                        
                        // 记录伤害时间
                        lastDamageTime[hitCollider] = Time.time;
                        
                        // 执行攻击
                        weapon.PerformAttack();
                    }
                }
            }
        }
        #endregion
    }
}
