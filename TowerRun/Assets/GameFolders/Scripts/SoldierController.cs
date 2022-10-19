using System;
using UnityEngine;
using UnityEngine.AI;

public class SoldierController : MonoBehaviour
{
    private Rigidbody _rb;
    private NavMeshAgent _agent;
    private Animator _animator;
    
    private Vector3 _target;
    public bool Grounded { get; private set; } = true;

    public bool IsAlive { get; set; }
    public bool IsLeader { get; set; }
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    public void StartAttack(Vector3 target)
    {
        IsAlive = true;
        _target = target;
        _agent.SetDestination(_target);
        _animator.SetBool("isRun",true);
    }

    public void JumpSoldier()
    {
        Grounded = false;
        if (_agent.enabled)
        {
            _agent.SetDestination(transform.position);
            
            _agent.updatePosition = false;
            _agent.updateRotation = false;
            _agent.isStopped = true;
        }
        
        // make the jump
        _animator.SetTrigger("isJump");
        _rb.isKinematic = false;
        _rb.AddRelativeForce(Vector3.up * 4, ForceMode.Impulse);
        _rb.useGravity = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider != null && collision.collider.CompareTag("Ground"))
        {
            if (!Grounded)
            {
                if (_agent.enabled)
                {
                    _agent.SetDestination(_target);
                    _agent.updatePosition = true;
                    _agent.updateRotation = true;
                    _agent.isStopped = false;
                }
                _rb.isKinematic = true;
                _rb.useGravity = false;
                Grounded = true;
            }
        }
    }
}
