using System;
using System.Collections;
using System.Collections.Generic;
using MyGame.Character.Control;
using MyGame.Character.Systems;
using UnityEngine;
public enum State
{
    Idle, 
    Move, 
    Attack
}
/// <summary>
/// 基础状态类,状态类负责调用行为函数、动画逻辑，实现状态逻辑；同时检测状态转换
/// </summary>
public class BaseState : IState
{
    protected StateMachine _stateMachine;
    protected MovementController _movementController;
    protected AnimationSystem _animationSystem;
    protected String StateName;



    public BaseState(StateMachine stateMachine,
                                      MovementController movementController,
                                      AnimationSystem animationSystem,
                                      String stateName)
    {
        this._stateMachine = stateMachine;
        this._movementController = movementController;
        this._animationSystem = animationSystem;
        StateName = stateName;
    }

    public virtual void Enter()
    {
        //通过Handle切换的状态会消耗一次输入，缺少一次处理，需要状态机记录并在此补充处理
    }

    public virtual void Update()
    {
    }

    public virtual void Exit()
    {
    }

    public virtual string GetStateName()
    {
        return StateName;
    }
    
    public virtual bool CanTransitionTo(string stateName)
    {
        // 默认允许所有状态转换
        return true;
    }

    /// <summary>
    /// 处理移动输入
    /// </summary>
    /// <param name="moveInput">移动输入向量</param>
    public virtual void HandleMoveInput(Vector2 moveInput) { }
    
    /// <summary>
    /// 处理普通攻击输入
    /// </summary>
    public virtual void HandleAttackInput() { }
}