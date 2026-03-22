using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using MyGame.Character.Control;
using MyGame.Character.Systems;
using UnityEngine;

public class MoveState : BaseState
{
    public MoveState(StateMachine stateMachine, MovementController movementController, AnimationSystem animationSystem) : base(stateMachine, movementController, animationSystem, "Move")
    {
    }

    public override void Enter()
    {
        _movementController?.StopMovement();

        // 第一次移动输入被前一个状态切换时消耗了，所以这里需要重新补充处理一次
        HandleMoveInput(_stateMachine.GetCurrentMoveInput());
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
        //Logger.Log.Log.Info(LogModules.PLAYER, "Exiting Move State", character);
    }

    public override bool CanTransitionTo(string stateName)
    {
        // 可以从移动状态转换到空闲或攻击状态
        return stateName == "Idle" || stateName == "Attack";
    }
    //当前状态有移动输入时的处理，
    public override void HandleMoveInput(Vector2 moveInput)
    {
        base.HandleMoveInput(moveInput);

        // 如果没有移动输入，切换到空闲状态
        if (moveInput.magnitude < 0.1f)
        {
            //_movementController?.StopMovement();
            _stateMachine.ChangeState("Idle");
            return;
        }

        // 如果有移动输入，执行移动逻辑
        if (moveInput.magnitude > 0.1f)
        {
            _movementController.Move(moveInput);
            
            // 播放移动动画
            _animationSystem?.PlayMoveAnimation(moveInput);
        }

    }
        
    /// <summary>
    /// 处理攻击输入
    /// 当有攻击输入时，切换到攻击状态
    /// </summary>
    public override void HandleAttackInput()
    {
        base.HandleAttackInput();

        // 如果有攻击输入，切换到攻击状态
        _stateMachine.ChangeState("Attack");
    }
}
