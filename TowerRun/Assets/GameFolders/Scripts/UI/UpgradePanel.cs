using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] private GameObject upgradeFrame;
    [SerializeField] private Button openButton, closeButton;

    private CameraFollower _cameraFollower;

    private void Awake()
    {
        _cameraFollower = Camera.main.GetComponent<CameraFollower>();
    }

    private void OnEnable()
    {
        openButton.gameObject.SetActive(true);
        upgradeFrame.transform.localScale = Vector3.zero;
        
        openButton.onClick.AddListener(OpenFrame);
        closeButton.onClick.AddListener(CloseFrame);
    }

    private void OpenFrame()
    {
        openButton.gameObject.SetActive(false);
        upgradeFrame.transform.DOScale(Vector3.one, 1f);

        _cameraFollower.GoUpgradeLook();
    }

    private void CloseFrame()
    {
        openButton.gameObject.SetActive(true);
        upgradeFrame.transform.DOScale(Vector3.zero, 1f);
        
        _cameraFollower.GoStartPos();
    }

}
