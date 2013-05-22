using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace System.Web.Mvc
{
	public static class InputExtensions
	{
		#region TextBoxes

		public static MvcHtmlString BootstrapTextBoxForWithBlockHelp<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string blockHelpText)
		{
			return htmlHelper.BootstrapTextBoxForWithBlockHelp(expression, (IDictionary<string, object>)null, blockHelpText);
		}

		public static MvcHtmlString BootstrapTextBoxForWithBlockHelp<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes, string blockHelpText)
		{
			return htmlHelper.BootstrapTextBoxForWithBlockHelp(expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), blockHelpText);
		}

		public static MvcHtmlString BootstrapTextBoxForWithBlockHelp<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes, string blockHelpText)
		{
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			return BootstrapTextBoxHelper(htmlHelper,
								 metadata,
								 metadata.Model,
								 ExpressionHelper.GetExpressionText(expression),
								 htmlAttributes,
								 blockHelpText);
		}

		private static MvcHtmlString BootstrapTextBoxHelper(this HtmlHelper htmlHelper, ModelMetadata metadata, object model, string expression, IDictionary<string, object> htmlAttributes, string blockHelpText)
		{
			return BootstrapInputHelper(htmlHelper, InputType.Text, metadata, expression, model, false /* useViewData */, false /* isChecked */, true /* setId */, true /* isExplicitValue */, htmlAttributes, blockHelpText);
		}

		#endregion

		internal static object GetModelStateValue(this HtmlHelper htmlHelper, string key, Type destinationType)
		{
			ModelState modelState;
			if (htmlHelper.ViewData.ModelState.TryGetValue(key, out modelState))
			{
				if (modelState.Value != null)
				{
					return modelState.Value.ConvertTo(destinationType, null /* culture */);
				}
			}
			return null;
		}

		internal static bool EvalBoolean(this HtmlHelper htmlHelper, string key)
		{
			return Convert.ToBoolean(htmlHelper.ViewData.Eval(key), CultureInfo.InvariantCulture);
		}

		internal static string EvalString(this HtmlHelper htmlHelper, string key)
		{
			return Convert.ToString(htmlHelper.ViewData.Eval(key), CultureInfo.CurrentCulture);
		}

		private static MvcHtmlString BootstrapInputHelper(HtmlHelper htmlHelper, InputType inputType, ModelMetadata metadata, string name, object value, bool useViewData, bool isChecked, bool setId, bool isExplicitValue, IDictionary<string, object> htmlAttributes, string blockHelpText)
		{
			string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
			if (String.IsNullOrEmpty(fullName))
			{
				throw new ArgumentException("Value cannot be null or empty.", "name");
			}

			TagBuilder tagBuilder = new TagBuilder("input");
			tagBuilder.MergeAttributes(htmlAttributes);
			tagBuilder.MergeAttribute("type", HtmlHelper.GetInputTypeString(inputType));
			tagBuilder.MergeAttribute("name", fullName, true);

			string valueParameter = Convert.ToString(value, CultureInfo.CurrentCulture);
			bool usedModelState = false;

			switch (inputType)
			{
				case InputType.CheckBox:
					bool? modelStateWasChecked = htmlHelper.GetModelStateValue(fullName, typeof(bool)) as bool?;
					if (modelStateWasChecked.HasValue)
					{
						isChecked = modelStateWasChecked.Value;
						usedModelState = true;
					}
					goto case InputType.Radio;
				case InputType.Radio:
					if (!usedModelState)
					{
						string modelStateValue = htmlHelper.GetModelStateValue(fullName, typeof(string)) as string;
						if (modelStateValue != null)
						{
							isChecked = String.Equals(modelStateValue, valueParameter, StringComparison.Ordinal);
							usedModelState = true;
						}
					}
					if (!usedModelState && useViewData)
					{
						isChecked = htmlHelper.EvalBoolean(fullName);
					}
					if (isChecked)
					{
						tagBuilder.MergeAttribute("checked", "checked");
					}
					tagBuilder.MergeAttribute("value", valueParameter, isExplicitValue);
					break;
				case InputType.Password:
					if (value != null)
					{
						tagBuilder.MergeAttribute("value", valueParameter, isExplicitValue);
					}
					break;
				default:
					string attemptedValue = (string)htmlHelper.GetModelStateValue(fullName, typeof(string));
					tagBuilder.MergeAttribute("value", attemptedValue ?? ((useViewData) ? htmlHelper.EvalString(fullName) : valueParameter), isExplicitValue);
					break;
			}

			if (setId)
			{
				tagBuilder.GenerateId(fullName);
			}

			// If there are any errors for a named field, we add the css attribute.
			ModelState modelState;
			if (htmlHelper.ViewData.ModelState.TryGetValue(fullName, out modelState))
			{
				if (modelState.Errors.Count > 0)
				{
					tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
				}
			}

			tagBuilder.MergeAttributes(htmlHelper.GetUnobtrusiveValidationAttributes(name, metadata));

			if (inputType == InputType.CheckBox)
			{
				// Render an additional <input type="hidden".../> for checkboxes. This
				// addresses scenarios where unchecked checkboxes are not sent in the request.
				// Sending a hidden input makes it possible to know that the checkbox was present
				// on the page when the request was submitted.
				StringBuilder inputItemBuilder = new StringBuilder();
				inputItemBuilder.Append(tagBuilder.ToString(TagRenderMode.SelfClosing));

				TagBuilder hiddenInput = new TagBuilder("input");
				hiddenInput.MergeAttribute("type", HtmlHelper.GetInputTypeString(InputType.Hidden));
				hiddenInput.MergeAttribute("name", fullName);
				hiddenInput.MergeAttribute("value", "false");
				inputItemBuilder.Append(hiddenInput.ToString(TagRenderMode.SelfClosing));
				return MvcHtmlString.Create(inputItemBuilder.ToString());
			}
			else if (!String.IsNullOrEmpty(blockHelpText))
			{
				// Render an additional <span class="help-block"> inside 
				// the <input ...> for the help text
				StringBuilder inputItemBuilder = new StringBuilder();
				inputItemBuilder.Append(tagBuilder.ToString(TagRenderMode.StartTag));

				TagBuilder span = new TagBuilder("span");
				span.MergeAttribute("class", "help-block");
				inputItemBuilder.Append(span.ToString(TagRenderMode.StartTag));
				inputItemBuilder.Append(blockHelpText);
				inputItemBuilder.Append(span.ToString(TagRenderMode.EndTag));
				inputItemBuilder.Append(tagBuilder.ToString(TagRenderMode.EndTag));

				return MvcHtmlString.Create(inputItemBuilder.ToString());
			}

			return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.SelfClosing));
		}
	}
}
