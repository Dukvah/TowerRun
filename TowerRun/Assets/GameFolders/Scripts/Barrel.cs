using UnityEngine;
using UnityEngine.AI;

public class Barrel : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Animator _animator;
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    public void RollToTarget(Vector3 target)
    {
        _agent.SetDestination(target);
        _animator.SetBool("isRoll",true);
    }
    
}
