using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuMediator : MonoBehaviour
{
    [SerializeField]
    private Button _shipsButton;
    [SerializeField]
    private Button _equipmentButton;

    private MenuEventsAggregator _eventsAggregator;

    [Inject]
    public void Cnstruct(MenuEventsAggregator eventsAggregator)
    {
        _eventsAggregator = eventsAggregator;
    }

    private void OnEnable()
    {
        _shipsButton.onClick.AddListener(OnShipsClick);
        _equipmentButton.onClick.AddListener(OnEquipmentShopClick);
    }

    private void OnDisable()
    {
        _shipsButton.onClick.RemoveAllListeners();
        _equipmentButton.onClick.RemoveAllListeners();
    }

    private void OnShipsClick()
    {
        _eventsAggregator.ShipsClicked(this.gameObject);
    }

    private void OnEquipmentShopClick()
    {
        _eventsAggregator.EquipmentShopClicked(this.gameObject);
    }
}
