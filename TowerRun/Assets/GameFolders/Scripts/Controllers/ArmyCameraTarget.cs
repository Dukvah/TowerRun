using UnityEngine;
using UnityEngine.AI;

public class ArmyCameraTarget : MonoBehaviour
{
    private NavMeshAgent _agent;
    private ArmyController _armyController;

    private void Awake()
    {
        _armyController = GetComponentInParent<ArmyController>();
        _agent = GetComponent<NavMeshAgent>();
    }

    public void SetStart(Vector3 target)
    {
        _agent.speed = PlayerPrefs.GetFloat("Speed", 2);
        _agent.SetDestination(target);

        InvokeRepeating(nameof(CalculateDistance),0f,1f);
    }

    public void Stop()
    {
        CancelInvoke(nameof(CalculateDistance));
    }

    private void CalculateDistance()
    {
        var distance =  Vector3.Distance(transform.position, _armyController.Leader.transform.position);
       
        if (distance > 1)
            _agent.speed = PlayerPrefs.GetFloat("Speed", 2) * 0.7f;
        else
            _agent.speed = PlayerPrefs.GetFloat("Speed", 2);
    }
}
