using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    GUARD,
    PATROL,
    CHASE,
    DEAD,
}

[RequireComponent(typeof(NavMeshAgent))]

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent _agent;

    private EnemyState _enemyState;

    private GameObject _attackTarget;

    private Animator _animator;


    [Header("Basic Setting")]

    public float SightRadius;

    public bool isGuard;

    private float _speed;

    public float LookAtTime;
    private float _remainLookAtTime;

    //
    bool _isWalk;

    bool _isChase;

    bool _isFollow;

    [Header("Patrol Setting")]
    public float PatrolRange;

    private Vector3 _wayPoint;

    private Vector3 _guidePos;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        _animator = GetComponent<Animator>();

        _speed = _agent.speed;

        _guidePos = transform.position;

        _remainLookAtTime = LookAtTime;
    }

    private void Start()
    {
        if (isGuard)
        {
            _enemyState = EnemyState.GUARD;
        }
        else
        {
            _enemyState = EnemyState.PATROL;
            GetNewWayPoint();
        }
    }

    private void Update()
    {
        SwitchState();

        SwitchAnimation();
    }

    private void SwitchState()
    {
        bool foundPlayer = FoundPlayer();

        if (foundPlayer)
        {
            _enemyState = EnemyState.CHASE;
        }

        switch (_enemyState)
        {
            case EnemyState.GUARD:
                {

                }
                break;
            case EnemyState.PATROL:
                {
                    _isChase = false;
                    _agent.speed = _speed * 0.5f;

                    if (Vector3.Distance(_wayPoint, transform.position) <= _agent.stoppingDistance)
                    {
                        _isWalk = false;

                        if (_remainLookAtTime > 0)
                        {
                            _remainLookAtTime -= Time.deltaTime;
                        }
                        else
                        {
                            GetNewWayPoint();
                        }
                    }
                    else
                    {
                        _isWalk = true;
                        _agent.destination = _wayPoint;
                    }
                }
                break;
            case EnemyState.CHASE:
                {
                    _isWalk = false;
                    _isChase = true;

                    if (foundPlayer)
                    {
                        _isFollow = true;
                        _agent.destination = _attackTarget.transform.position;
                    }
                    else
                    {
                        _isFollow = false;
                        if (_remainLookAtTime > 0)
                        {
                            _remainLookAtTime -= Time.deltaTime;
                            _agent.destination = transform.position;
                        }
                        else
                        {
                            if (isGuard)
                            {
                                _enemyState = EnemyState.GUARD;
                            }
                            else
                            {
                                _enemyState = EnemyState.PATROL;
                            }
                        }
                    }
                }
                break;
            case EnemyState.DEAD:
                {

                }
                break;

        }
    }

    private void SwitchAnimation()
    {
        _animator.SetBool("Walk", _isWalk);
        _animator.SetBool("Chase", _isChase);
        _animator.SetBool("Follow", _isFollow);
    }

    private bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, SightRadius);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                _attackTarget = collider.gameObject;

                return true;
            }
        }

        return false;
    }

    void GetNewWayPoint()
    {
        _remainLookAtTime = LookAtTime;

        float randomX = Random.Range(-PatrolRange, PatrolRange);
        float randomZ = Random.Range(-PatrolRange, PatrolRange);

        Vector3 randomPoint = new Vector3(_guidePos.x + randomX, transform.position.y, _guidePos.z + randomZ);

        // FIX
        NavMeshHit hit;
        // SamplePosition: 指定范围内，找到导航导航网格上最近的点。对导航网格进行采样
        _wayPoint = NavMesh.SamplePosition(randomPoint, out hit, PatrolRange, 1) ? hit.position : transform.position;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, SightRadius);
    }
}
