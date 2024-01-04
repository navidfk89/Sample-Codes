using Game.Achievements.Controllers.Base;
using Game.Achievements.Triggers;
using Game.Encounters;
using Game.Gamplay.Combat;

namespace Game.Achievements.Controllers;

internal class TheShapesAchievementController : AbstractAchievementController, IEndCombatTrigger, IAchievementTrigger
{
	private EncounterType _encounterType;

	private ICombatGameplayController _combatGameplayController;

	public void ProcessEndCombat(bool isWin)
	{
		if (isWin && _combatGameplayController.Encounter.EncounterType == _encounterType)
		{
			_achievementManager.CompleteAchievement(this);
		}
	}

	protected override void ResolveCombatDependencies()
	{
		base.ResolveCombatDependencies();
		_combatGameplayController = ServiceLocator.Resolve<ICombatGameplayController>();
	}

	protected override void FetchStaticParameter()
	{
		base.FetchStaticParameter();
		_encounterType = _data.ParameterEffect.GetParameterValue(ParameterType.encounterType, EncounterType.None);
	}
}
