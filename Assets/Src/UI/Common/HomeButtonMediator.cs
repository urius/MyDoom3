using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HomeButtonMediator : MonoBehaviour
{
    [SerializeField]
    private GameObject _screenRoot;
    [SerializeField]
    private Button _button;

    private MenuEventsAggregator _menuEventsAggregator;

    [Inject]
    public void Construct(MenuEventsAggregator menuEventsAggregator)
    {
        _menuEventsAggregator = menuEventsAggregator;
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClicked);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }

    private void OnClicked()
    {
        _menuEventsAggregator.HomeClicked(_screenRoot);
    }
}
