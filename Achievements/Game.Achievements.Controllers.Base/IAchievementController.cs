using Game.Achievements.Models;

namespace Game.Achievements.Controllers.Base;

public interface IAchievementController
{
	IAchievementData Data { get; }
}
