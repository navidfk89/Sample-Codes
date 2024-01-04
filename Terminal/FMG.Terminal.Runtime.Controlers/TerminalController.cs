using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FMG.Terminal.Runtime.Interfaces;
using FMG.Terminal.Runtime.Models;
using FMG.Terminal.Runtime.Tools;
using UnityEngine;

namespace FMG.Terminal.Runtime.Controlers;

public class TerminalController : ITerminal, ITerminalExecuter, ITerminalInternal
{
	private readonly IFactory<ICommandHandler> _commandHandlerFactory;

	private readonly ITerminalView _view;

	public ICommandSetting Settings { get; }

	public static ITerminal Create(ITerminalView view)
	{
		CommandSettings settings = Resources.Load<CommandSettings>("CommandSettings");
		if ((object)settings == null)
		{
			throw new Exception("Setting is null");
		}
		return new TerminalController(settings, view);
	}

	private TerminalController(CommandSettings settings, ITerminalView view)
	{
		Settings = settings;
		_view = view;
		_commandHandlerFactory = new Factory<ICommandHandler>("CommandHandler", Settings.Assemblies.ToArray());
	}

	public void ExecuteCommand(string inputValue)
	{
		if (!TryGetCommand(inputValue, out var command))
		{
			_view.ShowError("");
			return;
		}
		if (!TryGetCommandHandler(command, out var commandHandler))
		{
			_view.ShowError("");
			return;
		}
		if (!commandHandler.CanExecute(out var message))
		{
			_view.ShowError(message);
			return;
		}
		string result = commandHandler.Execute();
		_view.ShowError(result);
	}

	private bool TryGetCommand(string rawData, out Command command)
	{
		command = null;
		ICommandData staticData = null;
		if (!TryParse(rawData, out var commandName, out var parameters))
		{
			return false;
		}
		staticData = Settings.Commands.FirstOrDefault((ICommandData x) => Regex.IsMatch(x.Name, commandName, RegexOptions.IgnoreCase));
		if (staticData == null)
		{
			return false;
		}
		command = new Command(parameters, staticData);
		return true;
	}

	private bool TryGetCommandHandler(Command command, out ICommandHandler commandHandler)
	{
		commandHandler = _commandHandlerFactory.CreateInstance(command.StaticData.Name, command, this);
		if (commandHandler == null)
		{
			return false;
		}
		return true;
	}

	private bool TryParse(string rawData, out string commandName, out Parameter[] parameters)
	{
		if (string.IsNullOrEmpty(rawData))
		{
			commandName = string.Empty;
			parameters = new Parameter[0];
			return false;
		}
		string[] words = rawData.Trim().Split(' ');
		if (words.Length == 0)
		{
			commandName = string.Empty;
			parameters = new Parameter[0];
			return false;
		}
		commandName = words[0];
		if (words.Length == 1)
		{
			parameters = new Parameter[0];
			return true;
		}
		List<Parameter> parametersList = new List<Parameter>();
		foreach (string word in words.Skip(1))
		{
			string[] splitName = word.Trim().Split(':');
			if (splitName.Length != 0)
			{
				Parameter parameter = new Parameter
				{
					Name = splitName[0]
				};
				if (splitName.Length == 2)
				{
					parameter.Value = splitName[1];
				}
				parametersList.Add(parameter);
			}
		}
		parameters = parametersList.ToArray();
		return true;
	}
}
