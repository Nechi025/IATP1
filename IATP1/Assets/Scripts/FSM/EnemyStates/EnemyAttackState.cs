using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAttackState<T> : State<T>
{
    EnemyModel _model;

    public EnemyAttackState(EnemyModel model)
    {
        _model = model;
    }

    public override void Execute()
    {
        _model.Attack();
        GameManager.Instance.YouLose();
    }
}
