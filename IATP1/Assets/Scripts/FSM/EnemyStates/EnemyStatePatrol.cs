using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatePatrol<T> : State<T>
{
    Transform[] _points;
    int _nextPoint = 0;
    float _minDistance;
    EnemyModel _model;
    ObstacleAvoidance _obs;

    public EnemyStatePatrol(Transform[] points, float minDistance, EnemyModel model, ObstacleAvoidance obs)
    {
        _points = points;
        _minDistance = minDistance;
        _model = model;
        _obs = obs;
    }

    public override void Execute()
    {
        var dir = _obs.GetDir((_points[_nextPoint].position - _model.transform.position).normalized);
        _model.Move(dir.normalized);
        _model.LookDir(dir.normalized);

        if (Vector3.Distance(_model.transform.position, _points[_nextPoint].position) < _minDistance)
        {
            _nextPoint+=1;
            if (_nextPoint >= _points.Length)
            {
                _nextPoint = 0;
            }
        }
    }
}

