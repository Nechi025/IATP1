using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStateSteering<T> : State<T>
{
    ISteering _steering;
    EnemySkeletonModel _skeleton;
    ObstacleAvoidance _obs;
    public SkeletonStateSteering(EnemySkeletonModel skeleton, ISteering steering, ObstacleAvoidance obs)
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
