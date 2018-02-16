namespace Neusta.Shared.AspNetCore.Extensions.Base
{
	using System.Net;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using Neusta.Shared.Logging;

	[Authorize]
	[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
	public abstract class BaseAuthorizedController<TController> : BaseController<TController>
		where TController : BaseAuthorizedController<TController>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BaseAuthorizedController{TController}"/> class.
		/// </summary>
		protected BaseAuthorizedController()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseAuthorizedController{TController}"/> class.
		/// </summary>
		protected BaseAuthorizedController(ILogger<TController> logger)
			: base(logger)
		{
		}
	}
}