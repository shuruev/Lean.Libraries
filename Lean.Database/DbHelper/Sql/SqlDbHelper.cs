using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Lean.Database
{
	/// <summary>
	/// Provides access to the Microsoft SQL database.
	/// </summary>
	public class SqlDbHelper : DbHelper
	{
		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public SqlDbHelper(string connectionString, TimeSpan commandTimeout)
			: base(connectionString, commandTimeout)
		{
		}

		#region Connection

		/// <summary>
		/// Creates new connection.
		/// </summary>
		protected override DbConnection CreateConnection()
		{
			return new SqlConnection(m_connectionString);
		}

		#endregion

		#region Bulk copy

		/// <summary>
		/// Executes bulk copy command.
		/// </summary>
		public void ExecuteBulkCopy(
			SqlConnection connection,
			string destinationTableName,
			DataTable table,
			params SqlBulkCopyColumnMapping[] mappings)
		{
			ExecuteBulkCopy(
				connection,
				null,
				destinationTableName,
				table,
				mappings);
		}

		/// <summary>
		/// Executes bulk copy command.
		/// </summary>
		public void ExecuteBulkCopy(
			SqlConnection connection,
			SqlTransaction transaction,
			string destinationTableName,
			DataTable table,
			params SqlBulkCopyColumnMapping[] mappings)
		{
			using (SqlBulkCopy bulk = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
			{
				bulk.BulkCopyTimeout = m_commandTimeoutInSeconds;
				bulk.DestinationTableName = destinationTableName;

				foreach (SqlBulkCopyColumnMapping mapping in mappings)
				{
					bulk.ColumnMappings.Add(mapping);
				}

				tbulk.WriteToServer(table);
			}
		}

		#endregion
	}
}
