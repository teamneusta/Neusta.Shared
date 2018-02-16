namespace Neusta.Shared.DataAccess.EntityFramework
{
	using System;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using JetBrains.Annotations;

	[PublicAPI]
	[AttributeUsage(AttributeTargets.Class)]
	public class NamespaceSchemaTableAttribute : TableAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NamespaceSchemaTableAttribute"/> class.
		/// </summary>
		public NamespaceSchemaTableAttribute(Type type)
			: base(CalculateTableName(type))
		{
			this.Schema = CalculateSchemaName(type);
		}

		#region Private methods

		/// <summary>
		/// Calculates the name of the table.
		/// </summary>
		private static string CalculateTableName(Type type)
		{
			string name = type.Name;
			string schema = CalculateSchemaName(type);
			if (!string.IsNullOrEmpty(schema) && name.StartsWith(schema, StringComparison.OrdinalIgnoreCase))
			{
				name = name.Substring(schema.Length);
			}
			if (name.Length > 6 && name.EndsWith(@"Entity", StringComparison.OrdinalIgnoreCase))
			{
				name = name.Substring(0, name.Length - 6);
			}
			return name;
		}

		/// <summary>
		/// Calculates the name of the schema.
		/// </summary>
		private static string CalculateSchemaName(Type type)
		{
			string typeNamespace = type.Namespace;
			string assemblyName = type.Assembly.GetName().Name;
			if (string.IsNullOrEmpty(typeNamespace) || string.Equals(typeNamespace, assemblyName, StringComparison.OrdinalIgnoreCase))
			{
				return null;
			}
			string schema = typeNamespace.Split('.').Last();
			if (!string.Equals(schema, "@Model", StringComparison.OrdinalIgnoreCase))
			{
				schema = schema.ToLowerInvariant();
			}
			return schema;
		}

		#endregion
	}
}