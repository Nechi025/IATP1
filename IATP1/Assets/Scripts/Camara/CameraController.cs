using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    ILineOfSight _los;
    IAlert _alert;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        _los = GetComponent<ILineOfSight>();
        _alert = GetComponent<IAlert>();
    }

    private void Update()
    {
        if (_los.CheckRange(target) && _los.CheckAngle(target) && _los.CheckView(target))
        {
            _alert.Alert = true;
            anim.SetFloat("PlayerInView", 1);
        }
        else
        {
            _alert.Alert = false;
            anim.SetFloat("PlayerInView", 0);
        }
    }
}
