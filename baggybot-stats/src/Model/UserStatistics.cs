using System.Collections.Generic;
using baggybot_stats.Database.Model;

namespace baggybot_stats.Model
{
	public class UserStatistics
	{
		public List<UserStatistic> Statistics { get; private set; }

		public UserStatistics(List<UserStatistic> statistics)
		{
			Statistics = statistics;
		}
	}
}
