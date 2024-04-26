using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : ISteering
{
    Transform _entity;
    Transform _target;

    public Flee(Transform entity, Transform target)
    {
        _entity = entity;
        _target = target;
    }

    public Vector3 GetDir()
    {
        Vector3 dir = (_entity.position - _target.position).normalized;
        return dir;
    }
}
