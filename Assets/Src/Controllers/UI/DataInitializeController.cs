using Zenject;

public class DataInitializeController : IInitializable
{
    private MenuEventsAggregator _eventsAggregator;
    private PlayerDataModel _playerDataModel;

    [Inject]
    public void Construct(
        MenuEventsAggregator eventsAggregator,
        PlayerDataModel playerDataModel)
    {
        _eventsAggregator = eventsAggregator;
        _playerDataModel = playerDataModel;
    }

    public void Initialize()
    {
        _playerDataModel.Load();
    }
}
