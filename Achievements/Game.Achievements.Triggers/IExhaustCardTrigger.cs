using Game.Cards.Controllers.Behaviours;

namespace Game.Achievements.Triggers;

public interface IExhaustCardTrigger : IAchievementTrigger
{
	void ProcessExhaustCard(ICardBehaviour cardBehaviour);
}
