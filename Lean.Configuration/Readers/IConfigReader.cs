namespace Lean.Configuration
{
	/// <summary>
	/// Reads configuration values.
	/// </summary>
	public interface IConfigReader
	{
		/// <summary>
		/// Gets configuration value by specified name.
		/// Returns null if value does not exist.
		/// </summary>
		string GetValue(string name);
	}
}
