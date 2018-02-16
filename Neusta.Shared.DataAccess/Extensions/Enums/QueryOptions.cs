// ReSharper disable once CheckNamespace

namespace System.Linq
{
	using JetBrains.Annotations;

	[Flags]
	public enum QueryOptions
	{
		/// <summary>
		/// No options.
		/// </summary>
		None = 0,

		/// <summary>
		///The entities returned will not tracked for changes and not updated automatically when SaveChanges is called.
		/// </summary>
		[PublicAPI]
		NoTracking = 1,

		/// <summary>
		/// The query will stream the results instead of buffering.
		/// </summary>
		[PublicAPI]
		AsStreaming = 2
	}
}