using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FMG.Terminal.Runtime.Controlers;
using UnityEngine;

namespace FMG.Terminal.Runtime.Tools;

public class Factory<TObject> : IFactory<TObject> where TObject : class
{
	protected readonly string _suffixPhrase;

	protected readonly Dictionary<string, Type> _objects = new Dictionary<string, Type>();

	protected readonly List<Assembly> _assemblies = new List<Assembly>();

	public Factory(string suffixPhrase, params Assembly[] targetAsemblies)
	{
		_suffixPhrase = suffixPhrase;
		IEnumerable<Type> types = FetchTypes();
		PrepareDictionary(types);
	}

	public virtual TObject CreateInstance(string objectName)
	{
		if (!_objects.ContainsKey(objectName))
		{
			Debug.LogError("There is No object With Name : " + objectName);
			return null;
		}
		Type objectsType = _objects[objectName];
		return Activator.CreateInstance(objectsType) as TObject;
	}

	public virtual TObject CreateInstance(string objectName, params object[] args)
	{
		if (!_objects.ContainsKey(objectName))
		{
			Debug.LogError("There is No object With Name : " + objectName);
			return null;
		}
		Type objectsType = _objects[objectName];
		return Activator.CreateInstance(objectsType, args) as TObject;
	}

	private IEnumerable<Type> FetchTypes()
	{
		Type type = typeof(TObject);
		return from t in AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly x) => x.GetTypes())
			where t.IsClass && !t.IsAbstract
			where type.IsAssignableFrom(t)
			select t;
	}

	private void PrepareDictionary(IEnumerable<Type> types)
	{
		foreach (Type type in types)
		{
			string className = type.Name.Replace(_suffixPhrase, string.Empty);
			_objects.Add(className, type);
		}
	}
}
