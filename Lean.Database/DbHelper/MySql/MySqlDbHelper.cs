using System;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace Lean.Database
{
	/// <summary>
	/// Provides access to the MySQL database.
	/// </summary>
	public class MySqlDbHelper : DbHelper
	{
		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public MySqlDbHelper(string connectionString, TimeSpan commandTimeout)
			: base(connectionString, commandTimeout)
		{
		}

		#region Connection

		/// <summary>
		/// Creates new connection.
		/// </summary>
		protected override DbConnection CreateConnection()
		{
			return new MySqlConnection(m_connectionString);
		}

		#endregion
	}
}
