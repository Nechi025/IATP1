using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateInactive<T> : State<T>
{
    public Animator animator;

    public override void Enter()
    {
        animator.SetBool("Active", false);
    }

    public override void Sleep()
    {
        animator.SetBool("Active", true);
    }
}
