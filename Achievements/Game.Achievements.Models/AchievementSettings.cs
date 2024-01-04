using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Achievements.Models;

public class AchievementSettings : SerializedScriptableObject, IAchievementSettings
{
	public const string FileName = "AchievementsSettings";

	[SerializeField]
	private Dictionary<AchievementType, AchievementData> _achievements;

	public void Initialize()
	{
		foreach (AchievementData achievement in _achievements.Values)
		{
			achievement.Initialize();
		}
	}

	public IAchievementData GetAchievement(AchievementType type)
	{
		if (_achievements.TryGetValue(type, out var data))
		{
			return data;
		}
		throw new MissingDataException($"Achievement type of {type} is missing.");
	}
}
