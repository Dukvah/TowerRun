using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class EndGameController : MonoBehaviour
{
    [SerializeField] private GameObject gun, gunTarget;
    [SerializeField] private List<Rigidbody> bullets = new();
    [SerializeField] private Transform firePos, cameraPos;
    [SerializeField] private LineRenderer lr;
    [SerializeField] private TextMeshPro ammoText;
    [SerializeField] private float power = 1;
    
    private CameraFollower _cameraFollower;
    private Rigidbody _bulletPrefab;
    
    private bool _isTriggered, _isPlayer, _isBack, _canTakeAim;
    private int _ammo;

    private void Awake()
    {
        _cameraFollower = Camera.main.GetComponent<CameraFollower>();
        lr.SetPosition(0,gun.transform.position);
    }

    private void Update()
    {
        if (!_canTakeAim) return;
        
        if (Input.touchCount == 1)
        {
            Touch screenTouch = Input.GetTouch(0);
            
            switch (screenTouch.phase)
            {
                case TouchPhase.Began:
                    lr.enabled = true;
                    break;
                case TouchPhase.Moved:
                {
                    var vertical = -screenTouch.deltaPosition.y / 2;
                    var horizontal = screenTouch.deltaPosition.x / 2;
                    gun.transform.Rotate(vertical, horizontal, 0f);
                
                    var angleX = gun.transform.localRotation.eulerAngles.x;
                    var angleY = gun.transform.localRotation.eulerAngles.y;

                    angleX = Mathf.Clamp(NormalizeAngle(angleX), -25.0f, 10.0f);
                    angleY = Mathf.Clamp(NormalizeAngle(angleY), -45.0f, 45.0f);
                
                    gun.transform.localRotation = Quaternion.Euler(angleX, angleY, 0f);
                
                    var linePos = gun.transform.TransformPoint(Vector3.forward * 5);
                    lr.SetPosition(1, linePos);
                    break;
                }
                case TouchPhase.Ended:
                    Fire();
                    lr.enabled = false;
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Soldier"))
        {
            other.gameObject.tag = "Untagged";
            
            if (!_isTriggered)
            {
                _isTriggered = true;
                _cameraFollower.GoBossFightPos(cameraPos);
                GameManager.Instance.goBattle.Invoke();
                ammoText.gameObject.SetActive(true);
                IncreaseAmmo();
            }
            else
            {
                IncreaseAmmo();
            }
        }
    }

    private void IncreaseAmmo()
    {
        _ammo++;
        ammoText.text = $"{_ammo}";
        ammoText.gameObject.transform.DORewind();
        ammoText.gameObject.transform.DOPunchScale(Vector3.one, 0.5f);

        GameManager.Instance.SoldierCount--;
        if (GameManager.Instance.SoldierCount <= 0)
        {
            _bulletPrefab = bullets[PlayerPrefs.GetInt("SoldierIndex", 0)];
            gunTarget.SetActive(true);
            _canTakeAim = true;
            // savasa basla
        }
    }
    
    private void Fire()
    {
        var bullet = Instantiate(_bulletPrefab, firePos.position, gun.transform.rotation);
        bullet.isKinematic = false;
        bullet.useGravity = true;
        bullet.AddForce(bullet.transform.forward * power);
    }
    
    private float NormalizeAngle(float input)
    {       
        while (input > 360)
        {
            input -= 360;
        } 
        while (input < -360)
        {
            input += 360;
        }
        
        if (input > 180)
            input -= 360;        
        
        if (input < -180)
            input += 360;
        
        return input;
    }
}
