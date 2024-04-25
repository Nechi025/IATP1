using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    ILineOfSight _los;
    FSM<CameraStatesEnum> _fsm;

    private void Awake()
    {
        _los = GetComponent<ILineOfSight>();
        InitializeFSM();
    }

    void InitializeFSM()
    {
        _fsm = new FSM<CameraStatesEnum>();

        var normal = new CameraStateNormal<CameraStatesEnum>(_los, target, CameraStatesEnum.Alert);
        var alert = GetComponent<CameraStateAlert>();

        normal.AddTransition(CameraStatesEnum.Alert, alert);
        alert.AddTransition(CameraStatesEnum.Normal, normal);

        _fsm.SetInit(normal);
    }

    private void Update()
    {
        _fsm.OnUpdate();
    }
}
