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
[RequireComponent(typeof(CharacterStat))]

public class EnemyController : MonoBehaviour, IEndGameObserver
{
    private NavMeshAgent _agent;

    private EnemyState _enemyState;

    protected GameObject _attackTarget;

    private Animator _animator;

    protected CharacterStat _characterStat;

    private Collider _coll;


    [Header("Basic Setting")]

    public float SightRadius;

    public bool isGuard;

    private float _speed;

    public float LookAtTime;

    private float _remainLookAtTime;

    private float _lastAttackTime;

    //
    bool _isWalk;

    bool _isChase;

    bool _isFollow;

    bool _isDead;

    bool _playerDead;

    [Header("Patrol Setting")]
    public float PatrolRange;

    private Vector3 _wayPoint;

    private Vector3 _guardPos;

    private Quaternion _guardRotation;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        _animator = GetComponent<Animator>();

        _characterStat = GetComponent<CharacterStat>();

        _coll = GetComponent<BoxCollider>();

        _speed = _agent.speed;

        _guardPos = transform.position;

        _guardRotation = transform.rotation;

        _remainLookAtTime = LookAtTime;

        _playerDead = false;

        //_characterStat.MaxHealth = 2;
    }

    private void Start()
    {
        // FIXME : FIX when Switch Scene
        GameManager.Instance.AddObserver(this);

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

    void OnEnable()
    {
        //GameManager.Instance.AddObserver(this);
    }

    void OnDisable()
    {
        if (GameManager.IsInitialize)
        {
            // FIXME 
            GameManager.Instance.RemoveObserver(this);
        }
    }

    private void Update()
    {
        _isDead = _characterStat.CurrentHealth == 0;

        if (!_playerDead)
        {
            SwitchState();

            SwitchAnimation();

            _lastAttackTime -= Time.deltaTime;
        }
    }

    private void SwitchState()
    {

        bool foundPlayer = false;
        if (_isDead)
        {
            _enemyState = EnemyState.DEAD;
        }
        else
        {
            foundPlayer = FoundPlayer();
            if (foundPlayer)
            {
                _enemyState = EnemyState.CHASE;
            }
        }

        switch (_enemyState)
        {
            case EnemyState.GUARD:
                {
                    _isWalk = true;
                    _isChase = false;
                    _agent.isStopped = false;
                    _agent.destination = _guardPos;

                    // 开销：sqrMagniture < Distance
                    if (Vector3.SqrMagnitude(_guardPos - transform.position) <= _agent.stoppingDistance)
                    {
                        _isWalk = false;

                        transform.rotation = Quaternion.Lerp(transform.rotation, _guardRotation, 0.01f);
                    }
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
                        _agent.isStopped = false;
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

                    if (TargetInAttackRange() || TargetInSkillRange())
                    {
                        _isFollow = false;
                        _agent.isStopped = true;

                        if (_lastAttackTime < 0)
                        {
                            _lastAttackTime = _characterStat.CoolDown;

                            // critical attack
                            _characterStat.isCritical = Random.value < _characterStat.CriticalChance;
                            // attack

                            Attack();

                        }
                    }
                }
                break;
            case EnemyState.DEAD:
                {
                    // 
                    _coll.enabled = false;
                    //_agent.enabled = false;
                    _agent.radius = 0;


                    Destroy(gameObject, 2.0f);
                }
                break;

        }
    }

    void Attack()
    {
        transform.LookAt(_attackTarget.transform.position);

        //_animator.SetTrigger("Attack");
        if (TargetInSkillRange())
        {
            _animator.SetTrigger("Attack");
            _animator.SetBool("Skill", true);
        }
        else if (TargetInAttackRange())
        {
            _animator.SetTrigger("Attack");
            _animator.SetBool("Skill", false);
        }

    }

    bool TargetInAttackRange()
    {
        if (_attackTarget)
        {
            var inRange = Vector3.Distance(transform.position, _attackTarget.transform.position) <= _characterStat.AttackRange;

            return inRange;
        }

        return false;
    }

    bool TargetInSkillRange()
    {
        if (!_attackTarget)
        {
            return false;
        }

        var inRange = Vector3.Distance(transform.position, _attackTarget.transform.position) <= _characterStat.SkillRange;

        return inRange;
    }

    private void SwitchAnimation()
    {
        _animator.SetBool("Walk", _isWalk);
        _animator.SetBool("Chase", _isChase);
        _animator.SetBool("Follow", _isFollow);
        _animator.SetBool("Critical", _characterStat.isCritical);
        _animator.SetBool("Death", _isDead);
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

        Vector3 randomPoint = new Vector3(_guardPos.x + randomX, transform.position.y, _guardPos.z + randomZ);

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

    // Animation Event
    private void Hit()
    {
        if (_attackTarget != null && transform.isFacingTarget(_attackTarget.transform)) 
        {
            var attackStat = _attackTarget.GetComponent<CharacterStat>();

            attackStat.TakeDamage(_characterStat, attackStat);
        }
    }

    public void EndNotifiy()
    {
        // play win animation
        _isChase = false;
        _isWalk = false;

        _attackTarget = null;
        _animator.SetBool("Win", true);

        _playerDead = true;

        // stop move
        // stop agent
    }
}
