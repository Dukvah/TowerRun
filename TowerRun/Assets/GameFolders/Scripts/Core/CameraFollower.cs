using System;
using UnityEngine;
using DG.Tweening;


public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new(0, -5, 5);
    [SerializeField] private Space offsetPositionSpace = Space.Self;
    [SerializeField] [Range(0, 1)] private float lerpSpeed = 0.125f;
    [SerializeField] private Transform readyPos;

    private Transform _target;
    private Sequence _mySequence;
    private Sequence _myRotateSequence;
    
    private Vector3 _startPos, _lerpPos, _lerpRot;
    private Quaternion _startRot;
    private bool _lookAt = true;
    private bool _canMove = false;
    
    private void Awake()
    {
        _startPos = gameObject.transform.position;
        _startRot = gameObject.transform.rotation;
    }

    private void OnEnable()
    {
        GameManager.Instance.resetCamera.AddListener(ResetPosition);
    }
    private void OnDisable()
    {
        GameManager.Instance.resetCamera.RemoveListener(ResetPosition);
    }
    public void CameraSetTarget(Transform target)
    {
        _target = target;
    }
    public void CameraSetup(Transform target)
    {
        _target = target;
        
        _mySequence = DOTween.Sequence();
        _myRotateSequence = DOTween.Sequence();
        
        _myRotateSequence.Append(transform.DORotate(readyPos.rotation.eulerAngles,4f));
        _mySequence.Append(transform.DOMove(readyPos.position, 5f).OnComplete(() =>
        {
            _canMove = true;
        }));
        
        // transform.DORotate(readyPos.rotation.eulerAngles,4f);
        // transform.DOMove(readyPos.position, 5f).OnComplete(() =>
        // {
        //     _canMove = true;
        // });
    }
    private void LateUpdate() => CameraMove();
    
    private void CameraMove()
    {
        if (!_canMove) return;
        
        if(offsetPositionSpace == Space.Self)
        {
            _lerpPos = Vector3.Lerp(_target.position, _target.TransformPoint(offset), lerpSpeed);
            transform.position = _lerpPos;
        }
        else
        {
            transform.position = _target.position + offset;
        }
 
        // compute rotation
        if(_lookAt)
        {
            transform.LookAt(_target);
        }
        else
        {
            transform.rotation = _target.rotation;
        }

    }

    private void ResetPosition()
    {
        _mySequence?.Kill();
        _myRotateSequence?.Kill();
        
        _canMove = false;
        _target = null;

        gameObject.transform.position = _startPos;
        gameObject.transform.rotation = _startRot;
    }
    public void GoLosePose()
    {
        _canMove = false;
        transform.DOMove(_startPos,3f);
    }
    public void GoBossFightPos(Transform fightPos)
    {
        _canMove = false;
        
        _mySequence = DOTween.Sequence();
        _myRotateSequence = DOTween.Sequence();
        
        _myRotateSequence.Append(transform.DORotate(fightPos.localRotation.eulerAngles,1f));
        _mySequence.Append(transform.DOMove(fightPos.localPosition, 1f));
        
        //transform.DORotate(fightPos.localRotation.eulerAngles,1f);
        //transform.DOMove(fightPos.localPosition, 1f);
    }
    
}
