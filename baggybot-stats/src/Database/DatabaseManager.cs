using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using baggybot_stats.ApiModel;
using baggybot_stats.Database.Model;

namespace baggybot_stats.Database
{
	public class DatabaseManager
	{
		private readonly SqlConnector conn = new SqlConnector();

		private Dictionary<int,int> activity = new Dictionary<int, int>();
		public IReadOnlyDictionary<int, int> Activity;
		public DatabaseManager()
		{
			Activity = new ReadOnlyDictionary<int, int>(activity);
		}

		public StatisticsOverview Stats
		{
			get
			{
				var orderedQuotes = (from quote in conn.Quotes
									 orderby quote.TakenAt descending
									 select quote);
				return new StatisticsOverview
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
					LinkedUrls = from url in conn.LinkedUrls
								 join user in conn.Users on url.LastUsedById equals user.Id
								 orderby url.Uses descending
								 select LinkedUrl.WithUser(url, user),
					UsedEmoticons = from emoticon in conn.Emoticons
									join user in conn.Users on emoticon.LastUsedById equals user.Id
									orderby emoticon.Uses descending
									select UsedEmoticon.WithUser(emoticon, user)
				};
			}
		}

		private void UpdateData()
		{
			foreach (var line in conn.IrcLog)
			{
				if (usageDict.ContainsKey(line.SentAt.TimeOfDay.Hours))
				{
					usageDict[line.SentAt.TimeOfDay.Hours]++;
				}
				else
				{
					usageDict[line.SentAt.TimeOfDay.Hours] = 1;
				}
			}
		}

		public void OpenConnection(string connectionString)
		{
			conn.OpenConnection(connectionString);
		}
	}
}
