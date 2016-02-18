﻿using LinqToDB.Mapping;

namespace baggybot_stats.Database.Model
{
	[Table(Name = "misc_data")]
	public class MiscData : Poco
	{
		[Column(Name = "id"), Identity, PrimaryKey]
		public int Id { get; set; }

		[Column(Name = "type")]
		public string Type { get; set; }

		[Column(Name = "enabled")]
		public bool Enabled { get; set; }

		[Column(Name = "key")]
		public string Key { get; set; }

		[Column(Name = "value")]
		public string Value { get; set; }
	}
}
