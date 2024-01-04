using Game.Achievements.Manager;
using Game.Achievements.Models;

namespace Game.Achievements.Controllers.Base;

public class AbstractAchievementController : IAchievementController
{
	protected IAchievementData _data;

	protected IAchievementManager _achievementManager;

	public IAchievementData Data => _data;

	public void Initialize(IAchievementManager achievementManager, IAchievementData data)
	{
		_achievementManager = achievementManager;
		_data = data;
		ResolveDependencies();
		FetchStaticParameter();
	}

	public virtual void PrepareForCombat()
	{
		ResolveCombatDependencies();
	}

	protected virtual void ResolveDependencies()
	{
	}

	protected virtual void ResolveCombatDependencies()
	{
	}

	protected virtual void FetchStaticParameter()
	{
	}
}
