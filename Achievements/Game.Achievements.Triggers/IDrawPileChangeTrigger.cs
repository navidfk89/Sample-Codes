namespace Game.Achievements.Triggers;

public interface IDrawPileChangeTrigger : IAchievementTrigger
{
	void ProcessDrawPileChange(int totalCardInDrawPile);
}
