using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;

namespace DevilDaggersWebsite.Core.Tasks.Cron
{
	[Serializable]
	public sealed class CrontabFieldImpl : IObjectReference
	{
		public static readonly CrontabFieldImpl Minute = new CrontabFieldImpl(CrontabFieldKind.Minute, 0, 59, null);
		public static readonly CrontabFieldImpl Hour = new CrontabFieldImpl(CrontabFieldKind.Hour, 0, 23, null);
		public static readonly CrontabFieldImpl Day = new CrontabFieldImpl(CrontabFieldKind.Day, 1, 31, null);

		public static readonly CrontabFieldImpl Month = new CrontabFieldImpl(CrontabFieldKind.Month, 1, 12, new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" });

		public static readonly CrontabFieldImpl DayOfWeek = new CrontabFieldImpl(CrontabFieldKind.DayOfWeek, 0, 6, new[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" });

		private static readonly CrontabFieldImpl[] fieldByKind = { Minute, Hour, Day, Month, DayOfWeek };

		private static readonly CompareInfo comparer = CultureInfo.InvariantCulture.CompareInfo;
		private static readonly char[] comma = { ',' };

		private readonly string[]? names;

		private CrontabFieldImpl(CrontabFieldKind kind, int minValue, int maxValue, string[]? names)
		{
			Debug.Assert(names == null || names.Length == maxValue - minValue + 1);
			Debug.Assert(Enum.IsDefined(typeof(CrontabFieldKind), kind));
			Debug.Assert(minValue >= 0);
			Debug.Assert(maxValue >= minValue);

			this.names = names;
			Kind = kind;
			MinValue = minValue;
			MaxValue = maxValue;
		}

		public CrontabFieldKind Kind { get; }
		public int MinValue { get; }
		public int MaxValue { get; }

		public int ValueCount => MaxValue - MinValue + 1;

		object IObjectReference.GetRealObject(StreamingContext context) => FromKind(Kind);

		public static CrontabFieldImpl FromKind(CrontabFieldKind kind)
		{
			if (!Enum.IsDefined(typeof(CrontabFieldKind), kind))
				throw new ArgumentException($"Invalid crontab field kind. Valid values are {string.Join(", ", Enum.GetNames(typeof(CrontabFieldKind)))}.", nameof(kind));

			return fieldByKind[(int)kind];
		}

		public void Format(CrontabField field, TextWriter writer, bool noNames)
		{
			if (field == null)
				throw new ArgumentNullException(nameof(field));

			if (writer == null)
				throw new ArgumentNullException(nameof(writer));

			int next = field.GetFirst();
			int count = 0;

			while (next != -1)
			{
				int first = next;
				int last;

				do
				{
					last = next;
					next = field.Next(last + 1);
				} while (next - last == 1);

				if (count == 0
					&& first == MinValue && last == MaxValue)
				{
					writer.Write('*');
					return;
				}

				if (count > 0)
					writer.Write(',');

				if (first == last)
				{
					FormatValue(first, writer, noNames);
				}
				else
				{
					FormatValue(first, writer, noNames);
					writer.Write('-');
					FormatValue(last, writer, noNames);
				}

				count++;
			}
		}

		private void FormatValue(int value, TextWriter writer, bool noNames)
		{
			Debug.Assert(writer != null);

			if (noNames || names == null)
			{
				if (value >= 0 && value < 100)
				{
					FastFormatNumericValue(value, writer);
				}
				else
				{
					writer.Write(value.ToString(CultureInfo.InvariantCulture));
				}
			}
			else
			{
				int index = value - MinValue;
				writer.Write(names[index]);
			}
		}

		private static void FastFormatNumericValue(int value, TextWriter writer)
		{
			Debug.Assert(value >= 0 && value < 100);
			Debug.Assert(writer != null);

			if (value >= 10)
			{
				writer.Write((char)('0' + value / 10));
				writer.Write((char)('0' + value % 10));
			}
			else
			{
				writer.Write((char)('0' + value));
			}
		}

		public void Parse(string str, CrontabFieldAccumulator acc)
		{
			if (acc == null)
				throw new ArgumentNullException(nameof(acc));

			if (string.IsNullOrEmpty(str))
				return;

			try
			{
				InternalParse(str, acc);
			}
			catch (FormatException e)
			{
				ThrowParseException(e, str);
			}
		}

		private static void ThrowParseException(Exception innerException, string str)
		{
			Debug.Assert(str != null);
			Debug.Assert(innerException != null);

			throw new FormatException($"'{str}' is not a valid crontab field expression.",
				innerException);
		}

		private void InternalParse(string str, CrontabFieldAccumulator acc)
		{
			Debug.Assert(str != null);
			Debug.Assert(acc != null);

			if (str.Length == 0)
				throw new FormatException("A crontab field value cannot be empty.");

			// Next, look for a list of values (e.g. 1,2,3).
			int commaIndex = str.IndexOf(",", StringComparison.Ordinal);

			if (commaIndex > 0)
			{
				foreach (string token in str.Split(comma))
					InternalParse(token, acc);
			}
			else
			{
				int every = 1;

				// Look for stepping first (e.g. */2 = every 2nd).
				int slashIndex = str.IndexOf("/", StringComparison.Ordinal);

				if (slashIndex > 0)
				{
					every = int.Parse(str.Substring(slashIndex + 1), CultureInfo.InvariantCulture);
					str = str.Substring(0, slashIndex);
				}

				// Next, look for wildcard (*).
				if (str.Length == 1 && str[0] == '*')
				{
					acc(-1, -1, every);
					return;
				}

				// Next, look for a range of values (e.g. 2-10).
				int dashIndex = str.IndexOf("-", StringComparison.Ordinal);

				if (dashIndex > 0)
				{
					int first = ParseValue(str.Substring(0, dashIndex));
					int last = ParseValue(str.Substring(dashIndex + 1));

					acc(first, last, every);
					return;
				}

				// Finally, handle the case where there is only one number.
				int value = ParseValue(str);

				if (every == 1)
				{
					acc(value, value, 1);
				}
				else
				{
					Debug.Assert(every != 0);

					acc(value, MaxValue, every);
				}
			}
		}

		private int ParseValue(string str)
		{
			Debug.Assert(str != null);

			if (str.Length == 0)
				throw new FormatException("A crontab field value cannot be empty.");

			char firstChar = str[0];

			if (firstChar >= '0' && firstChar <= '9')
				return int.Parse(str, CultureInfo.InvariantCulture);

			if (names == null)
			{
				throw new FormatException($"'{str}' is not a valid value for this crontab field. It must be a numeric value between {MinValue} and {MaxValue} (all inclusive).");
			}

			for (int i = 0; i < names.Length; i++)
			{
				if (comparer.IsPrefix(names[i], str, CompareOptions.IgnoreCase))
					return i + MinValue;
			}

			throw new FormatException($"'{str}' is not a known value name. Use one of the following: {string.Join(", ", names)}.");
		}
	}
}