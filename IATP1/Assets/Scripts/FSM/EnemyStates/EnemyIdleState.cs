using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState<T> : State<T>
{
    EnemyModel _model;

    public EnemyIdleState(EnemyModel model)
    {
        _model = model;
    }

    public override void Execute()
    {
        _model.Move(new Vector3(0, 0, 0));
    }
}
