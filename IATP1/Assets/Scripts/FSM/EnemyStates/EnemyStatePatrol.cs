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

    public override void Enter()
    {
        base.Enter();
        SetWayPoints(_model._waypoints);
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
        _isFinishPath = false;
    }
    void Run()
    {
        var point = _waypoints[_nextPoint];
        Vector3 dir = _obs.GetDir((point - _model.transform.position).normalized);
        if (dir.magnitude < 0.2f)
        {
            if (_nextPoint + 1 < _waypoints.Count)
                _nextPoint++;
            else
            {
                _isFinishPath = true;
                return;
            }
            /*if (_nextPoint + 1 < _waypoints.Count)
            {
                Debug.Log("a");
                //_model._controller.RunAStar(_model.transform.position, _model._waypoints[3].transform.position);
                _nextPoint++;
            }
            
            else
            {
                Debug.Log("ab");
                _model.indexWaypoint = Random.Range(0, _model._waypoints.Count);
                _model._controller.RunAStar(_model.transform.position, _model._waypoints[_model.indexWaypoint].transform.position);
                _nextPoint = 0;
            }*/
        }
        _model.Move(dir.normalized);
        _model.LookDir(dir);
    }

    public bool IsFinishPath => _isFinishPath;
}


