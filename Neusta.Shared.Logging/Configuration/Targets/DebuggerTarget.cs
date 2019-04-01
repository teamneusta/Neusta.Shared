﻿namespace Neusta.Shared.Logging.Configuration.Targets
{
	using System.Diagnostics;

	public sealed class DebuggerTarget : ILoggingTarget
	{
		private readonly string name;
		private string layout;

		/// <summary>
		/// Initializes a new instance of the <see cref="DebuggerTarget"/> class.
		/// </summary>
		internal DebuggerTarget(string name)
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

		#region Properties

		/// <summary>
		/// Gets or sets the layout.
		/// </summary>
		public string Layout
		{
			[DebuggerStepThrough]
			get { return this.layout; }
			[DebuggerStepThrough]
			set { this.layout = value; }
		}

		#endregion
	}
}