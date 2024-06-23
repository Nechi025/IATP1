using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashStateIdle<T> : State<T>
{
    Animator _anim;
    public CrashStateIdle(Animator anim)
    {
        _anim = anim;
    }
    public override void Enter()
    {
        base.Enter();
        //_anim.SetTrigger("Idle");
        _anim.SetFloat("Vel", 0);
    }
}
