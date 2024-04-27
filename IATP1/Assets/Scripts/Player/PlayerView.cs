using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public Animator _anim;
    public CharacterController player;

    private void Update()
    {
        _anim.SetFloat("Vel", player.velocity.magnitude);
    }
}