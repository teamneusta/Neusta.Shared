namespace Neusta.Shared.Core.DisposableObjects
{
	using System;
	using System.Diagnostics;
	using System.Threading.Tasks;
	using JetBrains.Annotations;

	public static class DisposeExtensions
	{
		[PublicAPI]
		[DebuggerNonUserCode]
		public static void SafeUsing<T>(T instance, Action<T> func)
			where T : IDisposable
		{
			try
			{
				func(instance);
			}
			finally
			{
				instance.SafeDispose();
			}
		}

		[PublicAPI]
		[DebuggerNonUserCode]
		public static void SafeUsing<T>(Func<T> factory, Action<T> func)
			where T : IDisposable
		{
			T instance = factory();
			try
			{
				func(instance);
			}
			finally
			{
				instance.SafeDispose();
			}
		}

		[PublicAPI]
		[DebuggerNonUserCode]
		public static async Task SafeUsingAsync<T>(T instance, Func<T, Task> func)
			where T : IDisposable
		{
			try
			{
				await func(instance).ConfigureAwait(false);
			}
			finally
			{
				instance.SafeDispose();
			}
		}

		[PublicAPI]
		[DebuggerNonUserCode]
		public static async Task SafeUsingAsync<T>(Func<T> factory, Func<T, Task> func)
			where T : IDisposable
		{
			T instance = factory();
			try
			{
				await func(instance).ConfigureAwait(false);
			}
			finally
			{
				instance.SafeDispose();
			}
		}

		[PublicAPI]
		[DebuggerNonUserCode]
		public static async Task SafeUsingAsync<T>(Func<Task<T>> factory, Action<T> func)
			where T : IDisposable
		{
			T instance = await factory();
			try
			{
				func(instance);
			}
			finally
			{
				instance.SafeDispose();
			}
		}

		[PublicAPI]
		[DebuggerNonUserCode]
		public static async Task SafeUsingAsync<T>(Func<Task<T>> factory, Func<T, Task> func)
			where T : IDisposable
		{
			T instance = await factory();
			try
			{
				await func(instance).ConfigureAwait(false);
			}
			finally
			{
				instance.SafeDispose();
			}
		}

		[PublicAPI]
		[DebuggerNonUserCode]
		public static void DisposeAnNull<T>(ref T instance)
			where T : IDisposable
		{
			instance.Dispose();
			instance = default(T);
		}

		[PublicAPI]
		[DebuggerNonUserCode]
		public static void SafeDisposeAnNull<T>(ref T instance)
			where T : IDisposable
		{
			instance.SafeDispose();
			instance = default(T);
		}

		[PublicAPI]
		[DebuggerNonUserCode]
		public static void SafeDispose(this IDisposableObject disposable)
		{
			if (disposable != null && !disposable.IsDisposed)
			{
				try
				{
					disposable.Dispose();
				}
				catch
				{
					// Ignore exceptions
				}
			}
		}

		[PublicAPI]
		[DebuggerNonUserCode]
		public static void SafeDispose(this IDisposable disposable)
		{
			if (disposable != null)
			{
				try
				{
					disposable.Dispose();
				}
				catch
				{
					// Ignore exceptions
				}
			}
		}
	}
}