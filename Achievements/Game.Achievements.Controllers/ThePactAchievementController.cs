using Game.Achievements.Controllers.Base;
using Game.Achievements.Triggers;
using Game.Cards.Controllers.Behaviours;

namespace Game.Achievements.Controllers;

internal class ThePactAchievementController : AbstractAchievementController, IStartCombatTrigger, IAchievementTrigger, IEndCombatTrigger, IExhaustCardTrigger
{
	private int _exhaustedCardsThreshold;

	private int _exhaustedCardsCount;

	public void ProcessStartCombat()
	{
		_exhaustedCardsCount = 0;
	}

	public void ProcessEndCombat(bool isWin)
	{
		if (_exhaustedCardsCount >= _exhaustedCardsThreshold)
		{
			_achievementManager.CompleteAchievement(this);
		}
	}

	public void ProcessExhaustCard(ICardBehaviour cardBehaviour)
	{
		_exhaustedCardsCount++;
	}

	protected override void FetchStaticParameter()
	{
		base.FetchStaticParameter();
		_exhaustedCardsThreshold = _data.ParameterEffect.GetParameterValue(ParameterType.card);
	}
}
