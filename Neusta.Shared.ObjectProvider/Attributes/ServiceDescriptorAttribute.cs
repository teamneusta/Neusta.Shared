namespace Neusta.Shared.ObjectProvider
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using JetBrains.Annotations;

	[PublicAPI]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class ServiceDescriptorAttribute : Attribute
	{
		private readonly Type serviceType;
		private readonly ServiceLifetime serviceLifetime;

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceDescriptorAttribute"/> class.
		/// </summary>
		public ServiceDescriptorAttribute()
			: this(null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceDescriptorAttribute"/> class.
		/// </summary>
		public ServiceDescriptorAttribute(Type serviceType)
			: this(serviceType, ServiceLifetime.Transient)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceDescriptorAttribute"/> class.
		/// </summary>
		public ServiceDescriptorAttribute(ServiceLifetime serviceLifetime)
		{
			this.serviceLifetime = serviceLifetime;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceDescriptorAttribute"/> class.
		/// </summary>
		public ServiceDescriptorAttribute(Type serviceType, ServiceLifetime serviceLifetime)
		{
			this.serviceType = serviceType;
			this.serviceLifetime = serviceLifetime;
		}

		/// <summary>
		/// Gets the type of the service.
		/// </summary>
		public Type ServiceType
		{
			[DebuggerStepThrough]
			get { return this.serviceType; }
		}

		/// <summary>
		/// Gets the service lifetime.
		/// </summary>
		public ServiceLifetime ServiceLifetime
		{
			[DebuggerStepThrough]
			get { return this.serviceLifetime; }
		}

		/// <summary>
		/// Gets the service types.
		/// </summary>
		public IEnumerable<Type> GetServiceTypes(Type fallbackType)
		{
			if (this.serviceType == null)
			{
				yield return fallbackType;

				var fallbackTypes = fallbackType.GetBaseTypes();
				foreach (var type in fallbackTypes)
				{
					if (type == typeof(object))
					{
						continue;
					}
					yield return type;
				}
				yield break;
			}

			if (!fallbackType.IsAssignableTo(this.ServiceType))
			{
				throw new InvalidOperationException($@"Type ""{fallbackType.ToFriendlyName()}"" is not assignable to ""{this.ServiceType.ToFriendlyName()}"".");
			}

			yield return this.ServiceType;
		}
	}
}