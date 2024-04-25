using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    PlayerModel _player;
    PlayerView _view;
    FSM<StatesEnum> _fsm;

    private void Awake()
    {
        _player = GetComponent<PlayerModel>();
        _view = GetComponent<PlayerView>();
        InitializeFSM();
    }

    void InitializeFSM()
    {
        _fsm = new FSM<StatesEnum>();

        var idle = new PlayerStateIdle<StatesEnum>(StatesEnum.Walk);
        var walk = new PlayerStateWalk<StatesEnum>(_player, StatesEnum.Idle);

        idle.AddTransition(StatesEnum.Walk, walk);
        walk.AddTransition(StatesEnum.Idle, idle);

        _fsm.SetInit(idle);
    }

    private void Update()
    {
        _fsm.OnUpdate();
    }
}
