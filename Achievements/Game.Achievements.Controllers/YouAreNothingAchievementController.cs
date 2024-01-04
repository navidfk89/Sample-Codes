using Game.Achievements.Controllers.Base;
using Game.Achievements.Triggers;
using Game.Gamplay;
using Game.Gamplay.Combat;
using Game.Map;

namespace Game.Achievements.Controllers;

internal class YouAreNothingAchievementController : AbstractAchievementController, IEndCombatTrigger, IAchievementTrigger
{
	private RoomType _roomType;

	private ITurnManager _turnManager;

	private IGameplayManager _gameplayManager;

	public void ProcessEndCombat(bool isWin)
	{
		if (_turnManager.FirstRound && isWin && _gameplayManager.CurrentRoomType == _roomType)
		{
			_achievementManager.CompleteAchievement(this);
		}
	}

	protected override void ResolveCombatDependencies()
	{
		base.ResolveCombatDependencies();
		_turnManager = ServiceLocator.Resolve<ITurnManager>();
		_gameplayManager = ServiceLocator.Resolve<IGameplayManager>();
	}

	protected override void FetchStaticParameter()
	{
		base.FetchStaticParameter();
		_roomType = _data.ParameterEffect.GetParameterValue(ParameterType.roomtype, RoomType.none);
	}
}
