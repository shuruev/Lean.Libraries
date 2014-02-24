using System.Data;

namespace Lean.Database
{
	/// <summary>
	/// Interface for data row for bulk operations.
	/// </summary>
	public interface IBulkRow
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
