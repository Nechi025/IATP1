using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float angle;
    public float radius;
    public LayerMask maskObs;

    public float timePrediction = 2;

    //Patrol
    public Transform[] points;
    public float minDistance;

    //Camera
    public CameraModel cam;

    EnemyModel _model;
    FSM<StatesEnum> _fsm;
    ILineOfSight _los;
    public float attackRange;
    public Rigidbody target;
    ITreeNode _root;
    ObstacleAvoidance _obs;

    private void Awake()
    {
        _model = GetComponent<EnemyModel>();
        _los = GetComponent<LineOfSight>();
    }

    private void Start()
    {
        InitializeFSM();
        InitializedTree();
    }

    void InitializeFSM()
    {
        _obs = new ObstacleAvoidance(_model.transform, angle, radius, maskObs);

        //create States
        var idle = new EnemyIdleState<StatesEnum>(_model);
        var attack = new EnemyAttackState<StatesEnum>(_model);
        var chase = new EnemyStateSteering<StatesEnum>(_model, new Pursuit(_model.transform, target, timePrediction), _obs);
        var chaseCam = new EnemyStateSteering<StatesEnum>(_model, new Seek(_model.transform, cam.transform), _obs);
        var patrol = new EnemyStatePatrol<StatesEnum>(points, minDistance, _model, _obs);
        var reload = new EnemyReloadState<StatesEnum>(_model);

        //Add transitions 
        idle.AddTransition(StatesEnum.Attack, attack);
        idle.AddTransition(StatesEnum.Chase, chase);
        idle.AddTransition(StatesEnum.Patrol, patrol);
        idle.AddTransition(StatesEnum.ChaseCam, chaseCam);
        idle.AddTransition(StatesEnum.Reload, reload);

        attack.AddTransition(StatesEnum.Idle, idle);
        attack.AddTransition(StatesEnum.Chase, chase);
        attack.AddTransition(StatesEnum.Patrol, patrol);
        attack.AddTransition(StatesEnum.ChaseCam, chaseCam);
        attack.AddTransition(StatesEnum.Reload, reload);

        chase.AddTransition(StatesEnum.Idle, idle);
        chase.AddTransition(StatesEnum.Attack, attack);
        chase.AddTransition(StatesEnum.Patrol, patrol);
        chase.AddTransition(StatesEnum.ChaseCam, chaseCam);
        chase.AddTransition(StatesEnum.Reload, reload);

        patrol.AddTransition(StatesEnum.Idle, idle);
        patrol.AddTransition(StatesEnum.Attack, attack);
        patrol.AddTransition(StatesEnum.Chase, chase);
        patrol.AddTransition(StatesEnum.ChaseCam, chaseCam);
        patrol.AddTransition(StatesEnum.Reload, reload);

        chaseCam.AddTransition(StatesEnum.Idle, idle);
        chaseCam.AddTransition(StatesEnum.Chase, chase);
        chaseCam.AddTransition(StatesEnum.Attack, attack);
        chaseCam.AddTransition(StatesEnum.Patrol, patrol);
        chaseCam.AddTransition(StatesEnum.Reload, reload);

        reload.AddTransition(StatesEnum.Idle, idle);
        reload.AddTransition(StatesEnum.Chase, chase);
        reload.AddTransition(StatesEnum.Attack, attack);
        reload.AddTransition(StatesEnum.Patrol, patrol);
        reload.AddTransition(StatesEnum.ChaseCam, chaseCam);

        //create FSM
        _fsm = new FSM<StatesEnum>(idle);
    }

    void InitializedTree()
    {
        //Actions
        var idle = new ActionNode(() => _fsm.Transition(StatesEnum.Idle));
        var attack = new ActionNode(() => _fsm.Transition(StatesEnum.Attack));
        var chase = new ActionNode(() => _fsm.Transition(StatesEnum.Chase));
        var patrol = new ActionNode(() => _fsm.Transition(StatesEnum.Patrol));  
        var chaseCam = new ActionNode(() => _fsm.Transition(StatesEnum.ChaseCam));
        var reload = new ActionNode(() => _fsm.Transition(StatesEnum.Reload));

        //Random
        //Mas items
        var dic = new Dictionary<ITreeNode, float>();
        dic[reload] = 10;
        dic[attack] = 30;

        var random = new RandomNode(dic);

        //Questions
        var qIsCooldown = new QuestionNode(() => _model.IsCooldown, idle, random);
        var qIsCooldownOutOfRange = new QuestionNode(() => _model.IsCooldown, idle, chase);
        var qAttackRange = new QuestionNode(QuestionAttackRange, qIsCooldown, qIsCooldownOutOfRange);
        var camLoS = new QuestionNode(QuestionCamLoS, chaseCam, patrol);
        var qLoS = new QuestionNode(QuestionLoS, qAttackRange, camLoS);

        _root = qLoS;
    }

    bool QuestionAttackRange()
    {
        return _los.CheckRange(target.transform, attackRange);
    }

    bool QuestionCamLoS()
    {
        return cam.Alert;
    }

    bool QuestionLoS()
    {
        return _los.CheckRange(target.transform) && _los.CheckAngle(target.transform) && _los.CheckView(target.transform);
    }

    private void Update()
    {
        _fsm.OnUpdate();
        _root.Execute();
    }
}
