using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grunt : EnemyController
{

    [Header("Skill")]
    public float kickForce = 10;


    public void KickOff()
    {
        if (_attackTarget != null)
        {
            transform.LookAt(_attackTarget.transform);

            Vector3 dir = _attackTarget.transform.position - transform.position;
            dir.Normalize();

            var atkAgent = _attackTarget.GetComponent<NavMeshAgent>();
            atkAgent.isStopped = true;
            atkAgent.velocity = dir * kickForce;

            var animator = _attackTarget.GetComponent<Animator>();
            animator.SetTrigger("Dizzy");
        }
    }

}
