using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : ISteering
{
    Transform _entity;
    Vector3 _target;

    public Seek(Transform entity)
    {
        _entity = entity;
        //_target = target;
    }

    public void SetTarget(Vector3 target)
    {
        _target = target;
    }


    public Vector3 GetDir()
    {
        Vector3 dir = (_target - _entity.position).normalized;
        return dir;
    }
}
