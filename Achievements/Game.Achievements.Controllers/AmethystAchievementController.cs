using Game.Achievements.Controllers.Base;
using Game.Achievements.Triggers;
using Game.Gamplay;
using Game.Gamplay.Profile;

namespace Game.Achievements.Controllers;

internal class AmethystAchievementController : AbstractAchievementController, IRunCompleteTrigger, IAchievementTrigger
{
	private ClassType _heroType;

	private IGameplayProfileManager _gameplayProfileManager;

	public void ProcessRunComplete(EndGameData endGameData)
	{
		if (endGameData.IsGameWin && _gameplayProfileManager.Profile.Character == _heroType)
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
		_heroType = _data.ParameterEffect.GetParameterValue(ParameterType.classType, ClassType.any);
	}
}
