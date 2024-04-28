using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateSteering<T> : State<T>
{
    ISteering _steering;
    EnemyModel _skeleton;
    ObstacleAvoidance _obs;

    public EnemyStateSteering(EnemyModel skeleton, ISteering steering, ObstacleAvoidance obs)
    {
        _steering = steering;
        _skeleton = skeleton;
        _obs = obs;
    }

    public override void Execute()
    {
        var dir = _obs.GetDir(_steering.GetDir());
        _skeleton.Move(dir);
        _skeleton.LookDir(dir);
    }
}
