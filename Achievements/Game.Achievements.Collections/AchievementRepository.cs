using System;
using System.Threading.Tasks;
using Framework;
using Game.Achievements.Models;

namespace Game.Achievements.Collections;

public class AchievementRepository : IAchievementRepository, IServicable, IService
{
	private IAchievementSettings _settings;

	public AchievementRepository()
	{
		ServiceLocator.Register<IAchievementRepository>(this);
	}

	public IAchievementData GetAchievement(AchievementType type)
	{
		return _settings.GetAchievement(type);
	}

	public async Task Initialize()
	{
		await LoadSettings();
		await Task.CompletedTask;
	}

	private async Task LoadSettings()
	{
		AchievementSettings settings = await AssetLoader.LoadAsync<AchievementSettings>("AchievementsSettings");
		if (settings == null)
		{
			throw new MissingDataException("Achievement Settings are missing.");
		}
		settings.Initialize();
		_settings = settings;
	}
}
