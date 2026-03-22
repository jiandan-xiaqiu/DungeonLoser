using System;
using System.Collections;
using System.Collections.Generic;
using MyGame.Character.Control;
using MyGame.Character.Systems;
using UnityEngine;

public class AttackState : BaseState
{
    private float attackStartTime;
    private float attackDuration = 0.5f; // 攻击动作持续时间
    private bool hasAttacked = false;

    public AttackState(StateMachine stateMachine, MovementController movementController, AnimationSystem animationSystem) : base(stateMachine, movementController, animationSystem, "Attack")
    {
    }

    public override void Enter()
    {
        
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
    }

    public override bool CanTransitionTo(string stateName)
    {
        // 攻击状态只能在攻击完成后转换，所以这里返回false
        // 转换由状态内部逻辑控制
        return false;
    }
    public override void HandleMoveInput(Vector2 moveInput)
    {
        base.HandleMoveInput(moveInput);

        // 如果没有移动输入，停止移动
        if (moveInput.magnitude < 0.1f)
        {
            _movementController?.StopMovement();
            return;
        }

        // 如果有移动输入，执行移动逻辑
        if (moveInput.magnitude > 0.1f)
        {
            _movementController.Move(moveInput);
        }

    }
        
    /// <summary>
    /// 处理普通攻击输入
    /// 当有攻击输入时，切换到攻击状态
    /// </summary>
    public override void HandleAttackInput()
    {
        base.HandleAttackInput();

        //调用攻击系统
    }
}
