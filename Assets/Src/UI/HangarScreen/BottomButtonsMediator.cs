using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BottomButtonsMediator : MonoBehaviour
{
    [SerializeField] private SellButtonView _sellButtonView;
    [SerializeField] private Button _selectLevelButton;
    [SerializeField] private GameObject _screenRoot;

    private MenuEventsAggregator _menuEventsAggregator;
    private PlayerDataModel _plyerData;

    [Inject]
    public void Construct(
        MenuEventsAggregator menuEventsAggregator,
        PlayerDataModel plyerData)
    {
        _menuEventsAggregator = menuEventsAggregator;
        _plyerData = plyerData;
    }

    private void OnEnable()
    {
        _menuEventsAggregator.EquipmentSlotMouseDown += OnStartDrag;
    }

    private void Start()
    {
        _sellButtonView.HideAsync(false);

        _selectLevelButton.onClick.AddListener(OnSelectLevelClicked);
    }

    private void OnDisable()
    {
        _menuEventsAggregator.EquipmentSlotMouseDown -= OnStartDrag;
        _menuEventsAggregator.FlyingEquipmentMouseUp -= OnStopDrag;
    }

    private void OnStartDrag(EquipmentData equipment)
    {
        UpdateSelectLevelButtonVisibility(equipment == null);

        if (equipment == null)
        {
            return;
        }

        _menuEventsAggregator.FlyingEquipmentMouseUp += OnStopDrag;

        _sellButtonView.SetPriceText($"{equipment.Config.Cost * _plyerData.SellMultiplier}$");
        _sellButtonView.Show(true);
    }

    private async void OnStopDrag(EquipmentData equipmentData, Vector3 point)
    {
        _menuEventsAggregator.FlyingEquipmentMouseUp -= OnStopDrag;

        if (equipmentData == null)
        {
            return;
        }

        if (_sellButtonView.HitTestPoint(point))
        {
            _menuEventsAggregator.RequestSellEqipment(equipmentData);
        }

        await _sellButtonView.HideAsync(true);

        UpdateSelectLevelButtonVisibility(true);
    }

    private void UpdateSelectLevelButtonVisibility(bool isVisible)
    {
        _selectLevelButton.gameObject.SetActive(isVisible);
    }

    private void OnSelectLevelClicked()
    {
        _menuEventsAggregator.PlayClicked(_screenRoot);
    }
}
