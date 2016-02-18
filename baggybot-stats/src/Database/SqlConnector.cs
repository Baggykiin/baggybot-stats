using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using baggybot_stats.Database.Model;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.PostgreSQL;

namespace baggybot_stats.Database
{
	public class SqlConnector : IDisposable
	{
		private DataConnection connection;

		public ITable<LinkedUrl> LinkedUrls { get; private set; }
		public ITable<User> Users { get; private set; }
		public ITable<UserCredential> UserCredentials { get; private set; }
		public ITable<UserStatistic> UserStatistics { get; private set; }
		public ITable<UsedEmoticon> Emoticons { get; private set; }
		public ITable<IrcLog> IrcLog { get; private set; }
		public ITable<KeyValuePair> KeyValuePairs { get; private set; }
		public ITable<Quote> Quotes { get; private set; }
		public ITable<UsedWord> Words { get; private set; }
		public ITable<MiscData> MiscData { get; private set; }

		private ITable<Metadata> metadata;

		private ConnectionState internalState;

		public ConnectionState ConnectionState
		{
			get
			{
				if (connection == null)
				{
					return ConnectionState.Closed;
				}
				else
				{
					return internalState;
				}
			}
		}

		public void Insert<T>(T row)
		{
			connection.Insert(row);
		}

		private void HandleConnectionFailure()
		{
			internalState = ConnectionState.Closed;
		}

		public bool OpenConnection(string connectionString)
		{
			if (string.IsNullOrWhiteSpace(connectionString))
			{
				return false;
			}
			internalState = ConnectionState.Connecting;
			connection = PostgreSQLTools.CreateDataConnection(connectionString);
			internalState = ConnectionState.Open;
			connection.OnClosing += (sender, args) => internalState = ConnectionState.Closed;
			connection.OnClosed += (sender, args) => internalState = ConnectionState.Closed;
			metadata = connection.GetTable<Metadata>();
			UserCredentials = connection.GetTable<UserCredential>();
			Quotes = connection.GetTable<Quote>();
			UserStatistics = connection.GetTable<UserStatistic>();
			Emoticons = connection.GetTable<UsedEmoticon>();
			KeyValuePairs = connection.GetTable<KeyValuePair>();
			LinkedUrls = connection.GetTable<LinkedUrl>();
			Users = connection.GetTable<User>();
			Words = connection.GetTable<UsedWord>();
			IrcLog = connection.GetTable<IrcLog>();
			MiscData = connection.GetTable<MiscData>();

			return true;
		}

		public bool CloseConnection()
		{
			connection.Close();
			return true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool cleanAll)
		{
			connection?.Dispose();
		}

		~SqlConnector()
		{
			Dispose(false);
		}

		internal int ExecuteStatement(string statement)
		{
			var cmd = connection.CreateCommand();
			cmd.CommandText = statement;
			return cmd.ExecuteNonQuery();
		}

		internal List<object[]> ExecuteQuery(string query)
		{
			var data = new List<object[]>();
			using (var cmd = connection.CreateCommand())
			{
				cmd.CommandText = query;
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						var row = new object[reader.FieldCount];
						for (var i = 0; i < reader.FieldCount; i++)
						{
							row[i] = reader[i];
						}
						data.Add(row);
					}
				}
			}
			return data;
		}

		public void Update<T>(T match) where T:Poco
		{
			connection.Update(match);
		}
	}
}
