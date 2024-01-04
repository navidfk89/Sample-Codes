using Game.Achievements.Controllers.Base;
using Game.Achievements.Triggers;
using Game.Gamplay.Combat;

namespace Game.Achievements.Controllers;

internal class PurityAchievementController : AbstractAchievementController, IDrawPileChangeTrigger, IAchievementTrigger, IDiscardPileChangeTrigger, IHandChangeTrigger
{
	private int _thresholdCards;

	private ICardManager _cardManager;

	public void ProcessDrawPileChange(int totalCardInDrawPile)
	{
		CheckCardsCount();
	}

	public void ProcessDiscardPileChange(int totalCardsInDiscardPile)
	{
		CheckCardsCount();
	}

	public void ProcessHandChange(int totalCardsInHand)
	{
		CheckCardsCount();
	}

	private void CheckCardsCount()
	{
		int totalCards = _cardManager.TotalCardInDiscardList + _cardManager.TotalCardInHandList + _cardManager.TotalCardInDrawPileList;
		if (totalCards <= _thresholdCards)
		{
			_achievementManager.CompleteAchievement(this);
		}
	}

	protected override void ResolveCombatDependencies()
	{
		base.ResolveCombatDependencies();
		_cardManager = ServiceLocator.Resolve<ICardManager>();
	}

	protected override void FetchStaticParameter()
	{
		base.FetchStaticParameter();
		_thresholdCards = _data.ParameterEffect.GetParameterValue(ParameterType.card);
	}
}
