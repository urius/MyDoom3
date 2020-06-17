using System;
using UnityEngine.SceneManagement;
using Zenject;

public class SelectLevelScreenController : IInitializable, IDisposable
{
    private MenuEventsAggregator _menuEventsAggregator;

    [Inject]
    public void Construct(
        MenuEventsAggregator menuEventsAggregator)
    {
        _menuEventsAggregator = menuEventsAggregator;
    }

    public void Initialize()
    {
        _menuEventsAggregator.OnStartLevelClicked += OnStartLevelClicked;
    }

    public void Dispose()
    {
        _menuEventsAggregator.OnStartLevelClicked -= OnStartLevelClicked;
    }

    private void OnStartLevelClicked(int levelIndex)
    {
        SceneManager.LoadScene("GameScene");
    }
}
