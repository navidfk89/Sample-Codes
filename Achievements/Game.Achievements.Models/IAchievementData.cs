using Game.Models;
using UnityEngine;

namespace Game.Achievements.Models;

public interface IAchievementData
{
	AchievementType Type { get; }

	string Name { get; }

	IParameterEffect ParameterEffect { get; }

	Sprite ActiveSprite { get; }

	Sprite DisableSprite { get; }
}
