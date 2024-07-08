using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateSteering<T> : State<T>
{
    ISteering _steering;
    EnemyModel _skeleton;
    ObstacleAvoidance _obs;
    Vector3 _target;
    List<Vector3> _waypoints;
    int _nextPoint = 0;
    bool _isFinishPath = true;

    public EnemyStateSteering(EnemyModel skeleton, ISteering steering, ObstacleAvoidance obs, Vector3 target)
    {
        _steering = steering;
        _skeleton = skeleton;
        _obs = obs;
        _target = target;
    }

    public override void Enter()
    {
        List<Node> path = _skeleton._controller.RunAStar(_skeleton.transform.position, _target);
        Debug.Log("Path found in Enter: " + path.Count + " nodes");
        SetWayPoints(path);
    }

    

    public void SetWayPoints(List<Node> newPoints)
    {
        var list = new List<Vector3>();
        for (int i = 0; i < newPoints.Count; i++)
        {
            list.Add(newPoints[i].transform.position);
        }
        Debug.Log("Converting nodes to waypoints: " + list.Count);
        SetWaypoints(list);
    }

    public void SetWaypoints(List<Vector3> waypoints)
    {
        _waypoints = waypoints;
        _nextPoint = 0;
        if (_waypoints.Count > 0)
        {
            Debug.Log("Waypoints set: " + _waypoints.Count);
            (_steering as Seek).SetTarget(_waypoints[_nextPoint]);
        }
        _isFinishPath = false;
    }

    public override void Execute()
    {

        var dir = _obs.GetDir(_steering.GetDir());

        if (dir.magnitude < 0.2f)
        {
            Debug.Log("Reached waypoint: " + _nextPoint);
            if (_nextPoint + 1 < _waypoints.Count)
            {
                _nextPoint++;
                (_steering as Seek).SetTarget(_waypoints[_nextPoint]);
            }
            else
            {
                _isFinishPath = true;
                return;
            }
        }

        _skeleton.Move(dir);
        _skeleton.LookDir(dir);
    }

    public bool IsFinishPath => _isFinishPath;
}
