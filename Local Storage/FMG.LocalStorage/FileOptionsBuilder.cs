using FMG.Security.Cryptography;

namespace FMG.LocalStorage;

public class FileOptionsBuilder
{
	public FileOptions Options { get; private set; } = new FileOptions();


	public string Path
	{
		set
		{
			Options.Path = value;
		}
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
