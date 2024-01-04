using Game.Models;
using UnityEngine;

namespace Game.Achievements.Models;

public class AchievementData : ScriptableObject, IAchievementData
{
	[SerializeField]
	private AchievementType _type;

	[SerializeField]
	private string _name;

	[SerializeField]
	public ParameterEffect _parameterEffect;

	[SerializeField]
	private Sprite _activeSprite;

	[SerializeField]
	private Sprite _disableSprite;

	public AchievementType Type => _type;

	public string Name => _name;

	public IParameterEffect ParameterEffect => _parameterEffect;

	public Sprite ActiveSprite => _activeSprite;

	public Sprite DisableSprite => _disableSprite;

	public void Initialize()
	{
		_parameterEffect.Initialize();
	}
}
