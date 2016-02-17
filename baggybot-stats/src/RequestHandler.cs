using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using baggybot_stats.Database;
using baggybot_stats.Monitoring;
using Nancy;

namespace baggybot_stats
{
	public class RequestHandler : NancyModule
	{
		private static readonly SqlConnector conn;
		static RequestHandler()
		{
			Logger.Log("Creating DB connection");
			conn = new SqlConnector();
			//conn.OpenConnection("Server=localhost;Port=5432;Database=baggybot_irc;User Id=baggybot;Password=baggybot;");
			conn.OpenConnection("Server=hubble.jgeluk.net;Port=5432;Database=baggybot_irc;User Id=baggybot;Password=YQAgcS2QFES5ltn01pjF1rU!5;SSL Mode=Require;");
		}

		public RequestHandler()
		{
			Get["/"] = parameters => View["html-root/views/home.cshtml", conn];
		}
	}
}
