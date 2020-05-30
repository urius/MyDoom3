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

    private ShieldsConfigProvider _shieldsConfigProvider;
    private LocalizationProvider _localizationProvider;

    [Inject]
    private void Construct(
        LocalizationProvider localizationProvider,
        ShieldsConfigProvider shieldsConfigProvider)
    {
        _shieldsConfigProvider = shieldsConfigProvider;
        _localizationProvider = localizationProvider;
    }

    private void OnEnable()
    {
        _shieldsTab.Selected += OnShieldsSelected;
    }

    private void OnDisable()
    {
        _shieldsTab.Selected -= OnShieldsSelected;
    }

    private void OnShieldsSelected()
    {
        ClearScrollItems();

        foreach (var shieldConfig in _shieldsConfigProvider.Configs)
        {
            var view = Instantiate(_scrollItemPrefab, _scrollContent);
            var component = view.GetComponent<BuyEquipmentScrollItemView>();
            SetupShieldScrollItem(component, shieldConfig);
        }
    }

    private void SetupShieldScrollItem(BuyEquipmentScrollItemView view, ShieldConfig shieldConfig)
    {
        view.SetImage(shieldConfig.IconSprite);
        view.SetCaptionText(_localizationProvider.GetLocalization(LocalizationGroupId.ShieldNames, shieldConfig.NameId));
        view.SetCostText(shieldConfig.Cost + "$");

        void clickHandler()
        {
            _localEventsAggregator.ShieldChosen(shieldConfig);
        }
        view.Clicked += clickHandler;

        void dispose()
        {
            view.Clicked -= clickHandler;
            Destroy(view.gameObject);
        }

        _removeItemFuncs.Add(dispose);
    }

    private void ClearScrollItems()
    {
        _removeItemFuncs.ForEach(f => f());
        _removeItemFuncs.Clear();
    }
}
