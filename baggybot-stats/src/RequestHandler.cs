using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using baggybot_stats.ApiModel;
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
		private static readonly SqlConnector conn;
		static RequestHandler()
		{
			Logger.Log("Creating DB connection");
			conn = new SqlConnector();
			//conn.OpenConnection("Server=localhost;Port=5432;Database=baggybot_irc;User Id=baggybot;Password=baggybot;");
			conn.OpenConnection("Server=hubble.jgeluk.net;Port=5432;Database=baggybot_irc;User Id=baggybot;Password=YQAgcS2QFES5ltn01pjF1rU!5;SSL Mode=Require;");
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
				Logger.Log($"{DateTime.Now} {Request.UserHostAddress}: GET / -- {Request.Headers.Referrer} -- {Request.Headers.UserAgent}", LogLevel.Irc);
				return View["home.cshtml", new {token = GenerateToken(Session)}];
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
				var orderedQuotes = (from quote in conn.Quotes
									 orderby quote.TakenAt descending
									 select quote);
				return Response.AsJson(new ApiResponse
				{
					Success = true,
					Data = new StatisticsOverview
					{
						FeaturedQuote = null,
						UserOverview = (from stat in conn.UserStatistics
										join user in conn.Users on stat.UserId equals user.Id
										orderby stat.Lines descending
										select new UserOverview
										{
											Actions = stat.Actions,
											Lines = stat.Lines,
											Profanities = stat.Profanities,
											Words = stat.Words,
											Username = user.Name,
											WordsPerLine = stat.Words / (double)stat.Lines,
											RandomQuote = (from quote in orderedQuotes
														   where quote.AuthorId == stat.UserId
														   select quote.Text).First()
										}),
						LinkedUrls = (from url in conn.LinkedUrls
									  join user in conn.Users on url.LastUsedById equals user.Id
									  select LinkedUrl.WithUser(url, user)),
						UsedEmoticons = (from emoticon in conn.Emoticons
										 join user in conn.Users on emoticon.LastUsedById equals user.Id
										 orderby emoticon.Uses descending
										 select UsedEmoticon.WithUser(emoticon, user))
					}
				});
			};
		}
	}
}
