using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyController : MonoBehaviour
{
    [SerializeField] private List<SoldierController> soldiers = new List<SoldierController>();

    [SerializeField] private Transform targetBoss;

    private CameraFollower _cameraFollower;

    private void Awake()
    {
        _cameraFollower = Camera.main.GetComponent<CameraFollower>();
    }

    private void Start()
    {
        StartCoroutine(StartAttack());
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(JumpArmy());
        }
    }

    private Transform SetLeader()
    {
        Transform temp = soldiers[0].transform;
        
        foreach (var soldier in soldiers)
        {
            if (soldier.IsAlive)
            {
                soldier.IsLeader = true;
                temp = soldier.transform;
                return temp;
            }
        }

        return temp;
    }
    private IEnumerator StartAttack()
    {
        foreach (var soldier in soldiers)
        {
            soldier.StartAttack(targetBoss.position);
            yield return new WaitForSeconds(0.5f);
        }
        
        _cameraFollower.CameraSetup(SetLeader());
    }
    
    private IEnumerator JumpArmy()
    {
        foreach (var soldier in soldiers)
        {
            soldier.JumpSoldier();
            yield return new WaitForSeconds(0.2f);
        }
    }
}
