using System.Linq;
using Game.Achievements.Controllers.Base;
using Game.Achievements.Triggers;
using Game.Cards.Models;
using Game.Gamplay;

namespace Game.Achievements.Controllers;

internal class CommonSenseAchievementController : AbstractAchievementController, IRunCompleteTrigger, IAchievementTrigger
{
	private IGameplayCardInventory _gameplayCardInventory;

	public void ProcessRunComplete(EndGameData endGameData)
	{
		if (!_gameplayCardInventory.Cards.Any((ICardInDeckData x) => x.StaticData.Rarity == RarityType.uncommon || x.StaticData.Rarity == RarityType.rare))
		{
			_achievementManager.CompleteAchievement(this);
		}
	}

	protected override void ResolveDependencies()
	{
		base.ResolveDependencies();
		_gameplayCardInventory = ServiceLocator.Resolve<IGameplayCardInventory>();
	}
}
