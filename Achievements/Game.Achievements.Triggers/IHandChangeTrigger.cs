namespace Game.Achievements.Triggers;

public interface IHandChangeTrigger : IAchievementTrigger
{
	void ProcessHandChange(int totalCardsInHand);
}
