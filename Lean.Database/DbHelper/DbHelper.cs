using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;

namespace Lean.Database
{
	/// <summary>
	/// Provides access to the database.
	/// </summary>
	public abstract class DbHelper
	{
		/// <summary>
		/// Database connection string.
		/// </summary>
		protected readonly string m_connectionString;

		/// <summary>
		/// Database command timeout in seconds.
		/// </summary>
		protected readonly int m_commandTimeoutInSeconds;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		protected DbHelper(string connectionString, TimeSpan commandTimeout)
		{
			if (String.IsNullOrEmpty(connectionString))
				throw new ArgumentNullException("connectionString");
			if (commandTimeout <= TimeSpan.Zero)
				throw new ArgumentOutOfRangeException("commandTimeout");

			m_connectionString = connectionString;
			m_commandTimeoutInSeconds = Convert.ToInt32(commandTimeout.TotalSeconds);
		}

		#region Properties

		/// <summary>
		/// Gets connection string.
		/// </summary>
		public string ConnectionString
		{
			get { return m_connectionString; }
		}

		/// <summary>
		/// Gets command timeout in seconds.
		/// </summary>
		public int CommandTimeoutInSeconds
		{
			get { return m_commandTimeoutInSeconds; }
		}

		#endregion

		#region Connection

		/// <summary>
		/// Creates new connection.
		/// </summary>
		protected abstract DbConnection CreateConnection();

		/// <summary>
		/// Creates and opens new connection.
		/// </summary>
		public DbConnection OpenConnection()
		{
			var conn = CreateConnection();
			conn.Open();
			return conn;
		}

		#endregion

		#region Commands

		/// <summary>
		/// Chooses command type.
		/// </summary>
		protected virtual CommandType ChooseCommandType(string commandText)
		{
			if (commandText.Contains(" "))
				return CommandType.Text;

			return CommandType.StoredProcedure;
		}

		/// <summary>
		/// Creates command.
		/// </summary>
		private DbCommand CreateCommand(
			DbConnection connection,
			DbTransaction transaction,
			string commandText,
			params DbParameter[] parameters)
		{
			DbCommand cmd = connection.CreateCommand();
			cmd.Transaction = transaction;
			cmd.CommandType = ChooseCommandType(commandText);

			cmd.CommandTimeout = m_commandTimeoutInSeconds;
			cmd.CommandText = commandText;

			foreach (DbParameter parameter in parameters)
			{
				cmd.Parameters.Add(parameter);
			}

			return cmd;
		}

		/// <summary>
		/// Executes non-query command.
		/// </summary>
		public void ExecuteNonQuery(
			DbConnection connection,
			string commandText,
			params DbParameter[] parameters)
		{
			ExecuteNonQuery(
				connection,
				null,
				commandText,
				parameters);
		}

		/// <summary>
		/// Executes non-query command.
		/// </summary>
		public void ExecuteNonQuery(
			DbConnection connection,
			DbTransaction transaction,
			string commandText,
			params DbParameter[] parameters)
		{
			using (DbCommand cmd = CreateCommand(
				connection,
				transaction,
				commandText,
				parameters))
			{
				cmd.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// Executes scalar command.
		/// </summary>
		public object ExecuteScalar(
			DbConnection connection,
			string commandText,
			params DbParameter[] parameters)
		{
			return ExecuteScalar(
				connection,
				null,
				commandText,
				parameters);
		}

		/// <summary>
		/// Executes scalar command.
		/// </summary>
		public object ExecuteScalar(
			DbConnection connection,
			DbTransaction transaction,
			string commandText,
			params DbParameter[] parameters)
		{
			using (DbCommand cmd = CreateCommand(
				connection,
				transaction,
				commandText,
				parameters))
			{
				return cmd.ExecuteScalar();
			}
		}

		/// <summary>
		/// Executes reader command.
		/// </summary>
		public void ExecuteReader(
			DbConnection connection,
			string commandText,
			Action<IDataRecord> actionForReader,
			params DbParameter[] parameters)
		{
			ExecuteReader(
				connection,
				null,
				commandText,
				actionForReader,
				parameters);
		}

		/// <summary>
		/// Executes reader command.
		/// </summary>
		public void ExecuteReader(
			DbConnection connection,
			DbTransaction transaction,
			string commandText,
			Action<IDataRecord> actionForReader,
			params DbParameter[] parameters)
		{
			using (DbCommand cmd = CreateCommand(
				connection,
				transaction,
				commandText,
				parameters))
			{
				using (DbDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						actionForReader(reader);
					}
				}
			}
		}

		/// <summary>
		/// Executes reader command.
		/// </summary>
		public T ExecuteReader<T>(
			DbConnection connection,
			string commandText,
			Func<IDataRecord, T> returnFromReader,
			params DbParameter[] parameters)
		{
			return ExecuteReader(
				connection,
				null,
				commandText,
				returnFromReader,
				parameters);
		}

		/// <summary>
		/// Executes reader command.
		/// </summary>
		public T ExecuteReader<T>(
			DbConnection connection,
			DbTransaction transaction,
			string commandText,
			Func<IDataRecord, T> returnFromReader,
			params DbParameter[] parameters)
		{
			using (DbCommand cmd = CreateCommand(
				connection,
				transaction,
				commandText,
				parameters))
			{
				using (DbDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						return returnFromReader(reader);
					}
				}
			}

			return default(T);
		}

		#endregion

		#region Bulk copy

		/// <summary>
		/// Creates bulk table for data rows.
		/// </summary>
		public DataTable CreateBulkTable<T>(IEnumerable<T> rows) where T : IBulkRow, new()
		{
			T sample = new T();
			DataTable table = sample.CreateTable();
			table.Locale = CultureInfo.InvariantCulture;

			foreach (T row in rows)
			{
				row.AddToTable(table);
			}

			return table;
		}

		/// <summary>
		/// Creates bulk table with single column of simple type.
		/// </summary>
		public DataTable CreateBulkTable<T>(IEnumerable<T> values, string columnName)
		{
			DataTable table = new DataTable();
			table.Locale = CultureInfo.InvariantCulture;
			table.Columns.Add(columnName, typeof(T));

			foreach (T value in values)
			{
				table.Rows.Add(value);
			}

			return table;
		}

		/// <summary>
		/// Creates bulk table using more flexible syntax for describing columns and rows generation.
		/// </summary>
		public DataTable CreateBulkTable<T>(
			IEnumerable<T> values,
			Func<T, object[]> rowGenerator,
			params Tuple<string, Type>[] columns)
		{
			DataTable table = new DataTable();
			table.Locale = CultureInfo.InvariantCulture;

			foreach (var column in columns)
			{
				table.Columns.Add(column.Item1, column.Item2);
			}

			foreach (T value in values)
			{
				table.Rows.Add(rowGenerator.Invoke(value));
			}

			return table;
		}

		#endregion
	}
}
