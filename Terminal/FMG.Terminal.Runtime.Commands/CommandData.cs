using Assets.FMG.Terminal.Runtime.Models;
using FMG.Terminal.Runtime.Models;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FMG.Terminal.Runtime.Commands;

[CreateAssetMenu(fileName = "CommandData", menuName = "FMG/Terminal/Create Command")]
public class CommandData : SerializedScriptableObject, ICommandData
{
	[SerializeField]
	private Option[] _options;

	[field: SerializeField]
	public string Name { get; set; }

	[field: SerializeField]
	public string Description { get; set; }

	public IOption[] Options => _options;

	public override string ToString()
	{
		return Name + "\t" + Description;
	}
}
