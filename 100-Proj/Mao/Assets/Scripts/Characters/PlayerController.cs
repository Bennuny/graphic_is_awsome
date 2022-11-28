using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent _agent;

    private Animator _animator;

    private GameObject _attackTarget;

    private float _lastAttackTime;

    private CharacterStat _characterStat;

    private Collider _coll;

    private bool _isDead;

    private float _stopDistance;
     
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        _animator = GetComponent<Animator>();

        _characterStat = GetComponent<CharacterStat>();

        _coll = GetComponent<BoxCollider>();

        _stopDistance = _agent.stoppingDistance;
    }


    private void Update()
    {
        _isDead = _characterStat.CurrentHealth == 0;

        if (_isDead)
        {
            GameManager.Instance.NotifiyObserver();
            // Death
            SwitchAnimation();
        }
        else
        {
            // Death
            SwitchAnimation();

            _lastAttackTime -= Time.deltaTime;
        }
    }

    private void Start()
    {
        MouseManager.Instance.OnMouseClicked += MoveToTarget;

        MouseManager.Instance.OnEnemyClicked += EventAttack;


        GameManager.Instance.RigisterPlayer(_characterStat);
    }

    private void EventAttack(GameObject target)
    {
        if (target != null && !_isDead)
        {
            _attackTarget = target;

            StartCoroutine(MoveToAttackTarget());
        }
    }

    IEnumerator MoveToAttackTarget()
    {
        _agent.isStopped = false;
        _agent.stoppingDistance = _characterStat.AttackRange;
        transform.LookAt(_attackTarget.transform);

        while (Vector3.Distance(_attackTarget.transform.position, transform.position) > _characterStat.AttackRange)
        {
            _agent.destination = _attackTarget.transform.position;
            yield return null;
        }

        _agent.isStopped = true;

        // attack
        if (_lastAttackTime < 0)
        {
            // critical attack
            _characterStat.isCritical = Random.value <= _characterStat.CriticalChance;
            _animator.SetBool("Critical", _characterStat.isCritical);
            _animator.SetTrigger("Attack");
            _lastAttackTime = _characterStat.CoolDown;
        }
    }

    private void SwitchAnimation()
    {
        _animator.SetFloat("Speed", _agent.velocity.sqrMagnitude);
        _animator.SetBool("Death", _isDead);
    }

    public void MoveToTarget(Vector3 target)
    { 
        StopAllCoroutines();
        if (_isDead)
        {
            return;
        }
        _agent.isStopped = false;
        _agent.destination = target;
        _agent.stoppingDistance = _stopDistance;
    }

    // Animation Event
    private void Hit()
    {
        if (_attackTarget.CompareTag("Attackable"))
        {
            if (_attackTarget.GetComponent<Rock>())
            {
                _attackTarget.GetComponent<Rock>()._state = Rock.RockStates.HiEnemey;

                _attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
                _attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
            }
        }
        else
        {
            var targetStats = _attackTarget.GetComponent<CharacterStat>();
            targetStats.TakeDamage(_characterStat, targetStats);
        }
    }
}
