namespace Neusta.Shared.ObjectProvider.Base
{
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Threading;

	public abstract class BaseUpdateableContainerAdapter<T> : BaseContainerAdapter<T>, IUpdateableContainerAdapter
		where T : BaseUpdateableContainerAdapter<T>
	{
		private readonly ICollection<IServiceDescriptor> serviceDescriptors;
		private int updateLevel;

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseUpdateableContainerAdapter{T}" /> class.
		/// </summary>
		protected BaseUpdateableContainerAdapter(IContainerConfiguration configuration)
			: base(configuration)
		{
			this.serviceDescriptors = new List<IServiceDescriptor>();
		}

		#region Implementation of IUpdateableContainerAdapter

		/// <summary>
		/// Gets the service descriptors.
		/// </summary>
		public IEnumerable<IServiceDescriptor> ServiceDescriptors
		{
			[DebuggerStepThrough]
			get { return this.serviceDescriptors; }
		}

		public void BeginUpdate()
		{
			Interlocked.Increment(ref this.updateLevel);
		}

		public void EndUpdate()
		{
			if (Interlocked.Decrement(ref this.updateLevel) == 0)
			{
				this.InternalUpdateContainer();
			}
		}

		public void RegisterServiceDescriptor(IServiceDescriptor serviceDescriptor)
		{
			if (!this.serviceDescriptors.Contains(serviceDescriptor))
			{
				this.serviceDescriptors.Add(serviceDescriptor);
				this.InternalRegisterServiceDescriptor(serviceDescriptor);
				if (this.updateLevel == 0)
				{
					this.InternalUpdateContainer();
				}
			}
		}

		public void UnregisterServiceDescriptor(IServiceDescriptor serviceDescriptor)
		{
			if (this.serviceDescriptors.Remove(serviceDescriptor))
			{
				this.InternalUnregisterServiceDescriptor(serviceDescriptor);
				if (this.updateLevel == 0)
				{
					this.InternalUpdateContainer();
				}
			}
		}

		#endregion

		#region Protected Methods

		protected virtual void InternalRegisterServiceDescriptor(IServiceDescriptor serviceDescriptor)
		{
		}

		protected virtual void InternalUnregisterServiceDescriptor(IServiceDescriptor serviceDescriptor)
		{
		}

		protected virtual void InternalUpdateContainer()
		{
		}

		#endregion
	}
}