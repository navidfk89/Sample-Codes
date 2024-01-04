using System;
using System.Collections.Generic;
using System.Text;
using Assets.FMG.Terminal.Runtime.Controllers;
using FMG.Terminal.Runtime.Interfaces;
using FMG.Terminal.Runtime.Models;

namespace FMG.Terminal.Runtime.Commands;

internal class HelpCommandHandler : CommandHandler
{
	public HelpCommandHandler(ICommand data, ITerminal terminal)
		: base(data, terminal)
	{
	}

	public override string Execute()
	{
		StringBuilder content = new StringBuilder();
		content.AppendLine("Available commands :");
		foreach (ICommandData command in _terminal.Settings.Commands)
		{
			content.AppendLine(command.ToString());
		}
		return content.ToString();
	}

	public override IEnumerable<string> GetParameterSuggestions(string parameter)
	{
		throw new NotImplementedException();
	}
}
