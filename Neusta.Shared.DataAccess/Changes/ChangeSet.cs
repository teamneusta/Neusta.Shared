namespace Neusta.Shared.DataAccess.Changes
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;

	public class ChangeSet : IChangeSet
	{
		public static readonly ChangeSet Empty = new ChangeSet(null, new List<IEntityChange>());

		private readonly Type context;
		private readonly IEnumerable<IEntityChange> changes;

		/// <summary>
		/// Initializes a new instance of the <see cref="ChangeSet"/> class.
		/// </summary>
		public ChangeSet(Type contextType, IList<IEntityChange> changes)
		{
			this.context = contextType;
			this.changes = changes ?? Enumerable.Empty<IEntityChange>();
		}

		/// <summary>
		/// Gets the context.
		/// </summary>
		public Type Context
		{
			[DebuggerStepThrough]
			get { return this.context; }
		}

		/// <summary>
		/// Gets the changes.
		/// </summary>
		public IEnumerable<IEntityChange> Changes
		{
			[DebuggerStepThrough]
			get { return this.changes; }
		}

		#region Overrides of Object

		public override string ToString()
		{
			var sb = new StringBuilder(64);
			sb.Append("ChangeSet[");
			if (this.changes.Any())
			{
				var needSep = false;
				int cnt = this.changes.Count(x => x.State == ChangeState.Added);
				if (cnt > 0)
				{
					sb.Append(@"ADD:");
					sb.Append(cnt);
					needSep = true;
				}

				cnt = this.changes.Count(x => x.State == ChangeState.Modified);
				if (cnt > 0)
				{
					if (needSep)
					{
						sb.Append("|");
					}

					sb.Append(@"MDFY:");
					sb.Append(cnt);
					needSep = true;
				}

				cnt = this.changes.Count(x => x.State == ChangeState.Deleted);
				if (cnt > 0)
				{
					if (needSep)
					{
						sb.Append("|");
					}

					sb.Append(@"|DEL:");
					sb.Append(cnt);
				}
			}
			else
			{
				sb.Append(@"empty");
			}
			sb.Append(@"]");
			return sb.ToString();
		}

		#endregion
	}
}