namespace Neusta.Shared.DataAccess.EntityFramework.Conventions
{
	using System;
	using System.Data.Entity.ModelConfiguration.Conventions;

	public class ExtendedDateTimeColumnTypeConvention : Convention
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExtendedDateTimeColumnTypeConvention"/> class.
		/// </summary>
		public ExtendedDateTimeColumnTypeConvention()
		{
			this.Properties<DateTime>().Configure(c => c
				.HasColumnType("datetime2")
				.HasPrecision(3)
			);

			this.Properties<DateTimeOffset>().Configure(c => c
				.HasColumnType("datetimeoffset")
				.HasPrecision(3)
			);
		}
	}
}