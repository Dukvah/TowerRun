using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject winPanel, losePanel, inGamePanel, tutorialPanel, battlePanel, upgradePanel, menuPanel,levelsPanel;
    [SerializeField] private List<string> moneyMulti = new();
    [SerializeField] private TextMeshProUGUI moneyText, minionCountText;
    [SerializeField] private GameObject moneyIcon, minionIcon;
    [SerializeField] private Button exitUpgradePanel, btnLevels;
    
    private LevelManager _levelManager;
    private Button _btnNext, _btnRestart, _btnStore;
    
    private void Awake()
    {
        ScriptAssign();
        ButtonAssign();
    }

    private void Start()
    {
        GameManager.Instance.onMoneyChange.Invoke();
        GameManager.Instance.onSoldierCountChange.Invoke();
        GameManager.Instance.gameReady.Invoke();
    }
    
    private void OnEnable()
    {
        GameManager.Instance.levelFailed.AddListener(() => ShowPanel(losePanel, true));
        GameManager.Instance.levelSuccess.AddListener(() => ShowPanel(winPanel, true));
        GameManager.Instance.gameReady.AddListener(GameReady);
        GameManager.Instance.gameStart.AddListener(HasGameStart);
        GameManager.Instance.onMoneyChange.AddListener(SetMoneyText);
        GameManager.Instance.onSoldierCountChange.AddListener(SetMinionText);
        GameManager.Instance.onBossFight.AddListener(BossFight);
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
            GameManager.Instance.onBossFight.RemoveListener(BossFight);
            GameManager.Instance.chestOpen.RemoveListener(ChestOpen);
        }
    }
    
    private void ScriptAssign()
    {
        _levelManager = FindObjectOfType<LevelManager>();
    }

    private void ButtonAssign()
    {
        _btnStore = menuPanel.GetComponentInChildren<Button>();
        _btnNext = winPanel.GetComponentInChildren<Button>();
        _btnRestart = losePanel.GetComponentInChildren<Button>();
        
        _btnStore.onClick.AddListener(() =>
        {
            upgradePanel.SetActive(true);
            GameManager.Instance.buttonClick.Invoke();
        });
        _btnNext.onClick.AddListener(() =>
        {
            _levelManager.LoadLevel(_levelManager.LevelIndex + 1);
            GameManager.Instance.buttonClick.Invoke();
        });
        _btnRestart.onClick.AddListener(() =>
        {
            _levelManager.LoadLevel(_levelManager.LevelIndex);
            GameManager.Instance.buttonClick.Invoke();
        });
        exitUpgradePanel.onClick.AddListener(() =>
        {
            upgradePanel.SetActive(false);
            GameManager.Instance.buttonClick.Invoke();
        });
        btnLevels.onClick.AddListener(() =>
        {
            OpenLevelsMenu();
            GameManager.Instance.buttonClick.Invoke();
        });
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
        menuPanel.SetActive(true);
        inGamePanel.SetActive(true);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        battlePanel.SetActive(false);
        upgradePanel.SetActive(false);
        ShowTutorial();
    }

    private void CloseAllPanels()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        battlePanel.SetActive(false);
        inGamePanel.SetActive(false);
        tutorialPanel.SetActive(false);
        menuPanel.SetActive(false);
    }
    private void HasGameStart()
    {
        tutorialPanel.SetActive(false);
        upgradePanel.SetActive(false);
        menuPanel.SetActive(false);
        GameManager.Instance.goArmy.Invoke();
    }

    private void BossFight()
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
        if (moneyIcon.activeSelf)
        {
            moneyIcon.transform.DORewind();
            moneyIcon.transform.DOPunchScale(Vector3.one / 4, 0.5f);
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
            moneyText.text = temp.ToString("F2") + " " + moneyMulti[value-1];
        }
    }
    private void SetMinionText()
    {
        if (GameManager.Instance.SoldierCount < 0) return;
        
        if (minionIcon.activeSelf)
        {
            minionIcon.transform.DORewind();
            minionIcon.transform.DOPunchScale(Vector3.one / 4, 0.5f);
        }
        
        minionCountText.text = GameManager.Instance.SoldierCount.ToString();
    }
    private void ShowTutorial()
    {
        tutorialPanel.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void OpenLevelsMenu()
    {
        Time.timeScale = 0;
        levelsPanel.SetActive(true);
    }
}
