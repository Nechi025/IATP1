using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; 
    ILineOfSight _los;
    IAlert _alert;

    private void Awake()
    {
        _los = GetComponent<ILineOfSight>();
        _alert = GetComponent<IAlert>();
    }

    private void Update()
    {
        if (_los.CheckRange(target) && _los.CheckAngle(target) && _los.CheckView(target))
        {
            _alert.Alert = true;
        }
        else
        {
            _alert.Alert = false;
        }
    }
}
