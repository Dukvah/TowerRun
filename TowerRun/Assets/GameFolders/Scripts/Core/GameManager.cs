using UnityEngine.Events;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public UnityEvent gameStart = new();
    [HideInInspector] public UnityEvent gameReady = new();
    [HideInInspector] public UnityEvent levelSuccess = new();
    [HideInInspector] public UnityEvent levelFailed = new();
    [HideInInspector] public UnityEvent onMoneyChange = new();
    [HideInInspector] public UnityEvent onSoldierCountChange = new();
    [HideInInspector] public UnityEvent onBossFight = new();
    [HideInInspector] public UnityEvent chestOpen = new();
    
    // CAMERA
    [HideInInspector] public UnityEvent resetCamera = new();

    // ARMY
    [HideInInspector] public UnityEvent goArmy = new();
    [HideInInspector] public UnityEvent goBattle = new();
    [HideInInspector] public UnityEvent selectSoldier = new();
    [HideInInspector] public UnityEvent changeSoldier = new();
    
    // UPGRADE
    [HideInInspector] public UnityEvent addSoldier = new();
    [HideInInspector] public UnityEvent buyButtonsInitialize = new();
    
    // AUDIO
    [HideInInspector] public UnityEvent stopMusic = new();
    [HideInInspector] public UnityEvent openMusic = new();
    [HideInInspector] public UnityEvent buttonClick = new();
    [HideInInspector] public UnityEvent menuMusic = new();
    [HideInInspector] public UnityEvent inGameMusic = new();
    private float _playerMoney;
    public float PlayerMoney
    {
        get => _playerMoney;
        set
        {
            _playerMoney = value;
            onMoneyChange.Invoke();
        }
    }
    private float _soldierCount;
    public float SoldierCount
    {
        get => _soldierCount;
        set
        {
            _soldierCount = value;
            onSoldierCountChange.Invoke();
        }
    }
    private void Start()
    {
        LoadData();
    }
    private void OnDisable()
    {
        SaveData();
    }
    private void LoadData()
    {
        PlayerMoney = PlayerPrefs.GetFloat("PlayerMoney", 1000);
    }

    private void SaveData()
    {
        PlayerPrefs.SetFloat("PlayerMoney", _playerMoney);
    }
}
