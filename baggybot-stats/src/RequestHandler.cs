using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using baggybot_stats.ApiModel;
using baggybot_stats.Database;
using baggybot_stats.Database.Model;
using baggybot_stats.Monitoring;
using LinqToDB;
using LinqToDB.SchemaProvider;
using Nancy;

namespace baggybot_stats
{
	public class RequestHandler : NancyModule
	{
		public static List<string> ValidRequestTokens = new List<string>();

		private static readonly SqlConnector conn;
		static RequestHandler()
		{
			Logger.Log("Creating DB connection");
			conn = new SqlConnector();
			//conn.OpenConnection("Server=localhost;Port=5432;Database=baggybot_irc;User Id=baggybot;Password=baggybot;");
			conn.OpenConnection("Server=hubble.jgeluk.net;Port=5432;Database=baggybot_irc;User Id=baggybot;Password=YQAgcS2QFES5ltn01pjF1rU!5;SSL Mode=Require;");
			ValidRequestTokens.Add("test");
		}

		private static string GenerateToken()
		{
			var token = Guid.NewGuid().ToString();
			lock (ValidRequestTokens)
			{
				ValidRequestTokens.Add(token);
			}
			return token;
		}

		private bool CheckToken(string token)
		{
			if (ValidRequestTokens.Contains(token))
			{
				lock (ValidRequestTokens)
				{
					ValidRequestTokens.Remove(token);
				}
				return true;
			}
			else
			{
				return false;
			}
		}

		public RequestHandler()
		{
			Get["/"] = parameters => View["html-root/views/home.cshtml", new {token = GenerateToken()}];

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
				else if (!CheckToken(token))
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
										 orderby emoticon.Uses descending
										 select emoticon)
					}
				});
			};
		}
	}
}
