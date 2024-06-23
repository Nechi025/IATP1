using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    PlayerModel _player;
    PlayerView _view;
    FSM<StatesEnumAll> _fsm;

    private void Awake()
    {
        _player = GetComponent<PlayerModel>();
        _view = GetComponent<PlayerView>();
        InitializeFSM();
    }

    void InitializeFSM()
    {
        _fsm = new FSM<StatesEnumAll>();

        var idle = new PlayerStateIdle<StatesEnumAll>(StatesEnumAll.Walk);
        var walk = new PlayerStateWalk<StatesEnumAll>(_player, StatesEnumAll.Idle);

        idle.AddTransition(StatesEnumAll.Walk, walk);
        walk.AddTransition(StatesEnumAll.Idle, idle);

        _fsm.SetInit(idle);
    }

    private void Update()
    {
        _fsm.OnUpdate();
    }
}
