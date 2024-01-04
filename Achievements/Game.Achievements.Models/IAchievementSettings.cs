namespace Game.Achievements.Models;

public interface IAchievementSettings
{
	IAchievementData GetAchievement(AchievementType type);
}
