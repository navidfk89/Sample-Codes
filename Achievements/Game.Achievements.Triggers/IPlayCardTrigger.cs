using Game.Cards.Controllers.Behaviours;

namespace Game.Achievements.Triggers;

public interface IPlayCardTrigger : IAchievementTrigger
{
	void ProcessPlayCard(ICardBehaviour cardBehaviour);
}
