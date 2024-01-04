using Game.Achievements.Controllers.Base;
using Game.Achievements.Triggers;

namespace Game.Achievements.Controllers;

internal class AdrenalineAchievementController : AbstractAchievementController, IChangeEnergyTrigger, IAchievementTrigger
{
	private int _energyThreshold;

	public void ProcessChangeEnergy(int currentEnergy, int capacity)
	{
		if (currentEnergy >= _energyThreshold)
		{
			_achievementManager.CompleteAchievement(this);
		}
	}

	protected override void FetchStaticParameter()
	{
		base.FetchStaticParameter();
		_energyThreshold = _data.ParameterEffect.GetParameterValue(ParameterType.energy);
	}
}
