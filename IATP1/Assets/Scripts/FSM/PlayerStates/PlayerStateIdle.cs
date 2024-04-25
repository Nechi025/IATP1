using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateIdle<T> : State<T>
{
    T _inputMovement;
    public PlayerStateIdle(T inputMovement)
    {
        _inputMovement = inputMovement;
    }

    public override void Execute()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (x != 0 || z != 0)
        {
            _fsm.Transition(_inputMovement);
        }
    }
}
