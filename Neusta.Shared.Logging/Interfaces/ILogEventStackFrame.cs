namespace Neusta.Shared.Logging
{
	using System.IO;
	using JetBrains.Annotations;

	public interface ILogEventStackFrame
	{
		/// <summary>
		/// Gets the name of the file.
		/// </summary>
		[PublicAPI]
		string FileName { get; }

		/// <summary>
		/// Gets the file line number.
		/// </summary>
		[PublicAPI]
		int FileLineNumber { get; }

		/// <summary>
		/// Gets the file column number.
		/// </summary>
		[PublicAPI]
		int FileColumnNumber { get; }

		/// <summary>
		/// Gets the IL offset.
		/// </summary>
		[PublicAPI]
		int ILOffset { get; }

		/// <summary>
		/// Gets the native offset.
		/// </summary>
		[PublicAPI]
		int NativeOffset { get; }

		/// <summary>
		/// Gets a value indicating whether this instance has a method.
		/// </summary>
		[PublicAPI]
		bool HasMethod { get; }

		/// <summary>
		/// Gets the name of the method type.
		/// </summary>
		[PublicAPI]
		string MethodTypeName { get; }

		/// <summary>
		/// Gets the name of the method.
		/// </summary>
		[PublicAPI]
		string MethodName { get; }

		/// <summary>
		/// Serializes this instance to the given <see cref="BinaryWriter"/>.
		/// </summary>
		[PublicAPI]
		void SerializeToBinaryWriter(BinaryWriter writer);
	}
}