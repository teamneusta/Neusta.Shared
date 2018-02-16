namespace Neusta.Shared.Services.Result
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using JetBrains.Annotations;

	public class ServiceResult : IServiceResult
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static readonly IReadOnlyDictionary<string, IReadOnlyCollection<IServiceResultError>> emptyErrorsDictionary = new Dictionary<string, IReadOnlyCollection<IServiceResultError>>();

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Dictionary<string, List<IServiceResultError>> errors;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int? resultCode;

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceResult"/> class.
		/// </summary>
		public ServiceResult()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceResult"/> class.
		/// </summary>
		public ServiceResult(int resultCode)
		{
			this.resultCode = resultCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceResult"/> class.
		/// </summary>
		public ServiceResult(IServiceResult result)
		{
			this.resultCode = result.ResultCode;
			this.AddErrors(result.Errors.ToDictionary(x => x.Key, y => (IEnumerable<IServiceResultError>)y.Value));
		}

		#region Implementation of IServiceResult

		/// <summary>
		/// Gets the errors.
		/// </summary>
		public IReadOnlyDictionary<string, IReadOnlyCollection<IServiceResultError>> Errors
		{
			get
			{
				if ((this.errors == null) || !this.errors.Any())
				{
					return emptyErrorsDictionary;
				}
				return this.errors.ToDictionary(x => x.Key, y => (IReadOnlyCollection<IServiceResultError>)y.Value);
			}
		}

		/// <summary>
		/// Gets the result code.
		/// </summary>
		public int? ResultCode
		{
			[DebuggerStepThrough]
			get { return this.resultCode; }
		}

		#endregion

		#region Public Methods

		[PublicAPI]
		public void SetResultCode(int resultCode)
		{
			this.resultCode = resultCode;
		}

		[PublicAPI]
		public void AddError(string key, string message)
		{
			this.EnsureErrorsInitialized();
			this.QueryItemErrors(key).Add(new ServiceResultError(message));
		}

		[PublicAPI]
		public void AddError(string key, Exception exception)
		{
			this.EnsureErrorsInitialized();
			this.QueryItemErrors(key).Add(new ServiceResultError(exception));
		}

		[PublicAPI]
		public void AddError(string key, string message, Exception exception)
		{
			this.EnsureErrorsInitialized();
			this.QueryItemErrors(key).Add(new ServiceResultError(message, exception));
		}

		[PublicAPI]
		public void AddErrors(IDictionary<string, string[]> errors)
		{
			this.AddErrors(errors?.ToDictionary(x => x.Key, y => y.Value as IEnumerable<string>));
		}

		[PublicAPI]
		public void AddErrors(IDictionary<string, IEnumerable<string>> errors)
		{
			if (errors != null)
			{
				this.EnsureErrorsInitialized();
				foreach (KeyValuePair<string, IEnumerable<string>> kvp in errors)
				{
					if (kvp.Value != null)
					{
						ICollection<IServiceResultError> itemErrors = this.QueryItemErrors(kvp.Key);
						foreach (string message in kvp.Value)
						{
							itemErrors.Add(new ServiceResultError(message));
						}
					}
				}
			}
		}

		[PublicAPI]
		public void AddErrors(IDictionary<string, IEnumerable<IServiceResultError>> errors)
		{
			if (errors != null)
			{
				this.EnsureErrorsInitialized();
				foreach (KeyValuePair<string, IEnumerable<IServiceResultError>> kvp in errors)
				{
					if (kvp.Value != null)
					{
						ICollection<IServiceResultError> itemErrors = this.QueryItemErrors(kvp.Key);
						foreach (IServiceResultError error in kvp.Value)
						{
							itemErrors.Add(error);
						}
					}
				}
			}
		}

		#endregion

		#region Public static helper methods

		/// <summary>
		/// Gets an empty <see cref="ServiceResult"/>.
		/// </summary>
		public static ServiceResult Empty
		{
			[DebuggerStepThrough]
			get { return new ServiceResult(); }
		}


		/// <summary>
		/// Creates a new <see cref="ServiceResult"/> from the specified data.
		/// </summary>
		[PublicAPI]
		public static ServiceResult<TData> FromData<TData>(TData data)
		{
			return new ServiceResult<TData>(data);
		}

		/// <summary>
		/// Creates a new <see cref="ServiceResult"/> from the specified result code.
		/// </summary>
		[PublicAPI]
		public static ServiceResult<TData> FromResultCode<TData>(int resultCode)
		{
			return new ServiceResult<TData>(resultCode);
		}

		/// <summary>
		/// Creates a new <see cref="ServiceResult"/> from the specified result code.
		/// </summary>
		[PublicAPI]
		public static ServiceResult FromResultCode(int resultCode)
		{
			return new ServiceResult(resultCode);
		}

		#endregion

		#region Private Methods

		private ICollection<IServiceResultError> QueryItemErrors(string key)
		{
			if (!this.errors.TryGetValue(key, out List<IServiceResultError> itemErrors))
			{
				itemErrors = new List<IServiceResultError>();
				this.errors[key] = itemErrors;
			}
			return itemErrors;
		}

		private void EnsureErrorsInitialized()
		{
			if (this.errors == null)
			{
				this.errors = new Dictionary<string, List<IServiceResultError>>(StringComparer.OrdinalIgnoreCase);
			}
		}

		#endregion
	}
}
