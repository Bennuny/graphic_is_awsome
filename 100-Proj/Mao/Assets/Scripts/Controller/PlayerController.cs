using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent _agent;

    private Animator _animator;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        _animator = GetComponent<Animator>();
    }


    private void Update()
    {
        SwitchAnimation();
    }

    private void Start()
    {
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
    }

    private void SwitchAnimation()
    {
        _animator.SetFloat("Speed", _agent.velocity.sqrMagnitude);
    }

    public void MoveToTarget(Vector3 target)
    {
        _agent.destination = target;
    }
}
