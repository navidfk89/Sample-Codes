using System.Linq;

namespace FMG.Terminal.Runtime.Models;

public class Command : ICommand
{
	public Parameter[] Parameters { get; }

	public ICommandData StaticData { get; }

	public Command(Parameter[] parameters, ICommandData commandData)
	{
		Parameters = parameters;
		StaticData = commandData;
	}

	public bool TryGetParameter(string name, out string value, string defaultValue = null)
	{
		Parameter parameter = Parameters.Where((Parameter x) => x.Name == name).FirstOrDefault();
		if (parameter == null)
		{
			value = defaultValue;
			return false;
		}
		value = parameter.Value;
		return true;
	}

	public bool TryGetParameter(string name, out int value, int defaultValue = 0)
	{
		Parameter parameter = Parameters.Where((Parameter x) => x.Name == name).FirstOrDefault();
		if (parameter == null)
		{
			value = defaultValue;
			return false;
		}
		return int.TryParse(parameter.Value, out value);
	}

	public bool TryGetParameter(string name, out string[] value, string[] defaultValue = null)
	{
		Parameter parameter = Parameters.Where((Parameter x) => x.Name == name).FirstOrDefault();
		if (parameter == null)
		{
			value = null;
			return false;
		}
		value = parameter.Value.Split(',');
		return true;
	}

	public bool TryGetParameter(string name, out bool value, bool defaultValue = false)
	{
		Parameter parameter = Parameters.Where((Parameter x) => x.Name == name).FirstOrDefault();
		if (parameter == null)
		{
			value = false;
			return false;
		}
		value = true;
		return true;
	}
}
