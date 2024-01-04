using Game.Achievements.Controllers.Base;
using Game.Achievements.Triggers;
using Game.Characters;
using Game.Powers;
using Game.Powers.Models;

namespace Game.Achievements.Controllers;

internal class JaxxedAchievementController : AbstractAchievementController, IApplyPowerTrigger, IAchievementTrigger
{
	private PowerType _powerType;

	private int _powerThresholdCount;

	private ICharacterBehaviour _hero;

	public void ProcessApplyPower(IPowerInGameData powerInGameData)
	{
		if (powerInGameData.Owner.Data.IsHero && powerInGameData.Owner.HasPower(_powerType))
		{
			IPowerInGameData powerData = _hero.GetPowerData(_powerType);
			if (powerData.Value >= _powerThresholdCount)
			{
				_achievementManager.CompleteAchievement(this);
			}
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
		_powerType = _data.ParameterEffect.GetParameterValue(ParameterType.power, PowerType.none);
		_powerThresholdCount = _data.ParameterEffect.GetParameterValue(ParameterType.count);
	}
}
