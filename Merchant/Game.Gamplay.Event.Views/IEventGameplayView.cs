using System.Collections.Generic;
using Game.Events.Controllers;
using Game.Events.Models;
using Game.Events.Views;
using Game.UI;

namespace Game.Gamplay.Event.Views;

public interface IEventGameplayView
{
	IUINavigator Navigator { get; }

	IEnumerable<IOptionView> OptionViews { get; }

	void Prepare(IEventData data);

	void ShowDialog(IDialog dialog);

	void ShowOptions(IEnumerable<IOptionController> optionControllers);
}
