using System;
using UnityEngine.SceneManagement;
using Zenject;

public class SelectLevelScreenController : IInitializable, IDisposable
{
    private MenuEventsAggregator _menuEventsAggregator;
    private LevelsConfigProvider _levelsConfigProvider;
    private CurrentLevelConfigHolder _currentLevelConfigHolder;

    [Inject]
    public void Construct(
        MenuEventsAggregator menuEventsAggregator,
        LevelsConfigProvider levelsConfigProvider,
        CurrentLevelConfigHolder currentLevelConfigHolder)
    {
        _menuEventsAggregator = menuEventsAggregator;
        _levelsConfigProvider = levelsConfigProvider;
        _currentLevelConfigHolder = currentLevelConfigHolder;
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
        _currentLevelConfigHolder.CurrentLevelConfig = _levelsConfigProvider.LevelConfigs[levelIndex];

        SceneManager.LoadScene("GameScene");
    }
}
