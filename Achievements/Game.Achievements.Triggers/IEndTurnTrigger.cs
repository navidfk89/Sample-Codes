namespace Game.Achievements.Triggers;

public interface IEndTurnTrigger : IAchievementTrigger
{
	void ProcessEndTurn();
}
