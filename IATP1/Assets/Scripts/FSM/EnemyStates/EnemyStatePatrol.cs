using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatePatrol<T> : State<T>, IPoints
{
    EnemyModel _model;
    List<Vector3> _waypoints;
    int _nextPoint = 0;
    bool _isFinishPath = true;
    ObstacleAvoidance _obs;
    public EnemyStatePatrol(EnemyModel model, ObstacleAvoidance obs)
    {
        _model = model;
        _obs = obs;
    }
    public override void Execute()
    {
        base.Execute();
        Run();
    }

    public void SetWayPoints(List<Node> newPoints)
    {
        var list = new List<Vector3>();
        for (int i = 0; i < newPoints.Count; i++)
        {
            list.Add(newPoints[i].transform.position);
        }
        SetWayPoints(list);
    }
    public void SetWayPoints(List<Vector3> newPoints)
    {
        _nextPoint = 0;
        if (newPoints.Count == 0) return;
        //_anim.Play("CIA_Idle");
        _waypoints = newPoints;
        var pos = _waypoints[_nextPoint];
        pos.y = _model.transform.position.y;
        _isFinishPath = false;
    }
    void Run()
    {
        if (IsFinishPath) return;
        var point = _waypoints[_nextPoint];
        var posPoint = point;
        //posPoint.y = _model.transform.position.y;
        Vector3 dir = _obs.GetDir((posPoint - _model.transform.position).normalized);
        if (dir.magnitude < 0.2f)
        {
            if (_nextPoint + 1 < _waypoints.Count)
                _nextPoint++;
            else
            {
                _model.indexWaypoint = Random.Range(0, _model._waypoints.Length);
                _model._controller.target = _model._waypoints[_model.indexWaypoint];

                _nextPoint = 0;
                
                //_isFinishPath = true;
                return;
            }
        }
        _model.Move(dir.normalized);
        _model.LookDir(dir);
    }
    public bool IsFinishPath => _isFinishPath;
}


