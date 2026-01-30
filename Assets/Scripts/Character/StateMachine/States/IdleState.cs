using System;
using System.Collections;
using System.Collections.Generic;
using MyGame.Character.Control;
using MyGame.Character.Systems;
using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(StateMachine stateMachine, MovementController movementController, AnimationSystem animationSystem) : base(stateMachine, movementController, animationSystem, "Idle")
    {
    }

    public override void Enter()
    {
        base.Enter();

        _movementController?.StopMovement();

        // 播放空闲动画
        _animationSystem?.PlayIdleAnimation();
    }

    /// <summary>
    /// 退出空闲状态
    /// </summary>
    public override void Exit()
    {
        base.Exit();
    }

    /// <summary>
    /// 更新空闲状态
    /// </summary>
    public override void Update()
    {
        base.Update();
        // 检查是否有其他输入需要处理
    }

    #region 输入处理
    /// <summary>
    /// 处理移动输入
    /// 当有移动输入时，切换到移动状态
    /// </summary>
    /// <param name="movementInput">移动输入向量</param>
    public override void HandleMoveInput(Vector2 movementInput)
    {
        base.HandleMoveInput(movementInput);

        // 如果有移动输入，切换到移动状态
        if (movementInput.magnitude > 0.1f)
        {
            _stateMachine.ChangeState("Move");
        }
    }
    

    public override bool CanTransitionTo(string stateName)
    {
        // 可以从空闲状态转换到移动、攻击或休息状态
        return stateName == "Move" || stateName == "Attack";
    }
    #endregion
}
