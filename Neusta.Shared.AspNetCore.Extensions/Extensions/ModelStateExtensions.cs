// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Mvc.ModelBinding
{
	using System.Collections.Generic;
	using System.Linq;
	using Neusta.Shared.Services;

	public static class ModelStateExtensions
	{
		/// <summary>
		/// Converts the given <see cref="ModelStateDictionary"/> to a simple validation result.
		/// </summary>
		public static IDictionary<string, string[]> ToValidationResult(this ModelStateDictionary modelState)
		{
			if (modelState.IsValid)
			{
				return null;
			}
			var result = modelState.ToDictionary(x => x.Key, y => y.Value.Errors.Select(z => z.ErrorMessage).ToArray());
			return result;
		}

		/// <summary>
		/// Merges the service result errors into the given <see cref="ModelStateDictionary"/>.
		/// </summary>
		public static bool MergeModelErrors(this ModelStateDictionary modelState, IServiceResult result)
		{
			if (result == null)
			{
				return false;
			}
			bool merged = false;
			foreach (KeyValuePair<string, IReadOnlyCollection<IServiceResultError>> modelError in result.Errors)
			{
				foreach (IServiceResultError error in modelError.Value)
				{
					if (error.Exception != null)
					{
						modelState.TryAddModelException(modelError.Key, error.Exception);
					}
					else
					{
						modelState.TryAddModelError(modelError.Key, error.Message);
					}
					merged = true;
				}
			}
			return merged;
		}
	}
}