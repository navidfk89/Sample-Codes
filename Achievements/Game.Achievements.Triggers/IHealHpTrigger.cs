using Game.Actions.Models;

namespace Game.Achievements.Triggers;

public interface IHealHpTrigger : IAchievementTrigger
{
	void ProcessHealHpTrigger(ChangeHPInfo data);
}
