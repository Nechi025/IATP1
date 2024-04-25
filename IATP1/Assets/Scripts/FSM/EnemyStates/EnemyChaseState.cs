using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState<T> : State<T>
{
    EnemyModel _model;
    Transform _target;

    public EnemyChaseState(EnemyModel model, Transform target)
    {
        _model = model;
        _target = target;
    }

    public override void Execute()
    {
        Vector3 dir = _target.position - _model.transform.position;
        _model.Move(dir.normalized);
        _model.LookDir(dir.normalized);
    }
}
