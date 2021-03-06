﻿namespace Lean.Configuration.Tests
{
	/// <summary>
	/// Implementation used for testing purposes.
	/// </summary>
	public class FakeConfigReader : IConfigReader
	{
		/// <summary>
		/// Gets configuration value by specified name.
		/// Returns null if value does not exist.
		/// </summary>
		public string GetValue(string name)
		{
			return name;
		}
	}
}
