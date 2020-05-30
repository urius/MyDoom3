using System;
using UnityEngine;
using Zenject;

public class MenuRootCanvasMediator : MonoBehaviour
{
    [SerializeField]
    private GameObject _mainMenuScreen;
    [SerializeField]
    private GameObject _shipsScreen;
    [SerializeField]
    private GameObject _equipmentScreen;

    private MenuEventsAggregator _eventsAggregator;
    private DiContainer _diContainer;

    [Inject]
    public void Construct(DiContainer diContainer, MenuEventsAggregator eventsAggregator)
    {
        _eventsAggregator = eventsAggregator;
        _diContainer = diContainer;
    }

    public void OnEnable()
    {
        _eventsAggregator.HomeClicked += OnHomeClicked;
        _eventsAggregator.ShipsClicked += OnShipsClicked;
        _eventsAggregator.EquipmentShopClicked += OnEquipmentShopClicked;
    }

    public void OnDisable()
    {
        _eventsAggregator.HomeClicked -= OnHomeClicked;
        _eventsAggregator.ShipsClicked -= OnShipsClicked;
        _eventsAggregator.EquipmentShopClicked -= OnEquipmentShopClicked;
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
}
