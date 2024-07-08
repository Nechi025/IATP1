using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel : MonoBehaviour
{
    public float attackCooldown;
    Coroutine _cooldown;
    public float speed = 5;
    Rigidbody _rb;
    public Action onAttack = delegate { };
    public Action onReload = delegate { };
    public AgentController _controller;
    public List<Node> _waypoints;
    public Node currentWaypointIndex;
    public int indexWaypoint = 0;
    public int chanceIncrease = 0;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 dir)
    {
        dir *= speed;
        dir.y = _rb.velocity.y;
        _rb.velocity = dir;
    }

    public void LookDir(Vector3 dir)
    {
        if (dir.x == 0 && dir.z == 0) return;
        transform.forward = dir;
    }

    public void Attack()
    {
        _cooldown = StartCoroutine(Cooldown());
        onAttack();
    }

    public void Reload()
    {
        _cooldown = StartCoroutine(Cooldown());
        chanceIncrease += 15;
        onReload();
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        _cooldown = null;
    }

    public bool IsCooldown => _cooldown != null;
}
