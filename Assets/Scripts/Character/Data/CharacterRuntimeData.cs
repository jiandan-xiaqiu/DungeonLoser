using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 角色运行时数据
/// </summary>
public class CharacterRuntimeData
{
    //上限
    public double CurrentHpMax;
    public int CurrentMpMax;
    public int CurrentStaMax;
    //当前值
    public double CurrentHP;
    public int CurrentMP;
    public double CurrentATK;
    public double CurrentDEF;
    public int CurrentVit;
    /// <summary>
    /// 初始化角色运行时数据,应该受角色基础数据与当前装备道具等共同影响，暂未实现
    /// </summary>
    public CharacterRuntimeData()
    {
        
    }
}
/// <summary>
/// 角色统计数据
/// </summary>
public class CharacterStats
{
    private CharacterRuntimeData _runtimeData;

    public CharacterStats(CharacterRuntimeData runtimeData)
    {
        _runtimeData = runtimeData;
    }
}