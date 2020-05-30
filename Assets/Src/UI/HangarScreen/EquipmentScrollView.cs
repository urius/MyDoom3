using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentScrollView : MonoBehaviour
{
    public event Action<int> ItemMouseDown = delegate { };

    [SerializeField]
    private GameObject _scrollItemPrefab;
    [SerializeField]
    private GameObject _scrollContent;
    [SerializeField]
    private List<EquipmentSlotView> _slotViews;
    [SerializeField]
    private Collider2D _collider;
    public Collider2D Collider => _collider;

    private readonly List<Action> _disposeActions = new List<Action>();

    private void OnEnable()
    {
        for (var i = 0; i < _slotViews.Count; i++)
        {
            var disposeAction = AddSlotListeners(i, _slotViews[i]);
            _disposeActions.Add(disposeAction);
        }
    }

    public void SetupItem(int index, EquipmentType equipmentType, Sprite iconSprite)
    {
        _slotViews[index].SetSlotType(ToSlotType(equipmentType));
        _slotViews[index].SetSpriteIcon(iconSprite);

        SetItemVisibility(index, true);
    }

    public void ResetView(int index)
    {
        _slotViews[index].SetSlotType(EquipmentSlotType.Default);
        _slotViews[index].SetSpriteIcon(null);
    }

    public void SetItemVisibility(int itemIdex, bool visibility)
    {
        _slotViews[itemIdex].IconVisibility = visibility;
    }

    private void OnDisable()
    {
        _disposeActions.ForEach(a => a());
        _disposeActions.Clear();
    }

    private Action AddSlotListeners(int i, EquipmentSlotView view)
    {
        void OnMouseDown()
        {
            ItemMouseDown(i);
        }
        view.OnMouseDown += OnMouseDown;

        return () => view.OnMouseDown -= OnMouseDown;
    }

    private EquipmentSlotType ToSlotType(EquipmentType equipmentType)
    {
        switch (equipmentType)
        {
            case EquipmentType.Engine:
                return EquipmentSlotType.Engine;
            case EquipmentType.Shield:
                return EquipmentSlotType.Shield;
            case EquipmentType.Weapon:
                return EquipmentSlotType.Weapon;
            default:
                throw new NotSupportedException($"slot type for equipment type {equipmentType} is not supported");
        }
    }
}
