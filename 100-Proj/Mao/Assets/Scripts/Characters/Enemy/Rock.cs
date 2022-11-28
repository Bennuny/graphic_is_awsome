using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rock : MonoBehaviour
{
    public enum RockStates
    {
        HitPlayer,
        HiEnemey,
        HitNothing
    }

    private Rigidbody _rb;

    public RockStates _state;

    [Header("Basic Setting")]
    public float force;

    public GameObject BreakObject;

    public GameObject _target;

    public Vector3 _direction;

    public int _damage;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = Vector3.one;

        _state = RockStates.HitPlayer;

        FlyToTarget();
    }

    private void FixedUpdate()
    {
        if (_rb.velocity.sqrMagnitude < 1.0f)
        {
            _state = RockStates.HitNothing;
        }
    }

    public void FlyToTarget()
    {
        if (_target == null)
        {
            _target = FindObjectOfType<PlayerController>().gameObject;
        }

        _direction = (_target.transform.position - transform.position + Vector3.up).normalized;

        _rb.AddForce(_direction * force, ForceMode.Impulse);
    }


    private void OnCollisionEnter(Collision collision)
    {
        switch (_state)
        {
            case RockStates.HitPlayer:
                {
                    if (collision.gameObject.CompareTag("Player"))
                    {
                        // 击退
                        var agent = collision.gameObject.GetComponent<NavMeshAgent>();
                        agent.isStopped = true;
                        agent.velocity = _direction * force;

                        // 眩晕
                        collision.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");

                        var defenerStat = collision.gameObject.GetComponent<CharacterStat>();
                        defenerStat.TakeDamage(_damage, defenerStat);

                        _state = RockStates.HitNothing;
                    }
                }
                break;
            case RockStates.HiEnemey:
                {
                    if (collision.gameObject.GetComponent<Golem>())
                    {
                        var defenerStat = collision.gameObject.GetComponent<CharacterStat>();
                        defenerStat.TakeDamage(_damage, defenerStat);

                        var breakParticle = Instantiate(BreakObject, transform.transform);

                        Destroy(gameObject);

                        _state = RockStates.HitNothing;
                    }
                }
                break;
        }
    }
}
