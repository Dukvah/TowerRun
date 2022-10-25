using System;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    
    [SerializeField] private Vector3 offset = new(0, -5, 5);
    [SerializeField] [Range(0, 1)] private float lerpSpeed = 0.125f;
    
    private CinemachineBrain _cmBrain;

    private Transform _target;
    private Animator _animator;

    Vector3 _lerpPos;
    bool _canMove = false;
    
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

    private void LateUpdate() => CameraMove();


    public void SetCameraMove(Transform target)
    {
        _target = target;
        _cmBrain.enabled = false;
        _canMove = true;
    }
    private void CameraMove()
    {
        if (!_canMove) return;
        
        _lerpPos = Vector3.Lerp(transform.localPosition, _target.localPosition - offset, lerpSpeed);
        transform.localPosition = _lerpPos;

    }
    public void LookTarget()
    {
        CameraSetTarget(_target);
    }
    
}
