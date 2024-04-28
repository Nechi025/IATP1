using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnCamera : MonoBehaviour
{
    public CameraModel model;

    private void OnTriggerEnter(Collider other)
    {
        model.anim.SetBool("Active", false);
    }
}
