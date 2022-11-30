using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class EnemySoldier : MonoBehaviour
{
    [SerializeField] private EnemyArmy enemyArmy;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject deathEffect;

    private void Start()
    {
        InvokeRepeating(nameof(SetRandomDestination),0f,0.01f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            DeathEffect();
            enemyArmy.SoldierDied();
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
    private void DeathEffect()
    {
        var ps = Instantiate(deathEffect, transform.position + new Vector3(0f, 0.75f, 0f), Quaternion.identity);
        Destroy(ps, 1f);
    }

    #endregion
}
