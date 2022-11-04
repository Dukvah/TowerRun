using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject winPanel, losePanel, inGamePanel, tutorialPanel, battlePanel, upgradePanel;
    [SerializeField] private List<string> moneyMulti = new();
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private GameObject money;
    
    private LevelManager _levelManager;
    private Button _btnNext, _btnRestart;
    
    private void Awake()
    {
        ScriptAssign();
        ButtonAssign();
    }

    private void Start()
    {
        GameManager.Instance.onMoneyChange.Invoke();
        GameManager.Instance.gameReady.Invoke();
    }
    
    private void OnEnable()
    {
        GameManager.Instance.levelFailed.AddListener(() => ShowPanel(losePanel, true));
        GameManager.Instance.levelSuccess.AddListener(() => ShowPanel(winPanel, true));
        GameManager.Instance.gameReady.AddListener(GameReady);
        GameManager.Instance.gameStart.AddListener(HasGameStart);
        GameManager.Instance.onMoneyChange.AddListener(SetMoneyText);
        GameManager.Instance.endBattle.AddListener(EndBattle);
        GameManager.Instance.chestOpen.AddListener(ChestOpen);
    }

    private void OnDisable()
    {
        if (GameManager.Instance)
        {
            GameManager.Instance.levelFailed.RemoveListener(() => ShowPanel(losePanel, true));
            GameManager.Instance.levelSuccess.RemoveListener(() => ShowPanel(winPanel, true));
            GameManager.Instance.gameStart.RemoveListener(HasGameStart);
            GameManager.Instance.gameReady.RemoveListener(GameReady);
            GameManager.Instance.endBattle.RemoveListener(EndBattle);
            GameManager.Instance.chestOpen.RemoveListener(ChestOpen);
        }
    }
    
    private void ScriptAssign()
    {
        _levelManager = FindObjectOfType<LevelManager>();
    }

    private void ButtonAssign()
    {
        _btnNext = winPanel.GetComponentInChildren<Button>();
        _btnRestart = losePanel.GetComponentInChildren<Button>();

        _btnNext.onClick.AddListener(() => _levelManager.LoadLevel(1));
        _btnRestart.onClick.AddListener(() => _levelManager.LoadLevel(0));
    }
    
    private void ShowPanel(GameObject panel, bool canvasMode = false)
    {
        CloseAllPanels();
        panel.SetActive(true);
        
        GameObject panelChild = panel.transform.GetChild(0).gameObject;
        
        panelChild.transform.localScale = Vector3.zero;
        panelChild.SetActive(true);
        panelChild.transform.DOScale(Vector3.one, 0.5f);
    }
    
    private void GameReady()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        battlePanel.SetActive(false);
        inGamePanel.SetActive(true);
        upgradePanel.SetActive(true);
        ShowTutorial();
    }

    private void CloseAllPanels()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        battlePanel.SetActive(false);
        inGamePanel.SetActive(false);
        tutorialPanel.SetActive(false);
    }
    private void HasGameStart()
    {
        tutorialPanel.SetActive(false);
        upgradePanel.SetActive(false);
        GameManager.Instance.goArmy.Invoke();
    }

    private void EndBattle()
    {
        inGamePanel.SetActive(false);
        battlePanel.SetActive(true);
    }
    private void ChestOpen()
    {
        battlePanel.SetActive(false);
        inGamePanel.SetActive(true);
    }
    private void SetMoneyText()
    {
        if (money.activeSelf)
        {
            money.transform.DORewind();
            money.transform.DOPunchScale(Vector3.one, 0.5f);
        }

        int moneyDigit = GameManager.Instance.PlayerMoney.ToString().Length;
        int value = (moneyDigit - 1) / 3;
        if (value < 1)
        {
            moneyText.text = GameManager.Instance.PlayerMoney.ToString();
        }
        else
        {
            float temp = GameManager.Instance.PlayerMoney / Mathf.Pow(1000, value);
            moneyText.text = temp.ToString("F2") + " " + moneyMulti[value];
        }
    }
    private void ShowTutorial()
    {
        tutorialPanel.transform.GetChild(0).gameObject.SetActive(true);
    }
}
