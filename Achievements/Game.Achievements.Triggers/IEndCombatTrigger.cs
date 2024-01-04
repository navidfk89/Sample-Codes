namespace Game.Achievements.Triggers;

public interface IEndCombatTrigger : IAchievementTrigger
{
	void ProcessEndCombat(bool isWin);
}
