using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ArmyController : MonoBehaviour
{
    [SerializeField] private List<SoldierController> soldiers = new List<SoldierController>();

    [SerializeField] private Transform targetBoss;
    [SerializeField] private BarrelStack barrelStack;
    [SerializeField] private BossController bossController;
    
    [Header("ArmyProperties")]
    [SerializeField] private int requiredMinion;
    
    [Header("Effects")]
    [SerializeField] private GameObject upgradeEffect;
    
    private CameraFollower _cameraFollower;
    private SoldierController _leader;

    private int _soldierCount;
    private float _jumpValue;
    private void Awake()
    {
        _cameraFollower = Camera.main.GetComponent<CameraFollower>();
        SoldierInitialize();
    }
    

    private void OnEnable()
    {
        GameManager.Instance.goArmy.AddListener(StartAttack);
        GameManager.Instance.goBattle.AddListener(GoBattle);
        GameManager.Instance.addSoldier.AddListener(AddSoldier);
    }

    private void SoldierInitialize()
    {
        _soldierCount = (int)PlayerPrefs.GetFloat("SoldierCount", 1);

        for (int i = 0; i < _soldierCount; i++)
        {
            soldiers[i].IsAlive = true;
            soldiers[i].gameObject.SetActive(true);
        }
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
        StartAttackAsync(SetLeader(), true);
        barrelStack.SendBarrels();
    }
    public void UpdateAttack()
    {
        StartAttackAsync(SetLeader());
    }

    private void StartAttackAsync(Transform leader, bool firstTime = false)
    {
        if (leader == null)
        {
            NoMoreMinion();
            return;
        }

        foreach (var soldier in soldiers)
        {
            if (!soldier.IsLeader && soldier.IsAlive)
            {
                soldier.SetTarget(leader, true);
            }
        }

        if (firstTime)
        {
            _jumpValue = PlayerPrefs.GetFloat("Jump", 5);
            InvokeRepeating(nameof(CanJump),5f,0.01f);
            _cameraFollower.CameraSetup(leader);
        }
        else
            _cameraFollower.CameraSetTarget(leader);
        
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
                soldier.JumpSoldier(_jumpValue);
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    private void GoBattle()
    {
        barrelStack.StopBarrels();
        
        var howMany = 0;
        
        foreach (var soldier in soldiers)
        {
            if (soldier.IsAlive)
            {
                howMany++;
            }
        }
        
        var i = 0;
        foreach (var soldier in soldiers)
        {
            
            if (soldier.IsAlive)
            {
                var radians = 2 * Mathf.PI / howMany * i;
                var horizontal = Mathf.Cos(radians);
                var vertical = Mathf.Sin(radians);
                
                var targetDir = new Vector3(horizontal, 0, vertical);
                
                var targetPos = bossController.gameObject.transform.position + targetDir * 1.8f;
                
                soldier.GoEnd(targetPos);
                i++;
            }
        }
        CancelInvoke(nameof(CanJump));
        StartCoroutine(CheckWinLose());
    }

    private IEnumerator CheckWinLose()
    {
        yield return new WaitForSeconds(2f);
        _cameraFollower.SetCameraMove(bossController.transform);
        int minionCount = 0;
        foreach (var soldier in soldiers)
        {
            if (soldier.IsAlive)
            {
                minionCount++;
            }
        }
        
        bossController.SetAnim(minionCount >= requiredMinion);
        foreach (var soldier in soldiers)
        {
            if (soldier.IsAlive)
            {
                soldier.SetAnim(minionCount >= requiredMinion);
            }
        }


        yield return new WaitForSeconds(2f);
        
        if (minionCount < requiredMinion)
        {
            GameManager.Instance.levelFailed.Invoke();
        }
        else
        {
            GameManager.Instance.levelSuccess.Invoke();
        }
    }

    private void NoMoreMinion()
    {
        CancelInvoke(nameof(CanJump));
        _cameraFollower.GoLosePose();
        GameManager.Instance.levelFailed.Invoke();
    }

    private void AddSoldier()
    {
        _soldierCount = (int)PlayerPrefs.GetFloat("SoldierCount", 1);
        
        soldiers[_soldierCount - 1].IsAlive = true;
        
        var tempSoldier = soldiers[_soldierCount - 1].gameObject;
        tempSoldier.SetActive(true);
        
        
        var effect = Instantiate(upgradeEffect, tempSoldier.transform.localPosition + Vector3.up, quaternion.identity);
        Destroy(effect, 2f);
    }
}
