namespace Neusta.Shared.DataAccess.EntityFramework.Conventions
{
	using System;
	using System.Data.Entity.ModelConfiguration.Configuration;
	using System.Data.Entity.ModelConfiguration.Conventions;

	public class DecimalPrecisionAttributeConvention : PrimitivePropertyAttributeConfigurationConvention<DecimalPrecisionAttribute>
	{
		/// <summary>
		/// Applies this convention to a property that has an attribute of type TAttribute applied.
		/// </summary>
		public override void Apply(ConventionPrimitivePropertyConfiguration configuration, DecimalPrecisionAttribute attribute)
		{
			byte precision = 18;
			byte scale = 2;

			if (attribute.HasScale)
			{
				if (attribute.HasPrecision && attribute.Scale > attribute.Precision)
				{
					throw new InvalidOperationException("Scale must be between 0 and the Precision value.");
				}
				scale = attribute.Scale;
			}

			if (attribute.HasPrecision)
			{
				if (attribute.Precision < 1 || attribute.Precision > 38)
				{
					throw new InvalidOperationException("Precision must be between 1 and 38.");
				}
			}
			else if (attribute.HasScale)
			{
				precision = (byte)Math.Min(38, scale + 16);
			}

			configuration.HasPrecision(precision, scale);
		}
	}
}