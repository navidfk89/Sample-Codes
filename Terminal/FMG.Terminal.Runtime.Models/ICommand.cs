namespace FMG.Terminal.Runtime.Models;

public interface ICommand
{
	ICommandData StaticData { get; }

	bool TryGetParameter(string name, out string value, string defaultValue = null);

	bool TryGetParameter(string name, out bool value, bool defaultValue = false);

	bool TryGetParameter(string name, out int value, int defaultValue = 0);

	bool TryGetParameter(string name, out string[] value, string[] defaultValue = null);
}
