namespace FMG.Terminal.Runtime.Controlers;

public interface IFactory<TObject> where TObject : class
{
	TObject CreateInstance(string type);

	TObject CreateInstance(string type, params object[] args);
}
