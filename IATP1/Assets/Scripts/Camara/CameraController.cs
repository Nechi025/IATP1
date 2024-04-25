using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    ILineOfSight _los;
    FSM<CameraStatesEnum> _fsm;
    ITreeNode _root;

    private void Awake()
    {
        _los = GetComponent<ILineOfSight>();
        InitializeFSM();
        InitializeTree();
    }

    void InitializeFSM()
    {
        _fsm = new FSM<CameraStatesEnum>();

        var normal = new State<CameraStatesEnum>();
        var alert = GetComponent<CameraStateAlert>();

        normal.AddTransition(CameraStatesEnum.Alert, alert);
        alert.AddTransition(CameraStatesEnum.Normal, normal);

        _fsm.SetInit(normal);
    }

    void InitializeTree()
    {
        //Action
        var alert = new ActionNode(()=>_fsm.Transition(CameraStatesEnum.Alert));
        var normal = new ActionNode(() => _fsm.Transition(CameraStatesEnum.Normal));

        //Question
        var qLoS = new QuestionNode(QuestionLoS, alert, normal);

        _root = qLoS;
    }

    bool QuestionLoS()
    {
        return _los.CheckRange(target) && _los.CheckAngle(target) && _los.CheckView(target);
    }

    private void Update()
    {
        _fsm.OnUpdate();
        _root.Execute();
    }
}
