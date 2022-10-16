using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<Level> levels = new List<Level>();
    [SerializeField] private int levelIndex = 0;
    [SerializeField] private GameObject loadPanel;
    [SerializeField] private CameraFollower cameraFollower;
    
    private GameObject _currentLevel;
    
    private void Awake()
    {
        LoadData();
        LoadLevel(0, false);
    }
    
    private void Start()
    {
        GameManager.Instance.gameReady.Invoke();
    }
    public void LoadLevel(int value, bool loadpanel = true)
    {
        if (levels.Count == 0)
        {
            if (loadpanel)
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }

            return;
        }

        levelIndex += value;
        levelIndex %= levels.Count;

        if (!loadpanel)
        {
            ShowLoadPanel();
        }
        else
        {
            loadPanel.SetActive(true);
        }
    }

    public void ShowLoadPanel()
    {
        GameManager.Instance.gameReady.Invoke();
        if (_currentLevel) Destroy(_currentLevel);
        if (levels[levelIndex]) _currentLevel = Instantiate(levels[levelIndex].levelPrefab) as GameObject;
    }

    private void OnDisable()
    {
        SaveData();
    }

    private void LoadData()
    {
        levelIndex = PlayerPrefs.GetInt("Level", 0);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("Level", levelIndex);
    }
}
