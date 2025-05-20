namespace ClassicByte.Valency.PackageManager;

public class Program
{
	static void Main(string[] args)
	{
		new MainCore([.. args]).Run();
	}
}