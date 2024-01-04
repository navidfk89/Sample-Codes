namespace Game.Achievements.Triggers;

public interface IDiscardPileChangeTrigger : IAchievementTrigger
{
	void ProcessDiscardPileChange(int totalCardsInDiscardPile);
}
