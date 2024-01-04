using Game.Achievements.Controllers.Base;
using Game.Achievements.Triggers;
using Game.Actions.Models;
using Game.Powers;

namespace Game.Achievements.Controllers;

internal class PlagueAchievementController : AbstractAchievementController, IStartCombatTrigger, IAchievementTrigger, ILoseHpTrigger
{
	private int _enemiesDieCountThreshold;

	private PowerType _powerType;

	private int _enemiesDieCount;

	public void ProcessStartCombat()
	{
		_enemiesDieCount = 0;
	}

	public void ProcessLoseHpTrigger(ChangeHPInfo data)
	{
		if (!data.Target.Data.IsHero && !data.Target.Data.IsAlive)
		{
			if (data.Source == _powerType.ToString())
			{
				_enemiesDieCount++;
			}
			if (_enemiesDieCount >= _enemiesDieCountThreshold)
			{
				_achievementManager.CompleteAchievement(this);
			}
		}
	}

	protected override void FetchStaticParameter()
	{
		base.FetchStaticParameter();
		_enemiesDieCountThreshold = _data.ParameterEffect.GetParameterValue(ParameterType.count);
		_powerType = _data.ParameterEffect.GetParameterValue(ParameterType.power, PowerType.none);
	}
}
