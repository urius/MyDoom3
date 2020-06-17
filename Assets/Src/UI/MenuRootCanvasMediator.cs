using System;
using UnityEngine;
using Zenject;

public class MenuRootCanvasMediator : MonoBehaviour
{
    [SerializeField]
    private GameObject _selectLevelScreen;
    [SerializeField]
    private GameObject _mainMenuScreen;
    [SerializeField]
    private GameObject _shipsScreen;
    [SerializeField]
    private GameObject _equipmentScreen;
    [SerializeField]
    private GameObject _inventoryScreen;

    private MenuEventsAggregator _eventsAggregator;
    private DiContainer _diContainer;
    private PlayerDataModel _playerDataModel;

    [Inject]
    public void Construct(
        DiContainer diContainer,
        MenuEventsAggregator eventsAggregator,
        PlayerDataModel playerDataModel)
    {
        _eventsAggregator = eventsAggregator;
        _diContainer = diContainer;
        _playerDataModel = playerDataModel;
    }

    public void OnEnable()
    {
        _eventsAggregator.PlayClicked += OnPlayClicked;
        _eventsAggregator.HomeClicked += OnHomeClicked;
        _eventsAggregator.ShipsClicked += OnShipsClicked;
        _eventsAggregator.EquipmentShopClicked += OnEquipmentShopClicked;
        _eventsAggregator.InventoryClicked += OnInventoryClicked;
    }

    private async void Start()
    {
        await _playerDataModel.DataLoadedTask;

        _diContainer.InstantiatePrefab(_mainMenuScreen, transform);
    }

    public void OnDisable()
    {
        _eventsAggregator.PlayClicked -= OnPlayClicked;
        _eventsAggregator.HomeClicked -= OnHomeClicked;
        _eventsAggregator.ShipsClicked -= OnShipsClicked;
        _eventsAggregator.EquipmentShopClicked -= OnEquipmentShopClicked;
        _eventsAggregator.InventoryClicked -= OnInventoryClicked;
    }

    private void OnPlayClicked(GameObject activeScreen)
    {
        Destroy(activeScreen);
        _diContainer.InstantiatePrefab(_selectLevelScreen, transform);
    }

    private void OnShipsClicked(GameObject activeScreen)
    {
        Destroy(activeScreen);
        _diContainer.InstantiatePrefab(_shipsScreen, transform);
    }

    private void OnHomeClicked(GameObject activeScreen)
    {
        Destroy(activeScreen);
        _diContainer.InstantiatePrefab(_mainMenuScreen, transform);
    }

    private void OnEquipmentShopClicked(GameObject activeScreen)
    {
        Destroy(activeScreen);
        _diContainer.InstantiatePrefab(_equipmentScreen, transform);
    }

    private void OnInventoryClicked(GameObject activeScreen)
    {
        Destroy(activeScreen);
        _diContainer.InstantiatePrefab(_inventoryScreen, transform);
    }
}
