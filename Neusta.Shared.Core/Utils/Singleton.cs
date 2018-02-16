namespace Neusta.Shared.Core.Utils
{
	using System;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Linq;
	using System.Reflection;
	using System.Threading;
	using JetBrains.Annotations;

	/// <summary>
	/// Eine einfache Singleton-Klasse, die threadsicher ist und ohne Locking funktioniert, daher ist
	/// sie sehr schnell. Das Singleton muss von dieser Klasse abgeleitet sein und darf nur einen parameterlosen
	/// Konstruktor definieren.
	/// </summary>
	[Singleton(@"Instance")]
	[DebuggerNonUserCode]
	public abstract class Singleton<T> : ISingleton
		where T : Singleton<T>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Singleton{T}"/> class.
		/// </summary>
		protected Singleton()
		{
		}

		/// <summary>
		/// Gets the singleton instance.
		/// </summary>
		[PublicAPI]
		[SingletonInstanceProperty]
		public static T Instance
		{
			[DebuggerStepThrough]
			get { return Container.instance; }
		}

		/// <summary>
		/// Sets the singleton instance/implementation.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void Set(T value)
		{
			Interlocked.Exchange(ref Container.instance, value);
		}

		#region Private Methods / Classes

		[DebuggerNonUserCode]
		private static class Container
		{
			// ReSharper disable once StaticMemberInGenericType
			// ReSharper disable once InconsistentNaming
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			internal static T instance;

			static Container()
			{
				ConstructorInfo ctor = typeof(T)
					.GetTypeInfo()
					.DeclaredConstructors
					.FirstOrDefault(match => match.GetParameters().Length == 0);
				if (ctor == null)
				{
					throw new MissingMethodException(typeof(T).Name + " does not provide a parameterless constructor.");
				}
				var obj = (T)ctor.Invoke(null);
				Interlocked.CompareExchange(ref instance, obj, null);
			}
		}

		#endregion
	}
}