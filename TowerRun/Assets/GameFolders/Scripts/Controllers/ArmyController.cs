using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyController : MonoBehaviour
{
    [SerializeField] private List<SoldierController> soldiers = new();

    [SerializeField] private Transform targetBoss;
    [SerializeField] private BarrelStack barrelStack;
    [SerializeField] private ArmyCameraTarget targetCamera;
    
    [Header("ArmyProperties")]
    [SerializeField] private int requiredMinion;
    
    [Header("Effects")]
    [SerializeField] private GameObject upgradeEffect;
    
    private CameraFollower _cameraFollower;
    public SoldierController Leader { get; private set; }

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
        GameManager.Instance.changeSoldier.AddListener(ChangeSoldier);
    }

    private void SoldierInitialize()
    {
        GameManager.Instance.SoldierCount = (int)PlayerPrefs.GetFloat("SoldierCount", 1);
        _soldierCount = (int)PlayerPrefs.GetFloat("SoldierCount", 1);
        
        for (int i = 0; i < _soldierCount; i++)
        {
            soldiers[i].IsAlive = true;
            soldiers[i].transform.parent.gameObject.SetActive(true);
        }
    }
    private void CanJump()
    {
        if (Input.GetMouseButtonDown(0) && Leader.Grounded)
        {
            StartCoroutine(JumpArmy());
        }
    }
    
    private void StartAttack()
    {
        StartAttackAsync(SetLeader(), true);
        barrelStack.SendBarrels();
        targetCamera.SetStart(targetBoss.position);
        GameManager.Instance.inGameMusic.Invoke();
    }
    public void UpdateAttack()
    {
        StartAttackAsync(SetLeader());
    }

    private void StartAttackAsync(Transform leader, bool firstTime = false)
    {
        if (leader == null)
        {
            NoMoreSoldier();
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
            InvokeRepeating(nameof(CanJump),5f,Time.deltaTime);
            _cameraFollower.CameraSetup(targetCamera.transform);
        }
        else
            _cameraFollower.CameraSetTarget(targetCamera.transform);
        
    }
    
    private Transform SetLeader()
    {
        Transform temp = null;

        foreach (var soldier in soldiers)
        {
            if (soldier.IsAlive)
            {
                Leader = soldier;
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
        targetCamera.Stop();
        barrelStack.StopBarrels();
        CancelInvoke(nameof(CanJump));
    }
    private void NoMoreSoldier()
    {
        CancelInvoke(nameof(CanJump));
        _cameraFollower.GoLosePose();
        GameManager.Instance.levelFailed.Invoke();
    }

    private void AddSoldier()
    {
        _soldierCount = (int)PlayerPrefs.GetFloat("SoldierCount", 1);
        GameManager.Instance.SoldierCount = _soldierCount;
        
        soldiers[_soldierCount - 1].IsAlive = true;
        
        var tempSoldier = soldiers[_soldierCount - 1].transform.parent.gameObject;
        tempSoldier.SetActive(true);
        
        
        var effect = Instantiate(upgradeEffect, tempSoldier.transform.localPosition, Quaternion.identity);
        Destroy(effect, 2f);
    }

    private void ChangeSoldier()
    {
        foreach (var soldier in soldiers)
        {
            soldier.SoldierIndex = PlayerPrefs.GetInt("SoldierIndex", 0);
        }
    }
}
