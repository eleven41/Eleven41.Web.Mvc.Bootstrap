using Eleven41.Web.Mvc.Bootstrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace System.Web.Mvc
{
	public static class LabelExtensions
	{
		public static MvcHtmlString BootstrapLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
		{
			return BootstrapLabelFor<TModel, TValue>(html, expression, null);
		}

		public static MvcHtmlString BootstrapLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText)
		{
			return LabelHelper(html,
							   ModelMetadata.FromLambdaExpression(expression, html.ViewData),
							   ExpressionHelper.GetExpressionText(expression),
							   labelText);
		}

		public static MvcHtmlString BootstrapLabelForModel(this HtmlHelper html)
		{
			return BootstrapLabelForModel(html, null);
		}

		public static MvcHtmlString BootstrapLabelForModel(this HtmlHelper html, string labelText)
		{
			return LabelHelper(html, html.ViewData.ModelMetadata, String.Empty, labelText);
		}

		internal static MvcHtmlString LabelHelper(HtmlHelper html, ModelMetadata metadata, string htmlFieldName, string labelText = null)
		{
			string resolvedLabelText = labelText ?? metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
			if (String.IsNullOrEmpty(resolvedLabelText))
			{
				return MvcHtmlString.Empty;
			}

			TagBuilder tag = new TagBuilder("label");
			tag.Attributes.Add("for", TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)));
			tag.Attributes.Add("class", "control-label");
			tag.SetInnerText(resolvedLabelText);
			return new MvcHtmlString(tag.ToString(TagRenderMode.Normal));
		}

		public static BootstrapLabel BeginBootstrapLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string className)
		{
			return BeginBootstrapLabelFor<TModel, TValue>(html, expression, className, null);
		}

		public static BootstrapLabel BeginBootstrapLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string className, string labelText)
		{
			return BeginLabelHelper(html,
							   ModelMetadata.FromLambdaExpression(expression, html.ViewData),
							   ExpressionHelper.GetExpressionText(expression),
							   className,
							   labelText);
		}

		private static BootstrapLabel BeginLabelHelper(HtmlHelper html, ModelMetadata metadata, string htmlFieldName, string className, string labelText = null)
		{
			string resolvedLabelText = labelText ?? metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();

			TagBuilder tag = new TagBuilder("label");
			tag.Attributes.Add("for", TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)));
			tag.Attributes.Add("class", className);
			tag.SetInnerText(resolvedLabelText);

			html.ViewContext.Writer.Write(tag.ToString(TagRenderMode.StartTag));

			BootstrapLabel theLabel = new BootstrapLabel(html.ViewContext, resolvedLabelText);
			return theLabel;
		}
	}
}
