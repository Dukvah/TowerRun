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
        StartCoroutine(StartAttackAsync(SetLeader(),true));
        barrelStack.SendBarrels();
    }
    public void UpdateAttack()
    {
        StartCoroutine(StartAttackAsync(SetLeader()));
    }

    private IEnumerator StartAttackAsync(Transform leader, bool firstTime = false)
    {
        if (leader == null)
        {
            GameManager.Instance.levelFailed.Invoke();
            yield break;
        }
        
        _cameraFollower.CameraSetup(leader);
        
        foreach (var soldier in soldiers)
        {
            if (!soldier.IsLeader && soldier.IsAlive)
            {
                soldier.SetTarget(leader, true);
                yield return new WaitForSeconds(0.1f);
            }
        }
        
        if (firstTime)
            InvokeRepeating(nameof(CanJump),5f,0.01f);
    }
    
    private Transform SetLeader()
    {
        Transform temp = null;

        foreach (var soldier in soldiers)
        {
            if (soldier.IsAlive)
            {
                _leader = soldier;
                soldier.IsLeader = true;
                
                temp = soldier.transform;
                soldier.SetTarget(targetBoss);

                return temp;
            }
        }

        return temp;
    }
    
    private IEnumerator JumpArmy()
    {
        foreach (var soldier in soldiers)
        {
            if (soldier.IsAlive)
            {
                soldier.JumpSoldier();
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}
