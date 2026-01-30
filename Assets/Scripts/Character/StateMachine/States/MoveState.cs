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
        // animator.SetBool("IsMoving", true);
        // character.IsMoving = true;
        // Logger.Log.Log.Info(LogModules.PLAYER, "Entering Move State", character);
    }

    public override void Update()
    {
        Vector2 moveInput = character.InputActions.GamePlay.Move.ReadValue<Vector2>();
        
        // 如果没有移动输入，切换到空闲状态
        if (moveInput.magnitude < 0.1f)
        {
            character.StateMachine.ChangeState(new IdleState(character));
            return;
        }

        // 如果按住奔跑键，切换到奔跑状态
        if (character.InputActions.GamePlay.Run.ReadValue<float>() > 0.5f)
        {
            character.StateMachine.ChangeState(new RunState(character));
            return;
        }

        // 如果有攻击输入，切换到攻击状态
        if (character.InputActions.GamePlay.Attack.triggered)
        {
            character.StateMachine.ChangeState(new AttackState(character));
            return;
        }

        // 执行移动逻辑
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        Debug.Log(moveInput.x);
        character.MoveCharacter(moveDirection.normalized, character.MoveSpeed);
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
    public override void HandleMoveInput(Vector2 moveInput)
    {
        base.HandleMoveInput(moveInput);
    }
}
