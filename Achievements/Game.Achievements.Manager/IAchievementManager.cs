using Framework;
using Game.Achievements.Controllers.Base;

namespace Game.Achievements.Manager;

public interface IAchievementManager : IServicable
{
	void CompleteAchievement(IAchievementController controller);
}
