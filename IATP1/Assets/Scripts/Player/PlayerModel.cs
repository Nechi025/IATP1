using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 5;

    public float turnSmoothTime = 0.1f;

    float turnSmoothVelocity;

    public Transform cam;


    public void Move(Vector3 dir)
    {
        if (GameManager.Instance.isOnSight == false)
        {
            if (dir.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }
        }
        else return;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WinTrigger"))
        {
             GameManager.Instance.YouWin();
        }
    }
}
