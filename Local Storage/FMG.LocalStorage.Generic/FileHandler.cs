using System;
using System.IO;
using System.Threading.Tasks;
using FMG.Security.Hash;
using Newtonsoft.Json;
using UnityEngine;

namespace FMG.LocalStorage.Generic;

public class FileHandler<T> : IFileHandler<T>
{
	public virtual string FileName { get; protected set; }

	public virtual string FilePath { get; protected set; }

	public FileOptions Options { get; }

	public FileHandler(Action<FileOptionsBuilder> setupAction = null)
	{
		FileOptionsBuilder optionsBuilder = new FileOptionsBuilder();
		setupAction?.Invoke(optionsBuilder);
		Options = optionsBuilder.Options;
		SetFileName();
		SetFilePath();
	}

	public T Load()
	{
		T data = default(T);
		try
		{
			CheckOrCreateFile();
			using StreamReader reader = new StreamReader(FilePath);
			string serializedData = reader.ReadToEnd();
			if (serializedData == string.Empty)
			{
				return data;
			}
			Debug.Log("Load File " + FileName + " : \n  " + serializedData);
			data = Deserilize(serializedData);
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
		}
		return data;
	}

	public bool Save(T content)
	{
		try
		{
			CheckOrCreateFile();
			using (StreamWriter writer = new StreamWriter(FilePath, append: false))
			{
				string serializedData = Serialize(content);
				writer.Write(serializedData);
				writer.Flush();
			}
			return true;
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
			return false;
		}
	}

	public async Task<T> LoadAsync()
	{
		T data = default(T);
		try
		{
			CheckOrCreateFile();
			using StreamReader reader = new StreamReader(FilePath);
			string serializedData = await reader.ReadToEndAsync();
			if (serializedData == string.Empty)
			{
				return data;
			}
			data = Deserilize(serializedData);
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
			await Task.CompletedTask;
		}
		return data;
	}

	public async Task<bool> SaveAsync(T content)
	{
		try
		{
			CheckOrCreateFile();
			using (StreamWriter writer = new StreamWriter(FilePath, append: false))
			{
				string serializedData = Serialize(content);
				await writer.WriteAsync(serializedData);
				await writer.FlushAsync();
			}
			return true;
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
			await Task.CompletedTask;
			return false;
		}
	}

	protected virtual void SetFileName()
	{
		FileName = Options.FileName;
		if (string.IsNullOrEmpty(Options.FileName))
		{
			FileName = typeof(T).ToString();
		}
		if (Options.HashFileName)
		{
			FileName = FastHash.CalculateHash(FileName.GetBytes()).ToString();
		}
	}

	protected virtual void SetFilePath()
	{
		FilePath = Options.Path;
		if (string.IsNullOrEmpty(FilePath))
		{
			FilePath = Path.Combine(Application.persistentDataPath, FileName);
		}
	}

	protected void CheckOrCreateFile()
	{
		if (File.Exists(FilePath))
		{
			return;
		}
		CheckDirectoryExists();
		using (File.Create(FilePath))
		{
		}
	}

	private void CheckDirectoryExists()
	{
		string directoryPath = Path.GetDirectoryName(FilePath);
		if (directoryPath == null)
		{
			throw new Exception("Directory Path Could Not Find in " + FilePath);
		}
		if (!Directory.Exists(directoryPath))
		{
			Directory.CreateDirectory(directoryPath);
		}
	}

	protected virtual T Deserilize(string serializedData)
	{
		if (string.IsNullOrEmpty(serializedData))
		{
			return default(T);
		}
		if (Options.Cryptographer != null)
		{
			serializedData = Options.Cryptographer.Decrypt(serializedData);
		}
		if (Options.JsonConverters == null)
		{
			return JsonConvert.DeserializeObject<T>(serializedData);
		}
		return JsonConvert.DeserializeObject<T>(serializedData, Options.JsonConverters);
	}

	protected virtual string Serialize(object objectToSerialize)
	{
		string serializedData = JsonConvert.SerializeObject(objectToSerialize, Formatting.Indented);
		if (Options.Cryptographer != null)
		{
			serializedData = Options.Cryptographer.Encrypt(serializedData);
		}
		return serializedData;
	}
}
