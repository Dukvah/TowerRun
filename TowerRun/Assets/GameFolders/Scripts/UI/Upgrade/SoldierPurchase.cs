using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoldierPurchase : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject lockObject, tickObject, selectText;
    [SerializeField] private Button buyButton, selectButton;
    
    [Header("Card Properties")]
    [SerializeField] private int cardIndex , price;
    [SerializeField] private bool isFree;

    private void Awake()
    {
        if (isFree)
            PlayerPrefs.SetInt($"Minion{cardIndex}", 1);
    }

    private void OnEnable()
    {
        ItemInitialize();

        buyButton.onClick.AddListener(Buy);
        selectButton.onClick.AddListener(Select);
        
        GameManager.Instance.buyButtonsInitialize.AddListener(ButtonInitialize);
        GameManager.Instance.selectSoldier.AddListener(NewSelect);
    }

    // private void OnDisable()
    // {
    //     buyButton.onClick.RemoveListener(Buy);
    //     selectButton.onClick.RemoveListener(Select);
    //     GameManager.Instance.selectSoldier.RemoveListener(NewSelect);
    // }

    private void Buy()
    {
        if (PlayerPrefs.GetInt($"Minion{cardIndex}",0) == 0)
        {
            GameManager.Instance.PlayerMoney -= price;
            PlayerPrefs.SetInt($"Minion{cardIndex}", 1);
            ItemInitialize();
            
            GameManager.Instance.buyButtonsInitialize.Invoke();
            GameManager.Instance.buttonClick.Invoke();
        }
    }

    private void Select()
    {
        GameManager.Instance.selectSoldier.Invoke();
        
        selectText.SetActive(false);
        tickObject.SetActive(true);

        PlayerPrefs.SetInt("SoldierIndex", cardIndex);
        GameManager.Instance.changeSoldier.Invoke();
        GameManager.Instance.buttonClick.Invoke();
        GameManager.Instance.buttonClick.Invoke();
    }
    private void NewSelect()
    {
        tickObject.SetActive(false);
        selectText.SetActive(true);
    }
    private void ItemInitialize()
    {
        if (PlayerPrefs.GetInt($"Minion{cardIndex}",0) == 1)
        {
            lockObject.SetActive(false);
            buyButton.gameObject.SetActive(false);
            selectButton.gameObject.SetActive(true);
            SelectInitialize();
        }
        else
        {
           lockObject.SetActive(true);
           ButtonInitialize();
        }
    }

    private void ButtonInitialize()
    {
        priceText.text = price.ToString();
        buyButton.interactable = !(GameManager.Instance.PlayerMoney < price);
    }

    private void SelectInitialize()
    {
        if (PlayerPrefs.GetInt("SoldierIndex") == cardIndex)
        {
            selectText.SetActive(false);
            tickObject.SetActive(true);
        }
    }
}
