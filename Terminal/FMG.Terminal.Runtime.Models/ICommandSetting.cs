using System.Collections.Generic;
using System.Reflection;

namespace FMG.Terminal.Runtime.Models;

public interface ICommandSetting
{
	IEnumerable<ICommandData> Commands { get; }

	IEnumerable<Assembly> Assemblies { get; }
}
