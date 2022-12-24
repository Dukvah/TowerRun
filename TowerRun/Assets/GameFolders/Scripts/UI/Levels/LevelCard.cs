using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCard : MonoBehaviour
{
    [SerializeField] private LevelsPanel levelsPanel;
    [SerializeField] private GameObject lockObj;
    [SerializeField] private Button selectButton;
    [SerializeField] private TextMeshProUGUI indexText;
    [SerializeField] private int index;

    private void OnEnable()
    {
        indexText.text = $"{index}"; //Text assignment

        if (index > 0)
            lockObj.SetActive(!PlayerPrefs.HasKey($"Level{index - 1}")); //Lock open : close
        
        
        selectButton.onClick.AddListener(() =>
        {
            levelsPanel.LoadLevel(index); //Button assignment
            GameManager.Instance.buttonClick.Invoke();
        }); 
    }

    
}
