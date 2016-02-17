using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using baggybot_stats.Database;
using baggybot_stats.Monitoring;
using Nancy;
using Nancy.Hosting.Self;

namespace baggybot_stats
{
	class Program
	{
		static void Main(string[] args)
		{
			StaticConfiguration.DisableErrorTraces = false;
			Logger.Log("Starting BaggyBot stats page server");
			Logger.Log("Creating host");
			var host = new NancyHost(new Uri("http://127.0.0.1:57475"));
			Logger.Log("Starting host");
			host.Start();
			Logger.Log("Server is ready");
			Console.ReadKey();
		}
	}
}
