using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EquipmentScrollViewMediator : MonoBehaviour
{
    [SerializeField]
    private EquipmentScrollView _equipmentScrollView;

    private MenuEventsAggregator _menuEventsAggregator;
    private PlayerDataModel _playerDataModel;

    [Inject]
    private void Construct(
        MenuEventsAggregator menuEventsAggregator,
        PlayerDataModel playerDataModel)
    {
        _menuEventsAggregator = menuEventsAggregator;
        _playerDataModel = playerDataModel;
    }

    private void OnEnable()
    {
        _equipmentScrollView.ItemMouseDown += OnItemMouseDown;
        _menuEventsAggregator.FlyingEquipmentMouseUp += OnEquipmentMouseUp;
        _playerDataModel.InventoryEquipmentRemoved += OnInventoryEquipmentRemoved;
        _playerDataModel.InventoryEquipmentSet += OnInventoryEquipmentSet;
    }

    private void Start()
    {
        SetupItems(_playerDataModel.InventoryEqipments);
    }

    private void OnDisable()
    {
        _equipmentScrollView.ItemMouseDown -= OnItemMouseDown;
        _menuEventsAggregator.FlyingEquipmentMouseUp -= OnEquipmentMouseUp;
        _playerDataModel.InventoryEquipmentRemoved -= OnInventoryEquipmentRemoved;
        _playerDataModel.InventoryEquipmentSet -= OnInventoryEquipmentSet;
    }
    private void OnInventoryEquipmentSet(int inventoryindex, EquipmentConfigBase equipment)
    {
        _equipmentScrollView.SetupItem(inventoryindex, equipment.EquipmentType, equipment.IconSprite);
    }

    private void OnInventoryEquipmentRemoved(EquipmentConfigBase equipment)
    {
        _equipmentScrollView.ResetView(_playerDataModel.InventoryEqipments.Count);
        SetupItems(_playerDataModel.InventoryEqipments);
    }

    private void OnItemMouseDown(int itemIndex)
    {
        if (_playerDataModel.InventoryEqipments.Count > itemIndex)
        {
            _menuEventsAggregator.EquipmentSlotMouseDown(_playerDataModel.InventoryEqipments[itemIndex]);
            _equipmentScrollView.SetItemVisibility(itemIndex, false);
        }
    }
    private void OnEquipmentMouseUp(EquipmentConfigBase equipment, Vector3 point)
    {
        if (_equipmentScrollView.Collider.OverlapPoint(new Vector2(point.x, point.y)))
        {
            _menuEventsAggregator.FlyingEquipmentDropOverInventory(equipment);
        }

        for (int i = 0; i < _playerDataModel.InventoryEqipments.Count; i++)
        {
            _equipmentScrollView.SetItemVisibility(i, true);
        }
    }

    private void SetupItems(IEnumerable<EquipmentConfigBase> equipments)
    {
        var i = 0;
        foreach (var equipment in equipments)
        {
            _equipmentScrollView.SetupItem(i, equipment.EquipmentType, equipment.IconSprite);
            i++;
        }
    }
}
