using System;
using System.Collections.Generic;
using Assets.FMG.Terminal.Runtime.Controllers;
using FMG.Terminal.Runtime.Interfaces;
using FMG.Terminal.Runtime.Models;

namespace FMG.Terminal.Runtime.Commands;

internal class ClearCommandHandler : CommandHandler
{
	public ClearCommandHandler(ICommand data, ITerminal terminal)
		: base(data, terminal)
	{
	}

	public override string Execute()
	{
		return "Clearing console";
	}

	public override IEnumerable<string> GetParameterSuggestions(string parameter)
	{
		throw new NotImplementedException();
	}
}
