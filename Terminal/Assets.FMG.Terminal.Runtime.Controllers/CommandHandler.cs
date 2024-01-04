using System.Collections.Generic;
using FMG.Terminal.Runtime.Interfaces;
using FMG.Terminal.Runtime.Models;

namespace Assets.FMG.Terminal.Runtime.Controllers;

public abstract class CommandHandler : ICommandHandler
{
	protected readonly ICommand _data;

	protected readonly ITerminalInternal _terminal;

	public CommandHandler(ICommand data, ITerminalInternal terminal)
	{
		_data = data;
		_terminal = terminal;
		ResovleDependencies();
		FetchParameters();
	}

	protected virtual void ResovleDependencies()
	{
	}

	protected virtual void FetchParameters()
	{
	}

	public virtual bool CanExecute(out string message)
	{
		message = string.Empty;
		return true;
	}

	public abstract string Execute();

	public abstract IEnumerable<string> GetParameterSuggestions(string parameter);
}
