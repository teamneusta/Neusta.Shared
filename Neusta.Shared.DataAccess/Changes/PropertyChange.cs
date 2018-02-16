namespace Neusta.Shared.DataAccess.Changes
{
	using System.Diagnostics;
	using JetBrains.Annotations;

	[DebuggerDisplay("PropertyChange='{PropertyName}'")]
	public class PropertyChange : IPropertyChange
	{
		private readonly string propertyName;
		private readonly object originalValue;
		private readonly object currentValue;

		/// <summary>
		/// Initializes a new instance of the <see cref="PropertyChange"/> class.
		/// </summary>
		public PropertyChange(string propertyName, object originalValue, object currentValue)
		{
			this.propertyName = propertyName;
			this.originalValue = originalValue;
			this.currentValue = currentValue;
		}

		/// <summary>
		/// Gets the name of the property.
		/// </summary>
		[PublicAPI]
		public string PropertyName
		{
			[DebuggerStepThrough]
			get { return this.propertyName; }
		}

		/// <summary>
		/// Gets the original value.
		/// </summary>
		[PublicAPI]
		public object OriginalValue
		{
			[DebuggerStepThrough]
			get { return this.originalValue; }
		}

		/// <summary>
		/// Gets the current value.
		/// </summary>
		[PublicAPI]
		public object CurrentValue
		{
			[DebuggerStepThrough]
			get { return this.currentValue; }
		}
	}
}