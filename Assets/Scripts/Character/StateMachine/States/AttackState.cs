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
        // animator.SetTrigger("Attack");
        // attackStartTime = Time.time;
        // hasAttacked = false;
        // Logger.Log.Log.Info(LogModules.PLAYER, "Entering Attack State", character);
    }

    public override void Update()
    {
        // 执行攻击逻辑
        // if (!hasAttacked && Time.time - attackStartTime > 0.2f) // 攻击动画的攻击帧时间点
        // {
        //     character.PerformAttack();
        //     hasAttacked = true;
        // }

        // // 检查攻击是否完成
        // if (Time.time - attackStartTime >= attackDuration)
        // {
        //     // 根据移动输入决定切换到空闲或移动状态
        //     Vector2 moveInput = character.InputActions.GamePlay.Move.ReadValue<Vector2>();
        //     if (moveInput.magnitude > 0.1f)
        //     {
        //         if (character.InputActions.GamePlay.Run.ReadValue<float>() > 0.5f)
        //         {
        //             character.StateMachine.ChangeState(new RunState(character));
        //         }
        //         else
        //         {
        //             character.StateMachine.ChangeState(new MoveState(character));
        //         }
        //     }
        //     else
        //     {
        //         character.StateMachine.ChangeState(new IdleState(character));
        //     }
        // }
    }

    public override void Exit()
    {
        //Logger.Log.Log.Info(LogModules.PLAYER, "Exiting Attack State", character);
    }

    public override bool CanTransitionTo(string stateName)
    {
        // 攻击状态只能在攻击完成后转换，所以这里返回false
        // 转换由状态内部逻辑控制
        return false;
    }
}
