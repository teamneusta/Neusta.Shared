namespace Neusta.Shared.DataAccess.EntityFramework.Configuration
{
	using System;
	using System.Data.Entity;
	using Neusta.Shared.DataAccess.EntityFramework.Interceptors;
	using Neusta.Shared.DataAccess.EntityFramework.Utils.Internal;

	public abstract class OptimizedDbConfiguration : DbConfiguration
	{
		/// <summary>
		/// Initializes static members of the <see cref="OptimizedDbConfiguration"/> class.
		/// </summary>
		static OptimizedDbConfiguration()
		{
			ZExtensionsLicenseManager.Initialize();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OptimizedDbConfiguration"/> class.
		/// </summary>
		protected OptimizedDbConfiguration()
		{
			this.AddInterceptor(new StringTrimmerInterceptor());
		}
	}
}