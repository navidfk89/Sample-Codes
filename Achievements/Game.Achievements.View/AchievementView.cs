using Game.Achievements.Collections;
using Game.Achievements.Models;
using Game.Profile.Controllers;
using Game.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Achievements.View;

public sealed class AchievementView : BaseUIElement
{
	private IAchievementData _data;

	[SerializeField]
	private Image _image;

	[SerializeField]
	private GameObject _borderLocked;

	[SerializeField]
	private GameObject _borderUnlocked;

	[SerializeField]
	private Image _lockImage;

	[SerializeField]
	private AchievementTooltipContainer _achievementTooltipContainer;

	private IAchievementRepository _achievementRepository;

	private IProfileManager _profileManager;

	public void Initialize(AchievementType type)
	{
		ResolveDependencies();
		_data = _achievementRepository.GetAchievement(type);
		_achievementTooltipContainer.SetData(_data);
		SetSprite();
	}

	private void SetSprite()
	{
		if (_profileManager.HasAchievement(_data.Type))
		{
			_image.sprite = _data.ActiveSprite;
			_lockImage.gameObject.SetActive(value: false);
			_borderLocked.gameObject.SetActive(value: false);
			_borderUnlocked.gameObject.SetActive(value: true);
		}
		else
		{
			_image.sprite = _data.DisableSprite;
			_lockImage.gameObject.SetActive(value: true);
			_borderLocked.gameObject.SetActive(value: true);
			_borderUnlocked.gameObject.SetActive(value: false);
		}
	}

	private void ResolveDependencies()
	{
		_achievementRepository = ServiceLocator.Resolve<IAchievementRepository>();
		_profileManager = ServiceLocator.Resolve<IProfileManager>();
	}
}
