﻿using System;
using LinqToDB.Mapping;

namespace baggybot_stats.Database.Model
{
	[Table(Name = "irc_log")]
	public class IrcLog : Poco
	{
		// TODO: Add an IsAction boolean field
		[Column(Name = "id"), PrimaryKey, Identity]
		public int Id { get; set; }

		[Column(Name = "sent_at"), NotNull]
		public DateTime SentAt { get; set; }

		[Column(Name = "sender"), NotNull]
		public int? SenderId { get; set; }
		public User Sender { get; set; }

		[Column(Name = "channel"), NotNull]
		public string Channel { get; set; }

		[Column(Name = "nick"), NotNull]
		public string Nick { get; set; }

		[Column(Name = "message"), NotNull]
		public string Message { get; set; }

		public override string ToString()
		{
			return $"[{Channel}] <{Nick}> {Message}";
        }
	}
}
