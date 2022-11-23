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

    public EnemyState _enemyState;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void SwitchState()
    {
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
}
