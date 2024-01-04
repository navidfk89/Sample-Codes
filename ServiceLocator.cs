using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

public static class ServiceLocator
{
	private static Dictionary<string, Dictionary<Type, object>> _registerdServices = new Dictionary<string, Dictionary<Type, object>> { 
	{
		"global",
		new Dictionary<Type, object>()
	} };

	public static void Register<TService>(object serviceInstance, string scope = "global") where TService : IServicable
	{
		Type type = typeof(TService);
		if (IsValidRegistration(type, serviceInstance))
		{
			if (!_registerdServices.ContainsKey(scope))
			{
				CreateScope(scope);
			}
			if (!_registerdServices[scope].ContainsKey(type))
			{
				_registerdServices[scope].Add(type, serviceInstance);
			}
			else
			{
				_registerdServices[scope][type] = serviceInstance;
			}
		}
	}

	public static TService Resolve<TService>() where TService : IServicable
	{
		Type type = typeof(TService);
		try
		{
			foreach (KeyValuePair<string, Dictionary<Type, object>> kvp in _registerdServices)
			{
				if (kvp.Value.ContainsKey(type))
				{
					return (TService)kvp.Value[type];
				}
			}
		}
		catch (KeyNotFoundException)
		{
			Debug.LogError("Service of type '" + type.Name + "' not found in Service Locator.");
		}
		catch (ArgumentNullException)
		{
			Debug.LogError("Null service type passed to Service Locator. This is not accepted behavior.");
		}
		return default(TService);
	}

	public static void RegisterSingleton<TSingleton>(object instance, string scope = "global") where TSingleton : class
	{
		Type type = typeof(TSingleton);
		if (instance == null)
		{
			Debug.LogError("Registering " + type.Name + " in service locater is failed because singleton instance is null.");
			return;
		}
		if (!_registerdServices.ContainsKey(scope))
		{
			CreateScope(scope);
		}
		if (!_registerdServices[scope].ContainsKey(type))
		{
			_registerdServices[scope].Add(type, instance);
		}
		else
		{
			_registerdServices[scope][type] = instance;
		}
	}

	public static TSingleton ResolveSingleton<TSingleton>() where TSingleton : class
	{
		Type type = typeof(TSingleton);
		try
		{
			foreach (KeyValuePair<string, Dictionary<Type, object>> kvp in _registerdServices)
			{
				if (kvp.Value.ContainsKey(type))
				{
					return (TSingleton)kvp.Value[type];
				}
			}
		}
		catch (KeyNotFoundException)
		{
			Debug.LogError("Singleton of type '" + type.Name + "' not found in Service Locator.");
		}
		catch (ArgumentNullException)
		{
			Debug.LogError("Null singleton type passed to Service Locator. This is not accepted behavior.");
		}
		throw new ArgumentNullException("Singleton of type '" + type.Name + "' not found in Service Locator.");
	}

	public static TComponent Register<TService, TComponent>(string objectName, string scope = "global") where TService : IServicable where TComponent : Component, TService
	{
		Type type = typeof(TService);
		if (!_registerdServices.ContainsKey(scope))
		{
			CreateScope(scope);
		}
		TComponent instance = new GameObject(objectName, typeof(TComponent)).GetComponent<TComponent>();
		if (!_registerdServices[scope].ContainsKey(type))
		{
			_registerdServices[scope].Add(type, instance);
		}
		else
		{
			UnityEngine.Object oldInstace = (UnityEngine.Object)_registerdServices[scope][type];
			UnityEngine.Object.Destroy(oldInstace);
			_registerdServices[scope][type] = instance;
		}
		return instance;
	}

	public static void CreateScope(string scope)
	{
		if (!_registerdServices.ContainsKey(scope))
		{
			_registerdServices.Add(scope, new Dictionary<Type, object>());
		}
		else
		{
			Debug.LogError("Scope:" + scope + " already is exsits!");
		}
	}

	public static void RemoveScope(string scope)
	{
		if (_registerdServices.ContainsKey(scope))
		{
			_registerdServices.Remove(scope);
		}
		else
		{
			Debug.LogError("Scope:" + scope + " not exsits!");
		}
	}

	private static bool IsValidRegistration(Type serviceType, object serviceInstance)
	{
		if (serviceInstance == null)
		{
			Debug.LogError("Registering " + serviceType.Name + " in service locater is failed because service instance is null.");
			return false;
		}
		if (!serviceType.IsInterface)
		{
			Debug.LogError("Service type " + serviceType.Name + " is not an interface, you should register interface instead of concrete classes for service type argument.");
			return false;
		}
		bool isTypeCorrect = false;
		Type[] tmp = serviceInstance.GetType().GetInterfaces();
		int length = tmp.Length;
		for (int i = 0; i < length; i++)
		{
			if (tmp[i] == serviceType)
			{
				isTypeCorrect = true;
				break;
			}
		}
		if (!isTypeCorrect)
		{
			Debug.LogErrorFormat("Service instance({0}) is not implementing right interface ({1}).", serviceInstance.GetType().Name, serviceType.Name);
			return false;
		}
		return true;
	}
}
