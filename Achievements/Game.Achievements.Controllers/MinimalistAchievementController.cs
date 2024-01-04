using Game.Achievements.Controllers.Base;
using Game.Achievements.Triggers;
using Game.Gamplay;

namespace Game.Achievements.Controllers;

internal class MinimalistAchievementController : AbstractAchievementController, IRunCompleteTrigger, IAchievementTrigger
{
	private int _cardCountThreshold;

	private IGameplayCardInventory _gameplayCardInventory;

	public void ProcessRunComplete(EndGameData endGameData)
	{
		if (endGameData.IsGameWin && _cardCountThreshold >= _gameplayCardInventory.CardsCount)
		{
			_achievementManager.CompleteAchievement(this);
		}
	}

	protected override void ResolveDependencies()
	{
		base.ResolveDependencies();
		_gameplayCardInventory = ServiceLocator.Resolve<IGameplayCardInventory>();
	}

	protected override void FetchStaticParameter()
	{
		base.FetchStaticParameter();
		_cardCountThreshold = _data.ParameterEffect.GetParameterValue(ParameterType.card);
	}
}
