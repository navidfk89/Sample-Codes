using System.Collections.Generic;
using FMG.Security.Cryptography;
using Newtonsoft.Json;

namespace FMG.LocalStorage.Generic;

public class FileOptionsBuilder
{
	public FileOptions Options { get; private set; } = new FileOptions();


	public string FileName
	{
		set
		{
			Options.FileName = value;
		}
	}

	public string Path
	{
		set
		{
			Options.Path = value;
		}
	}

	public void AddCustomJsonSerializers(params JsonConverter[] jsonConverters)
	{
		Options.JsonConverters = jsonConverters;
	}

	public void UseSecurity(ICryptographer cryptographer = null)
	{
		Options.Cryptographer = cryptographer ?? Cryptographer.Create();
	}

	public void HashFileName()
	{
		Options.HashFileName = true;
	}
}
public class FileOptionsBuilder<T>
{
	private ICryptographer _cryptographer;

	private JsonConverter[] _jsonConverters;

	private bool _hashFileName;

	public LocalDBOptions<T> Options { get; private set; } = new LocalDBOptions<T>();


	public string DBName { get; set; }

	public string Path { get; set; }

	public void UseCustomFileHandler(IFileHandler<List<T>> fileHandler)
	{
		if (!string.IsNullOrEmpty(DBName))
		{
			fileHandler.Options.FileName = DBName;
		}
		if (!string.IsNullOrEmpty(Path))
		{
			fileHandler.Options.Path = Path;
		}
		if (_cryptographer != null)
		{
			fileHandler.Options.Cryptographer = _cryptographer;
		}
		if (_jsonConverters != null)
		{
			fileHandler.Options.JsonConverters = _jsonConverters;
		}
		fileHandler.Options.HashFileName = _hashFileName;
		Options.FileHandler = fileHandler;
	}

	public void UseDefaultFileHandler()
	{
		Options.FileHandler = new FileHandler<List<T>>(delegate(FileOptionsBuilder setup)
		{
			if (!string.IsNullOrEmpty(DBName))
			{
				setup.Options.FileName = DBName;
			}
			if (!string.IsNullOrEmpty(Path))
			{
				setup.Options.Path = Path;
			}
			if (_cryptographer != null)
			{
				setup.UseSecurity(_cryptographer);
			}
			if (_jsonConverters != null)
			{
				setup.AddCustomJsonSerializers(_jsonConverters);
			}
			if (_hashFileName)
			{
				setup.HashFileName();
			}
		});
	}

	public void UseSecurity(ICryptographer cryptographer = null)
	{
		_cryptographer = cryptographer ?? Cryptographer.Create();
		if (Options.FileHandler != null)
		{
			Options.FileHandler.Options.Cryptographer = _cryptographer;
		}
	}

	public void AddCustomJsonSerializers(params JsonConverter[] jsonConverters)
	{
		_jsonConverters = jsonConverters;
		if (Options.FileHandler != null)
		{
			Options.FileHandler.Options.JsonConverters = jsonConverters;
		}
	}

	public void HashFileName()
	{
		_hashFileName = true;
	}
}
