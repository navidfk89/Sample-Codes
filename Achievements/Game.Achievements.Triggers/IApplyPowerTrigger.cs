using Game.Powers.Models;

namespace Game.Achievements.Triggers;

public interface IApplyPowerTrigger : IAchievementTrigger
{
	void ProcessApplyPower(IPowerInGameData powerInGameData);
}
