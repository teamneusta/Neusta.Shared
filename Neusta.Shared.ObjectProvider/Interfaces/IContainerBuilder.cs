namespace Neusta.Shared.ObjectProvider
{
	using System;
	using System.ComponentModel;
	using JetBrains.Annotations;

	public interface IContainerBuilder : IContainerBuilderBase, IFluentSyntax
	{

		/// <summary>
		/// Builds the object provider using the previously registered builder.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		IContainerAdapter Build();

		/// <summary>
		/// Builds the object provider using the previously registered builder and registers
		/// it as the default adapter.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		IContainerAdapter BuildAsDefault();
	}
}