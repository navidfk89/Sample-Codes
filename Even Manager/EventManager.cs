using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Framework;

public static class EventManager
{
	private static Dictionary<EventType, Delegate> _registerdEvents;

	static EventManager()
	{
		IEnumerable<EventType> events = Enum.GetValues(typeof(EventType)).Cast<EventType>();
		_registerdEvents = new Dictionary<EventType, Delegate>(events.Count());
		foreach (EventType @event in events)
		{
			_registerdEvents.Add(@event, null);
		}
	}

	public static void Broadcast(EventType eventType)
	{
		if ((object)_registerdEvents[eventType] != null)
		{
			((Action)_registerdEvents[eventType])();
		}
	}

	public static void AddListener(EventType eventType, Action action)
	{
		_registerdEvents[eventType] = (Action)Delegate.Combine((Action)_registerdEvents[eventType], action);
	}

	public static void RemoveListener(EventType eventType, Action action)
	{
		_registerdEvents[eventType] = (Action)Delegate.Remove((Action)_registerdEvents[eventType], action);
	}

	public static void Broadcast<T>(EventType eventType, T arg1)
	{
		if ((object)_registerdEvents[eventType] != null)
		{
			try
			{
				((Action<T>)_registerdEvents[eventType])(arg1);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogException(ex);
			}
			Debug("Broadcast", eventType);
		}
	}

	public static void AddListener<T>(EventType eventType, Action<T> action)
	{
		_registerdEvents[eventType] = (Action<T>)Delegate.Combine((Action<T>)_registerdEvents[eventType], action);
		Debug("Add", eventType);
	}

	public static void RemoveListener<T>(EventType eventType, Action<T> action)
	{
		_registerdEvents[eventType] = (Action<T>)Delegate.Remove((Action<T>)_registerdEvents[eventType], action);
		Debug("Remove", eventType);
	}

	public static void Broadcast<T, U>(EventType eventType, T arg1, U arg2)
	{
		if ((object)_registerdEvents[eventType] != null)
		{
			((Action<T, U>)_registerdEvents[eventType])(arg1, arg2);
		}
	}

	public static void AddListener<T, U>(EventType eventType, Action<T, U> action)
	{
		_registerdEvents[eventType] = (Action<T, U>)Delegate.Combine((Action<T, U>)_registerdEvents[eventType], action);
	}

	public static void RemoveListener<T, U>(EventType eventType, Action<T, U> action)
	{
		_registerdEvents[eventType] = (Action<T, U>)Delegate.Remove((Action<T, U>)_registerdEvents[eventType], action);
	}

	public static void Broadcast<T, U, V>(EventType eventType, T arg1, U arg2, V arg3)
	{
		if ((object)_registerdEvents[eventType] != null)
		{
			((Action<T, U, V>)_registerdEvents[eventType])(arg1, arg2, arg3);
		}
	}

	public static void AddListener<T, U, V>(EventType eventType, Action<T, U, V> action)
	{
		_registerdEvents[eventType] = (Action<T, U, V>)Delegate.Combine((Action<T, U, V>)_registerdEvents[eventType], action);
	}

	public static void RemoveListener<T, U, V>(EventType eventType, Action<T, U, V> action)
	{
		_registerdEvents[eventType] = (Action<T, U, V>)Delegate.Remove((Action<T, U, V>)_registerdEvents[eventType], action);
	}

	public static void Broadcast<T, U, V, W>(EventType eventType, T arg1, U arg2, V arg3, W arge4)
	{
		if ((object)_registerdEvents[eventType] != null)
		{
			((Action<T, U, V, W>)_registerdEvents[eventType])(arg1, arg2, arg3, arge4);
		}
	}

	public static void AddListener<T, U, V, W>(EventType eventType, Action<T, U, V, W> action)
	{
		_registerdEvents[eventType] = (Action<T, U, V, W>)Delegate.Combine((Action<T, U, V, W>)_registerdEvents[eventType], action);
	}

	public static void RemoveListener<T, U, V, W>(EventType eventType, Action<T, U, V, W> action)
	{
		_registerdEvents[eventType] = (Action<T, U, V, W>)Delegate.Remove((Action<T, U, V, W>)_registerdEvents[eventType], action);
	}

	private static void Debug(string message, EventType eventType)
	{
		if (eventType == EventType.OnGameplayFinished)
		{
		}
	}
}
