using Game.Characters.Enemies.Models;

namespace Game.Achievements.Triggers;

public interface IEnemyDieTrigger : IAchievementTrigger
{
	void ProcessEnemyDie(IEnemyData enemyData);
}
