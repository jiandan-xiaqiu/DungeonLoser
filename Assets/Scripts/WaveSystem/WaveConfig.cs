using UnityEngine;

namespace MyGame.WaveSystem
{
    /// <summary>
    /// 单个敌人类型配置
    /// </summary>
    [System.Serializable]
    public class EnemyTypeConfig
    {
        [Tooltip("敌人预制体")]
        public GameObject enemyPrefab;

        [Tooltip("该类型敌人在本轮中的数量")]
        public int count = 5;

        [Tooltip("该类型敌人的权重（影响随机选择）")]
        public float weight = 1f;
    }

    /// <summary>
    /// 关卡/轮次配置
    /// </summary>
    [System.Serializable]
    public class WaveConfig
    {
        [Tooltip("轮次编号")]
        public int waveNumber = 1;

        [Tooltip("本轮总敌人数量")]
        public int totalEnemyCount = 10;

        [Tooltip("敌人生成间隔（秒）")]
        public float spawnInterval = 2f;

        [Tooltip("生成半径")]
        public float spawnRadius = 10f;

        [Tooltip("最小生成距离")]
        public float spawnAreaMin = 5f;

        [Tooltip("本轮敌人类型配置")]
        public EnemyTypeConfig[] enemyTypes;

        [Tooltip("本轮完成后进入下一轮的等待时间")]
        public float nextWaveDelay = 5f;
    }
}