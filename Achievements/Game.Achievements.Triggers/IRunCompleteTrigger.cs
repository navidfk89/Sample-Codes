using Game.Gamplay;

namespace Game.Achievements.Triggers;

public interface IRunCompleteTrigger : IAchievementTrigger
{
	void ProcessRunComplete(EndGameData endGameData);
}
