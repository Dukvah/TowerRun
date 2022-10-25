using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyController : MonoBehaviour
{
    [SerializeField] private List<SoldierController> soldiers = new List<SoldierController>();

    [SerializeField] private Transform targetBoss;
    [SerializeField] private BarrelStack barrelStack;
    [SerializeField] private BossController bossController;
    [SerializeField] private int requiredMinion;

    private CameraFollower _cameraFollower;
    private SoldierController _leader;

    private void Awake()
    {
        _cameraFollower = Camera.main.GetComponent<CameraFollower>();
    }

    private void OnEnable()
    {
        GameManager.Instance.goArmy.AddListener(StartAttack);
        GameManager.Instance.goBattle.AddListener(GoBattle);
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
            StartCoroutine(NoMoreMinion());
            yield break;
        }

        foreach (var soldier in soldiers)
        {
            if (!soldier.IsLeader && soldier.IsAlive)
            {
                soldier.SetTarget(leader, true);
                yield return new WaitForSeconds(0.1f);
            }
        }

        if (firstTime)
        {
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
                soldier.JumpSoldier();
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    private void GoBattle()
    {
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

    private IEnumerator NoMoreMinion()
    {
        CancelInvoke(nameof(CanJump));
        _cameraFollower.SetCameraMove(bossController.transform);
        
        yield return new WaitForSeconds(2f);
        
        GameManager.Instance.levelFailed.Invoke();
    }
}
