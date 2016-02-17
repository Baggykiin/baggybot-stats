﻿using LinqToDB.Mapping;

namespace baggybot_stats.Database.Model
{
	[Table(Name = "irc_user")]
	public class User : Poco
	{
		[Column(Name = "id"), PrimaryKey, Identity]
		public int Id { get; set; }

		[Column(Name = "name"), NotNull]
		public string Name { get; set; }
	}
}
