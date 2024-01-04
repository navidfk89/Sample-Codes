using Game.Achievements.Controllers.Base;
using Game.Achievements.Triggers;
using Game.Characters.Enemies;
using Game.Characters.Enemies.Models;

namespace Game.Achievements.Controllers;

internal class TheGuardianAchievementController : AbstractAchievementController, IEnemyDieTrigger, IAchievementTrigger
{
	private EnemyType _enemyType;

	public void ProcessEnemyDie(IEnemyData enemyData)
	{
		if (enemyData.Type == _enemyType)
		{
			_achievementManager.CompleteAchievement(this);
		}
	}

	protected override void FetchStaticParameter()
	{
		base.FetchStaticParameter();
		_enemyType = _data.ParameterEffect.GetParameterValue(ParameterType.enemyType, EnemyType.None);
	}
}
