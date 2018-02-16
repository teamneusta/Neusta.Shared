namespace Neusta.Shared.Core.Utils
{
	using System.Diagnostics;
	using JetBrains.Annotations;

	public sealed class BoxedValue<T>
		where T : struct
	{
		private T value;

		/// <summary>
		/// Initializes a new instance of the <see cref="BoxedValue{T}"/> class.
		/// </summary>
		public BoxedValue()
		{
			this.value = default(T);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BoxedValue{T}"/> class.
		/// </summary>
		public BoxedValue(T value)
		{
			this.value = value;
		}

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		[PublicAPI]
		public T Value
		{
			[DebuggerStepThrough]
			get { return this.value; }
			[DebuggerStepThrough]
			set { this.value = value; }
		}

		/// <summary>
		/// Sets the specified value.
		/// </summary>
		[PublicAPI]
		public void Set(T value)
		{
			this.value = value;
		}

		/// <summary>
		/// Clears the value.
		/// </summary>
		[PublicAPI]
		public void Clear()
		{
			this.value = default(T);
		}
	}
}