// ReSharper disable once CheckNamespace

namespace System.Threading.Tasks
{
	using System.ComponentModel;
	using JetBrains.Annotations;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class TaskExtensions
	{
		[PublicAPI]
		public static void IgnoreTask(this Task task)
		{
		}
	}
}