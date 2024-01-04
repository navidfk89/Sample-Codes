using System;
using FMG.Terminal.Runtime.Models;
using UnityEngine;

namespace Assets.FMG.Terminal.Runtime.Models;

[Serializable]
public class Option : IOption
{
	public const string Format = "Name:{0}, Description:{1}, ValueType:{2}";

	[field: SerializeField]
	public string Name { get; set; }

	[field: SerializeField]
	public string Description { get; set; }

	[field: SerializeField]
	public Type ValueType { get; set; }

	public override string ToString()
	{
		return $"Name:{Name}, Description:{Description}, ValueType:{ValueType}";
	}
}
