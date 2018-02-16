namespace Neusta.Shared.AspNetCore.Extensions.Base
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Net;
	using System.Security.Authentication;
	using System.Threading.Tasks;
	using JetBrains.Annotations;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.ModelBinding;
	using Neusta.Shared.Logging;
	using Neusta.Shared.Services;

	[PublicAPI]
	[ApiExplorerSettings(IgnoreApi = false)]
	[ProducesResponseType(typeof(IDictionary<string, string[]>), (int)HttpStatusCode.BadRequest)]
	[ResponseCache(NoStore = true)]
	public abstract class BaseController<TController> : ControllerBase
		where TController : BaseController<TController>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static readonly ILogger<TController> staticLogger = LogManager.GetLogger<TController>();
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static readonly ILogger<TController> nullLogger = LogManager.GetNullLogger<TController>();

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly ILogger<TController> logger;

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseController{TController}"/> class.
		/// </summary>
		protected BaseController()
		{
			this.logger = staticLogger;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseController{TController}"/> class.
		/// </summary>
		protected BaseController(ILogger<TController> logger)
		{
			this.logger = logger ?? LogManager.GetNullLogger<TController>();
		}

		#region Protected Properties

		/// <summary>
		/// Gets the logger.
		/// </summary>
		[PublicAPI]
		protected ILogger<TController> Logger
		{
			[DebuggerStepThrough]
			get { return this.logger; }
		}

		#endregion

		#region Protected Methods

		protected async Task<IActionResult> EvaluateAndCallServiceAsync(Func<Task> service)
		{
			if (!this.ModelState.IsValid)
			{
				return this.BadRequest(this.ModelState.ToValidationResult());
			}
			try
			{
				await service();
				return this.Ok();
			}
			catch (AuthenticationException ex)
			{
				return this.StatusCode((int)HttpStatusCode.Unauthorized, ex);
			}
			catch (Exception ex)
			{
				this.logger.LogException(LogLevel.Error, ex.Message, ex);
				throw;
			}
		}

		protected async Task<IActionResult> EvaluateAndCallServiceAsync<TData>(Func<Task<TData>> service)
		{
			if (!this.ModelState.IsValid)
			{
				return this.BadRequest(this.ModelState);
			}
			try
			{
				TData result = await service();
				return this.Ok(result);
			}
			catch (AuthenticationException ex)
			{
				return this.StatusCode((int)HttpStatusCode.Unauthorized, ex);
			}
			catch (Exception ex)
			{
				this.logger.LogException(LogLevel.Error, ex.Message, ex);
				throw;
			}
		}

		protected async Task<IActionResult> EvaluateAndCallServiceAsync(Func<Task<IServiceResult>> service)
		{
			if (!this.ModelState.IsValid)
			{
				return this.BadRequest(this.ModelState.ToValidationResult());
			}
			try
			{
				IServiceResult result = await service();
				if (this.ModelState.MergeModelErrors(result))
				{
					if (result.ResultCode.HasValue)
					{
						return this.StatusCode(result.ResultCode.Value, this.ModelState.ToValidationResult());
					}
					return this.BadRequest(this.ModelState.ToValidationResult());
				}
				if (result.ResultCode.HasValue)
				{
					return this.StatusCode(result.ResultCode.Value);
				}
				return this.Ok();
			}
			catch (AuthenticationException ex)
			{
				return this.StatusCode((int)HttpStatusCode.Unauthorized, ex);
			}
			catch (Exception ex)
			{
				this.logger.LogException(LogLevel.Error, ex.Message, ex);
				throw;
			}
		}

		protected async Task<IActionResult> EvaluateAndCallServiceAsync<TData>(Func<Task<IServiceResult<TData>>> service)
		{
			if (!this.ModelState.IsValid)
			{
				return this.BadRequest(this.ModelState.ToValidationResult());
			}
			try
			{
				IServiceResult<TData> result = await service();
				if (result == null)
				{
					return this.NotFound();
				}
				if (this.ModelState.MergeModelErrors(result))
				{
					if (result.ResultCode.HasValue)
					{
						return this.StatusCode(result.ResultCode.Value, this.ModelState.ToValidationResult());
					}
					return this.BadRequest(this.ModelState.ToValidationResult());
				}
				if (result.ResultCode.HasValue)
				{
					int resultCodeValue = result.ResultCode.Value;
					if (result.Data != null)
					{
						return this.StatusCode(resultCodeValue, result.Data);
					}
					return this.StatusCode(resultCodeValue);
				}
				if (result.Data == null)
				{
					return this.NoContent();
				}
				return this.Ok(result.Data);
			}
			catch (AuthenticationException ex)
			{
				return this.StatusCode((int)HttpStatusCode.Unauthorized, ex);
			}
			catch (Exception ex)
			{
				this.logger.LogException(LogLevel.Error, ex.Message, ex);
				throw;
			}
		}

		#endregion
	}
}