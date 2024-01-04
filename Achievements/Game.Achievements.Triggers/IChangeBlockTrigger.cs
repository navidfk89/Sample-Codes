using Game.Actions.Models;

namespace Game.Achievements.Triggers;

public interface IChangeBlockTrigger : IAchievementTrigger
{
	void ProcessChangeBlock(BlockInfo blockInfo);
}
