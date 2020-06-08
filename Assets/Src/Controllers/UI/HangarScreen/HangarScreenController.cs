using System;
using UnityEngine;
using Zenject;

public class HangarScreenController : IInitializable, IDisposable
{
    private MenuEventsAggregator _menuEventsAggregator;
    private PlayerDataModel _playerDataModel;

    [Inject]
    public void Construct(
        PlayerDataModel playerDataModel,
        MenuEventsAggregator menuEventsAggregator)
    {
        _menuEventsAggregator = menuEventsAggregator;
        _playerDataModel = playerDataModel;
    }

    public void Initialize()
    {
        _menuEventsAggregator.FlyingEquipmentDropOverShip += OnDropOverShip;
        _menuEventsAggregator.FlyingEquipmentDropOverInventory += OnDropOverInventory;
    }

    public void Dispose()
    {
        _menuEventsAggregator.FlyingEquipmentDropOverShip -= OnDropOverShip;
        _menuEventsAggregator.FlyingEquipmentDropOverInventory -= OnDropOverInventory;
    }

    private void OnDropOverShip(int slotIndex, EquipmentConfigBase newEquipment)
    {
        if (_playerDataModel.ShipData.IsEquipped(newEquipment))
        {
            var fromSlotIndex = _playerDataModel.ShipData.GetEquipmentSlotIndex(newEquipment);

            _playerDataModel.ShipData.RemoveEquipment(newEquipment);

            var existingEquipment = _playerDataModel.ShipData.GetEquipment(newEquipment.EquipmentType, slotIndex);
            if (existingEquipment != null)
            {
                _playerDataModel.ShipData.RemoveEquipment(existingEquipment);
                _playerDataModel.ShipData.SetupEquipment(fromSlotIndex, existingEquipment);
            }
        }
        else
        {
            var prevSlotEquipment = _playerDataModel.ShipData.GetEquipment(newEquipment.EquipmentType, slotIndex);
            if (prevSlotEquipment != null)
            {
                _playerDataModel.SwapInventoryEquipment(newEquipment, prevSlotEquipment);
            }
            else
            {
                _playerDataModel.RemoveInventoryEquipment(newEquipment);
            }
        }

        _playerDataModel.ShipData.SetupEquipment(slotIndex, newEquipment);
    }

    private void OnDropOverInventory(EquipmentConfigBase equipment)
    {
        if (_playerDataModel.ShipData.IsEquipped(equipment))
        {
            _playerDataModel.ShipData.RemoveEquipment(equipment);
        }

        if (!_playerDataModel.HaveEquipmentInInventory(equipment))
        {
            _playerDataModel.AddInventoryEquipment(equipment);
        }
    }
}
