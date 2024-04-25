using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    PlayerModel _player;
    PlayerView _view;
    FSM<PlayerStatesEnum> _fsm;

    private void Awake()
    {
        _player = GetComponent<PlayerModel>();
        _view = GetComponent<PlayerView>();
        InitializeFSM();
    }

    void InitializeFSM()
    {
        _fsm = new FSM<PlayerStatesEnum>();

        var idle = new PlayerStateIdle<PlayerStatesEnum>(PlayerStatesEnum.Walk);
        var walk = new PlayerStateWalk<PlayerStatesEnum>(_player, PlayerStatesEnum.Idle);

        idle.AddTransition(PlayerStatesEnum.Walk, walk);
        walk.AddTransition(PlayerStatesEnum.Idle, idle);

        _fsm.SetInit(idle);
    }

    private void Update()
    {
        _fsm.OnUpdate();
    }
}
