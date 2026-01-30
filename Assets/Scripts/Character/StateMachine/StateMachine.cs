using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame.Character.Control;
using MyGame.Character.Systems;
//using System.Diagnostics;

/// <summary>
/// 状态机,监听玩家输入,做出决策
/// </summary>
public class StateMachine : MonoBehaviour
{
    private IState currentState;
    private IState previousState;
    private Dictionary<string, IState> states = new Dictionary<string, IState>();
    
    private PlayerController player;
    // 各系统引用
    private MovementController movementController;
    private AnimationSystem animationSystem;
    

    #region 初始化状态
    private void InitializeStates()
    {
        player = GetComponent<PlayerController>();
        movementController = player.GetComponent<MovementController>();
        animationSystem = player.GetComponent<AnimationSystem>();
        if (player == null)
        {
            Debug.LogError("状态机未找到"+nameof(PlayerController));
            return;
        }
        if (movementController == null)
        {
            Debug.LogError("状态机未找到"+nameof(MovementController));
            return;
        }
        if (animationSystem == null)
        {
            Debug.LogError("状态机未找到"+nameof(AnimationSystem));
            return;
        }
    }

    private void RegisterAllStates()
    {
        // 注册所有状态
        RegisterState(new IdleState(this, movementController, animationSystem));
        RegisterState(new MoveState(this, movementController, animationSystem));
        RegisterState(new AttackState(this, movementController, animationSystem));
        
        // 初始状态
        ChangeState("Idle");

    }
    #endregion
    
    
    #region 生命周期
    void Start()
    {
        InitializeStates();
        SubscribeToEvents();
        RegisterAllStates();
    }
        public void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
    }
    #endregion

    /// <summary>
    /// 订阅玩家输入事件
    /// </summary>
    private void SubscribeToEvents()
    {
        player.OnMoveInput += HandleMoveInput;
    }
    private void RegisterState(IState state)
    {
        string stateName = state.GetStateName();
        states[stateName] = state;
    }
    
    public void ChangeState(string stateName)
    {
        if (!states.ContainsKey(stateName))
        {
            Debug.LogError($"状态 {stateName} 未注册");
            return;
        }
        if (currentState != null)
        {
            if (!currentState.CanTransitionTo(stateName))
            {
                Debug.LogWarning($"不允许从 {currentState.GetStateName()} 转移到 {stateName}");
                return;
            }
            
            previousState = currentState;
            currentState.Exit();
        }
        currentState = states[stateName];
        Debug.Log($"切换到状态 {stateName}");
        currentState.Enter();
        
    }
    
    public string GetCurrentStateName()
    {
        return currentState?.GetStateName() ?? "None";
    }
    /// <summary>
    /// 返回上一个状态
    /// </summary>
    public void ReturnToPreviousState()
    {
        if (previousState != null)
        {
            ChangeState(previousState.GetStateName());
        }
    }
    
    public IState GetState(string stateName)
    {
        if (states.ContainsKey(stateName))
            return states[stateName];
        return null;
    }
    #region 事件处理

    /// <summary>
    /// 处理移动输入
    /// </summary>
    /// <param name="moveInput">移动输入向量</param>
    private void HandleMoveInput(Vector2 moveInput)
    {
        currentState.HandleMoveInput(moveInput);
    }

    #endregion
}
