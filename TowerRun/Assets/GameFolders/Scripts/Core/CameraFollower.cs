using System;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class CameraFollower : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera;

    private bool _isRun = false;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void CameraSetTarget(Transform target)
    {
        _virtualCamera.m_Follow = target;
        _virtualCamera.m_LookAt = target;
        _isRun = true;
    }
    public void CameraSetup(Transform target)
    {
        Vector3 temp = transform.position;
        temp.z += 20;
        temp.y -= 15;
        transform.DOMove(temp, 3f).OnComplete(() =>
        {
            CameraSetTarget(target);
        });
    }
    
}
