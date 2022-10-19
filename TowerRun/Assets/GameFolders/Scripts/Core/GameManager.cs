using UnityEngine.Events;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public UnityEvent gameStart = new();
    [HideInInspector] public UnityEvent gameReady = new();
    [HideInInspector] public UnityEvent gameEnd = new();
    [HideInInspector] public UnityEvent levelSuccess = new();
    [HideInInspector] public UnityEvent levelFailed = new();
    [HideInInspector] public UnityEvent onMoneyChange = new();
    [HideInInspector] public UnityEvent endBattle = new();
    [HideInInspector] public UnityEvent chestOpen = new();
    
    // ARMY
    [HideInInspector] public UnityEvent goArmy = new();

    public bool IsPlaying { get; set; } = false;
    
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
        PlayerMoney = PlayerPrefs.GetFloat("PlayerMoney", 0);
    }

    private void SaveData()
    {
        PlayerPrefs.SetFloat("PlayerMoney", _playerMoney);
    }
}
