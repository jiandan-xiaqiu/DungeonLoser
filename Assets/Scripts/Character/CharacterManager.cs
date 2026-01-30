using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;
using MyGame.Character.Systems;

public class CharacterManager : Singleton<CharacterManager>
{
    #region 字段
    private CharacterRuntimeData _characterRuntimeData;
    private HealthSystem _healthSystem;
    #endregion
    /// <summary>
    /// 初始化角色管理器
    /// </summary>
    /// <param name="characterRuntimeData">角色运行时数据</param>
    public void Initialize()
    {
        _characterRuntimeData = new CharacterRuntimeData();
        _healthSystem = new HealthSystem(_characterRuntimeData);
    }

    void Awake()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
