using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FMG.Security.Hash;
using UnityEngine;

namespace FMG.LocalStorage;

public class FileHandler : IFileHandler
{
	public virtual string FileName { get; protected set; }

	public virtual string FilePath { get; protected set; }

	public FileOptions Options { get; }

	public FileHandler(string fileName, Action<FileOptionsBuilder> setupAction = null)
	{
		FileOptionsBuilder optionsBuilder = new FileOptionsBuilder();
		setupAction?.Invoke(optionsBuilder);
		Options = optionsBuilder.Options;
		SetFileName(fileName);
		SetFilePath();
	}

	public virtual string Load()
	{
		string rawData = null;
		try
		{
			CheckOrCreateFile();
			using StreamReader reader = new StreamReader(FilePath);
			rawData = reader.ReadToEnd();
			if (string.IsNullOrEmpty(rawData))
			{
				return null;
			}
			if (Options.Cryptographer != null)
			{
				rawData = Options.Cryptographer.Decrypt(rawData);
			}
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
		}
		return rawData;
	}

	public virtual byte[] LoadBinary()
	{
		string content = Load();
		return Encoding.UTF8.GetBytes(content);
	}

	public virtual async Task<string> LoadAsync()
	{
		string rawData = null;
		try
		{
			CheckOrCreateFile();
			using StreamReader reader = new StreamReader(FilePath);
			rawData = await reader.ReadToEndAsync();
			if (string.IsNullOrEmpty(rawData))
			{
				return null;
			}
			if (Options.Cryptographer != null)
			{
				rawData = Options.Cryptographer.Decrypt(rawData);
			}
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
			await Task.CompletedTask;
		}
		return rawData;
	}

	public virtual async Task<byte[]> LoadBinaryAsync()
	{
		string content = await LoadAsync();
		return Encoding.UTF8.GetBytes(content);
	}

	public virtual bool Save(string content)
	{
		try
		{
			CheckOrCreateFile();
			using (StreamWriter writer = new StreamWriter(FilePath, append: false))
			{
				if (Options.Cryptographer != null)
				{
					content = Options.Cryptographer.Encrypt(content);
				}
				writer.Write(content);
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

	public virtual bool Save(byte[] data)
	{
		string stringData = Encoding.UTF8.GetString(data);
		return Save(stringData);
	}

	public virtual async Task<bool> SaveAsync(string content)
	{
		try
		{
			CheckOrCreateFile();
			using (StreamWriter writer = new StreamWriter(FilePath, append: false))
			{
				if (Options.Cryptographer != null)
				{
					content = Options.Cryptographer.Encrypt(content);
				}
				await writer.WriteAsync(content);
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

	public virtual async Task<bool> SaveAsynce(byte[] data)
	{
		string stringData = Encoding.UTF8.GetString(data);
		return await SaveAsync(stringData);
	}

	protected virtual void SetFileName(string fileName)
	{
		FileName = fileName;
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
		using (File.Create(FilePath))
		{
		}
	}
}
