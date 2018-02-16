namespace Neusta.Shared.Services.Result
{
	using System.Diagnostics;
	using JetBrains.Annotations;

	public class ServiceResult<TData> : ServiceResult, IServiceResult<TData>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private TData data;

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceResult{TData}"/> class.
		/// </summary>
		public ServiceResult()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceResult{TData}"/> class.
		/// </summary>
		public ServiceResult(TData data)
		{
			this.data = data;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceResult{TData}"/> class.
		/// </summary>
		public ServiceResult(int resultCode)
			: base(resultCode)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceResult{TData}"/> class.
		/// </summary>
		public ServiceResult(IServiceResult<TData> result)
			: base(result)
		{
			this.data = result.Data;
		}

		#region Implementation of IServiceResult{TData}

		/// <summary>
		/// Gets or sets the data.
		/// </summary>
		public TData Data
		{
			[DebuggerStepThrough]
			get { return this.data; }
			[DebuggerStepThrough]
			set { this.data = value; }
		}

		#endregion

		#region Public static helper methods

		/// <summary>
		/// Gets an empty <see cref="ServiceResult"/>.
		/// </summary>
		public new static ServiceResult<TData> Empty
		{
			[DebuggerStepThrough]
			get { return new ServiceResult<TData>(); }
		}

		/// <summary>
		/// Creates a new <see cref="ServiceResult"/> from the specified result code.
		/// </summary>
		[PublicAPI]
		public new static ServiceResult<TData> FromResultCode(int resultCode)
		{
			return new ServiceResult<TData>(resultCode);
		}

		#endregion
	}
}