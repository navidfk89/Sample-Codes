using Game.Achievements.Controllers.Base;
using Game.Achievements.Triggers;
using Game.Gamplay.Profile;

namespace Game.Achievements.Controllers;

internal class ShrugItOffAchievementController : AbstractAchievementController, IEndCombatTrigger, IAchievementTrigger
{
	private int _hp;

	private IGameplayProfileManager _gameplayProfileManager;

	public void ProcessEndCombat(bool isWin)
	{
		if (isWin && _gameplayProfileManager.Profile.Health == _hp)
		{
			_achievementManager.CompleteAchievement(this);
		}
	}

	protected override void ResolveDependencies()
	{
		base.ResolveDependencies();
		_gameplayProfileManager = ServiceLocator.Resolve<IGameplayProfileManager>();
	}

	protected override void FetchStaticParameter()
	{
		base.FetchStaticParameter();
		_hp = _data.ParameterEffect.GetParameterValue(ParameterType.hp);
	}
}
