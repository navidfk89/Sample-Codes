using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FMG.Terminal.Runtime.Controllers;

public class TerminalUiController
{
	private TextMeshProUGUI historyTextPrefab;

	private GameObject historyParentPanel;

	private TMP_InputField inputField = null;

	private List<string> commandHistory = new List<string>();

	private List<GameObject> textsHistory = new List<GameObject>();

	private int historyIndex;

	public int historyShowIndex = 0;

	public void AddCommandToHistory(string input)
	{
		commandHistory.Add(input);
		TextMeshProUGUI text = Object.Instantiate(historyTextPrefab);
		text.transform.SetParent(historyParentPanel.transform);
		text.text = input;
		textsHistory.Add(text.gameObject);
		historyIndex++;
	}

	public void ShowHistoryCommandUp()
	{
		if (historyShowIndex < commandHistory.Count)
		{
			historyShowIndex++;
			inputField.text = commandHistory[commandHistory.Count - historyShowIndex];
		}
	}

	public void ShowHistoryCommandDown()
	{
		if (historyShowIndex > 1)
		{
			historyShowIndex--;
			inputField.text = commandHistory[commandHistory.Count - historyShowIndex];
		}
	}

	public void ResetShowIndex()
	{
		historyShowIndex = 0;
	}

	public void ShowErrorTextToUser(string textInput)
	{
		TextMeshProUGUI text = Object.Instantiate(historyTextPrefab);
		text.transform.SetParent(historyParentPanel.transform);
		text.text = textInput;
		text.color = Color.red;
		textsHistory.Add(text.gameObject);
	}

	public void ClearHistory()
	{
		foreach (GameObject e in textsHistory)
		{
			Object.Destroy(e);
		}
		commandHistory.Clear();
		textsHistory.Clear();
	}
}
