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
    private EquipmentConfigBase _currentItemData;

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
        _localEventsAggregator.EquipmentChosen += OnEquipmentChosen;
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
        _localEventsAggregator.EquipmentChosen -= OnEquipmentChosen;
        _buyButton.onClick.RemoveAllListeners();
    }

    private void OnEquipmentChosen(EquipmentConfigBase config)
    {
        _currentItemData = config;

        if (_currentItemView != null)
        {
            Destroy(_currentItemView);
            _currentItemView = null;
        }

        _currentItemView = Instantiate(config.Prefab, _itemHolder);

        switch (config.EquipmentType)
        {
            case EquipmentType.Shield:
                SetupShieldInfo((ShieldConfig)config);
                break;
            case EquipmentType.Weapon:
                SetupWeaponInfo((WeaponConfig)config);
                break;
            case EquipmentType.Engine:
                SetupEngineInfo((EngineConfig)config);
                break;
        }
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

    private void SetupWeaponInfo(WeaponConfig config)
    {
        var name = _localizationProvider.GetLocalization(LocalizationGroupId.WeaponNames, config.NameId);
        var damageName = _localizationProvider.GetLocalization(LocalizationGroupId.Common, "damage");
        var cooldownName = _localizationProvider.GetLocalization(LocalizationGroupId.Common, "weapon_cooldown");
        var costName = _localizationProvider.GetLocalization(LocalizationGroupId.Common, "cost");

        _itemNameText.text = name;
        _infoText.text = $"\n{damageName}: {config.Damage}" +
                         $"\n{cooldownName}: {config.CooldownFrames}" +
                         $"\n" +
                         $"\n{costName}: {config.Cost}$";
    }

    private void SetupEngineInfo(EngineConfig config)
    {
        var name = _localizationProvider.GetLocalization(LocalizationGroupId.EngineNames, config.NameId);
        var speedName = _localizationProvider.GetLocalization(LocalizationGroupId.Common, "speed");
        var mobilityName = _localizationProvider.GetLocalization(LocalizationGroupId.Common, "mobility");
        var costName = _localizationProvider.GetLocalization(LocalizationGroupId.Common, "cost");

        _itemNameText.text = name;
        _infoText.text = $"\n{speedName}: {config.Speed}" +
                         $"\n{mobilityName}: {config.Mobility}" +
                         $"\n" +
                         $"\n{costName}: {config.Cost}$";
    }
}
