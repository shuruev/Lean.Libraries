using System.Configuration;

namespace Lean.Configuration
{
	/// <summary>
	/// Reads configuration values from application configuration.
	/// </summary>
	public class AppConfigReader : IConfigReader
	{
		/// <summary>
		/// Gets configuration value by specified name.
		/// Returns null if value does not exist.
		/// </summary>
		public string GetValue(string name)
		{
			return ConfigurationManager.AppSettings[name];
		}
	}
}
