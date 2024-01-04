using System;
using System.Linq;
using Game.Achievements.Controllers.Base;
using Game.Achievements.Models;
using Game.Achievements.Triggers;
using Game.Profile.Controllers;

namespace Game.Achievements.Controllers;

internal class EternalOneAchievementController : AbstractAchievementController, IGetAchievementTrigger, IAchievementTrigger
{
	private int _achievementCounts;

	private IProfileManager _profileManager;

	public void ProcessGetAchievement(IAchievementData achievementData)
	{
		if (_profileManager.Data.CollectedAchievements.Count() >= _achievementCounts)
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
		_achievementCounts = Enum.GetValues(typeof(AchievementType)).Cast<AchievementType>().Count((AchievementType x) => x != AchievementType.None);
	}
}
