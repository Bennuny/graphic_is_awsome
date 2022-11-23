using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent _agent;

    private Animator _animator;

    private GameObject _attackTarget;

    private float _lastAttackTime;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        _animator = GetComponent<Animator>();
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

        // TODO: attack range
        while (Vector3.Distance(_attackTarget.transform.position, transform.position) > 1)
        {
            _agent.destination = _attackTarget.transform.position;
            yield return null;
        }

        _agent.isStopped = true;

        // attack
        if (_lastAttackTime < 0)
        {
            _animator.SetTrigger("Attack");
            _lastAttackTime = 0.5f;
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
}
