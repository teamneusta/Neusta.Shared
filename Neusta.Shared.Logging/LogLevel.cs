namespace Neusta.Shared.Logging
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Globalization;
	using JetBrains.Annotations;

	/// <summary>
	/// Defines the available log levels.
	/// </summary>
	public sealed class LogLevel : IComparable, IComparable<LogLevel>, IEquatable<LogLevel>
	{
		/// <summary>
		/// The TRACE log level.
		/// </summary>
		public static readonly LogLevel Trace;

		/// <summary>
		/// The DEBUG log level.
		/// </summary>
		public static readonly LogLevel Debug;

		/// <summary>
		/// The INFO log level.
		/// </summary>
		public static readonly LogLevel Info;

		/// <summary>
		/// The WARN log level.
		/// </summary>
		public static readonly LogLevel Warn;

		/// <summary>
		/// The ERROR log level.
		/// </summary>
		public static readonly LogLevel Error;

		/// <summary>
		/// The FATAL log level.
		/// </summary>
		public static readonly LogLevel Fatal;

		/// <summary>
		/// A log level describing that no logging should happen (OFF).
		/// </summary>
		public static readonly LogLevel Off;

		/// <summary>
		/// A log level describing that everything should be logged (ALL).
		/// </summary>
		public static readonly LogLevel All;

		/// <summary>
		/// The maximum log level value.
		/// </summary>
		[PublicAPI]
		public static readonly LogLevel MaxLevel;

		/// <summary>
		/// The minimum log level value.
		/// </summary>
		[PublicAPI]
		public static readonly LogLevel MinLevel;

		/// <summary>
		/// The unspecified log level value.
		/// </summary>
		[PublicAPI]
		internal static readonly LogLevel Unspecified;

		/// <summary>
		/// A list containing all known log level objects.
		/// </summary>
		private static readonly LogLevel[] knownLevels;

		/// <summary>
		/// A list containing all log level objects that are visible to users.
		/// </summary>
		private static readonly LogLevel[] visibleLevels;

		/// <summary>
		/// Initializes static members of the LogLevel class.
		/// </summary>
		static LogLevel()
		{
			int idx = 0;
			All = new LogLevel(@"All", idx++);
			Trace = new LogLevel(@"Trace", idx++);
			Debug = new LogLevel(@"Debug", idx++);
			Info = new LogLevel(@"Info", idx++);
			Warn = new LogLevel(@"Warn", idx++);
			Error = new LogLevel(@"Error", idx++);
			Fatal = new LogLevel(@"Fatal", idx++);
			Off = new LogLevel(@"Off", idx);
			Unspecified = new LogLevel(string.Empty, 0);

			knownLevels = new[] { All, Trace, Debug, Info, Warn, Error, Fatal, Off };
			visibleLevels = new[] { All, Trace, Debug, Info, Warn, Error, Fatal };

			MinLevel = All;
			MaxLevel = Fatal;
		}

		/// <summary>
		/// The name of this log level.
		/// </summary>
		private readonly string name;

		/// <summary>
		/// The ordinal value of this log level.
		/// </summary>
		private readonly int ordinal;

		/// <summary>
		/// Initializes a new instance of the <see cref="LogLevel"/> class.
		/// </summary>
		private LogLevel(string name, int ordinal)
		{
			this.name = name;
			this.ordinal = ordinal;
		}

		/// <summary>
		/// Gets the name of this log level.
		/// </summary>
		public string Name
		{
			[DebuggerStepThrough]
			get { return this.name; }
		}

		/// <summary>
		/// Gets the ordinal value of this log level.
		/// </summary>
		public int Ordinal
		{
			[DebuggerStepThrough]
			get { return this.ordinal; }
		}

		/// <summary>
		/// Compares two <see cref="LogLevel"/> objects and returns a value indicating whether the first one is equal to the second one.
		/// </summary>
		[DebuggerStepThrough]
		public static bool operator ==(LogLevel level1, LogLevel level2)
		{
			return (level1 ?? LogLevel.Unspecified).Ordinal == (level2 ?? LogLevel.Unspecified).Ordinal;
		}

		/// <summary>
		/// Compares two <see cref="LogLevel"/> objects and returns a value indicating whether the first one is not equal to the second one.
		/// </summary>
		[DebuggerStepThrough]
		public static bool operator !=(LogLevel level1, LogLevel level2)
		{
			return (level1 ?? LogLevel.Unspecified).Ordinal != (level2 ?? LogLevel.Unspecified).Ordinal;
		}

		/// <summary>
		/// Compares two <see cref="LogLevel"/> objects and returns a value indicating whether the first one is greater than the second one.
		/// </summary>
		[DebuggerStepThrough]
		public static bool operator >(LogLevel level1, LogLevel level2)
		{
			return level1.Ordinal > level2.Ordinal;
		}

		/// <summary>
		/// Compares two <see cref="LogLevel"/> objects and returns a value indicating whether the first one is greater than or equal to the second one.
		/// </summary>
		[DebuggerStepThrough]
		public static bool operator >=(LogLevel level1, LogLevel level2)
		{
			return level1.Ordinal >= level2.Ordinal;
		}

		/// <summary>
		/// Compares two <see cref="LogLevel"/> objects and returns a value indicating whether the first one is less than the second one.
		/// </summary>
		[DebuggerStepThrough]
		public static bool operator <(LogLevel level1, LogLevel level2)
		{
			return level1.Ordinal < level2.Ordinal;
		}

		/// <summary>
		/// Compares two <see cref="LogLevel"/> objects and returns a value indicating whether the first one is less than or equal to the second one.
		/// </summary>
		[DebuggerStepThrough]
		public static bool operator <=(LogLevel level1, LogLevel level2)
		{
			return level1.Ordinal <= level2.Ordinal;
		}

		/// <summary>
		/// Gets the <see cref="LogLevel"/>s.
		/// </summary>
		public static IEnumerable<LogLevel> GetLevels()
		{
			return visibleLevels;
		}

		/// <summary>
		/// Gets the <see cref="LogLevel"/> that corresponds to the specified ordinal.
		/// </summary>
		public static LogLevel FromOrdinal(int ordinal)
		{
			LogLevel best = All;
			foreach (LogLevel level in knownLevels)
			{
				if (level.Ordinal == ordinal)
				{
					return level;
				}
				else if ((level.Ordinal < ordinal) && (level.Ordinal > best.Ordinal))
				{
					best = level;
				}
			}
			return best;
		}

		/// <summary>
		/// Returns the <see cref="LogLevel"/> that corresponds to the supplied <see langword="string"/>.
		/// </summary>
		public static LogLevel FromString(string levelName)
		{
			foreach (LogLevel level in knownLevels)
			{
				if (string.Equals(level.Name, levelName, StringComparison.OrdinalIgnoreCase))
				{
					return level;
				}
			}
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Unknown log level: {0}", levelName));
		}

		/// <summary>
		/// Returns the <see cref="LogLevel"/> that corresponds to the supplied <see langword="string"/>.
		/// </summary>
		public static LogLevel FromString(string levelName, LogLevel defaultLogLevel)
		{
			foreach (LogLevel level in knownLevels)
			{
				if (string.Equals(level.Name, levelName, StringComparison.OrdinalIgnoreCase))
				{
					return level;
				}
			}
			return defaultLogLevel;
		}

		/// <summary>
		/// Returns a string representation of the log level.
		/// </summary>
		[DebuggerStepThrough]
		public override string ToString()
		{
			return this.name;
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		[DebuggerStepThrough]
		public override int GetHashCode()
		{
			return this.ordinal;
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
		/// </summary>
		[DebuggerStepThrough]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as LogLevel);
		}

		/// <summary>
		/// Determines whether the specified <see cref="LogLevel"/> is equal to this instance.
		/// </summary>
		[DebuggerStepThrough]
		public bool Equals(LogLevel obj)
		{
			if ((object)obj == null)
			{
				return false;
			}
			return this.ordinal == obj.Ordinal;
		}

		/// <summary>
		/// Compares the level to the other <see cref="LogLevel"/> object.
		/// </summary>
		[DebuggerStepThrough]
		public int CompareTo(LogLevel obj)
		{
			return this.ordinal - obj.Ordinal;
		}

		/// <summary>
		/// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
		/// </summary>
		public int CompareTo(object obj)
		{
			var level = obj as LogLevel;
			if (!ReferenceEquals(level, null))
			{
				return this.ordinal - level.Ordinal;
			}
			return 0;
		}
	}
}