namespace Neusta.Shared.DataAccess.EntityFramework.Conventions
{
	using System;
	using System.Data.Entity.ModelConfiguration.Configuration;
	using System.Data.Entity.ModelConfiguration.Conventions;

	public class DateAttributeConvention : PrimitivePropertyAttributeConfigurationConvention<DateAttribute>
	{
		/// <summary>
		/// Applies this convention to a property that has an attribute of type TAttribute applied.
		/// </summary>
		public override void Apply(ConventionPrimitivePropertyConfiguration configuration, DateAttribute attribute)
		{
			Type propType = configuration.ClrPropertyInfo.PropertyType;
			Type underlyingType = Nullable.GetUnderlyingType(propType);
			if (underlyingType != null)
			{
				propType = underlyingType;
			}
			if (propType != typeof(DateTime) && propType != typeof(DateTimeOffset))
			{
				throw new InvalidOperationException("Type of property " + configuration.ClrPropertyInfo.Name + " type must be DateTime or DateTimeOffset");
			}
			configuration.HasColumnType("date");
		}
	}
}