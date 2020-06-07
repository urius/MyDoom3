using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ShipPresentationMediator : MonoBehaviour
{
    [SerializeField]
    private Camera _shipRenderCamera;
    [SerializeField]
    private Transform _ship3dHolder;
    [SerializeField]
    private RectTransform _rectTransform;
    [SerializeField]
    private Transform _shipSlotIconsHolder;
    [SerializeField]
    private GameObject _slotPrefab;

    private MenuEventsAggregator _menuEventsAggregator;
    private PlayerDataModel _playerDataModel;

    private readonly Dictionary<EquipmentType, List<EquipmentSlotView>> _slotsByType = new Dictionary<EquipmentType, List<EquipmentSlotView>>()
    {
        [EquipmentType.Weapon] = new List<EquipmentSlotView>(),
        [EquipmentType.Engine] = new List<EquipmentSlotView>(),
        [EquipmentType.Shield] = new List<EquipmentSlotView>(),
    };

    [Inject]
    public void Construct(
        MenuEventsAggregator menuEventsAggregator,
        PlayerDataModel playerDataModel)
    {
        _menuEventsAggregator = menuEventsAggregator;
        _playerDataModel = playerDataModel;
    }

    private void OnEnable()
    {
        _menuEventsAggregator.FlyingEquipmentMouseUp += OnFlyingEquipmentMouseUp;
        _playerDataModel.ShipData.EquipmentSet += OnEquipmentSet;
        _playerDataModel.ShipData.EquipmentRemoved += OnEquipmentRemoved;
    }


    private void Start()
    {
        var ship3dView = Instantiate(_playerDataModel.ShipConfig.ShipPrefab, _ship3dHolder);
        var weaponsComponent = ship3dView.GetComponent<WeaponsComponent>();
        var equipmentComponent = ship3dView.GetComponent<EquipmentSlotsComponent>();

        CreateSlotsOnShip(EquipmentType.Weapon, weaponsComponent.WeaponTransforms, _slotPrefab);
        CreateSlotsOnShip(EquipmentType.Engine, equipmentComponent.EngineSlots, _slotPrefab);
        CreateSlotsOnShip(EquipmentType.Shield, equipmentComponent.ShieldSlots, _slotPrefab);

        ShowAllEquipment();
    }

    private void ShowAllEquipment()
    {
        foreach (var kvp in _slotsByType)
        {
            for (var i = 0; i < kvp.Value.Count; i++)
            {
                var equipment = _playerDataModel.ShipData.GetEquipment(kvp.Key, i);
                if (equipment != null)
                {
                    ShowEquipmentItem(i, equipment);
                }
            }
        }
    }

    private void OnDisable()
    {
        _menuEventsAggregator.FlyingEquipmentMouseUp -= OnFlyingEquipmentMouseUp;
        _playerDataModel.ShipData.EquipmentSet -= OnEquipmentSet;
        _playerDataModel.ShipData.EquipmentRemoved -= OnEquipmentRemoved;
    }

    private void OnFlyingEquipmentMouseUp(EquipmentBase equipment, Vector3 position)
    {
        var slots = _slotsByType[equipment.EquipmentType];
        var pos2d = new Vector2(position.x, position.y);

        var slotIndex = _playerDataModel.ShipData.GetEquipmentSlotIndex(equipment);
        if (slotIndex != -1)
        {
            slots[slotIndex].IconVisibility = true;
        }

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].ItemCollider.OverlapPoint(pos2d))
            {
                Debug.Log("Drop equipment on ship!");
                _menuEventsAggregator.FlyingEquipmentDropOverShip(i, equipment);
                return;
            }
        }
    }

    private void OnSlotMouseDown(EquipmentType slotType, int index)
    {
        _slotsByType[slotType][index].IconVisibility = false;
        _menuEventsAggregator.EquipmentSlotMouseDown(_playerDataModel.ShipData.GetEquipment(slotType, index));
    }

    private void OnEquipmentSet(int slotIndex, EquipmentBase equipment)
    {
        ShowEquipmentItem(slotIndex, equipment);
    }

    private void ShowEquipmentItem(int slotIndex, EquipmentBase equipment)
    {
        _slotsByType[equipment.EquipmentType][slotIndex].SetSpriteIcon(equipment.IconSprite);
    }

    private void OnEquipmentRemoved(int slotIndex, EquipmentBase equipment)
    {
        _slotsByType[equipment.EquipmentType][slotIndex].SetSpriteIcon(null);
    }

    private void CreateSlotsOnShip(EquipmentType slotType, Transform[] slotOrigimalTransforms, GameObject slotPrefab)
    {
        foreach (var originalTransform in slotOrigimalTransforms)
        {
            var slotIcon = Instantiate(slotPrefab, _shipSlotIconsHolder);
            var viewportPos = _shipRenderCamera.WorldToViewportPoint(originalTransform.position);
            var positionOnImage = new Vector2(_rectTransform.rect.width * viewportPos.x, _rectTransform.rect.height * viewportPos.y);
            slotIcon.transform.localPosition = new Vector3(positionOnImage.x, positionOnImage.y);
            var slotIndex = _slotsByType[slotType].Count;
            var equipmentSlotView = slotIcon.GetComponent<EquipmentSlotView>();
            if (equipmentSlotView == null)
            {
                throw new InvalidOperationException($"[ShipPresentationMediator::CreateSlotsOnShip] : prefab {slotPrefab.name} has no {nameof(EquipmentSlotView)}");
            }
            _slotsByType[slotType].Add(equipmentSlotView);

            equipmentSlotView.OnMouseDown += () =>
            {
                OnSlotMouseDown(slotType, slotIndex);
            };
        }
    }
}
