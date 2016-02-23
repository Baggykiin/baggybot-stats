namespace baggybot_stats.Configuration
{
	public class Configuration
	{
		public string ConnectionString { get; set; }
		public string[] ListenUrls { get; set; } = new string[0];
	}
}