using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FMG.Terminal.Runtime.Commands;
using FMG.Terminal.Runtime.Models;
using UnityEngine;

public class CommandSettings : ScriptableObject, ICommandSetting
{
	public const string AssetName = "CommandSettings";

	public List<CommandData> _commands = new List<CommandData>();

	[SerializeField]
	private List<string> _assemblies;

	public IEnumerable<ICommandData> Commands => _commands;

	public IEnumerable<Assembly> Assemblies => from x in AppDomain.CurrentDomain.GetAssemblies()
		where _assemblies.Contains(x.GetName().Name)
		select x;

	public void AddToCommands(CommandData command)
	{
	}
}
