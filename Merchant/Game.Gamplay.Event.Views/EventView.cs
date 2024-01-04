namespace Game.Gamplay.Event.Views;

public class EventView : IEventView
{
	public virtual void Open()
	{
		ResolveDependencies();
	}

	public virtual void Close()
	{
	}

	protected virtual void ResolveDependencies()
	{
	}
}
