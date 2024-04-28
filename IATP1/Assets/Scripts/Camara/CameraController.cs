using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    CameraModel _model;
    ILineOfSight _los;
    FSM<CameraStatesEnum> _fsm;
    ITreeNode _root;
    

    private void Awake()
    {
        _los = GetComponent<ILineOfSight>();
        _model = GetComponent<CameraModel>();
        InitializeFSM();
        InitializeTree();
    }

    void InitializeFSM()
    {
        _fsm = new FSM<CameraStatesEnum>();

        var normal = new State<CameraStatesEnum>();
        var alert = GetComponent<CameraStateAlert>();
        var inactive = new State<CameraStatesEnum>();

        normal.AddTransition(CameraStatesEnum.Alert, alert);
        normal.AddTransition(CameraStatesEnum.Default, inactive);

        alert.AddTransition(CameraStatesEnum.Normal, normal);

        inactive.AddTransition(CameraStatesEnum.Normal, normal);

        _fsm.SetInit(normal);
    }

    void InitializeTree()
    {
        //Action
        var alert = new ActionNode(()=>_fsm.Transition(CameraStatesEnum.Alert));

        var normal = new ActionNode(() => _fsm.Transition(CameraStatesEnum.Normal));
        var inactive = new ActionNode(() => _fsm.Transition(CameraStatesEnum.Default));

        //Question
        var qLoS = new QuestionNode(QuestionLoS, alert, normal);
        var qActive = new QuestionNode(QuestionActive, qLoS, inactive);

        _root = qActive;
    }

    bool QuestionLoS()
    {
        return _los.CheckRange(target) && _los.CheckAngle(target) && _los.CheckView(target);
    }

    bool QuestionActive()
    {
        return _model.anim.GetBool("Active");
    }

    private void Update()
    {
        _fsm.OnUpdate();
        _root.Execute();

        if (_model.anim.GetBool("Active") == true)
        _model.Alert = QuestionLoS();
        else _model.Alert = false;
    }
}
