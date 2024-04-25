using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateWalk<T> : State<T>
{
    PlayerModel _player;
    T _idleInput;

    public PlayerStateWalk(PlayerModel player, T idleInput)
    {
        _player = player;
        _idleInput = idleInput;
    }

    public override void Execute()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(x, 0f, z).normalized;
        _player.Move(dir);

        if (x == 0 && z == 0)
        {
            _player.controller.Move(new Vector3(0, 0, 0));
            _fsm.Transition(_idleInput);
        }
    }
}
