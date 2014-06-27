using System;
using System.Data;

namespace Lean.Database
{
	/// <summary>
	/// Provides extension methods for reading standard types from data record.
	/// </summary>
	public static class ReadExtension
	{
		/// <summary>
		/// Reads non-nullable Guid.
		/// </summary>
		public static Guid ReadGuid(this IDataRecord reader, string columnName)
		{
			object value = reader[columnName];
			return (Guid)value;
		}

		/// <summary>
		/// Reads nullable Guid.
		/// </summary>
		public static Guid? ReadGuidOrNull(this IDataRecord reader, string columnName)
		{
			object value = reader[columnName];
			return value == DBNull.Value ? null : (Guid?)value;
		}

		/// <summary>
		/// Reads non-nullable String.
		/// </summary>
		public static string ReadString(this IDataRecord reader, string columnName)
		{
			object value = reader[columnName];
			return (string)value;
		}

		/// <summary>
		/// Reads nullable String.
		/// </summary>
		public static string ReadStringOrNull(this IDataRecord reader, string columnName)
		{
			object value = reader[columnName];
			return value == DBNull.Value ? null : (string)value;
		}

		/// <summary>
		/// Reads non-nullable Boolean.
		/// </summary>
		public static bool ReadBoolean(this IDataRecord reader, string columnName)
		{
			object value = reader[columnName];
			return (bool)value;
		}

		/// <summary>
		/// Reads nullable Boolean.
		/// </summary>
		public static bool? ReadBooleanOrNull(this IDataRecord reader, string columnName)
		{
			object value = reader[columnName];
			return value == DBNull.Value ? null : (bool?)value;
		}

		/// <summary>
		/// Reads non-nullable DateTime.
		/// </summary>
		public static DateTime ReadDateTime(this IDataRecord reader, string columnName)
		{
			object value = reader[columnName];
			return (DateTime)value;
		}

		/// <summary>
		/// Reads nullable DateTime.
		/// </summary>
		public static DateTime? ReadDateTimeOrNull(this IDataRecord reader, string columnName)
		{
			object value = reader[columnName];
			return value == DBNull.Value ? null : (DateTime?)value;
		}

		/// <summary>
		/// Reads non-nullable Byte.
		/// </summary>
		public static byte ReadByte(this IDataRecord reader, string columnName)
		{
			object value = reader[columnName];
			return (byte)value;
		}

		/// <summary>
		/// Reads nullable Byte.
		/// </summary>
		public static byte? ReadByteOrNull(this IDataRecord reader, string columnName)
		{
			object value = reader[columnName];
			return value == DBNull.Value ? null : (byte?)value;
		}

		/// <summary>
		/// Reads non-nullable Byte[].
		/// </summary>
		public static byte[] ReadBinary(this IDataRecord reader, string columnName)
		{
			object value = reader[columnName];
			return (byte[])value;
		}

		/// <summary>
		/// Reads nullable Byte[].
		/// </summary>
		public static byte[] ReadBinaryOrNull(this IDataRecord reader, string columnName)
		{
			object value = reader[columnName];
			return value == DBNull.Value ? null : (byte[])value;
		}

		/// <summary>
		/// Reads non-nullable Int16.
		/// </summary>
		public static short ReadInt16(this IDataRecord reader, string columnName)
		{
			object value = reader[columnName];
			return (short)value;
		}

		/// <summary>
		/// Reads nullable Int16.
		/// </summary>
		public static short? ReadInt16OrNull(this IDataRecord reader, string columnName)
		{
			object value = reader[columnName];
			return value == DBNull.Value ? null : (short?)value;
		}

		/// <summary>
		/// Reads non-nullable Int32.
		/// </summary>
		public static int ReadInt32(this IDataRecord reader, string columnName)
		{
			object value = reader[columnName];
			return (int)value;
		}

		/// <summary>
		/// Reads nullable Int32.
		/// </summary>
		public static int? ReadInt32OrNull(this IDataRecord reader, string columnName)
		{
			object value = reader[columnName];
			return value == DBNull.Value ? null : (int?)value;
		}

		/// <summary>
		/// Reads non-nullable Int64.
		/// </summary>
		public static long ReadInt64(this IDataRecord reader, string columnName)
		{
			object value = reader[columnName];
			return (long)value;
		}

		/// <summary>
		/// Reads nullable Int64.
		/// </summary>
		public static long? ReadInt64OrNull(this IDataRecord reader, string columnName)
		{
			object value = reader[columnName];
			return value == DBNull.Value ? null : (long?)value;
		}

		/// <summary>
		/// Reads non-nullable Single.
		/// </summary>
		public static float ReadSingle(this IDataRecord reader, string columnName)
		{
			object value = reader[columnName];
			return (float)value;
		}

		/// <summary>
		/// Reads nullable Single.
		/// </summary>
		public static float? ReadSingleOrNull(this IDataRecord reader, string columnName)
		{
			object value = reader[columnName];
			return value == DBNull.Value ? null : (float?)value;
		}

		/// <summary>
		/// Reads non-nullable Double.
		/// </summary>
		public static double ReadDouble(this IDataRecord reader, string columnName)
		{
			object value = reader[columnName];
			return (double)value;
		}

		/// <summary>
		/// Reads nullable Double.
		/// </summary>
		public static double? ReadDoubleOrNull(this IDataRecord reader, string columnName)
		{
			object value = reader[columnName];
			return value == DBNull.Value ? null : (double?)value;
		}

		/// <summary>
		/// Reads non-nullable Decimal.
		/// </summary>
		public static decimal ReadDecimal(this IDataRecord reader, string columnName)
		{
			object value = reader[columnName];
			return (decimal)value;
		}

		/// <summary>
		/// Reads nullable Decimal.
		/// </summary>
		public static decimal? ReadDecimalOrNull(this IDataRecord reader, string columnName)
		{
			object value = reader[columnName];
			return value == DBNull.Value ? null : (decimal?)value;
		}
	}
}
