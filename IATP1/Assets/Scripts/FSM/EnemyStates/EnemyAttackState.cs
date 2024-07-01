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

    IEnumerator second()
    {
        yield return new WaitForSeconds(5);
    }

    public override void Execute()
    {
        _model.Attack();

        SceneManager.LoadScene("LoseScreen");
    }
}
