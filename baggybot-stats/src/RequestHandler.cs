using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using baggybot_stats.ApiModel;
using baggybot_stats.Configuration;
using baggybot_stats.Database;
using baggybot_stats.Database.Model;
using baggybot_stats.Monitoring;
using LinqToDB;
using LinqToDB.SchemaProvider;
using Nancy;
using Nancy.Session;

namespace baggybot_stats
{
	public class RequestHandler : NancyModule
	{
		private static readonly DatabaseManager dbMgr;
		static RequestHandler()
		{
			Logger.Log("Creating DB connection");

			//conn.OpenConnection("Server=localhost;Port=5432;Database=baggybot_irc;User Id=baggybot;Password=baggybot;");
			dbMgr.OpenConnection(ConfigManager.Config.ConnectionString);
		}

		private static string GenerateToken(ISession session)
		{
			var token = Guid.NewGuid().ToString();
			session["token"] = token;
			return token;
		}

		public RequestHandler()
		{
			Get["/"] = parameters =>
			{
				var token = GenerateToken(Session);
				Logger.Log($"{DateTime.Now} {Request.UserHostAddress}: GET / -- {Request.Headers.Referrer} -- {Request.Headers.UserAgent} -- token: {token}", LogLevel.Debug);
				return View["home.cshtml", new { token = token }];
			};
			Get["/api/stats"] = parameters =>
			{
				var token = (string)Request.Query.token;
				
				if (string.IsNullOrWhiteSpace(token))
				{
					return Response.AsJson(new ApiResponse
					{
						Success = false,
						Data = Error.AuthenticationRequired
					});
				}
				if ((string)Session["token"] != token)
				{
					return Response.AsJson(new ApiResponse
					{
						Success = false,
						Data = Error.InvalidRequestToken
					});
				}
				return Response.AsJson(new ApiResponse
				{
					Success = true,
					Data = erro 
				});
			};
		}
	}
}
