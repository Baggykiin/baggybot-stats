﻿using LinqToDB.Mapping;

namespace baggybot_stats.Database.Model
{
	[Table(Name = "key_value_pair")]
	public class KeyValuePair : Poco
	{
		[Column(Name = "id"), PrimaryKey, Identity]
		public int Id { get; set; }

		[Column(Name = "key"), NotNull]
		public string Key { get; set; }

		[Column(Name = "value"), NotNull]
		public int Value { get; set; }
	}
}
