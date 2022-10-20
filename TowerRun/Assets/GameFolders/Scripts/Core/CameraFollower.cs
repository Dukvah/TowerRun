using System;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    private CinemachineBrain _cmBrain;

    private Transform _target;
    private Animator _animator;
    
    private void Awake()
    {
        _cmBrain = GetComponent<CinemachineBrain>();
        _animator = GetComponent<Animator>();
    }

    public void CameraSetTarget(Transform target)
    {
        _virtualCamera.m_Follow = target;
        _virtualCamera.m_LookAt = target;
        _cmBrain.enabled = true;
    }
    public void CameraSetup(Transform target)
    {
        Vector3 temp = transform.position;
        temp.z += 17;
        temp.y -= 20;

        _target = target;
        
        _animator.SetBool("startGame",true);
        // gameObject.transform.DOMove(temp, 5f).OnComplete(() =>
        // {
        //     CameraSetTarget(_target);
        // });
    }

    public void LookTarget()
    {
        CameraSetTarget(_target);
    }
    
}
