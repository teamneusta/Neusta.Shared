// ReSharper disable once CheckNamespace
namespace System.Threading
{
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Threading.Tasks;
	using JetBrains.Annotations;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class SemaphoreSlimExtensions
	{
		/// <summary>
		/// Waits for a <see cref="SemaphoreSlim"/>.
		/// </summary>
		[PublicAPI]
		[DebuggerNonUserCode]
		public static bool SafeWait(this SemaphoreSlim semaphoreSlim)
		{
			try
			{
				semaphoreSlim.Wait();
				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Waits for a <see cref="SemaphoreSlim"/>.
		/// </summary>
		[PublicAPI]
		[DebuggerNonUserCode]
		public static bool SafeWait(this SemaphoreSlim semaphoreSlim, int timeout)
		{
			try
			{
				return semaphoreSlim.Wait(timeout);
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Waits for a <see cref="SemaphoreSlim"/>.
		/// </summary>
		[PublicAPI]
		[DebuggerNonUserCode]
		public static bool SafeWait(this SemaphoreSlim semaphoreSlim, TimeSpan timeout)
		{
			try
			{
				return semaphoreSlim.Wait(timeout);
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Waits for a <see cref="SemaphoreSlim"/>.
		/// </summary>
		[PublicAPI]
		[DebuggerNonUserCode]
		public static bool SafeWait(this SemaphoreSlim semaphoreSlim, TimeSpan timeout, CancellationToken cancellationToken)
		{
			try
			{
				return semaphoreSlim.Wait(timeout, cancellationToken);
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Waits for a <see cref="SemaphoreSlim"/>.
		/// </summary>
		[PublicAPI]
		[DebuggerNonUserCode]
		public static bool SafeWait(this SemaphoreSlim semaphoreSlim, CancellationToken cancellationToken)
		{
			try
			{
				semaphoreSlim.Wait(cancellationToken);
				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Waits for a <see cref="SemaphoreSlim"/>.
		/// </summary>
		[PublicAPI]
		[DebuggerNonUserCode]
		public static async Task<bool> SafeWaitAsync(this SemaphoreSlim semaphoreSlim)
		{
			try
			{
				await semaphoreSlim.WaitAsync().ConfigureAwait(false);
				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Waits for a <see cref="SemaphoreSlim"/>.
		/// </summary>
		[PublicAPI]
		[DebuggerNonUserCode]
		public static async Task<bool> SafeWaitAsync(this SemaphoreSlim semaphoreSlim, int timeout)
		{
			try
			{
				return await semaphoreSlim.WaitAsync(timeout).ConfigureAwait(false);
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Waits for a <see cref="SemaphoreSlim"/>.
		/// </summary>
		[PublicAPI]
		[DebuggerNonUserCode]
		public static async Task<bool> SafeWaitAsync(this SemaphoreSlim semaphoreSlim, TimeSpan timeout)
		{
			try
			{
				return await semaphoreSlim.WaitAsync(timeout).ConfigureAwait(false);
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Waits for a <see cref="SemaphoreSlim"/>.
		/// </summary>
		[PublicAPI]
		[DebuggerNonUserCode]
		public static async Task<bool> SafeWaitAsync(this SemaphoreSlim semaphoreSlim, TimeSpan timeout, CancellationToken cancellationToken)
		{
			try
			{
				return await semaphoreSlim.WaitAsync(timeout, cancellationToken).ConfigureAwait(false);
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Waits for a <see cref="SemaphoreSlim"/>.
		/// </summary>
		[PublicAPI]
		[DebuggerNonUserCode]
		public static async Task<bool> SafeWaitAsync(this SemaphoreSlim semaphoreSlim, CancellationToken cancellationToken)
		{
			try
			{
				await semaphoreSlim.WaitAsync(cancellationToken).ConfigureAwait(false);
				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Waits for a <see cref="SemaphoreSlim"/> and disposes it.
		/// </summary>
		[PublicAPI]
		[DebuggerNonUserCode]
		public static void WaitAndDispose(this SemaphoreSlim semaphoreSlim)
		{
			if (semaphoreSlim.SafeWait())
			{
				semaphoreSlim.Release();
				semaphoreSlim.Dispose();
			}
		}

		/// <summary>
		/// Waits for a <see cref="SemaphoreSlim"/> and disposes it.
		/// </summary>
		[PublicAPI]
		[DebuggerNonUserCode]
		public static void WaitAndDispose(this SemaphoreSlim semaphoreSlim, int timeoutMilliseconds)
		{
			if (semaphoreSlim.SafeWait(timeoutMilliseconds))
			{
				semaphoreSlim.Release();
			}
			semaphoreSlim.Dispose();
		}

		/// <summary>
		/// Waits for a <see cref="SemaphoreSlim"/> and disposes it.
		/// </summary>
		[PublicAPI]
		[DebuggerNonUserCode]
		public static void WaitAndDispose(this SemaphoreSlim semaphoreSlim, TimeSpan timeout)
		{
			if (semaphoreSlim.SafeWait(timeout))
			{
				semaphoreSlim.Release();
			}
			semaphoreSlim.Dispose();
		}

		/// <summary>
		/// Waits for a <see cref="SemaphoreSlim"/> and disposes it.
		/// </summary>
		[PublicAPI]
		[DebuggerNonUserCode]
		public static void WaitAndDispose(this SemaphoreSlim semaphoreSlim, TimeSpan timeout, CancellationToken cancellationToken)
		{
			if (semaphoreSlim.SafeWait(timeout, cancellationToken))
			{
				semaphoreSlim.Release();
			}
			semaphoreSlim.Dispose();
		}

		/// <summary>
		/// Waits for a <see cref="SemaphoreSlim"/> and disposes it.
		/// </summary>
		[PublicAPI]
		[DebuggerNonUserCode]
		public static void WaitAndDispose(this SemaphoreSlim semaphoreSlim, CancellationToken cancellationToken)
		{
			if (semaphoreSlim.SafeWait(cancellationToken))
			{
				semaphoreSlim.Release();
			}
			semaphoreSlim.Dispose();
		}

		/// <summary>
		/// Waits for a <see cref="SemaphoreSlim"/> and disposes it.
		/// </summary>
		[PublicAPI]
		[DebuggerNonUserCode]
		public static async Task WaitAndDisposeAsync(this SemaphoreSlim semaphoreSlim)
		{
			if (await semaphoreSlim.SafeWaitAsync().ConfigureAwait(false))
			{
				semaphoreSlim.Release();
			}
			semaphoreSlim.Dispose();
		}

		/// <summary>
		/// Waits for a <see cref="SemaphoreSlim"/> and disposes it.
		/// </summary>
		[PublicAPI]
		[DebuggerNonUserCode]
		public static async Task WaitAndDisposeAsync(this SemaphoreSlim semaphoreSlim, int timeoutMilliseconds)
		{
			if (await semaphoreSlim.SafeWaitAsync(timeoutMilliseconds).ConfigureAwait(false))
			{
				semaphoreSlim.Release();
			}
			semaphoreSlim.Dispose();
		}

		/// <summary>
		/// Waits for a <see cref="SemaphoreSlim"/> and disposes it.
		/// </summary>
		[PublicAPI]
		[DebuggerNonUserCode]
		public static async Task WaitAndDisposeAsync(this SemaphoreSlim semaphoreSlim, TimeSpan timeout)
		{
			if (await semaphoreSlim.SafeWaitAsync(timeout).ConfigureAwait(false))
			{
				semaphoreSlim.Release();
			}
			semaphoreSlim.Dispose();
		}

		/// <summary>
		/// Waits for a <see cref="SemaphoreSlim"/> and disposes it.
		/// </summary>
		[PublicAPI]
		[DebuggerNonUserCode]
		public static async Task WaitAndDisposeAsync(this SemaphoreSlim semaphoreSlim, TimeSpan timeout, CancellationToken cancellationToken)
		{
			if (await semaphoreSlim.SafeWaitAsync(timeout, cancellationToken).ConfigureAwait(false))
			{
				semaphoreSlim.Release();
			}
			semaphoreSlim.Dispose();
		}

		/// <summary>
		/// Waits for a <see cref="SemaphoreSlim"/> and disposes it.
		/// </summary>
		[PublicAPI]
		[DebuggerNonUserCode]
		public static async Task WaitAndDisposeAsync(this SemaphoreSlim semaphoreSlim, CancellationToken cancellationToken)
		{
			if (await semaphoreSlim.SafeWaitAsync(cancellationToken).ConfigureAwait(false))
			{
				semaphoreSlim.Release();
			}
			semaphoreSlim.Dispose();
		}
	}
}