using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReloadState<T> : State<T>
{
    EnemyModel _model;

    public EnemyReloadState(EnemyModel model)
    {
        _model = model;
    }

    public override void Execute()
    {
        _model.Reload();
    }
}
