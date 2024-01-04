using Game.Achievements.Controllers.Base;
using Game.Achievements.Triggers;
using Game.Actions.Models;
using Game.Gamplay.Profile;

namespace Game.Achievements.Controllers;

internal class PerfectAchievementController : AbstractAchievementController, IStartCombatTrigger, IAchievementTrigger, ILoseHpTrigger, IEndCombatTrigger
{
	private bool _tookDamage;

	private IGameplayProfileManager _gameplayProfileManager;

	public void ProcessStartCombat()
	{
		_tookDamage = false;
	}

	public void ProcessLoseHpTrigger(ChangeHPInfo data)
	{
		if (data.Amount != 0 && data.Target.Data.IsHero)
		{
			_tookDamage = true;
		}
	}

	public void ProcessEndCombat(bool isWin)
	{
		if (isWin && !_tookDamage)
		{
			_achievementManager.CompleteAchievement(this);
		}
	}

	protected override void ResolveDependencies()
	{
		base.ResolveDependencies();
		_gameplayProfileManager = ServiceLocator.Resolve<IGameplayProfileManager>();
	}
}
