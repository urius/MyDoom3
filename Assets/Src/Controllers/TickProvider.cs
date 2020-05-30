using Zenject;

public class TickProvider : ITickable
{
    private EventsAggregator _eventsAggregator;

    public TickProvider(EventsAggregator eventsAggregator)
    {
        _eventsAggregator = eventsAggregator;
    }

    public void Tick()
    {
        _eventsAggregator.Ticked();
    }
}
