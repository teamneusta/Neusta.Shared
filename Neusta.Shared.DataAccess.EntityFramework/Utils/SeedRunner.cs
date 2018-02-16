namespace Neusta.Shared.DataAccess.EntityFramework.Utils
{
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Threading.Tasks;

	public class SeedRunner : ISeedRunner
	{
		private readonly IEnumerable<ISeed> initializer;

		/// <summary>
		/// Initializes a new instance of the <see cref="SeedRunner"/> class.
		/// </summary>
		public SeedRunner(IEnumerable<ISeed> initializer)
		{
			this.initializer = initializer;
		}

		/// <summary>
		/// Gets the initializer.
		/// </summary>
		public IEnumerable<ISeed> Initializer
		{
			[DebuggerStepThrough]
			get { return this.initializer; }
		}

		#region Implementation of ISeedRunner

		/// <summary>
		/// Runs the database initializer.
		/// </summary>
		public virtual void Execute()
		{
			Task.Run(this.ExecuteAsync).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Runs the database initializer.
		/// </summary>
		protected virtual async Task ExecuteAsync()
		{
			foreach (var initializer in this.initializer)
			{
				await initializer.InitializeAsync().ConfigureAwait(false);
			}
		}

		#endregion
	}
}