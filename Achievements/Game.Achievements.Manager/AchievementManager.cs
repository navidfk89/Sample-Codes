using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework;
using Game.Achievements.Controllers.Base;
using Game.Achievements.Models;
using Game.Achievements.Triggers;
using Game.Actions.Models;
using Game.Cards.Controllers.Behaviours;
using Game.Characters.Enemies.Models;
using Game.Gamplay;
using Game.Orbs;
using Game.Powers.Models;
using Game.Profile.Controllers;

namespace Game.Achievements.Manager;

public class AchievementManager : IAchievementManager, IServicable, IScopedService, IService
{
	private List<AbstractAchievementController> _achievementControllers;

	private AchievementFactory _achievementFactory;

	private IProfileManager _profileManager;

	public AchievementManager(string scope)
	{
		ServiceLocator.Register<IAchievementManager>(this, scope);
		_achievementFactory = new AchievementFactory(this);
		_achievementControllers = new List<AbstractAchievementController>();
	}

	public Task Initialize()
	{
		ResolveDependencies();
		RegisterEvents();
		CreateAchievementsControllers();
		return Task.CompletedTask;
	}

	public Task Dispose()
	{
		UnregisterEvents();
		return Task.CompletedTask;
	}

	private void ResolveDependencies()
	{
		_profileManager = ServiceLocator.Resolve<IProfileManager>();
	}

	public void CompleteAchievement(IAchievementController controller)
	{
		_profileManager.CollectAchievement(controller.Data.Type);
		GetAchievement(controller.Data);
	}

	private void CreateAchievementsControllers()
	{
		List<AchievementType> achievementTypes = (from AchievementType x in Enum.GetValues(typeof(AchievementType))
			where x != AchievementType.None
			select x).ToList();
		if (_profileManager.Data.CollectedAchievements != null)
		{
			foreach (AchievementType achievementType2 in _profileManager.Data.CollectedAchievements)
			{
				achievementTypes.Remove(achievementType2);
			}
		}
		foreach (AchievementType achievementType in achievementTypes)
		{
			AbstractAchievementController controller = _achievementFactory.CreateAchievementController(achievementType);
			_achievementControllers.Add(controller);
		}
	}

	private void RegisterEvents()
	{
		EventManager.AddListener<bool>(EventType.OnEndCombat, EndCombat);
		EventManager.AddListener(EventType.OnStartCombat, StartCombat);
		EventManager.AddListener<int>(EventType.OnDrawPileCountChanged, DrawPileChange);
		EventManager.AddListener<int>(EventType.OnDiscardCountChanged, DiscardPileChange);
		EventManager.AddListener<int>(EventType.OnHandCardCountChange, HandChange);
		EventManager.AddListener<ICardBehaviour>(EventType.OnCardPlayed, PlayCard);
		EventManager.AddListener<ICardBehaviour>(EventType.OnExhaustCard, ExhaustCard);
		EventManager.AddListener<int, int>(EventType.OnEnergyChanged, ChangeEnergy);
		EventManager.AddListener(EventType.OnTurnStarted, StartTurn);
		EventManager.AddListener(EventType.OnTurnEnd, EndTurn);
		EventManager.AddListener<IPowerInGameData>(EventType.OnApplyPower, ApplyPower);
		EventManager.AddListener<BlockInfo>(EventType.OnChangeBlock, ChangeBlock);
		EventManager.AddListener<IEnemyData>(EventType.OnEnemyDie, EnemyDie);
		EventManager.AddListener<OrbType>(EventType.OnChannelOrb, ChannelOrb);
		EventManager.AddListener<EndGameData>(EventType.OnRunComplete, RunComplete);
		EventManager.AddListener<ChangeHPInfo>(EventType.OnLoseHp, LoseHp);
		EventManager.AddListener<ChangeHPInfo>(EventType.OnHealHp, HealHp);
	}

