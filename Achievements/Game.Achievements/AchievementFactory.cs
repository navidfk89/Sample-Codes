using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Game.Achievements.Collections;
using Game.Achievements.Controllers.Base;
using Game.Achievements.Manager;
using Game.Achievements.Models;
using UnityEngine;

namespace Game.Achievements;

public class AchievementFactory
{
	private readonly Dictionary<AchievementType, Type> _achievementControllers;

	private readonly IAchievementManager _achievementManager;

	private readonly IAchievementRepository _achievementRepository;

	public AchievementFactory(IAchievementManager achievementManager)
	{
		_achievementRepository = ServiceLocator.Resolve<IAchievementRepository>();
		_achievementManager = achievementManager;
		_achievementControllers = new Dictionary<AchievementType, Type>();
		FetchAchievementControllersClasses();
	}

	public AbstractAchievementController CreateAchievementController(AchievementType type)
	{
		IAchievementData staticData = _achievementRepository.GetAchievement(type);
		AbstractAchievementController controller = (AbstractAchievementController)Activator.CreateInstance(_achievementControllers[type]);
		controller.Initialize(_achievementManager, staticData);
		return controller;
	}

	private void FetchAchievementControllersClasses()
	{
		IEnumerable<Type> achievementControllers = from t in Assembly.GetAssembly(typeof(AbstractAchievementController)).GetTypes()
			where t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(AbstractAchievementController))
			select t;
		foreach (Type achievementController in achievementControllers)
		{
			if (Enum.TryParse<AchievementType>(achievementController.Name.Replace("AchievementController", ""), out var achievementType))
			{
				_achievementControllers.Add(achievementType, achievementController);
			}
			else
			{
				Debug.LogError("Power: " + achievementController.Name + " naming is not valid!");
			}
		}
	}
}
