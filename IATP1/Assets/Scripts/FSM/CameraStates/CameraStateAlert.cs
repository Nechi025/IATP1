using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateAlert : StateMono<CameraStatesEnum>
{
    ILineOfSight _los;
    public CameraStatesEnum input;
    public Transform target;
    public Animator animator;

    private void Awake()
    {
        _los = GetComponent<LineOfSight>();
    }

    public override void Enter()
    {
        animator.SetFloat("PlayerInView", 1);
    }

    public override void Execute()
    {
        if (!_los.CheckRange(target) || !_los.CheckAngle(target) || !_los.CheckView(target))
        {
            _fsm.Transition(input);
        }
    }

    public override void Sleep()
    {
        animator.SetFloat("PlayerInView", 0);
    }
}