	private void UnregisterEvents()
	{
		EventManager.RemoveListener<bool>(EventType.OnEndCombat, EndCombat);
		EventManager.RemoveListener(EventType.OnStartCombat, StartCombat);
		EventManager.RemoveListener<int>(EventType.OnDrawPileCountChanged, DrawPileChange);
		EventManager.RemoveListener<int>(EventType.OnDiscardCountChanged, DiscardPileChange);
		EventManager.RemoveListener<int>(EventType.OnHandCardCountChange, HandChange);
		EventManager.RemoveListener<ICardBehaviour>(EventType.OnCardPlayed, PlayCard);
		EventManager.RemoveListener<ICardBehaviour>(EventType.OnExhaustCard, ExhaustCard);
		EventManager.RemoveListener<int, int>(EventType.OnEnergyChanged, ChangeEnergy);
		EventManager.RemoveListener(EventType.OnTurnStarted, StartTurn);
		EventManager.RemoveListener(EventType.OnTurnEnd, EndTurn);
		EventManager.RemoveListener<IPowerInGameData>(EventType.OnApplyPower, ApplyPower);
		EventManager.RemoveListener<BlockInfo>(EventType.OnChangeBlock, ChangeBlock);
		EventManager.RemoveListener<IEnemyData>(EventType.OnEnemyDie, EnemyDie);
		EventManager.RemoveListener<OrbType>(EventType.OnChannelOrb, ChannelOrb);
		EventManager.RemoveListener<EndGameData>(EventType.OnRunComplete, RunComplete);
		EventManager.RemoveListener<ChangeHPInfo>(EventType.OnLoseHp, LoseHp);
		EventManager.RemoveListener<ChangeHPInfo>(EventType.OnHealHp, HealHp);
	}

	private void EndCombat(bool isWin)
	{
		IEnumerable<IEndCombatTrigger> achievementControllers = GetAchievements<IEndCombatTrigger>();
		foreach (IEndCombatTrigger achievementController in achievementControllers)
		{
			achievementController.ProcessEndCombat(isWin);
		}
	}

	private void StartCombat()
	{
		PrepareForCombat();
		IEnumerable<IStartCombatTrigger> achievementControllers = GetAchievements<IStartCombatTrigger>();
		foreach (IStartCombatTrigger achievementController in achievementControllers)
		{
			achievementController.ProcessStartCombat();
		}
	}

	private void PrepareForCombat()
	{
		foreach (AbstractAchievementController controller in _achievementControllers)
		{
			controller.PrepareForCombat();
		}
	}

	private void DrawPileChange(int totalCardsInDrawPile)
	{
		IEnumerable<IDrawPileChangeTrigger> achievementControllers = GetAchievements<IDrawPileChangeTrigger>();
		foreach (IDrawPileChangeTrigger achievementController in achievementControllers)
		{
			achievementController.ProcessDrawPileChange(totalCardsInDrawPile);
		}
	}

	private void DiscardPileChange(int totalCardsInDiscardPile)
	{
		IEnumerable<IDiscardPileChangeTrigger> achievementControllers = GetAchievements<IDiscardPileChangeTrigger>();
		foreach (IDiscardPileChangeTrigger achievementController in achievementControllers)
		{
			achievementController.ProcessDiscardPileChange(totalCardsInDiscardPile);
		}
	}

	private void HandChange(int totalCardsInHand)
	{
		IEnumerable<IHandChangeTrigger> achievementControllers = GetAchievements<IHandChangeTrigger>();
		foreach (IHandChangeTrigger achievementController in achievementControllers)
		{
			achievementController.ProcessHandChange(totalCardsInHand);
		}
	}

	private void PlayCard(ICardBehaviour cardBehaviour)
	{
		IEnumerable<IPlayCardTrigger> achievementControllers = GetAchievements<IPlayCardTrigger>();
		foreach (IPlayCardTrigger achievementController in achievementControllers)
		{
			achievementController.ProcessPlayCard(cardBehaviour);
		}
	}

