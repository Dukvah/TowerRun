using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI priceText;
    private Button _upgradeButton;

    [Header("Card Properties")]
    [SerializeField] private string _name;
    [SerializeField] private List<float> values = new();
    [SerializeField] private List<float> prices = new();
    private int _cardLevel = 0;
    
    private void Awake()
    {
        _upgradeButton = GetComponent<Button>();
    }

    private void Start()
    {
        ItemInitialize();
    }

    private void OnEnable()
    {
        LoadData();
        ItemInitialize();
        GameManager.Instance.onMoneyChange.AddListener(ButtonInitialize);
        _upgradeButton.onClick.AddListener(Buy);
    }

    // private void OnDisable()
    // {
    //     if (GameManager.Instance)
    //         GameManager.Instance.onMoneyChange.RemoveListener(ButtonInitialize);
    //
    //     _upgradeButton.onClick.RemoveListener(Buy);
    // }

    void ItemInitialize()
    {
        ButtonInitialize();
    }

    void ButtonInitialize()
    {
        if (_cardLevel >= prices.Count)
        {
            priceText.text = "MAX";
            _upgradeButton.interactable = false;
            priceText.color = Color.red;
        }
        else
        {
            priceText.text = prices[_cardLevel].ToString();

            _upgradeButton.interactable = GameManager.Instance.PlayerMoney >= prices[_cardLevel];
            priceText.color = _upgradeButton.interactable ? Color.white : Color.red;
        }
    }

    private void Buy()
    {
        GameManager.Instance.PlayerMoney -= prices[_cardLevel];
        _cardLevel++;
        ItemInitialize();
        SaveData();
        
        GameManager.Instance.addSoldier.Invoke();
    }

    private void LoadData()
    {
        _cardLevel = PlayerPrefs.GetInt(_name + "Level", 0);
    }

    private void SaveData()
    {
        PlayerPrefs.SetFloat(_name, values[_cardLevel]);
        PlayerPrefs.SetInt(_name + "Level", _cardLevel);
    }
}
