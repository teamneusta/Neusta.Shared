namespace Neusta.Shared.Core.Utils
{
	using System;

	public struct TimeoutTracker
	{
		private readonly int m_start;
		private int m_total;

		/// <summary>
		/// Initializes a new instance of the <see cref="TimeoutTracker"/> struct.
		/// </summary>
		public TimeoutTracker(TimeSpan timeout)
		{
			var ltm = (long)timeout.TotalMilliseconds;
			if (ltm < -1 || ltm > int.MaxValue)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			this.m_total = (int)ltm;
			this.m_start = Environment.TickCount;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TimeoutTracker"/> struct.
		/// </summary>
		public TimeoutTracker(int millisecondsTimeout)
		{
			if (millisecondsTimeout < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout");
			}
			this.m_total = millisecondsTimeout;
			this.m_start = Environment.TickCount;
		}

		/// <summary>
		/// Gets the remaining milliseconds.
		/// </summary>
		public int RemainingMilliseconds
		{
			get
			{
				if (this.m_total == -1 || this.m_total == 0)
				{
					return this.m_total;
				}

				int elapsed = Environment.TickCount - this.m_start;

				// elapsed may be negative if TickCount has overflowed by 2^31 milliseconds.
				if (elapsed < 0 || elapsed >= this.m_total)
				{
					return 0;
				}

				return this.m_total - elapsed;
			}
		}

		/// <summary>
		/// Gets the elapsed milliseconds.
		/// </summary>
		public int ElapsedMilliseconds
		{
			get
			{
				int elapsed = Environment.TickCount - this.m_start;

				// elapsed may be negative if TickCount has overflowed by 2^31 milliseconds.
				if (elapsed < 0)
				{
					return int.MaxValue;
				}

				return elapsed;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this object is expired.
		/// </summary>
		public bool IsExpired => this.RemainingMilliseconds == 0;

		/// <summary>
		/// Expires this tracker immediately.
		/// </summary>
		public void Expire()
		{
			this.m_total = 0;
		}
	}
}