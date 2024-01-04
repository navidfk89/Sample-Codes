using Game.Actions.Models;

namespace Game.Achievements.Triggers;

public interface ILoseHpTrigger : IAchievementTrigger
{
	void ProcessLoseHpTrigger(ChangeHPInfo data);
}
