using Framework;
using Game.Achievements.Models;

namespace Game.Achievements.Collections;

public interface IAchievementRepository : IServicable
{
	IAchievementData GetAchievement(AchievementType type);
}
