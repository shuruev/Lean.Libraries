using System;
using System.Configuration;
using System.Globalization;

namespace Lean.Configuration
{
	/// <summary>
	/// Parses configuration settings.
	/// </summary>
	public static class ConfigParser
	{
		/// <summary>
		/// Checks whether specified configuration value is empty or absent.
		/// </summary>
		public static bool IsEmpty(this IConfigReader reader, string name)
		{
			string value = reader.GetValue(name);
			return value == null;
		}

		/// <summary>
		/// Gets configuration value of specified type.
		/// </summary>
		public static T Get<T>(this IConfigReader reader, string name, T defaultValue)
		{
			string value = reader.GetValue(name);
			if (value == null)
				return defaultValue;

			return Parse<T>(name, value);
		}

		/// <summary>
		/// Gets configuration value of specified type.
		/// </summary>
		public static T Get<T>(this IConfigReader reader, string name)
		{
			string value = reader.GetValue(name);
			if (value == null)
			{
				string message = String.Format("Configuration parameter '{0}' is not found.", name);
				throw new ConfigurationErrorsException(message);
			}

			return Parse<T>(name, value);
		}

		#region Parsing methods

		/// <summary>
		/// Parses configuration value of specified type.
		/// </summary>
		private static T Parse<T>(string name, string value)
		{
			if (value == null)
				throw new ArgumentNullException("value");

			try
			{
				if (typeof(T) == typeof(string))
				{
					return (T)(object)ParseString(value);
				}

				if (typeof(T) == typeof(bool))
				{
					return (T)(object)ParseBoolean(value);
				}

				if (typeof(T) == typeof(byte))
				{
					return (T)(object)ParseByte(value);
				}

				if (typeof(T) == typeof(short))
				{
					return (T)(object)ParseShort(value);
				}

				if (typeof(T) == typeof(int))
				{
					return (T)(object)ParseInteger(value);
				}

				if (typeof(T) == typeof(long))
				{
					return (T)(object)ParseLong(value);
				}

				if (typeof(T) == typeof(float))
				{
					return (T)(object)ParseFloat(value);
				}

				if (typeof(T) == typeof(double))
				{
					return (T)(object)ParseDouble(value);
				}

				if (typeof(T) == typeof(decimal))
				{
					return (T)(object)ParseDecimal(value);
				}

				if (typeof(T) == typeof(TimeSpan))
				{
					return (T)(object)ParseTimeSpan(value);
				}

				if (typeof(T) == typeof(DateTime))
				{
					return (T)(object)ParseDateTime(value);
				}

				if (typeof(T) == typeof(Guid))
				{
					return (T)(object)ParseGuid(value);
				}

				if (typeof(T).IsEnum)
				{
					return ParseEnumeration<T>(value);
				}
			}
			catch
			{
				throw new ConfigurationErrorsException(
					String.Format(
						"Configuration parameter '{0}' should contain value of type {1}.",
						name,
						typeof(T).Name));
			}

			throw new ConfigurationErrorsException(
				String.Format(
					"Cannot read configuration value of unknown type {0}.",
					typeof(T).Name));
		}

		/// <summary>
		/// Parses string configuration value.
		/// </summary>
		private static string ParseString(string value)
		{
			return value;
		}

		/// <summary>
		/// Parses boolean configuration value.
		/// </summary>
		private static bool ParseBoolean(string value)
		{
			return Boolean.Parse(value);
		}

		/// <summary>
		/// Parses byte configuration value.
		/// </summary>
		private static byte ParseByte(string value)
		{
			return Byte.Parse(value);
		}

		/// <summary>
		/// Parses short configuration value.
		/// </summary>
		private static short ParseShort(string value)
		{
			return Int16.Parse(value);
		}

		/// <summary>
		/// Parses integer configuration value.
		/// </summary>
		private static int ParseInteger(string value)
		{
			return Int32.Parse(value);
		}

		/// <summary>
		/// Parses long configuration value.
		/// </summary>
		private static long ParseLong(string value)
		{
			return Int64.Parse(value);
		}

		/// <summary>
		/// Parses float configuration value.
		/// </summary>
		private static float ParseFloat(string value)
		{
			return Single.Parse(value);
		}

		/// <summary>
		/// Parses double configuration value.
		/// </summary>
		private static double ParseDouble(string value)
		{
			return Double.Parse(value);
		}

		/// <summary>
		/// Parses decimal configuration value.
		/// </summary>
		private static decimal ParseDecimal(string value)
		{
			return Decimal.Parse(value);
		}

		/// <summary>
		/// Parses time span configuration value.
		/// </summary>
		private static TimeSpan ParseTimeSpan(string value)
		{
			return TimeSpan.Parse(value);
		}

		/// <summary>
		/// Parses date/time configuration value.
		/// </summary>
		private static DateTime ParseDateTime(string value)
		{
			return DateTime.Parse(value, DateTimeFormatInfo.InvariantInfo);
		}

		/// <summary>
		/// Parses GUID configuration value.
		/// </summary>
		private static Guid ParseGuid(string value)
		{
			return new Guid(value);
		}

		/// <summary>
		/// Parses enumeration value.
		/// </summary>
		private static T ParseEnumeration<T>(string value)
		{
			return (T)Enum.Parse(typeof(T), value, true);
		}

		#endregion
	}
}
