using Game.Achievements.Controllers.Base;
using Game.Achievements.Triggers;
using Game.Cards;
using Game.Cards.Controllers.Behaviours;
using Game.Characters.Enemies;
using Game.Characters.Enemies.Models;

namespace Game.Achievements.Controllers;

internal class OohDonutAchievementController : AbstractAchievementController, IEndTurnTrigger, IAchievementTrigger, IPlayCardTrigger, IEnemyDieTrigger
{
	private CardType _cardType;

	private EnemyType _enemyType;

	private bool _feedCardPlayed;

	public void ProcessEndTurn()
	{
		_feedCardPlayed = false;
	}

	public void ProcessPlayCard(ICardBehaviour cardBehaviour)
	{
		if (cardBehaviour.Data.StaticData.Type != _cardType)
		{
			_feedCardPlayed = false;
		}
		else
		{
			_feedCardPlayed = true;
		}
	}

	public void ProcessEnemyDie(IEnemyData enemyData)
	{
		if (_feedCardPlayed && enemyData.Type == _enemyType)
		{
			_achievementManager.CompleteAchievement(this);
		}
	}

	protected override void FetchStaticParameter()
	{
		base.FetchStaticParameter();
		_cardType = _data.ParameterEffect.GetParameterValue(ParameterType.cardType, CardType.none);
		_enemyType = _data.ParameterEffect.GetParameterValue(ParameterType.enemyType, EnemyType.None);
	}
}
