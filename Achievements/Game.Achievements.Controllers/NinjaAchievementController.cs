using Game.Achievements.Controllers.Base;
using Game.Achievements.Triggers;
using Game.Cards;
using Game.Cards.Controllers.Behaviours;

namespace Game.Achievements.Controllers;

internal class NinjaAchievementController : AbstractAchievementController, IStartTurnTrigger, IAchievementTrigger, IPlayCardTrigger
{
	private int _playCardThreshold;

	private CardType _cardType;

	private int _playedCards;

	public void ProcessStarTurn()
	{
		_playedCards = 0;
	}

	public void ProcessPlayCard(ICardBehaviour cardBehaviour)
	{
		if (cardBehaviour.Data.StaticData.Type == _cardType)
		{
			_playedCards++;
			if (_playedCards >= _playCardThreshold)
			{
				_achievementManager.CompleteAchievement(this);
			}
		}
	}

	protected override void FetchStaticParameter()
	{
		base.FetchStaticParameter();
		_playCardThreshold = _data.ParameterEffect.GetParameterValue(ParameterType.card);
		_cardType = _data.ParameterEffect.GetParameterValue(ParameterType.cardType, CardType.none);
	}
}
