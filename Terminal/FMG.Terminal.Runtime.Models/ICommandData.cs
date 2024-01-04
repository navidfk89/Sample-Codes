namespace FMG.Terminal.Runtime.Models;

public interface ICommandData
{
	string Name { get; }

	string Description { get; }

	IOption[] Options { get; }
}
