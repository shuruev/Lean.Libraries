using Microsoft.WindowsAzure.ServiceRuntime;

namespace Lean.Configuration
{
	/// <summary>
	/// Reads configuration values both from Windows Azure configuration
	/// and standard application configuration as a fallback.
	/// </summary>
	public class AzureConfigReader : AppConfigReader
	{
		/// <summary>
		/// Gets configuration value by specified name.
		/// Returns null if value does not exist.
		/// </summary>
		public override string GetValue(string name)
		{
			string value = null;

			if (RoleEnvironment.IsAvailable)
			{
				try
				{
					value = RoleEnvironment.GetConfigurationSettingValue(name);
				}
				catch
				{
				}
			}

			return value ?? base.GetValue(name);
		}
	}
}
