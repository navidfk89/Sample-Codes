using System;
using System.Collections.Generic;
using FMG.Terminal.Runtime.Controlers;
using FMG.Terminal.Runtime.Interfaces;
using TMPro;
using UnityEngine;

public class TerminalView : MonoBehaviour, ITerminalView
{
	[SerializeField]
	private string prefix = string.Empty;

	[Header("UI")]
	[SerializeField]
	private GameObject uiCanvas = null;

	[SerializeField]
	private TMP_InputField inputField = null;

	[SerializeField]
	private TextMeshProUGUI textPrefab;

	[SerializeField]
	private GameObject historyPanel;

	private List<string> commandHistory = new List<string>();

	private List<GameObject> textsHistory = new List<GameObject>();

	private float pausedTimeScale;

	private int historyIndex;

	public static TerminalView instance;

	private ITerminal terminalController;

	private string[] usedCommands;

	private int historyShowIndex = 0;

	private void Awake()
	{
		if (instance != null && instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		terminalController = TerminalController.Create(this);
		inputField.pointSize = 90f;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			ShowHistoryCommandUp();
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			ShowHistoryCommandDown();
		}
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			Toggle();
		}
		if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
		{
			ProcessCommand();
			inputField.Select();
			inputField.ActivateInputField();
		}
	}

	private void AddCommandToHistory(string input)
	{
		commandHistory.Add(input);
		TextMeshProUGUI text = UnityEngine.Object.Instantiate(textPrefab);
		text.transform.SetParent(historyPanel.transform);
		text.text = input;
		textsHistory.Add(text.gameObject);
		historyIndex++;
	}

	private void ShowHistoryCommandUp()
	{
		if (historyShowIndex < commandHistory.Count)
		{
			historyShowIndex++;
			inputField.text = commandHistory[commandHistory.Count - historyShowIndex];
		}
	}

	private void ShowHistoryCommandDown()
	{
		if (historyShowIndex > 1)
		{
			historyShowIndex--;
			inputField.text = commandHistory[commandHistory.Count - historyShowIndex];
		}
	}

	private void Toggle()
	{
		if (uiCanvas.activeSelf)
		{
			Time.timeScale = pausedTimeScale;
			uiCanvas.SetActive(value: false);
			historyShowIndex = 0;
			ClearHistory();
		}
		else
		{
			pausedTimeScale = Time.timeScale;
			Time.timeScale = 0f;
			uiCanvas.SetActive(value: true);
			inputField.ActivateInputField();
		}
	}

	private void ProcessCommand()
	{
		string inputValue = inputField.text;
		AddCommandToHistory(inputValue);
		try
		{
			terminalController.ExecuteCommand(inputValue);
		}
		catch
		{
			Debug.LogError(inputValue);
		}
		inputField.text = string.Empty;
	}

	private void ShowTextToUser(string textInput)
	{
		TextMeshProUGUI text = UnityEngine.Object.Instantiate(textPrefab);
		text.transform.SetParent(historyPanel.transform);
		text.text = textInput;
		text.color = Color.red;
		textsHistory.Add(text.gameObject);
	}

	private void ClearHistory()
	{
		foreach (GameObject e in textsHistory)
		{
			UnityEngine.Object.Destroy(e);
		}
		commandHistory.Clear();
		textsHistory.Clear();
	}

	private void OnDisable()
	{
		ClearHistory();
	}

	public void ShowError(string message)
	{
		throw new NotImplementedException();
	}
}
