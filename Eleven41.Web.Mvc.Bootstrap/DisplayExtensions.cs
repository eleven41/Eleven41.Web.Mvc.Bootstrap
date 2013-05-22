using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc.Html;

namespace System.Web.Mvc
{
	public static class DisplayExtensions
	{
		public static MvcHtmlString DisplayForNotEmpty<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
		{
			return html.DisplayForNotEmpty(expression, "&nbsp;");
		}

		public static MvcHtmlString DisplayForNotEmpty<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string empty)
		{
			var result = html.DisplayFor(expression);
			if (String.IsNullOrEmpty(result.ToString()))
				return MvcHtmlString.Create(empty);
			return result;
		}

		public static MvcHtmlString DisplayNameFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
		{
			return DisplayNameFor<TModel, TValue>(html, expression, null);
		}

		public static MvcHtmlString DisplayNameFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText)
		{
			return DisplayNameHelper(html,
							   ModelMetadata.FromLambdaExpression(expression, html.ViewData),
							   ExpressionHelper.GetExpressionText(expression),
							   labelText);
		}

		internal static MvcHtmlString DisplayNameHelper(HtmlHelper html, ModelMetadata metadata, string htmlFieldName, string labelText = null)
		{
			string resolvedLabelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
			if (String.IsNullOrEmpty(resolvedLabelText))
			{
				return MvcHtmlString.Empty;
			}

			return new MvcHtmlString(resolvedLabelText);
		}
	}
}
