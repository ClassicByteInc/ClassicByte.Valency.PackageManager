using ClassicByte.Valency.PackageManager.Core;

namespace ClassicByte.Valency.PackageManager
{
	internal class SourceManager
	{



		private const string DefaultSourceUrl = "";
		public List<Source> LocalSources
		{
			get
			{
				if (!UtilPath.SourceList.Exists)
				{
#if DEBUG
					return null;
#endif
				}
				return null;
			}
		}
	}
}