	private void ExhaustCard(ICardBehaviour cardBehaviour)
	{
		IEnumerable<IExhaustCardTrigger> achievementControllers = GetAchievements<IExhaustCardTrigger>();
		foreach (IExhaustCardTrigger achievementController in achievementControllers)
		{
			achievementController.ProcessExhaustCard(cardBehaviour);
		}
	}

	private void ChangeEnergy(int currentEnergy, int capacity)
	{
		IEnumerable<IChangeEnergyTrigger> achievementControllers = GetAchievements<IChangeEnergyTrigger>();
		foreach (IChangeEnergyTrigger achievementController in achievementControllers)
		{
			achievementController.ProcessChangeEnergy(currentEnergy, capacity);
		}
	}

	private void StartTurn()
	{
		IEnumerable<IStartTurnTrigger> achievementControllers = GetAchievements<IStartTurnTrigger>();
		foreach (IStartTurnTrigger achievementController in achievementControllers)
		{
			achievementController.ProcessStarTurn();
		}
	}

	private void EndTurn()
	{
		IEnumerable<IEndTurnTrigger> achievementControllers = GetAchievements<IEndTurnTrigger>();
		foreach (IEndTurnTrigger achievementController in achievementControllers)
		{
			achievementController.ProcessEndTurn();
		}
	}

	private void ApplyPower(IPowerInGameData powerInGameData)
	{
		IEnumerable<IApplyPowerTrigger> achievementControllers = GetAchievements<IApplyPowerTrigger>();
		foreach (IApplyPowerTrigger achievementController in achievementControllers)
		{
			achievementController.ProcessApplyPower(powerInGameData);
		}
	}

	private void ChangeBlock(BlockInfo blockInfo)
	{
		IEnumerable<IChangeBlockTrigger> achievementControllers = GetAchievements<IChangeBlockTrigger>();
		foreach (IChangeBlockTrigger achievementController in achievementControllers)
		{
			achievementController.ProcessChangeBlock(blockInfo);
		}
	}

	private void EnemyDie(IEnemyData enemyData)
	{
		IEnumerable<IEnemyDieTrigger> achievementControllers = GetAchievements<IEnemyDieTrigger>();
		foreach (IEnemyDieTrigger achievementController in achievementControllers)
		{
			achievementController.ProcessEnemyDie(enemyData);
		}
	}

	private void ChannelOrb(OrbType orbType)
	{
		IEnumerable<IChannelOrbTrigger> achievementControllers = GetAchievements<IChannelOrbTrigger>();
		foreach (IChannelOrbTrigger achievementController in achievementControllers)
		{
			achievementController.ProcessChannelOrb(orbType);
		}
	}

	private void RunComplete(EndGameData endGameData)
	{
		IEnumerable<IRunCompleteTrigger> achievementControllers = GetAchievements<IRunCompleteTrigger>();
		foreach (IRunCompleteTrigger achievementController in achievementControllers)
		{
			achievementController.ProcessRunComplete(endGameData);
		}
	}

	private void HealHp(ChangeHPInfo data)
	{
		IEnumerable<IHealHpTrigger> achievementControllers = GetAchievements<IHealHpTrigger>();
		foreach (IHealHpTrigger achievementController in achievementControllers)
		{
			achievementController.ProcessHealHpTrigger(data);
		}
	}

	private void LoseHp(ChangeHPInfo data)
	{
		IEnumerable<ILoseHpTrigger> achievementControllers = GetAchievements<ILoseHpTrigger>();
		foreach (ILoseHpTrigger achievementController in achievementControllers)
		{
			achievementController.ProcessLoseHpTrigger(data);
		}
	}

	private void GetAchievement(IAchievementData achievementData)
	{
		IEnumerable<IGetAchievementTrigger> achievementControllers = GetAchievements<IGetAchievementTrigger>();
		foreach (IGetAchievementTrigger achievementController in achievementControllers)
		{
			achievementController.ProcessGetAchievement(achievementData);
		}
	}

	private IEnumerable<T> GetAchievements<T>() where T : class, IAchievementTrigger
	{
		return _achievementControllers.Where((AbstractAchievementController x) => x is T).Cast<T>();
	}
}
