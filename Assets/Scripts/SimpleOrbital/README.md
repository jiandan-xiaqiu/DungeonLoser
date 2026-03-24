# 简单武器环绕系统 (Simple Orbital System)

一个简化版的武器环绕系统，专注于核心功能：武器旋转、自动伤害和敌人掉落。适用于肉鸽（Roguelike）游戏。

## 系统架构

```
┌─────────────────────────────────────────────────┐
│                 简单武器环绕系统                  │
├───────────────────┬───────────────────┬───────────┤
│ WeaponOrbitalSystem │    EnemyHealth    │ ItemDropSystem │
│  (武器环绕逻辑)   │  (敌人健康)       │  (物品掉落)   │
└───────────────────┴───────────────────┴───────────┘
```

## 核心功能

1. **武器环绕**：武器自动围绕角色旋转
2. **自动攻击**：武器接触敌人时自动造成伤害
3. **敌人掉落**：击杀敌人后掉落武器或升级现有武器
4. **肉鸽元素**：随机掉落和武器升级系统

## 系统组件

- **`Weapon.cs`** - 武器数据类，定义武器基本属性
- **`WeaponOrbitalSystem.cs`** - 核心系统，管理武器环绕和碰撞检测
- **`EnemyHealth.cs`** - 敌人健康组件，处理敌人生命值和死亡
- **`ItemDropSystem.cs`** - 物品掉落系统，处理敌人死亡掉落
- **`SimpleOrbitalTest.cs`** - 测试脚本
- **`README.md`** - 使用说明

## 快速开始

### 方法一：使用测试场景

1. 创建一个新的空场景
2. 创建一个空GameObject，添加 `SimpleOrbitalTest` 组件
3. 配置测试参数：
   - `Weapon Prefab` - 武器预制体（可选）
   - `Enemy Count` - 敌人数量
   - 武器属性设置
4. 运行场景，使用以下控制：
   - `WASD` - 移动玩家
   - `Space` - 添加新武器
   - `C` - 清除所有武器
   - `I` - 查看武器信息

### 方法二：手动集成

1. **添加必要组件**：
   - 给玩家GameObject添加以下组件：
     - `WeaponOrbitalSystem`
     - `ItemDropSystem`

2. **配置武器系统**：
   ```csharp
   // 创建武器数据
   Weapon sword = new Weapon();
   sword.weaponName = "Sword";
   sword.weaponPrefab = weaponPrefab;
   sword.damage = 20f;
   sword.attackRange = 0.5f;
   sword.orbitRadius = 2f;
   sword.orbitSpeed = 3f;
   
   // 添加武器
   WeaponOrbitalSystem weaponSystem = player.GetComponent<WeaponOrbitalSystem>();
   weaponSystem.AddWeapon(sword);
   ```

3. **配置敌人**：
   - 给敌人GameObject添加 `EnemyHealth` 组件
   - 注册敌人到掉落系统：
   ```csharp
   ItemDropSystem dropSystem = player.GetComponent<ItemDropSystem>();
   dropSystem.RegisterEnemy(enemy);
   ```

4. **配置物品掉落**：
   ```csharp
   // 添加可能的武器掉落
   dropSystem.AddPossibleWeapon(sword);
   ```

## 系统配置

### WeaponOrbitalSystem
- `Character Transform` - 角色的Transform（默认为自身）
- `Damage Check Interval` - 伤害检测间隔
- `Enemy Layer` - 敌人图层
- `Auto Equip Default Weapon` - 是否自动装备默认武器
- `Default Weapon` - 默认武器配置

### ItemDropSystem
- `Drop Chance` - 掉落概率
- `Weapon Drop Chance` - 武器掉落概率
- `Weapon Upgrade Chance` - 武器升级概率
- `Item Drop Force` - 掉落力度
- `Pickup Range` - 拾取范围
- `Attract Speed` - 吸引速度

## 示例代码

### 1. 动态添加武器
```csharp
public void AddRandomWeapon()
{
    Weapon newWeapon = new Weapon();
    newWeapon.weaponName = "Random Weapon";
    newWeapon.weaponPrefab = weaponPrefab;
    newWeapon.damage = Random.Range(10f, 30f);
    newWeapon.orbitRadius = Random.Range(1.5f, 3f);
    newWeapon.orbitSpeed = Random.Range(2f, 4f);
    
    weaponSystem.AddWeapon(newWeapon);
}
```

### 2. 自定义掉落逻辑
```csharp
public void CustomDropLogic(GameObject enemy)
{
    // 基于敌人类型决定掉落
    if (enemy.CompareTag("Boss"))
    {
        // 必掉武器
        DropWeapon(enemy.transform.position);
    }
    else if (enemy.CompareTag("Elite"))
    {
        // 高概率掉落升级
        if (Random.value < 0.5f)
        {
            UpgradeExistingWeapon();
        }
    }
}
```

## 注意事项

1. **图层设置**：
   - 确保敌人在正确的图层（默认：Enemy）
   - 玩家应在 Player 图层

2. **预制体配置**：
   - 武器预制体应包含 Collider2D（用于视觉效果）
   - 敌人预制体应包含 Collider2D（用于碰撞检测）

3. **性能监控**：
   - 当武器数量较多时，注意监控性能
   - 建议武器数量不超过 8-10 个

## 版本信息

- **版本**：1.0.0
- **兼容**：Unity 2020.3+
- **平台**：2D 游戏

## 核心特性

- **简单易用**：组件化设计，易于集成
- **高度可扩展**：支持添加新武器类型
- **性能优化**：高效的碰撞检测
- **完整功能**：武器环绕、自动攻击、敌人掉落

这个系统完全独立于您现有的状态机系统，可以作为一个全新的战斗模式使用。您可以根据需要扩展系统，添加更多武器类型和游戏机制。
