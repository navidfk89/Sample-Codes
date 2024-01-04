using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Game.Environment;
using Game.Events;
using Game.Events.Controllers;
using Game.Events.Models;
using Game.Events.Views;
using Game.Gamplay.Views;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Gamplay.Event.Views;

public class EventGameplayView : GameplayBaseView, IEventGameplayView, IEscapable
{
	[SerializeField]
	private GameObject _defaultUi;

	[SerializeField]
	private TextMeshProUGUI _eventNameText;

	[SerializeField]
	private Image _eventVisualPlace;

	[SerializeField]
	private TextMeshProUGUI _dialogText;

	[SerializeField]
	private Transform _defaultOptionsParent;

	[SerializeField]
	private Transform _sceneOptionsParent;

	[SerializeField]
	private OptionView[] _options;

	[SerializeField]
	private Sprite _activeOptionSprite;

	[SerializeField]
	private Sprite _deactiveOptionSprite;

	private IEventView _eventView;

	private IEventData _data;

	private EventScreenVisual _currentEventScreenVisual;

	private IEnvironmentManager _environmentManager;

	private IDialog dialog1;

	public IUINavigator Navigator => _uiController;

	public IEnumerable<IOptionView> OptionViews => _options;

	public override void Open()
	{
		base.Open();
		ResolveDependencies();
		_environmentManager.Show(GameplayType.events);
		ShowEvent();
	}

	private void Start()
	{
		EventManager.AddListener<string>(Framework.EventType.onWheelRewardRevealed, UpdateDialog);
	}

	public override void Close()
	{
		base.Close();
		_eventView.Close();
		OptionView[] options = _options;
		foreach (OptionView option in options)
		{
			option.Close();
		}
	}

	public void HandleEscape()
	{
		throw new NotImplementedException();
	}

	public void Prepare(IEventData data)
	{
		_data = data;
		ResolveGameplayDependencies();
	}

	private void ShowEvent()
	{
		_defaultUi.SetActive(value: false);
		Game.Events.EventType type = _data.Type;
		Game.Events.EventType eventType = type;
		if (eventType == Game.Events.EventType.Neow)
		{
			_eventView = new NeowEventView();
			SetOptionsParent(_sceneOptionsParent);
		}
		else
		{
			_eventView = new EventView();
			ShowEventDefault();
		}
		_eventView.Open();
	}

	private void ShowEventDefault()
	{
		_eventNameText.SetText(_data.Name);
		if (_currentEventScreenVisual != null)
		{
			UnityEngine.Object.Destroy(_currentEventScreenVisual.gameObject);
		}
		if (_data.EventScreenVisual == null)
		{
			Debug.LogWarning("event visual data is null : " + _data.Type);
		}
		else
		{
			EventScreenVisual visual = UnityEngine.Object.Instantiate(_data.EventScreenVisual, _eventVisualPlace.transform);
			_currentEventScreenVisual = visual;
		}
		SetOptionsParent(_defaultOptionsParent);
		_defaultUi.SetActive(value: true);
	}

	public void ShowDialog(IDialog dialog)
	{
		dialog1 = dialog;
		string remove = dialog.Story;
		int count = remove.Count((char t) => t == '<');
		remove = RemoveUnwantedTexts(remove, count);
		int lettersCount = remove.Length;
		_dialogText.SetText(dialog.Story);
		if (lettersCount >= 200)
		{
			_dialogText.fontSize = 28f;
			_dialogText.lineSpacing = 0f;
		}
		else
		{
			_dialogText.fontSize = 30f;
			_dialogText.lineSpacing = 0f;
		}
	}

	private void UpdateDialog(string text)
	{
		_dialogText.SetText(text);
	}

	private string RemoveUnwantedTexts(string text, int charCount)
	{
		string editedText = text;
		for (int i = 0; i < charCount; i++)
		{
			int indexOfFirst = editedText.IndexOf("<");
			int indexOfLast = editedText.IndexOf(">");
			int diff = indexOfLast + 1 - indexOfFirst;
			editedText = editedText.Remove(indexOfFirst, diff);
		}
		return editedText;
	}

	public void ShowOptions(IEnumerable<IOptionController> optionControllers)
	{
		OptionView[] options = _options;
		foreach (OptionView optionView in options)
		{
			optionView.Close();
		}
		int index = 0;
		foreach (IOptionController option in optionControllers)
		{
			_options[index].Initialize(option).SetActive(option.CheckCanExecute()).RegisterOnClick(delegate
			{
				option.Execute();
				EventManager.Broadcast(Framework.EventType.addOptionData, option.Data.Content);
			})
				.Open();
			index++;
		}
	}

	private void ResolveDependencies()
	{
		if (_environmentManager == null)
		{
			_environmentManager = ServiceLocator.Resolve<IEnvironmentManager>();
		}
	}

	private void ResolveGameplayDependencies()
	{
	}

	private void ShowNeowEvent()
	{
	}

	private void SetOptionsParent(Transform parent)
	{
		OptionView[] options = _options;
		foreach (OptionView option in options)
		{
			option.transform.SetParent(parent);
		}
	}

	public Game.Events.EventType GetEventType()
	{
		return _data.Type;
	}
}
