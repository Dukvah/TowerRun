using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoldierController : MonoBehaviour
{
    [SerializeField] private List<GameObject> soldierVisuals = new();
    [SerializeField] private List<Rigidbody> bombs = new();
    [SerializeField] private List<Animator> animators = new();
    [SerializeField] private GameObject deathEffect;
    
    private Rigidbody _rb;
    private Collider _col;
    private NavMeshAgent _agent;
    private ArmyController _armyController;
    private Transform _target;
    
    
    public bool Grounded { get; private set; } = true;
    public bool IsAlive { get; set; }
    public bool IsLeader { get; set; }

    private int _soldierIndex;
    public int SoldierIndex
    {
        get => _soldierIndex;
        set
        {
            _soldierIndex = value;
            ChangeSoldier();
        }
    }
    
    private void Awake()
    {
        _col = GetComponentInChildren<Collider>();
        _armyController = GetComponentInParent<ArmyController>();
        _rb = GetComponent<Rigidbody>();
        _agent = GetComponentInParent<NavMeshAgent>();
        
        _agent.speed = PlayerPrefs.GetFloat("Speed", 2);
        _soldierIndex = PlayerPrefs.GetInt("SoldierIndex", 0);
        
        ChangeSoldier();
    }

    public void SetTarget(Transform target, bool repeat = false)
    {
        _target = target;
        _agent.speed = PlayerPrefs.GetFloat("Speed", 2);
        _agent.SetDestination(_target.position);
        animators[_soldierIndex].SetBool("isRun",true);

        if (repeat)
        {
            _agent.speed *= 1.5f;
             InvokeRepeating(nameof(FollowLeader),0,0.02f);
        }
    }
    private void FollowLeader()
    {
        switch (IsAlive && _target != null)
        {
            case true:
                _agent.SetDestination(_target.position);
                break;
            case false:
                CancelInvoke(nameof(FollowLeader));
                IsAlive = false;
                break;
        }
    }
    public void JumpSoldier(float jumpValue)
    {
        // make the jump
        animators[_soldierIndex].SetBool("isJump",true);
        _rb.isKinematic = false;
        _rb.AddRelativeForce(Vector3.up * jumpValue, ForceMode.Impulse);
        _rb.useGravity = true;
        
        Grounded = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            if (!Grounded)
            {
                animators[_soldierIndex].SetBool("isJump",false);
                _rb.isKinematic = true;
                _rb.useGravity = false;
                Grounded = true;
                transform.localPosition = Vector3.zero;
            }
        }
        
        if (collision.collider.CompareTag("Barrel"))
        {   
            IsAlive = false;
            Grounded = false;
            _col.enabled = false;
            _agent.enabled = false;
            
            _rb.isKinematic = false;
            _rb.useGravity = true;
            _rb.constraints = RigidbodyConstraints.None;
            _rb.AddForce(transform.right * 10, ForceMode.Impulse);

            GameManager.Instance.SoldierCount--;
            
            if (IsLeader)
            {
                IsLeader = false;
                _armyController.UpdateAttack();
            }
            
            Destroy(transform.parent.gameObject, 2f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EndGameArea") && IsAlive)
        {
            IsAlive = false;

            bombs[_soldierIndex].isKinematic = false;
            bombs[_soldierIndex].useGravity = true;
            bombs[_soldierIndex].AddRelativeForce(new Vector3(0,1,1), ForceMode.Impulse); 
            Destroy(bombs[_soldierIndex],0.5f);
            
            DeathEffect();
            transform.parent.gameObject.SetActive(false);
        }
    }

    private void ChangeSoldier(bool effect = false)
    {
        foreach (var soldierVisual in soldierVisuals)
        {
            soldierVisual.SetActive(false);
        }
        soldierVisuals[_soldierIndex].SetActive(true);

        //if (effect)
        // ADD EFFECT
    }

    #region Effects

    private void DeathEffect()
    {
        var ps = Instantiate(deathEffect, transform.position + new Vector3(0f, 0.75f, 0f), Quaternion.identity);
        Destroy(ps, 1f);
    }

    #endregion
}
