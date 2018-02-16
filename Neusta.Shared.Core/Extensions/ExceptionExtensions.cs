//// ReSharper disable CheckNamespace
namespace System
{
	using System.ComponentModel;
	using System.Linq;
	using System.Reflection;
	using System.Threading.Tasks;
	using JetBrains.Annotations;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class ExceptionExtensions
	{
		/// <summary>
		/// Determines whether the exception indicates that something was aborted.
		/// </summary>
		[PublicAPI]
		public static bool IsAbortException(this Exception exception)
		{
			exception = exception.UnwrapAggregateException();
			while (exception.IsNotNull())
			{
				if (exception is TaskCanceledException)
				{
					return true;
				}
				if (exception is OperationCanceledException)
				{
					return true;
				}
				exception = exception.InnerException;
			}
			return false;
		}

		/// <summary>
		/// Determines whether the exception is an <see cref="OperationCanceledException" />.
		/// </summary>
		[PublicAPI]
		public static bool IsOperationCanceledException(this Exception exception)
		{
			exception = exception.UnwrapAggregateException();
			while (exception.IsNotNull())
			{
				if (exception is OperationCanceledException)
				{
					return true;
				}
				exception = exception.InnerException;
			}
			return false;
		}

		/// <summary>
		/// Determines whether the exception is an <see cref="IsTaskCanceledException" />.
		/// </summary>
		[PublicAPI]
		public static bool IsTaskCanceledException(this Exception exception)
		{
			exception = exception.UnwrapAggregateException();
			while (exception.IsNotNull())
			{
				if (exception is TaskCanceledException)
				{
					return true;
				}
				exception = exception.InnerException;
			}
			return false;
		}

		/// <summary>
		/// Determines whether the exception must be rethrown.
		/// </summary>
		[PublicAPI]
		public static bool MustBeRethrown(this Exception exception)
		{
			exception = exception.UnwrapAggregateException();
			while (exception.IsNotNull())
			{
				if (exception is TaskCanceledException)
				{
					return true;
				}
				if (exception is OperationCanceledException)
				{
					return true;
				}
				if (exception is OutOfMemoryException)
				{
					return true;
				}
				exception = exception.InnerException;
			}
			return false;
		}

		/// <summary>
		/// Unwraps the aggregate exception.
		/// </summary>
		[PublicAPI]
		public static Exception UnwrapAggregateException(this Exception ex)
		{
			var aggEx = ex as AggregateException;
			if (aggEx.IsNotNull())
			{
				return aggEx.InnerExceptions.First();
			}
			return ex;
		}

		/// <summary>
		/// Rethrows the inner exception of the given <see cref="TargetInvocationException" />.
		/// </summary>
		[PublicAPI]
		public static void RethrowInnerException(this TargetInvocationException exception)
		{
			Exception innerException = exception.InnerException;
			if (innerException != null)
			{
				FieldInfo stackTraceField = typeof(Exception).GetTypeInfo().GetDeclaredField("_remoteStackTraceString");
				stackTraceField.SetValue(innerException, innerException.StackTrace);
				throw innerException;
			}
		}
	}
}

//// ReSharper restore CheckNamespace