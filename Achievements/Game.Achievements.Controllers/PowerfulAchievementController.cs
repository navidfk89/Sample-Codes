using System.Linq;
using Game.Achievements.Controllers.Base;
using Game.Achievements.Triggers;
using Game.Characters;
using Game.Powers.Models;

namespace Game.Achievements.Controllers;

internal class PowerfulAchievementController : AbstractAchievementController, IApplyPowerTrigger, IAchievementTrigger
{
	private CategoryType _categoryType;

	private int _powerCountThreshold;

	private ICharacterBehaviour _hero;

	public void ProcessApplyPower(IPowerInGameData powerInGameData)
	{
		int buffPowerCount = _hero.GetPowers().Count((IPowerInGameData x) => x.StaticData.Category == _categoryType);
		if (buffPowerCount >= _powerCountThreshold)
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
		_powerCountThreshold = _data.ParameterEffect.GetParameterValue(ParameterType.power);
		_categoryType = _data.ParameterEffect.GetParameterValue(ParameterType.powerCategory, CategoryType.none);
	}
}
