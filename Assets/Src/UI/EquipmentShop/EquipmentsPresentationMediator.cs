using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EquipmentsPresentationMediator : MonoBehaviour
{
    [SerializeField]
    private TabButtonView _weaponsTab;
    [SerializeField]
    private TabButtonView _enginesTab;
    [SerializeField]
    private TabButtonView _shieldsTab;
    [SerializeField]
    private Transform _scrollContent;
    [SerializeField]
    private GameObject _scrollItemPrefab;
    [SerializeField]
    private EquipmentShopScreenEventsAggregator _localEventsAggregator;

    private readonly List<Action> _removeItemFuncs = new List<Action>();

    private EquipmentShopConfigsProvider _equipmentConfigsProvider;
    private LocalizationProvider _localizationProvider;

    [Inject]
    private void Construct(
        LocalizationProvider localizationProvider,
        EquipmentShopConfigsProvider equipmentConfigsProvider)
    {
        _equipmentConfigsProvider = equipmentConfigsProvider;
        _localizationProvider = localizationProvider;
    }

    private void OnEnable()
    {
        _weaponsTab.Selected += OnWeaponsSelected;
        _enginesTab.Selected += OnEnginesSelected;
        _shieldsTab.Selected += OnShieldsSelected;
    }

    private void Start()
    {
        _enginesTab.SetSelected();
    }

    private void OnWeaponsSelected()
    {
        ShowEquipmentItems(_equipmentConfigsProvider.GetWeapons());
    }

    private void OnEnginesSelected()
    {
        ShowEquipmentItems(_equipmentConfigsProvider.GetEngines());
    }

    private void OnShieldsSelected()
    {
        ShowEquipmentItems(_equipmentConfigsProvider.GetShields());
    }

    private void ShowEquipmentItems(EquipmentConfigBase[] itemConfigs)
    {
        ClearScrollItems();

        _localEventsAggregator.EquipmentChosen(itemConfigs[0]);

        foreach (var itemConfig in itemConfigs)
        {
            var view = Instantiate(_scrollItemPrefab, _scrollContent);
            var component = view.GetComponent<BuyEquipmentScrollItemView>();
            SetupEquipmentScrollItem(component, itemConfig);
        }
    }

    private void SetupEquipmentScrollItem(BuyEquipmentScrollItemView view, EquipmentConfigBase equipment)
    {
        view.SetImage(equipment.IconSprite);
        view.SetCaptionText(_localizationProvider.GetLocalization(GetLocalizationIdByEquipmentType(equipment.EquipmentType), equipment.NameId));
        view.SetCostText(equipment.Cost + "$");

        void clickHandler()
        {
            _localEventsAggregator.EquipmentChosen(equipment);
        }
        view.Clicked += clickHandler;

        void dispose()
        {
            view.Clicked -= clickHandler;
            Destroy(view.gameObject);
        }

        _removeItemFuncs.Add(dispose);
    }

    private LocalizationGroupId GetLocalizationIdByEquipmentType(EquipmentType equipmentType)
    {
        switch (equipmentType)
        {
            case EquipmentType.Weapon:
                return LocalizationGroupId.WeaponNames;
            case EquipmentType.Engine:
                return LocalizationGroupId.EngineNames;
            case EquipmentType.Shield:
                return LocalizationGroupId.ShieldNames;
            default:
                return LocalizationGroupId.Common;
        }
    }

    private void ClearScrollItems()
    {
        _removeItemFuncs.ForEach(f => f());
        _removeItemFuncs.Clear();
    }
}
