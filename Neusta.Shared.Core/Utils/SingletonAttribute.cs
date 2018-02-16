namespace Neusta.Shared.Core.Utils
{
	using System;
	using System.Diagnostics;

	[AttributeUsage(AttributeTargets.Class)]
	public sealed class SingletonAttribute : Attribute
	{
		private readonly string propertyName;

		/// <summary>
		/// Initializes a new instance of the <see cref="SingletonAttribute"/> class.
		/// </summary>
		public SingletonAttribute(string propertyName)
		{
			this.propertyName = propertyName;
		}

		/// <summary>
		/// Gets the name of the property.
		/// </summary>
		public string PropertyName
		{
			[DebuggerStepThrough]
			get { return this.propertyName; }
		}
	}
}