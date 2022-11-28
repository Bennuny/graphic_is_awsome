using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem : EnemyController
{
    [Header("Skill")]
    public float kickForce = 25;

    public GameObject _rockPrefab;

    public Transform handPos;

    // Animation Event
    public void KickOff()
    {
        if (_attackTarget != null && transform.isFacingTarget(_attackTarget.transform))
        {
            var attackStat = _attackTarget.GetComponent<CharacterStat>();

            Vector3 direction = (_attackTarget.transform.position - transform.position).normalized;

            var agent = _attackTarget.GetComponent<NavMeshAgent>();
            agent.isStopped = true;
            agent.velocity = direction * kickForce;

            _attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");

            attackStat.TakeDamage(_characterStat, attackStat);
        }
    }

    // Animation Event
    public void ThrowRock()
    {
        if (_attackTarget != null)
        {
            var rock = Instantiate(_rockPrefab, handPos.position, Quaternion.identity);
            rock.GetComponent<Rock>()._target = _attackTarget;
        }
    }
}
