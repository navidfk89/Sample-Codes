using Game.Achievements.Controllers.Base;
using Game.Achievements.Triggers;
using Game.Actions.Models;
using Game.Characters;
using Game.Characters.Enemies;
using Game.Characters.Enemies.Controllers.Behaviours;

namespace Game.Achievements.Controllers;

internal class TheTransientAchievementController : AbstractAchievementController, ILoseHpTrigger, IAchievementTrigger
{
	private EnemyType _enemyType;

	private ICharacterManager _characterManager;

	public void ProcessLoseHpTrigger(ChangeHPInfo data)
	{
		if (!data.Target.Data.IsHero)
		{
			IEnemyBehaviour enemyBehaviour = _characterManager.GetEnemy(data.Target.Data.CharacterId);
			if (!enemyBehaviour.Data.IsAlive && enemyBehaviour.StaticData.Type == _enemyType)
			{
				_achievementManager.CompleteAchievement(this);
			}
		}
	}

	protected override void ResolveCombatDependencies()
	{
		base.ResolveCombatDependencies();
		_characterManager = ServiceLocator.Resolve<ICharacterManager>();
	}

	protected override void FetchStaticParameter()
	{
		base.FetchStaticParameter();
		_enemyType = _data.ParameterEffect.GetParameterValue(ParameterType.enemyType, EnemyType.None);
	}
}
