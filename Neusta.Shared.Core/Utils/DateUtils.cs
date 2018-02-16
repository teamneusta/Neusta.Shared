namespace Neusta.Shared.Core.Utils
{
	using System;
	using System.Diagnostics;
	using System.Threading;
	using JetBrains.Annotations;

	public static class DateUtils
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static int lastNowTicks = -1;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static DateTime lastNow = DateTime.MinValue;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static int lastUtcNowTicks = -1;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static DateTime lastUtcNow = DateTime.MinValue;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static int lastOffsetNowTicks = -1;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static DateTimeOffset lastOffsetNow = DateTimeOffset.MinValue;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static int lastOffsetUtcNowTicks = -1;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static DateTimeOffset lastOffsetUtcNow = DateTimeOffset.MinValue;

		/// <summary>
		/// Gets the current date and time in an speed optimized way.
		/// </summary>
		[PublicAPI]
		public static DateTime Now
		{
			[DebuggerStepThrough]
			get
			{
				int currentTicks = Environment.TickCount;
				if (currentTicks != Volatile.Read(ref lastNowTicks))
				{
					DateTime currentNow = DateTime.Now;
					lastNow = currentNow;
					Volatile.Write(ref lastNowTicks, currentTicks);
					return currentNow;
				}
				else
				{
					return lastNow;
				}
			}
		}

		/// <summary>
		/// Gets the current date in an speed optimized way.
		/// </summary>
		[PublicAPI]
		public static DateTime Today
		{
			[DebuggerStepThrough]
			get { return Now.Date; }
		}

		/// <summary>
		/// Gets the current date and time in UTC format in an speed optimized way.
		/// </summary>
		[PublicAPI]
		public static DateTime UtcNow
		{
			[DebuggerStepThrough]
			get
			{
				int currentTicks = Environment.TickCount;
				if (currentTicks != Volatile.Read(ref lastUtcNowTicks))
				{
					DateTime currentUtcNow = DateTime.UtcNow;
					lastUtcNow = currentUtcNow;
					Volatile.Write(ref lastUtcNowTicks, currentTicks);
					return currentUtcNow;
				}
				else
				{
					return lastUtcNow;
				}
			}
		}


		/// <summary>
		/// Gets the current date in UTC format in an speed optimized way.
		/// </summary>
		[PublicAPI]
		public static DateTime UtcToday
		{
			[DebuggerStepThrough]
			get { return UtcNow.Date; }
		}

		/// <summary>
		/// Gets the current date and time including UTC offset in an speed optimized way.
		/// </summary>
		[PublicAPI]
		public static DateTimeOffset OffsetNow
		{
			[DebuggerStepThrough]
			get
			{
				int currentTicks = Environment.TickCount;
				if (currentTicks != Volatile.Read(ref lastOffsetNowTicks))
				{
					DateTimeOffset currentOffsetNow = DateTimeOffset.Now;
					lastOffsetNow = currentOffsetNow;
					Volatile.Write(ref lastOffsetNowTicks, currentTicks);
					return currentOffsetNow;
				}
				else
				{
					return lastOffsetNow;
				}
			}
		}

		/// <summary>
		/// Gets the current date in an speed optimized way.
		/// </summary>
		[PublicAPI]
		public static DateTimeOffset OffsetToday
		{
			[DebuggerStepThrough]
			get { return OffsetNow.Date; }
		}

		/// <summary>
		/// Gets the current date and time including UTC offset in an speed optimized way.
		/// </summary>
		[PublicAPI]
		public static DateTimeOffset OffsetUtcNow
		{
			[DebuggerStepThrough]
			get
			{
				int currentTicks = Environment.TickCount;
				if (currentTicks != Volatile.Read(ref lastOffsetUtcNowTicks))
				{
					DateTimeOffset currentOffsetUtcNow = DateTimeOffset.UtcNow;
					lastOffsetUtcNow = currentOffsetUtcNow;
					Volatile.Write(ref lastOffsetUtcNowTicks, currentTicks);
					return currentOffsetUtcNow;
				}
				else
				{
					return lastOffsetUtcNow;
				}
			}
		}

		/// <summary>
		/// Gets the current date in an speed optimized way.
		/// </summary>
		[PublicAPI]
		public static DateTimeOffset OffsetUtcToday
		{
			[DebuggerStepThrough]
			get { return OffsetNow.Date; }
		}

		/// <summary>
		/// Gets the <see cref="TimeSpan"/> since the given timestamp.
		/// </summary>
		[PublicAPI]
		public static TimeSpan GetTimeSpanSince(DateTime originalTimestamp)
		{
			return Now.Subtract(originalTimestamp);
		}

		/// <summary>
		/// Gets the <see cref="TimeSpan"/> since the given timestamp.
		/// </summary>
		[PublicAPI]
		public static TimeSpan GetTimeSpanSince(DateTimeOffset originalTimestamp)
		{
			return OffsetNow.Subtract(originalTimestamp);
		}

		/// <summary>
		/// Gets the <see cref="TimeSpan"/> since the given UTC timestamp.
		/// </summary>
		[PublicAPI]
		public static TimeSpan GetTimeSpanSinceUtc(DateTime originalTimestampUtc)
		{
			return UtcNow.Subtract(originalTimestampUtc);
		}

		/// <summary>
		/// Returns the minimum DateTime value.
		/// </summary>
		[PublicAPI]
		public static DateTime Min(DateTime value1, DateTime value2)
		{
			if (value1 < value2)
			{
				return value1;
			}
			return value2;
		}

		/// <summary>
		/// Returns the minimum DateTimeOffset value.
		/// </summary>
		[PublicAPI]
		public static DateTimeOffset Min(DateTimeOffset value1, DateTimeOffset value2)
		{
			if (value1 < value2)
			{
				return value1;
			}
			return value2;
		}

		/// <summary>
		/// Returns the maximum DateTime value.
		/// </summary>
		[PublicAPI]
		public static DateTime Max(DateTime value1, DateTime value2)
		{
			if (value1 > value2)
			{
				return value1;
			}
			return value2;
		}

		/// <summary>
		/// Returns the maximum DateTimeOffset value.
		/// </summary>
		[PublicAPI]
		public static DateTimeOffset Max(DateTimeOffset value1, DateTimeOffset value2)
		{
			if (value1 > value2)
			{
				return value1;
			}
			return value2;
		}

		/// <summary>
		/// Unix timestamp is seconds past epoch
		/// </summary>
		[PublicAPI]
		public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
		{
			DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
			return dtDateTime;
		}

		/// <summary>
		/// Java timestamp is millisecods past epoch
		/// </summary>
		[PublicAPI]
		public static DateTime JavaTimeStampToDateTime(long javaTimeStamp)
		{
			DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddMilliseconds(javaTimeStamp).ToLocalTime();
			return dtDateTime;
		}
	}
}