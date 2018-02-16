namespace Neusta.Shared.Logging.Configuration.Targets
{
	using System.Diagnostics;

	public sealed class ViewerTarget : ILoggingTarget
	{
		private readonly string name;

		/// <summary>
		/// Initializes a new instance of the <see cref="ViewerTarget"/> class.
		/// </summary>
		public ViewerTarget(string name)
		{
			this.name = name;
		}

		#region Implementation of ILoggingTarget

		/// <summary>
		/// Gets the target name.
		/// </summary>
		public string Name
		{
			[DebuggerStepThrough]
			get { return this.name; }
		}

		#endregion
	}
}