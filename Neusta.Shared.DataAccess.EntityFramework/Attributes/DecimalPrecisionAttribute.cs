namespace Neusta.Shared.DataAccess.EntityFramework
{
	using System;
	using System.Diagnostics;
	using JetBrains.Annotations;

	[PublicAPI]
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class DecimalPrecisionAttribute : Attribute
	{
		private byte? precision;
		private byte? scale;

		/// <summary>
		/// Initializes a new instance of the <see cref="DecimalPrecisionAttribute"/> class.
		/// </summary>
		public DecimalPrecisionAttribute()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DecimalPrecisionAttribute"/> class.
		/// </summary>
		public DecimalPrecisionAttribute(byte precision, byte scale)
		{
			this.precision = precision;
			this.scale = scale;
		}

		/// <summary>
		/// Gets a value indicating whether this instance has a precision.
		/// </summary>
		[PublicAPI]
		public bool HasPrecision
		{
			[DebuggerStepThrough]
			get { return this.precision.HasValue; }
		}

		/// <summary>
		/// Gets or sets the precision.
		/// </summary>
		[PublicAPI]
		public byte Precision
		{
			[DebuggerStepThrough]
			get { return this.precision.GetValueOrDefault(18); }
			[DebuggerStepThrough]
			set { this.precision = value; }
		}

		/// <summary>
		/// Gets a value indicating whether this instance has a scale.
		/// </summary>
		[PublicAPI]
		public bool HasScale
		{
			[DebuggerStepThrough]
			get { return this.scale.HasValue; }
		}

		/// <summary>
		/// Gets or sets the scale.
		/// </summary>
		[PublicAPI]
		public byte Scale
		{
			[DebuggerStepThrough]
			get { return this.scale.GetValueOrDefault(2); }
			[DebuggerStepThrough]
			set { this.scale = value; }
		}
	}
}