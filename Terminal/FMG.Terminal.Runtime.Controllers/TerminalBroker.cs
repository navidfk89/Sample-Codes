using System;

namespace FMG.Terminal.Runtime.Controllers;

public static class TerminalBroker
{
	public static Action<string> ShowError;

	public static Action<string> ShowLog;

	public static void CallShowError(string error)
	{
		if (ShowError != null)
		{
			ShowError(error);
		}
	}

	public static void CallShowLog(string log)
	{
		if (ShowLog != null)
		{
			ShowLog(log);
		}
	}
}
