using Game.Achievements.Controllers.Base;
using Game.Achievements.Triggers;
using Game.Acts;
using Game.Acts.Controllers;
using Game.Gamplay;
using Game.Gamplay.Profile;

namespace Game.Achievements.Controllers;

internal class AmethystPlusAchievementController : AbstractAchievementController, IRunCompleteTrigger, IAchievementTrigger
{
	private ClassType _heroType;

	private ActType _actType;

	private IGameplayProfileManager _gameplayProfileManager;

	private IActManager _actManager;

	public void ProcessRunComplete(EndGameData endGameData)
	{
		if (endGameData.IsGameWin && _gameplayProfileManager.Profile.Character == _heroType && _actManager.CurrentAct.StaticData.Type == _actType)
		{
			_achievementManager.CompleteAchievement(this);
		}
	}

	protected override void ResolveDependencies()
	{
		base.ResolveDependencies();
		_gameplayProfileManager = ServiceLocator.Resolve<IGameplayProfileManager>();
		_actManager = ServiceLocator.Resolve<IActManager>();
	}

	protected override void FetchStaticParameter()
	{
		base.FetchStaticParameter();
		_heroType = _data.ParameterEffect.GetParameterValue(ParameterType.classType, ClassType.any);
		_actType = _data.ParameterEffect.GetParameterValue(ParameterType.actNumber, ActType.none);
	}
}
