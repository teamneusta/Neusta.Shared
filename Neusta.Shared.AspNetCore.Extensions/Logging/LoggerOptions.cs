namespace Neusta.Shared.AspNetCore.Extensions.Logging
{
	using System.ComponentModel;
	using JetBrains.Annotations;

	[UsedImplicitly]
	public class LoggerOptions
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LoggerOptions"></see> class.
		/// </summary>
		public LoggerOptions()
		{
			this.IncludeScopes = true;
		}

		/// <summary>
		/// Gets or sets a value indicating whether scope details should be included in log output.
		/// </summary>
		[DefaultValue(true)]
		public bool IncludeScopes { get; set; }
	}
}