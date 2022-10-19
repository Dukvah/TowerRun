using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyController : MonoBehaviour
{
    [SerializeField] private List<SoldierController> soldiers = new List<SoldierController>();

    [SerializeField] private Transform targetBoss;
    [SerializeField] private BarrelStack barrelStack;

    private CameraFollower _cameraFollower;
    private SoldierController _leader;

    private void Awake()
    {
        _cameraFollower = Camera.main.GetComponent<CameraFollower>();
    }

    private void OnEnable()
    {
        GameManager.Instance.goArmy.AddListener(StartAttack);
    }

    private void CanJump()
    {
        if (Input.GetMouseButtonDown(0) && _leader.Grounded)
        {
            StartCoroutine(JumpArmy());
        }
    }
    
    private void StartAttack()
    {
        StartCoroutine(StartAttackAsync());
        barrelStack.SendBarrels();
        
    }
    private IEnumerator StartAttackAsync()
    {
        foreach (var soldier in soldiers)
        {
            soldier.StartAttack(targetBoss.position);
            yield return new WaitForSeconds(0.25f);
        }

        _cameraFollower.CameraSetup(SetLeader());
        InvokeRepeating(nameof(CanJump),5f,0.01f);
    }
    private Transform SetLeader()
    {
        Transform temp = soldiers[0].transform;

        foreach (var soldier in soldiers)
        {
            if (soldier.IsAlive)
            {
                _leader = soldier;
                
                soldier.IsLeader = true;
                temp = soldier.transform;
                return temp;
            }
        }

        return temp;
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
