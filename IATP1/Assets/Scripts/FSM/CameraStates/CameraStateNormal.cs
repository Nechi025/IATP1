using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateNormal<T> : State<T>
{
    ILineOfSight _los;
    Transform _target;
    T _inputAlert;

    public CameraStateNormal(ILineOfSight los, Transform target, T inputAlert)
    {
        _los = los;
        _target = target;
        _inputAlert = inputAlert;
    }

    public override void Execute()
    {
        if (_los.CheckRange(_target) && _los.CheckAngle(_target) && _los.CheckView(_target))
        {
            _fsm.Transition(_inputAlert);
        }
    }
}
