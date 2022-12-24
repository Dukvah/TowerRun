using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelsPanel : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Button backButton;

    private void OnEnable()
    {
        backButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            Time.timeScale = 1;
            
            GameManager.Instance.buttonClick.Invoke();
        });
    }
    
    public void LoadLevel(int index)
    {
        levelManager.LoadLevel(index);
        gameObject.SetActive(false);
        Time.timeScale = 1;
        GameManager.Instance.resetCamera.Invoke();
    }
}
