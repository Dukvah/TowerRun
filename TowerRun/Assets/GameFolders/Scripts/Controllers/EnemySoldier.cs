using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class EnemySoldier : MonoBehaviour
{
    [SerializeField] private EnemyArmy enemyArmy;
    [SerializeField] private NavMeshAgent agent;

    [Header("Effects")] 
    [SerializeField] private List<GameObject> stepDustEffects = new(); // object pool
    [SerializeField] private GameObject deathEffect;

    private IEnumerator _co;

    private void Start()
    {
        SetStepPool();
        InvokeRepeating(nameof(SetRandomDestination),0f,0.01f);
        
        _co = StepEffectCreator();
        StartCoroutine(_co);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            StopCoroutine(_co);
            DeathEffect();
            enemyArmy.SoldierDied();

            CancelInvoke(nameof(SetRandomDestination));
            CancelInvoke(nameof(Check));
            
            gameObject.SetActive(false);
        }
    }

    private void SetRandomDestination()
    {
        agent.SetDestination(GetRandomPoint());
        
        if (agent.hasPath)
        {
            CancelInvoke(nameof(SetRandomDestination));
            InvokeRepeating(nameof(Check),0f,0.01f);
        }
    }

    private void Check()
    {
        if (agent.velocity.magnitude < 0.05f)
        {
            CancelInvoke(nameof(Check));
            InvokeRepeating(nameof(SetRandomDestination),0f,0.01f);
        }
    }
    private Vector3 GetRandomPoint()
    {
        var point = Random.insideUnitCircle * 2f;
        return new Vector3(point.x,enemyArmy.transform.position.y ,point.y);
    }
    
    #region Effects

    private void SetStepPool()
    {
        foreach (var effect in stepDustEffects)
        {
            effect.transform.parent = null;
        }
    }
    private IEnumerator StepEffectCreator()
    {
        for (int i = 0; i < stepDustEffects.Count; i++)
        {
            yield return new WaitForSeconds(Random.Range(0.3f,0.5f));
            MakeStepEffect(i);
            
            if (i == stepDustEffects.Count - 1)
            {
                i = -1;
            }
        }
    }
    private void MakeStepEffect(int index)
    {
        stepDustEffects[index].transform.parent = gameObject.transform;
        stepDustEffects[index].transform.localPosition = new Vector3(0,0.1f,0);
        stepDustEffects[index].transform.rotation = transform.rotation;
        stepDustEffects[index].SetActive(true);
        stepDustEffects[index].transform.parent = null;
    }
    
    private void DeathEffect()
    {
        var ps = Instantiate(deathEffect, transform.position + new Vector3(0f, 0.75f, 0f), Quaternion.identity);
        Destroy(ps, 1f);
    }

    #endregion
}
