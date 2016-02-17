using System.Collections.Generic;
using Nancy.ViewEngines.Razor;

namespace baggybot_stats
{
	public class RazorConfiguration : IRazorConfiguration
	{
		public IEnumerable<string> GetAssemblyNames()
		{
			yield return "linq2db, Version=1.0.7.3, Culture=neutral, PublicKeyToken=f19f8aed7feff67e";
			yield return "Npgsql";
			/*yield return "baggybot_stats";*/
		}

		public IEnumerable<string> GetDefaultNamespaces()
		{
			return null;
			/*yield return "baggybot_stats";
			yield return "baggybot_stats.Database";
			yield return "baggybot_stats.Database.Models";*/
		}

		public bool AutoIncludeModelNamespace => true;
	}
}
