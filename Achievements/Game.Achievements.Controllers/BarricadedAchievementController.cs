using Game.Achievements.Controllers.Base;
using Game.Achievements.Triggers;
using Game.Actions.Models;
using Game.Characters;

namespace Game.Achievements.Controllers;

internal class BarricadedAchievementController : AbstractAchievementController, IChangeBlockTrigger, IAchievementTrigger
{
	private int _blockThreshold;

	private ICharacterBehaviour _hero;

	public void ProcessChangeBlock(BlockInfo blockInfo)
	{
		int block = _hero.Data.Block;
		if (block >= _blockThreshold)
		{
			_achievementManager.CompleteAchievement(this);
		}
	}

	protected override void ResolveCombatDependencies()
	{
		base.ResolveCombatDependencies();
		_hero = ServiceLocator.Resolve<ICharacterManager>().GetHero();
	}

	protected override void FetchStaticParameter()
	{
		base.FetchStaticParameter();
		_blockThreshold = _data.ParameterEffect.GetParameterValue(ParameterType.block);
	}
}
