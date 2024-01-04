using System;

namespace FMG.Terminal.Runtime.Models;

public interface IOption
{
	string Name { get; }

	string Description { get; }

	Type ValueType { get; }
}
