using Game.Characters.Views;
using Game.Environment;
using Game.NPC;

namespace Game.Gamplay.Event.Views;

public class NeowEventView : EventView
{
	private ICharacterObjectCreator _characterObjectCreator;

	private IEnvironmentManager _environmentManager;

	public override void Open()
	{
		base.Open();
		_characterObjectCreator.ShowHero();
		_environmentManager.GameplayEnvironment.ShowNpc(NpcType.Neow);
	}

	public override void Close()
	{
		base.Close();
		_characterObjectCreator.HideHero();
		_environmentManager.GameplayEnvironment.HideNpc(NpcType.Neow);
	}

	protected override void ResolveDependencies()
	{
		base.ResolveDependencies();
		_characterObjectCreator = ServiceLocator.Resolve<ICharacterObjectCreator>();
		_environmentManager = ServiceLocator.Resolve<IEnvironmentManager>();
	}
}
