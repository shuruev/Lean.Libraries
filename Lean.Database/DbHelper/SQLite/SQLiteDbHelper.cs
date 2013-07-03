using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace Lean.Database
{
	/// <summary>
	/// Provides access to the SQLite database.
	/// </summary>
	public class SQLiteDbHelper : DbHelper
	{
		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public SQLiteDbHelper(string connectionString, TimeSpan commandTimeout)
			: base(connectionString, commandTimeout)
		{
		}

		#region Connection

		/// <summary>
		/// Creates new connection.
		/// </summary>
		protected override DbConnection CreateConnection()
		{
			return new SQLiteConnection(m_connectionString);
		}

		#endregion

		#region Commands

		/// <summary>
		/// Chooses command type.
		/// </summary>
		protected override CommandType ChooseCommandType(string commandText)
		{
			return CommandType.Text;
		}

		#endregion
	}
}
