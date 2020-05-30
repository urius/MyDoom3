using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EquipmentItemDetailsMediator : MonoBehaviour
{
    [SerializeField]
    private EquipmentShopScreenEventsAggregator _localEventsAggregator;
    [SerializeField]
    private Transform _itemHolder;
    [SerializeField]
    private Text _itemNameText;
    [SerializeField]
    private Text _infoText;
    [SerializeField]
    private Button _buyButton;

    private GameObject _currentItemView;
    private EquipmentBase _currentItemData;

    private LocalizationProvider _localizationProvider;
    private MenuEventsAggregator _menuEventsAggregator;

    [Inject]
    private void Construct(
        LocalizationProvider localizationProvider,
        MenuEventsAggregator menuEventsAggregator)
    {
        _localizationProvider = localizationProvider;
        _menuEventsAggregator = menuEventsAggregator;
    }

    private void OnEnable()
    {
        _localEventsAggregator.ShieldChosen += OnShieldChosen;
        _buyButton.onClick.AddListener(OnBuyClick);
    }

    private void OnBuyClick()
    {
        if (_currentItemData != null)
        {
            _menuEventsAggregator.BuyEquipmentClicked(_currentItemData);
        }
    }

    private void OnDisable()
    {
        _localEventsAggregator.ShieldChosen -= OnShieldChosen;
        _buyButton.onClick.RemoveAllListeners();
    }

    private void OnShieldChosen(ShieldConfig config)
    {
        _currentItemData = config;

        if (_currentItemView != null)
        {
            Destroy(_currentItemView);
            _currentItemView = null;
        }

        _currentItemView = Instantiate(config.Prefab, _itemHolder);

        SetupShieldInfo(config);
    }

    private void SetupShieldInfo(ShieldConfig config)
    {
        var name = _localizationProvider.GetLocalization(LocalizationGroupId.ShieldNames, config.NameId);
        var capacityName = _localizationProvider.GetLocalization(LocalizationGroupId.Common, "capacity");
        var chargingSpeedName = _localizationProvider.GetLocalization(LocalizationGroupId.Common, "charging_speed");
        var costName = _localizationProvider.GetLocalization(LocalizationGroupId.Common, "cost");

        _itemNameText.text = name;
        _infoText.text = $"\n{capacityName}: {config.Capacity}" +
                         $"\n{chargingSpeedName}: {config.ChargingSpeed}" +
                         $"\n" +
                         $"\n{costName}: {config.Cost}$";
    }
}
