using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateAlert : StateMono<CameraStatesEnum>
{
    public Animator animator;

    public override void Enter()
    {
        animator.SetFloat("PlayerInView", 1);
    }

    public override void Sleep()
    {
        animator.SetFloat("PlayerInView", 0);
    }
}
