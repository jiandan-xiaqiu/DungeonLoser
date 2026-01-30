using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{

    void Enter();
    void Update();
    void Exit();
    
    /// <summary>
    /// 检查是否可以转换到目标状态
    /// </summary>
    /// <param name="stateName">目标状态名称</param>
    /// <returns>如果可以转换则返回true，否则返回false</returns>
    
    bool CanTransitionTo(string stateName);

    string GetStateName();

    /// <summary>
    /// 处理移动输入
    /// </summary>
    /// <param name="moveInput">移动输入向量</param>
    void HandleMoveInput(Vector2 moveInput);
}
