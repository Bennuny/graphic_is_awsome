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

    private GameObject _player;


    [Header("Basic Setting")]

    public float SightRadius;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        SwitchState();
    }

    private void SwitchState()
    {
        if (FoundPlayer())
        {
            _enemyState = EnemyState.CHASE;
            Debug.Log("Found Player");
        }

        switch (_enemyState)
        {
            case EnemyState.GUARD:
                {

                }
                break;
            case EnemyState.PATROL:
                {

                }
                break;
            case EnemyState.CHASE:
                {

                }
                break;
            case EnemyState.DEAD:
                {

                }
                break;

        }
    }

    private bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, SightRadius);

        foreach (var collider in colliders)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                _player = collider.gameObject;

                return true;
            }
        }

        return false;
    }
}
