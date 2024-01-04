using Game.Achievements.Models;

namespace Game.Achievements.Triggers;

public interface IGetAchievementTrigger : IAchievementTrigger
{
	void ProcessGetAchievement(IAchievementData achievementData);
}
