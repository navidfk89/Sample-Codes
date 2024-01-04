using System.Collections.Generic;

namespace FMG.Terminal.Runtime.Interfaces;

public interface ICommandHandler
{
	bool CanExecute(out string message);

	string Execute();

	IEnumerable<string> GetParameterSuggestions(string parameter);
}
