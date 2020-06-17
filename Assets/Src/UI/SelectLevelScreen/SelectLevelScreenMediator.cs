using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SelectLevelScreenMediator : MonoBehaviour
{
    [SerializeField] private SelectLevelScrollView _selectLevelScrollView;
    [SerializeField] private Button _startButton;

    private LevelsConfigProvider _levelsConfigProvider;
    private PlayerDataModel _playerDataModel;
    private MenuEventsAggregator _eventsAggregator;
    private int _currentLevelIndex;

    [Inject]
    public void Construct(
        LevelsConfigProvider levelsConfigProvider,
        PlayerDataModel playerDataModel,
        MenuEventsAggregator eventsAggregator)
    {
        _levelsConfigProvider = levelsConfigProvider;
        _playerDataModel = playerDataModel;
        _eventsAggregator = eventsAggregator;
    }

    private void OnEnable()
    {
        _startButton.onClick.AddListener(OnStartClick);
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveAllListeners();
    }

    private void OnStartClick()
    {
        _eventsAggregator.OnStartLevelClicked(_currentLevelIndex);
    }

    void Start()
    {
        var totalLevelsCount = _levelsConfigProvider.LevelConfigs.Length;
        _selectLevelScrollView.CreateItems(totalLevelsCount);

        SetupItems(totalLevelsCount);

        SetCurrent(GetCurrentLevelIndex());
    }

    private int GetCurrentLevelIndex()
    {
        var firstUncompletedLevelIndex = -1;
        for (var i = 0; i < _playerDataModel.Levels.Count; i++)
        {
            if (_playerDataModel.Levels[i].IsCompleted == false)
            {
                firstUncompletedLevelIndex = i;
                break;
            }
        }
        return firstUncompletedLevelIndex >= 0 ? firstUncompletedLevelIndex : _levelsConfigProvider.LevelConfigs.Length - 1;
    }

    private void SetCurrent(int index)
    {
        var totalLevelsCount = _levelsConfigProvider.LevelConfigs.Length;
        for (var i = 0; i < totalLevelsCount; i++)
        {
            var item = _selectLevelScrollView.GetItem(i);
            item.SetCurrent(i == index);
        }
        _currentLevelIndex = index;
    }

    private void OnItemClicked(int index)
    {
        if (index < _playerDataModel.Levels.Count && !_playerDataModel.Levels[index].IsLocked)
        {
            SetCurrent(index);
        }
    }

    private void SetupItems(int levelsCount)
    {
        for (var i = 0; i < levelsCount; i++)
        {
            var item = _selectLevelScrollView.GetItem(i);
            var curIndex = i;
            item.Clicked += () => OnItemClicked(curIndex);

            item.SetLevelNum(i + 1);
            if (i < _playerDataModel.Levels.Count)
            {
                SetupItem(item, _playerDataModel.Levels[i]);
            }
            else
            {
                item.SetCompleted(false);
                item.SetLocked(true);
            }
        }

        void SetupItem(SelectLevelScreenScrollItemInnerView item, LevelData levelData)
        {
            item.SetCompleted(levelData.IsCompleted);
            item.SetLocked(levelData.IsLocked);
        }
    }
}
