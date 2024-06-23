using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonController : MonoBehaviour
{
    public float timePrediction;
    public Rigidbody target;
    public float angle;
    public float radius;
    public LayerMask maskObs;

    FSM<StatesEnumAll> _fsm;
    ISteering _steering;
    EnemySkeletonModel _skeleton;
    ObstacleAvoidance _obs;

    private void Awake()
    {
        _skeleton = GetComponent<EnemySkeletonModel>();
        InitializeSteerings();
        InitializedFSM();
    }

    void InitializeSteerings()
    {
        var seek = new Seek(_skeleton.transform, target.transform);
        var flee = new Flee(_skeleton.transform, target.transform);
        var pursuit = new Pursuit(_skeleton.transform, target, timePrediction);
        var evade = new Evade(_skeleton.transform, target, timePrediction);
        _steering = pursuit;
        _obs = new ObstacleAvoidance(_skeleton.transform, angle, radius, maskObs);
    }

    void InitializedFSM()
    {
        _fsm = new FSM<StatesEnumAll>();

        var idle = new SkeletonStateIdle<StatesEnumAll>();
        var steering = new SkeletonStateSteering<StatesEnumAll>(_skeleton, _steering, _obs);

        idle.AddTransition(StatesEnumAll.Walk, steering);
        steering.AddTransition(StatesEnumAll.Idle, idle);

        _fsm.SetInit(steering);
    }

    void Update()
    {
        _fsm.OnUpdate();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, angle / 2, 0) * transform.forward * radius);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -angle / 2, 0) * transform.forward * radius);
    }
}
