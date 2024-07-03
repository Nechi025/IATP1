using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float angle;
    public float radius;
    public LayerMask maskObs;

    public float timePrediction = 2;

    //Camera
    public CameraModel cam;

    EnemyModel _model;
    FSM<StatesEnumAll> _fsm;
    ILineOfSight _los;
    public float attackRange;
    public Rigidbody target;
    ITreeNode _root;
    ObstacleAvoidance _obs;
    EnemyStatePatrol<StatesEnumAll> _stateFollowPoints;

    private void Awake()
    {
        _model = GetComponent<EnemyModel>();
        _los = GetComponent<LineOfSight>();
    }

    private void Start()
    {
        InitializeFSM();
        InitializedTree();
        Debug.Log("c");
        int random = Random.Range(0, _model._waypoints.Count);
        _model._controller.RunAStar(transform.position, _model._waypoints[3].transform.position);
    }

    void InitializeFSM()
    {
        _obs = new ObstacleAvoidance(_model.transform, angle, radius, maskObs);

        //create States
        var idle = new EnemyIdleState<StatesEnumAll>(_model);
        var attack = new EnemyAttackState<StatesEnumAll>(_model);
        var chase = new EnemyStateSteering<StatesEnumAll>(_model, new Pursuit(_model.transform, target, timePrediction), _obs);
        var chaseCam = new EnemyStateSteering<StatesEnumAll>(_model, new Seek(_model.transform, cam.transform), _obs);
         _stateFollowPoints = new EnemyStatePatrol<StatesEnumAll>(_model, _obs);
        var reload = new EnemyReloadState<StatesEnumAll>(_model);

        //Add transitions 
        idle.AddTransition(StatesEnumAll.Attack, attack);
        idle.AddTransition(StatesEnumAll.Chase, chase);
        idle.AddTransition(StatesEnumAll.Patrol, _stateFollowPoints);
        idle.AddTransition(StatesEnumAll.ChaseCam, chaseCam);
        idle.AddTransition(StatesEnumAll.Reload, reload);

        attack.AddTransition(StatesEnumAll.Idle, idle);
        attack.AddTransition(StatesEnumAll.Chase, chase);
        attack.AddTransition(StatesEnumAll.Patrol, _stateFollowPoints);
        attack.AddTransition(StatesEnumAll.ChaseCam, chaseCam);
        attack.AddTransition(StatesEnumAll.Reload, reload);

        chase.AddTransition(StatesEnumAll.Idle, idle);
        chase.AddTransition(StatesEnumAll.Attack, attack);
        chase.AddTransition(StatesEnumAll.Patrol, _stateFollowPoints);
        chase.AddTransition(StatesEnumAll.ChaseCam, chaseCam);
        chase.AddTransition(StatesEnumAll.Reload, reload);

        _stateFollowPoints.AddTransition(StatesEnumAll.Idle, idle);
        _stateFollowPoints.AddTransition(StatesEnumAll.Attack, attack);
        _stateFollowPoints.AddTransition(StatesEnumAll.Chase, chase);
        _stateFollowPoints.AddTransition(StatesEnumAll.ChaseCam, chaseCam);
        _stateFollowPoints.AddTransition(StatesEnumAll.Reload, reload);

        chaseCam.AddTransition(StatesEnumAll.Idle, idle);
        chaseCam.AddTransition(StatesEnumAll.Chase, chase);
        chaseCam.AddTransition(StatesEnumAll.Attack, attack);
        chaseCam.AddTransition(StatesEnumAll.Patrol, _stateFollowPoints);
        chaseCam.AddTransition(StatesEnumAll.Reload, reload);

        reload.AddTransition(StatesEnumAll.Idle, idle);
        reload.AddTransition(StatesEnumAll.Chase, chase);
        reload.AddTransition(StatesEnumAll.Attack, attack);
        reload.AddTransition(StatesEnumAll.Patrol, _stateFollowPoints);
        reload.AddTransition(StatesEnumAll.ChaseCam, chaseCam);

        //create FSM
        _fsm = new FSM<StatesEnumAll>(idle);
    }

    void InitializedTree()
    {
        //Actions
        var idle = new ActionNode(() => _fsm.Transition(StatesEnumAll.Idle));
        var attack = new ActionNode(() => _fsm.Transition(StatesEnumAll.Attack));
        var chase = new ActionNode(() => _fsm.Transition(StatesEnumAll.Chase));
        var patrol = new ActionNode(() => _fsm.Transition(StatesEnumAll.Patrol));  
        var chaseCam = new ActionNode(() => _fsm.Transition(StatesEnumAll.ChaseCam));
        var reload = new ActionNode(() => _fsm.Transition(StatesEnumAll.Reload));

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

    public IPoints GetStateWaypoints => _stateFollowPoints;
}
