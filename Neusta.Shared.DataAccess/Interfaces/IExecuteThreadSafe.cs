namespace Neusta.Shared.DataAccess
{
	using System;
	using System.Threading.Tasks;
	using JetBrains.Annotations;

	public interface IExecuteThreadSafe
	{
		/// <summary>
		/// Executes the specified action thread safe.
		/// </summary>
		[PublicAPI]
		void ExecuteThreadSafe(Action action);

		/// <summary>
		/// Executes the specified function thread safe.
		/// </summary>
		[PublicAPI]
		TResult ExecuteThreadSafe<TResult>(Func<TResult> func);

		/// <summary>
		/// Executes the specified asyncronous action thread safe.
		/// </summary>
		[PublicAPI]
		Task ExecuteThreadSafeAsync(Func<Task> asyncAction);

		/// <summary>
		/// Executes the specified asyncronous function thread safe.
		/// </summary>
		[PublicAPI]
		Task<TResult> ExecuteThreadSafeAsync<TResult>(Func<Task<TResult>> asyncFunc);
	}
}