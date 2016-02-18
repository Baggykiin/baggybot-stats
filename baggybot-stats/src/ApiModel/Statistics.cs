using System.Collections.Generic;
using baggybot_stats.Database.Model;

namespace baggybot_stats.ApiModel
{
	public class StatisticsOverview
	{
		public string FeaturedQuote { get; set; }
		public IEnumerable<UserOverview> UserOverview { get; set; }
		public IEnumerable<UsedEmoticon> UsedEmoticons { get; set; }
		public IEnumerable<LinkedUrl> LinkedUrls { get; set; } 
	}
}
