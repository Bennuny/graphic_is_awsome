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

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        _animator = GetComponent<Animator>();

        _characterStat = GetComponent<CharacterStat>();
    }


    private void Update()
    {
        SwitchAnimation();

        _lastAttackTime -= Time.deltaTime;
    }

    private void Start()
    {
        MouseManager.Instance.OnMouseClicked += MoveToTarget;

        MouseManager.Instance.OnEnemyClicked += EventAttack;
    }

    private void EventAttack(GameObject target)
    {
        if (target != null)
        {
            _attackTarget = target;

            StartCoroutine(MoveToAttackTarget());
        }
    }

    IEnumerator MoveToAttackTarget()
    {
        _agent.isStopped = false;
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
    }

    public void MoveToTarget(Vector3 target)
    {
        StopAllCoroutines();
        _agent.isStopped = false;
        _agent.destination = target;
    }

    // Animation Event
    private void Hit()
    {
        var targetStats = _attackTarget.GetComponent<CharacterStat>();
        targetStats.TakeDamage(_characterStat, targetStats);
    }
}
