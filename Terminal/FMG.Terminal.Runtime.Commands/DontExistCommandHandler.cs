using System;
using System.Collections.Generic;
using Assets.FMG.Terminal.Runtime.Controllers;
using FMG.Terminal.Runtime.Interfaces;
using FMG.Terminal.Runtime.Models;
using UnityEngine;

namespace FMG.Terminal.Runtime.Commands;

public class DontExistCommandHandler : CommandHandler
{
	public DontExistCommandHandler(ICommand data, ITerminal terminal)
		: base(data, terminal)
	{
	}

	public override string Execute()
	{
		Debug.LogWarning("This Command Doesnt Exist");
		return "";
	}

	public override IEnumerable<string> GetParameterSuggestions(string parameter)
	{
		throw new NotImplementedException();
	}
}
