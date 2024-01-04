using Game.Achievements.Controllers.Base;
using Game.Achievements.Triggers;
using Game.Gamplay;

namespace Game.Achievements.Controllers;

internal class WhoNeedsRelicsAchievementController : AbstractAchievementController, IRunCompleteTrigger, IAchievementTrigger
{
	private int _relicThresholdCount;

	private IGameplayRelicInventory _gameplayRelicInventory;

	public void ProcessRunComplete(EndGameData endGameData)
	{
		if (endGameData.IsGameWin && _relicThresholdCount >= _gameplayRelicInventory.RelicsCount)
		{
			_achievementManager.CompleteAchievement(this);
		}
	}

	protected override void ResolveDependencies()
	{
		base.ResolveDependencies();
		_gameplayRelicInventory = ServiceLocator.Resolve<IGameplayRelicInventory>();
	}

	protected override void FetchStaticParameter()
	{
		base.FetchStaticParameter();
		_relicThresholdCount = _data.ParameterEffect.GetParameterValue(ParameterType.count);
	}
}
