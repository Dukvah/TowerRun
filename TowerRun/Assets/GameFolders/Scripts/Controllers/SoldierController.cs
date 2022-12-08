using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class SoldierController : MonoBehaviour
{
    [Header("NEEDS")]
    [SerializeField] private List<GameObject> soldierVisuals = new();
    [SerializeField] private List<Rigidbody> bombs = new();
    [SerializeField] private List<Animator> animators = new();
    
    [Header("EFFECTS")]
    [SerializeField] private List<GameObject> stepDustEffects = new(); // object pool
    [SerializeField] private GameObject deathEffect;

    [Header("SOUNDS")] 
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip deadClip;
    
    private Rigidbody _rb;
    private Collider _col;
    private NavMeshAgent _agent;
    private ArmyController _armyController;
    private Transform _target;
    private AudioSource _audioSource;
    private IEnumerator _co;

    
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
        _audioSource = GetComponent<AudioSource>();
        
        _agent.speed = PlayerPrefs.GetFloat("Speed", 2);
        _soldierIndex = PlayerPrefs.GetInt("SoldierIndex", 0);
        
        SetStepPool();
        ChangeSoldier();
    }

    public void SetTarget(Transform target, bool repeat = false)
    {
        _target = target;
        _agent.speed = PlayerPrefs.GetFloat("Speed", 2);
        _agent.SetDestination(_target.position);
        animators[_soldierIndex].SetBool("isRun",true);

        _co = StepEffectCreator();
        StartCoroutine(_co);
        
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
        JumpSound();
        
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
            
            DieSound();
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
            StopCoroutine(_co);
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

    #region Sounds

    private void JumpSound()
    {
        if (PlayerPrefs.GetInt("Voice",1) == 0) return;
        _audioSource.clip = jumpClip;
        _audioSource.Play();
    }
    private void DieSound()
    {
        if (PlayerPrefs.GetInt("Voice",1) == 0) return;
        _audioSource.clip = deadClip;
        _audioSource.Play();
    }

    #endregion
}
