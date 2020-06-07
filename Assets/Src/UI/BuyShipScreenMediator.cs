using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BuyShipScreenMediator : MonoBehaviour
{
    [SerializeField]
    private SwapShipsAnimator _swapShipAnimator;
    [SerializeField]
    private Text _shipNameTxt;
    [SerializeField]
    private Text _pageCounterTxt;
    [SerializeField]
    private Text _shipInfoTxt;
    [SerializeField]
    private Button _buyButton;

    private ShipConfig[] _shipConfigs;
    private LocalizationProvider _localizationProvider;
    private MenuEventsAggregator _eventsAggregator;
    private int _currentShipIndex = 0;

    [Inject]
    public void Construct(
        ShipsConfigProvider shipsConfigProvider,
        LocalizationProvider localizationProvider,
        MenuEventsAggregator eventsAggregator)
    {
        _shipConfigs = shipsConfigProvider.Configs;
        _localizationProvider = localizationProvider;
        _eventsAggregator = eventsAggregator;
    }

    private ShipConfig CurrentShipConfig => _shipConfigs[_currentShipIndex];

    public void OnEnable()
    {
        _swapShipAnimator.SwitchAnimationStarted += OnSwitchAnimationStarted;
        _buyButton.onClick.AddListener(OnBuyShipClicked);
    }

    public void Start()
    {
        _swapShipAnimator.ShowCurrentPrefab(CurrentShipConfig.ShipPrefab);
        UpdateTexts(_currentShipIndex, _shipConfigs);
    }

    public void OnSwitchRight()
    {
        _currentShipIndex = ClampIndex(_currentShipIndex + 1);
        _swapShipAnimator.ShowPrefabFromRight(CurrentShipConfig.ShipPrefab);

    }

    public void OnSwitchLeft()
    {
        _currentShipIndex = ClampIndex(_currentShipIndex - 1);
        _swapShipAnimator.ShowPrefabFromLeft(CurrentShipConfig.ShipPrefab);
    }

    public void OnHomeClicked()
    {
        _eventsAggregator.HomeClicked(this.gameObject);
    }

    private void OnBuyShipClicked()
    {
        _eventsAggregator.BuyShipClicked(CurrentShipConfig);
    }

    private void OnSwitchAnimationStarted(int index)
    {
        UpdateTexts(index, _shipConfigs);
    }

    private int ClampIndex(int index)
    {
        if (index > _shipConfigs.Length - 1)
        {
            return 0;
        }
        else if (index < 0)
        {
            return _shipConfigs.Length - 1;
        }

        return index;
    }

    private void UpdateTexts(int currentShipIndex, ShipConfig[] shipConfigs)
    {
        _pageCounterTxt.text = currentShipIndex + " / " + (shipConfigs.Length - 1);
        var shipConfig = shipConfigs[currentShipIndex];
        _shipNameTxt.text = _localizationProvider.GetLocalization(LocalizationGroupId.ShipNames, shipConfig.NameId);
        _shipInfoTxt.text = $"{GetLocale("armor")}: {shipConfig.HP}" +
                            $"\n{GetLocale("cost")}: {shipConfig.Cost}$";
    }

    private string GetLocale(string id)
    {
        return _localizationProvider.GetLocalization(LocalizationGroupId.BuyShipScreen, id);
    }
}
