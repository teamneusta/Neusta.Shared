namespace Neusta.Shared.ObjectProvider
{
	using System;
	using System.Diagnostics;
	using JetBrains.Annotations;

	[PublicAPI]
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class MemberInjectionAttribute : Attribute
	{
		private readonly MemberInjectionMode memberInjectionMode;

		/// <summary>
		/// Initializes a new instance of the <see cref="MemberInjectionAttribute"/> class.
		/// </summary>
		public MemberInjectionAttribute(MemberInjectionMode memberInjectionMode)
		{
			this.memberInjectionMode = memberInjectionMode;
		}

		/// <summary>
		/// Gets the member injection mode.
		/// </summary>
		public MemberInjectionMode MemberInjectionMode
		{
			[DebuggerStepThrough]
			get { return this.memberInjectionMode; }
		}
	}
}