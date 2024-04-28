using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraModel : MonoBehaviour, IAlert
{
    bool _isAlert;
    public Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public bool Alert
    {
        set { _isAlert = value; }
        get { return _isAlert; }
    }
}
