namespace Game.Achievements.Triggers;

public interface IChangeEnergyTrigger : IAchievementTrigger
{
	void ProcessChangeEnergy(int currentEnergy, int capacity);
}
