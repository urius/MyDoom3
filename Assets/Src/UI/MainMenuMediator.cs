﻿using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuMediator : MonoBehaviour
{
    [SerializeField]
    private Button _playButton;
    [SerializeField]
    private Button _inventoryButton;
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
        _playButton.onClick.AddListener(OnPlayClick);
        _shipsButton.onClick.AddListener(OnShipsClick);
        _equipmentButton.onClick.AddListener(OnEquipmentShopClick);
        _inventoryButton.onClick.AddListener(OnInventoryClick);
    }

    private void OnDisable()
    {
        _playButton.onClick.RemoveAllListeners();
        _shipsButton.onClick.RemoveAllListeners();
        _equipmentButton.onClick.RemoveAllListeners();
        _inventoryButton.onClick.RemoveAllListeners();
    }

    private void OnPlayClick()
    {
        _eventsAggregator.PlayClicked(this.gameObject);
    }

    private void OnShipsClick()
    {
        _eventsAggregator.ShipsClicked(this.gameObject);
    }

    private void OnEquipmentShopClick()
    {
        _eventsAggregator.EquipmentShopClicked(this.gameObject);
    }

    private void OnInventoryClick()
    {
        _eventsAggregator.InventoryClicked(this.gameObject);
    }
}
