namespace Neusta.Shared.DataAccess.EntityFrameworkCore.Utils.Internal
{
	using System.Diagnostics;
	using Neusta.Shared.Logging;
	using Z.EntityFramework.Extensions;

	internal static class ZExtensionsLicenseManager
	{
		private static readonly ILogger logger = LogManager.GetLogger(typeof(ZExtensionsLicenseManager));

		private const string licenseKey = "040043a1-2148-0ade-14fb-92d710e545ae";
		private const string licenseName = "1621;100-neusta";
		private static readonly bool isLicenseValid;

		/// <summary>
		/// Initializes the <see cref="ZExtensionsLicenseManager"/> class.
		/// </summary>
		static ZExtensionsLicenseManager()
		{
			LicenseManager.AddLicense(licenseName, licenseKey);
			isLicenseValid = LicenseManager.ValidateLicense(out string errorMessage);
			if (!isLicenseValid)
			{
				logger.Fatal("Z.EntityFramework.Extensions license invalid: " + errorMessage);
			}
			else
			{
				logger.Trace("Z.EntityFramework.Extensions licensed.");
			}
		}

		/// <summary>
		/// Initializes the license manager.
		/// </summary>
		internal static void Initialize()
		{
		}

		/// <summary>
		/// Gets a value indicating whether the license valid.
		/// </summary>
		internal static bool IsLicenseValid
		{
			[DebuggerStepThrough]
			get { return isLicenseValid; }
		}
	}
}