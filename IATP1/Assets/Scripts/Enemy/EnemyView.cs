using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    public Animator _anim;
    public Rigidbody body;
    EnemyModel _model;
    
    private void Awake()
    {
        _model = GetComponent<EnemyModel>();
        _model.onAttack += OnAttack;
        _model.onReload += OnReload;
    }

    void OnAttack()
    { 
        _anim.SetTrigger("Attack");

    }
   
    void OnReload()
    {
        _anim.SetTrigger("Reload");
    }

    private void Update()
    {
        _anim.SetFloat("Vel", body.velocity.magnitude);
    }
}
