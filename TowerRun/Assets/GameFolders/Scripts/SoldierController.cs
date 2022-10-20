using System;
using UnityEngine;
using UnityEngine.AI;

public class SoldierController : MonoBehaviour
{
    private Rigidbody _rb;
    private Collider _col;
    private NavMeshAgent _agent;
    private Animator _animator;
    private ArmyController _armyController;

    private GameObject _visual;
    private Transform _target;
    public bool Grounded { get; private set; } = true;

    public bool IsAlive { get; private set; } = true;
    public bool IsLeader { get; set; }
    
    private void Awake()
    {
        _col = GetComponentInChildren<Collider>();
        _visual = transform.GetChild(0).gameObject;
        _armyController = GetComponentInParent<ArmyController>();
        _rb = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    public void SetTarget(Transform target, bool repeat = false)
    {
        _target = target;
        _agent.SetDestination(_target.position);
        _animator.SetBool("isRun",true);
        
        if (repeat)
        {
             InvokeRepeating(nameof(FollowLeader),0,0.02f);  
        }
    }
    private void FollowLeader()
    {
        switch (IsAlive)
        {
            case true:
                _agent.SetDestination(_target.position);
                break;
            case false:
                CancelInvoke(nameof(FollowLeader));
                break;
        }
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
        _rb.AddRelativeForce(Vector3.up * 5, ForceMode.Impulse);
        _rb.useGravity = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            if (!Grounded)
            {
                if (_agent.enabled)
                {
                    _agent.SetDestination(_target.position);
                    _agent.updatePosition = true;
                    _agent.updateRotation = true;
                    _agent.isStopped = false;
                }
                _rb.isKinematic = true;
                _rb.useGravity = false;
                Grounded = true;
            }
        }
        
        if (collision.collider.CompareTag("Barrel"))
        {
            Debug.Log("dieeeee");
            IsAlive = false;
            _col.enabled = false;
            _agent.enabled = false;
            
            _rb.isKinematic = false;
            _rb.useGravity = true;
            _rb.AddForce(transform.right * 10, ForceMode.Impulse);
            //_visual.SetActive(false);
            if (IsLeader)
            {
                IsLeader = false;
                _armyController.UpdateAttack();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EndGameArea"))
        {
            
        }
    }
}
