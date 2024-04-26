using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance
{
    float _angle;
    float _radius;
    Transform _entity;
    LayerMask _maskObs;

    public ObstacleAvoidance(Transform entity, float angle, float radius, LayerMask maskObs)
    {
        _angle = angle;
        _radius = radius;
        _entity = entity;
        _maskObs = maskObs;
    }

    public Vector3 GetDir(Vector3 currentDir)
    {
        Collider[] colls = Physics.OverlapSphere(_entity.position, _radius, _maskObs);
        Collider nearColl = null;
        float nearCollDistance = 0;
        currentDir.y = 0;
        Vector3 closetPoint = Vector3.zero;
        for (int i = 0; i < colls.Length; i++)
        {
            var currentColl = colls[i];
            closetPoint = currentColl.ClosestPoint(_entity.position);
            closetPoint.y = _entity.position.y;
            Vector3 dirToColl = currentColl.transform.position - _entity.position;
            float currentAngle = Vector3.Angle(dirToColl, currentDir);
            float distance = dirToColl.magnitude;

            if (currentAngle > _angle / 2) { continue; }

            if (nearColl == null)
            {
                nearColl = currentColl;
                nearCollDistance = Vector3.Distance(_entity.position, closetPoint);
                continue;
            }

            
            if (distance < nearCollDistance)
            {
                nearCollDistance = distance;
                nearColl = currentColl;
            }
        }
        if (nearColl == null)
        {
            return currentDir;
        }
        else
        {
            Vector3 newDir = (currentDir + (_entity.position - closetPoint).normalized).normalized;
            return Vector3.Lerp(currentDir, newDir, (_radius - nearCollDistance) / _radius);
        }
    }
}
