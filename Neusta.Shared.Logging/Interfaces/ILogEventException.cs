namespace Neusta.Shared.Logging
{
	using System.Collections.Generic;
	using System.IO;
	using JetBrains.Annotations;

	public interface ILogEventException
	{
		/// <summary>
		/// Gets the name of the exception type.
		/// </summary>
		[PublicAPI]
		string ExceptionTypeName { get; }

		/// <summary>
		/// Gets the message.
		/// </summary>
		[PublicAPI]
		string Message { get; }

		/// <summary>
		/// Gets the source.
		/// </summary>
		[PublicAPI]
		string Source { get; }

		/// <summary>
		/// Gets the help link.
		/// </summary>
		[PublicAPI]
		string HelpLink { get; }

		/// <summary>
		/// Gets a value indicating whether this instance has a stack trace.
		/// </summary>
		[PublicAPI]
		bool HasStackTrace { get; }

		/// <summary>
		/// Gets the stacktrace.
		/// </summary>
		[PublicAPI]
		ILogEventStackFrame[] StackTrace { get; }

		/// <summary>
		/// Gets a value indicating whether this instance has a target site.
		/// </summary>
		[PublicAPI]
		bool HasTargetSite { get; }

		/// <summary>
		/// Gets the name of the target site type.
		/// </summary>
		[PublicAPI]
		string TargetSiteTypeName { get; }

		/// <summary>
		/// Gets the name of the target site method.
		/// </summary>
		[PublicAPI]
		string TargetSiteMethodName { get; }

		/// <summary>
		/// Gets a value indicating whether this instance has an inner exception.
		/// </summary>
		[PublicAPI]
		bool HasInnerException { get; }

		/// <summary>
		/// Gets the inner exception.
		/// </summary>
		[PublicAPI]
		ILogEventException InnerException { get; }

		/// <summary>
		/// Gets a value indicating whether this instance has at least one collected exception.
		/// </summary>
		[PublicAPI]
		bool HasCollectedExceptions { get; }

		/// <summary>
		/// Gets the inner exceptions.
		/// </summary>
		[PublicAPI]
		ICollection<ILogEventException> CollectedExceptions { get; }

		/// <summary>
		/// Gets a value indicating whether this instance has properties.
		/// </summary>
		[PublicAPI]
		bool HasProperties { get; }

		/// <summary>
		/// Gets the dictionary of per-event context properties.
		/// </summary>
		[PublicAPI]
		IDictionary<object, object> Properties { get; }

		/// <summary>
		/// Serializes this instance to the given <see cref="BinaryWriter"/>.
		/// </summary>
		[PublicAPI]
		void SerializeToBinaryWriter(BinaryWriter writer);
	}
}