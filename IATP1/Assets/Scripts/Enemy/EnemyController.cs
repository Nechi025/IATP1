using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    EnemyModel _model;
    FSM<StatesEnum> _fsm;
    ILineOfSight _los;
    public float attackRange;
    public Transform target;
    ITreeNode _root;

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
        //create States
        var idle = new EnemyIdleState<StatesEnum>(_model);
        var attack = new EnemyAttackState<StatesEnum>(_model);
        var chase = new EnemyChaseState<StatesEnum>(_model, target);

        //Add transitions 
        idle.AddTransition(StatesEnum.Attack, attack);
        idle.AddTransition(StatesEnum.Chase, chase);

        attack.AddTransition(StatesEnum.Idle, idle);
        attack.AddTransition(StatesEnum.Chase, chase);

        chase.AddTransition(StatesEnum.Idle, idle);
        chase.AddTransition(StatesEnum.Attack, attack);

        //create FSM
        _fsm = new FSM<StatesEnum>(idle);
    }

    void InitializedTree()
    {
        //Actions
        var idle = new ActionNode(() => _fsm.Transition(StatesEnum.Idle));
        var attack = new ActionNode(() => _fsm.Transition(StatesEnum.Attack));
        var chase = new ActionNode(() => _fsm.Transition(StatesEnum.Chase));

        //Questions
        var qIsCooldown = new QuestionNode(() => _model.IsCooldown, idle, attack);
        var qIsCooldownOutOfRange = new QuestionNode(() => _model.IsCooldown, idle, chase);
        var qAttackRange = new QuestionNode(QuestionAttackRange, qIsCooldown, qIsCooldownOutOfRange);
        var qLoS = new QuestionNode(QuestionLoS, qAttackRange, idle);

        _root = qLoS;
    }

    bool QuestionAttackRange()
    {
        return _los.CheckRange(target, attackRange);
    }

    bool QuestionLoS()
    {
        return _los.CheckRange(target) && _los.CheckAngle(target) && _los.CheckView(target);
    }

    private void Update()
    {
        _fsm.OnUpdate();
        _root.Execute();
    }
}
