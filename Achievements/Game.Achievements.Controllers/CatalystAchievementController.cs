using Game.Achievements.Controllers.Base;
using Game.Achievements.Triggers;
using Game.Powers;
using Game.Powers.Models;

namespace Game.Achievements.Controllers;

internal class CatalystAchievementController : AbstractAchievementController, IApplyPowerTrigger, IAchievementTrigger
{
	private PowerType _powerType;

	private int _powerThresholdCount;

	public void ProcessApplyPower(IPowerInGameData powerInGameData)
	{
		if (!powerInGameData.Owner.Data.IsHero && powerInGameData.Owner.HasPower(_powerType))
		{
			IPowerInGameData powerData = powerInGameData.Owner.GetPowerData(_powerType);
			if (powerData.Value >= _powerThresholdCount)
			{
				_achievementManager.CompleteAchievement(this);
			}
		}
	}

	protected override void FetchStaticParameter()
	{
		base.FetchStaticParameter();
		_powerType = _data.ParameterEffect.GetParameterValue(ParameterType.power, PowerType.none);
		_powerThresholdCount = _data.ParameterEffect.GetParameterValue(ParameterType.count);
	}
}
