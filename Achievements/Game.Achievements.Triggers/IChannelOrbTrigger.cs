using Game.Orbs;

namespace Game.Achievements.Triggers;

public interface IChannelOrbTrigger : IAchievementTrigger
{
	void ProcessChannelOrb(OrbType orbType);
}
