using Game.Achievements.Controllers.Base;
using Game.Achievements.Triggers;
using Game.Gamplay;
using Game.Profile.Controllers;

namespace Game.Achievements.Controllers;

internal class SpeedClimberAchievementController : AbstractAchievementController, IRunCompleteTrigger, IAchievementTrigger
{
	private int _timeThreshold;

	private IProfileManager _profileManager;

	public void ProcessRunComplete(EndGameData endGameData)
	{
		if (_profileManager.ActiveRun.Time <= _timeThreshold)
		{
			_achievementManager.CompleteAchievement(this);
		}
	}

	protected override void ResolveDependencies()
	{
		base.ResolveDependencies();
		_profileManager = ServiceLocator.Resolve<IProfileManager>();
	}

	protected override void FetchStaticParameter()
	{
		base.FetchStaticParameter();
		_timeThreshold = _data.ParameterEffect.GetParameterValue(ParameterType.count);
	}
}
