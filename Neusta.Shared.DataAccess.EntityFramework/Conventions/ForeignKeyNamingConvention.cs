namespace Neusta.Shared.DataAccess.EntityFramework.Conventions
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity.Core.Metadata.Edm;
	using System.Data.Entity.Infrastructure;
	using System.Data.Entity.ModelConfiguration.Conventions;

	public class ForeignKeyNamingConvention : IStoreModelConvention<AssociationType>, IConvention
	{
		public void Apply(AssociationType item, DbModel model)
		{
			if (!item.IsForeignKey)
			{
				return;
			}

			ReferentialConstraint constraint = item.Constraint;
			ICollection<EdmProperty> fromProperties = constraint.FromProperties;
			ICollection<EdmProperty> toProperties = constraint.ToProperties;

			Dictionary<string, string> map = BuildPropertyNameMap(fromProperties, toProperties);
			if (map != null)
			{
				ApplyPropertyNameMap(fromProperties, map);
			}

			map = BuildPropertyNameMap(toProperties, fromProperties);
			if (map != null)
			{
				ApplyPropertyNameMap(toProperties, map);
			}
		}

		private static Dictionary<string, string> BuildPropertyNameMap(ICollection<EdmProperty> properties, ICollection<EdmProperty> otherEndProperties)
		{
			if (properties.Count != otherEndProperties.Count)
			{
				return null;
			}

			var map = new Dictionary<string, string>();

			using (IEnumerator<EdmProperty> propertiesEnumerator = properties.GetEnumerator())
			using (IEnumerator<EdmProperty> otherEndPropertiesEnumerator = otherEndProperties.GetEnumerator())
			{
				while (propertiesEnumerator.MoveNext() && otherEndPropertiesEnumerator.MoveNext())
				{
					EdmProperty otherEndEdmProperty = otherEndPropertiesEnumerator.Current;
					string otherEndPropertyName = otherEndEdmProperty.Name;
					if (otherEndEdmProperty.IsStoreGeneratedIdentity && string.Equals(otherEndPropertyName, "PkID"))
					{
						otherEndPropertyName = "ID";
					}

					string currentName = propertiesEnumerator.Current.Name;
					int underscoreIndex = currentName.LastIndexOf('_');
					if (underscoreIndex < 0 || !currentName.EndsWith("_" + otherEndPropertyName))
					{
						return null;
					}

					string mappedName = "Fk" + currentName.Substring(0, underscoreIndex) + otherEndPropertyName;
					map.Add(currentName, mappedName);
				}
			}

			return map;
		}

		private static void ApplyPropertyNameMap(IEnumerable<EdmProperty> properties, Dictionary<string, string> map)
		{
			foreach (EdmProperty edmProperty in properties)
			{
				string mappedName;
				if (map.TryGetValue(edmProperty.Name, out mappedName))
				{
					edmProperty.Name = mappedName;
				}
			}
		}
	}
}