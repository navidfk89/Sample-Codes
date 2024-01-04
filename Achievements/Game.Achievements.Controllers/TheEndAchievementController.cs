using System;
using System.Collections.Generic;
using System.Linq;
using Game.Achievements.Controllers.Base;
using Game.Achievements.Triggers;
using Game.Acts;
using Game.Acts.Controllers;
using Game.Gamplay;
using Game.Profile.Controllers;

namespace Game.Achievements.Controllers;

internal class TheEndAchievementController : AbstractAchievementController, IRunCompleteTrigger, IAchievementTrigger
{
	private ActType _actType;

	private IProfileManager _profileManager;

	private IActManager _actManager;

	public void ProcessRunComplete(EndGameData endGameData)
	{
		if (endGameData.IsGameWin && _actManager.CurrentAct.StaticData.Type == _actType)
		{
			IEnumerable<ClassType> classTypes = from ClassType x in Enum.GetValues(typeof(ClassType))
				where x != 0 && x != ClassType.watcher
				select x;
			if (_profileManager.Data.HeroesBeatenAct4.Count() >= classTypes.Count())
			{
				_achievementManager.CompleteAchievement(this);
			}
		}
	}

	protected override void ResolveDependencies()
	{
		base.ResolveDependencies();
		_profileManager = ServiceLocator.Resolve<IProfileManager>();
		_actManager = ServiceLocator.Resolve<IActManager>();
	}

	protected override void FetchStaticParameter()
	{
		base.FetchStaticParameter();
		_actType = _data.ParameterEffect.GetParameterValue(ParameterType.actNumber, ActType.none);
	}
}
