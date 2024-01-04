using Game.Achievements.Controllers.Base;
using Game.Achievements.Triggers;
using Game.Cards;
using Game.Cards.Controllers.Behaviours;

namespace Game.Achievements.Controllers;

internal class ComeAtMeAchievementController : AbstractAchievementController, IStartCombatTrigger, IAchievementTrigger, IEndCombatTrigger, IPlayCardTrigger
{
	private CardCategory _cardCategory;

	private bool _isAttackCardPlayed;

	public void ProcessStartCombat()
	{
		_isAttackCardPlayed = false;
	}

	public void ProcessEndCombat(bool isWin)
	{
		if (isWin && !_isAttackCardPlayed)
		{
			_achievementManager.CompleteAchievement(this);
		}
	}

	public void ProcessPlayCard(ICardBehaviour cardBehaviour)
	{
		if (cardBehaviour.Data.StaticData.Category == _cardCategory)
		{
			_isAttackCardPlayed = true;
		}
	}

	protected override void FetchStaticParameter()
	{
		base.FetchStaticParameter();
		_cardCategory = _data.ParameterEffect.GetParameterValue(ParameterType.cardCategory, CardCategory.none);
	}
}
