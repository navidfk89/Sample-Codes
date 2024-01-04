using Game.Achievements.Controllers.Base;
using Game.Achievements.Triggers;
using Game.Orbs;

namespace Game.Achievements.Controllers;

internal class NeonAchievementController : AbstractAchievementController, IChannelOrbTrigger, IAchievementTrigger, IStartTurnTrigger
{
	private int _channelOrbCountThreshold;

	private OrbType _orbType;

	private int _channeledOrbCount;

	public void ProcessChannelOrb(OrbType orbType)
	{
		if (orbType == _orbType)
		{
			_channeledOrbCount++;
			if (_channeledOrbCount >= _channelOrbCountThreshold)
			{
				_achievementManager.CompleteAchievement(this);
			}
		}
	}

	public void ProcessStarTurn()
	{
		_channeledOrbCount = 0;
	}

	protected override void FetchStaticParameter()
	{
		base.FetchStaticParameter();
		_channelOrbCountThreshold = _data.ParameterEffect.GetParameterValue(ParameterType.count);
		_orbType = _data.ParameterEffect.GetParameterValue(ParameterType.orbType, OrbType.Empty);
	}
}
