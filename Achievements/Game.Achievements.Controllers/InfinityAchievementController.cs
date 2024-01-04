using Game.Achievements.Controllers.Base;
using Game.Achievements.Triggers;
using Game.Cards.Controllers.Behaviours;

namespace Game.Achievements.Controllers;

internal class InfinityAchievementController : AbstractAchievementController, IStartTurnTrigger, IAchievementTrigger, IPlayCardTrigger
{
	private int _cardsCountThreshold;

	private int _cardsPlayed;

	public void ProcessStarTurn()
	{
		_cardsPlayed = 0;
	}

	public void ProcessPlayCard(ICardBehaviour cardBehaviour)
	{
		_cardsPlayed++;
		if (_cardsPlayed >= _cardsCountThreshold)
		{
			_achievementManager.CompleteAchievement(this);
		}
	}

	protected override void FetchStaticParameter()
	{
		base.FetchStaticParameter();
		_cardsCountThreshold = _data.ParameterEffect.GetParameterValue(ParameterType.card);
	}
}
