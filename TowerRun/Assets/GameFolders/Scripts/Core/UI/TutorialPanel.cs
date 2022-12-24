using System;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
    private Button _start;

    private void Awake()
    {
        _start = GetComponentInChildren<Button>();
    }

    private void OnEnable()
    {
        _start.onClick.AddListener(StartGameBtn);
    }

    private void OnDisable()
    {
        _start.onClick.RemoveListener(StartGameBtn);
    }

    private void StartGameBtn()
    {
        GameManager.Instance.gameStart.Invoke();
    }
}
