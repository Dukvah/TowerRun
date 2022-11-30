using UnityEngine;
using DG.Tweening;


public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new(0, -5, 5);
    [SerializeField] private Space offsetPositionSpace = Space.Self;
    [SerializeField] [Range(0, 1)] private float lerpSpeed = 0.125f;
    [SerializeField] private Transform upgradePos, readyPos;

    private Transform _target;

    private Vector3 _startPos, _lerpPos, _lerpRot;
    private bool _lookAt = true;
    private bool _canMove = false;
    
    private void Awake()
    {
        _startPos = gameObject.transform.position;
    }

    public void CameraSetTarget(Transform target)
    {
        _target = target;
    }
    public void CameraSetup(Transform target)
    {
        _target = target;
        transform.DORotate(readyPos.rotation.eulerAngles,4f);
        transform.DOMove(readyPos.position, 5f).OnComplete(() =>
        {
            _canMove = true;
        });
    }

    private void LateUpdate() => CameraMove();


    public void SetCameraMove(Transform target)
    {
        _target = target;
        _canMove = true;
    }
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
    public void GoLosePose()
    {
        _canMove = false;
        transform.DOMove(_startPos,3f);
    }

    public void GoUpgradeLook()
    {
        transform.DOMove(upgradePos.position, 1f);
    }
    public void GoStartPos()
    {
        transform.DOMove(_startPos, 1f);
    }

    public void GoBossFightPos(Transform fightPos)
    {
        _canMove = false;
        transform.DORotate(fightPos.localRotation.eulerAngles,1f);
        transform.DOMove(fightPos.localPosition, 1f);
    }
    
}
