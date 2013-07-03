using System.Data;

namespace Lean.Database
{
	/// <summary>
	/// Interface for data row.
	/// </summary>
	public interface IDataRow
	{
		/// <summary>
		/// Creates table for storing the rows.
		/// </summary>
		DataTable CreateTable();

		/// <summary>
		/// Adds current row to the specified table.
		/// </summary>
		void AddToTable(DataTable table);
	}
}
