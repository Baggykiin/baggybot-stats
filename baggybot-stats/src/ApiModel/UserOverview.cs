namespace baggybot_stats.ApiModel
{
	public class UserOverview
	{
		public string Username { get; set; }
		public int Lines { get; set; }
		public int Words { get; set; }
		public double WordsPerLine { get; set; }
		public int Actions { get; set; }
		public int Profanities { get; set; }
		public string RandomQuote { get; set; }
	}
}
