﻿using System;
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

    private void OnDropOverShip(int slotIndex, EquipmentBase newEquipment)
    {
        if (_playerDataModel.ShipData.IsEquipped(newEquipment))
        {
            _playerDataModel.ShipData.RemoveEquipment(newEquipment);
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

    private void OnDropOverInventory(EquipmentBase equipment)
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