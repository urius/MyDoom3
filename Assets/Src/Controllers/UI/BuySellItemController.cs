using System;
using UnityEngine;
using Zenject;

public class BuySellItemController : IInitializable, IDisposable
{
    private MenuEventsAggregator _menuEventsAggregator;
    private PlayerDataModel _playerDataModel;
    private ModelsFactory _modelsFactory;

    [Inject]
    public void Construct(
        MenuEventsAggregator menuEventsAggregator,
        PlayerDataModel playerDataModel,
        ModelsFactory modelsFactory)
    {
        _menuEventsAggregator = menuEventsAggregator;
        _playerDataModel = playerDataModel;
        _modelsFactory = modelsFactory;
    }

    public void Initialize()
    {
        _menuEventsAggregator.BuyEquipmentClicked += OnBuyEquipment;
        _menuEventsAggregator.BuyShipClicked += OnBuyShip;
    }

    public void Dispose()
    {
        _menuEventsAggregator.BuyEquipmentClicked -= OnBuyEquipment;
        _menuEventsAggregator.BuyShipClicked -= OnBuyShip;
    }

    private void OnBuyEquipment(EquipmentConfigBase equipment)
    {
        _playerDataModel.AddInventoryEquipment(_modelsFactory.CreateEquipment(equipment));
        Debug.Log("l: " + _playerDataModel.InventoryEqipments.Count);
    }

    private void OnBuyShip(ShipConfig ship)
    {
        var numWeapons = ship.ShipPrefab.GetComponent<WeaponsComponent>().WeaponTransforms.Length;
        _playerDataModel.ShipData = new ShipData(ship, new WeaponData[numWeapons]);
    }

}
