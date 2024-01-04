using FMG.Security.Cryptography;
using Newtonsoft.Json;

namespace FMG.LocalStorage.Generic;

public class FileOptions
{
	public string FileName { get; set; } = null;


	public string Path { get; set; } = null;


	public bool HashFileName { get; set; }

	public JsonConverter[] JsonConverters { get; set; }

	public ICryptographer Cryptographer { get; set; }
}
