using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web.Mvc
{
	public static class ModalExtensions
	{
		public static MvcHtmlString ShowDialog(this HtmlHelper html, string dialogId, string text)
		{
			return ShowDialog(html, dialogId, text, null);
		}

		public static MvcHtmlString ShowDialog(this HtmlHelper html, string dialogId, string text, string className)
		{
			TagBuilder tag = new TagBuilder("a");
			tag.Attributes.Add("href", "#" + dialogId);
			tag.Attributes.Add("data-toggle", "modal");

			if (!String.IsNullOrEmpty(className))
				tag.AddOrMergeAttribute("class", className);

			tag.SetInnerText(text);

			return new MvcHtmlString(tag.ToString(TagRenderMode.Normal));
		}

		public static BootstrapModal BeginBootstrapModal<TModel>(this HtmlHelper<TModel> html, string dialogId)
		{
			return BeginModalHelper(html, dialogId, null, null, null, BootstrapModalButtons.OK);
		}

		public static BootstrapModal BeginBootstrapModal<TModel>(this HtmlHelper<TModel> html, string dialogId, BootstrapModalButtons buttons)
		{
			return BeginModalHelper(html, dialogId, null, null, null, buttons);
		}

		public static BootstrapModal BeginBootstrapModal<TModel>(this HtmlHelper<TModel> html, string dialogId, string dialogTitle, BootstrapModalButtons buttons)
		{
			return BeginModalHelper(html, dialogId, dialogTitle, null, null, buttons);
		}

		public static BootstrapModal BeginBootstrapModal<TModel>(this HtmlHelper<TModel> html, string dialogId, string dialogTitle, Object dialogHtmlAttributes, BootstrapModalButtons buttons)
		{
			return BeginModalHelper(html, dialogId, dialogTitle, dialogHtmlAttributes, null, buttons);
		}

		public static BootstrapModal BeginBootstrapModal<TModel>(this HtmlHelper<TModel> html, string dialogId, string dialogTitle, Object dialogHtmlAttributes, Object bodyHtmlAttributes, BootstrapModalButtons buttons)
		{
			return BeginModalHelper(html, dialogId, dialogTitle, dialogHtmlAttributes, bodyHtmlAttributes, buttons);
		}

		private static BootstrapModal BeginModalHelper(this HtmlHelper html, string dialogId, string dialogTitle, Object dialogHtmlAttributes, Object bodyHtmlAttributes, BootstrapModalButtons buttons)
		{
			TagBuilder tag = new TagBuilder("div");

			tag.Attributes.Add("class", "modal hide fade");
			tag.Attributes.Add("role", "dialog");

			if (!String.IsNullOrEmpty(dialogId))
				tag.Attributes.Add("id", dialogId);

			// Add any extra attributes to the dialog
			tag.AddAttributes(dialogHtmlAttributes);
			
			// Write out the initial div
			html.ViewContext.Writer.Write(tag.ToString(TagRenderMode.StartTag));

			// Write out the header start
			TagBuilder headerTag = new TagBuilder("div");
			headerTag.Attributes.Add("class", "modal-header");
			html.ViewContext.Writer.Write(headerTag.ToString(TagRenderMode.StartTag));

			// Write out the close button
			TagBuilder closeTag = new TagBuilder("button");
			closeTag.Attributes.Add("type", "button");
			closeTag.Attributes.Add("class", "close");
			closeTag.Attributes.Add("data-dismiss", "modal");
			closeTag.Attributes.Add("aria-hidden", "true");
			closeTag.SetInnerText("x");
			html.ViewContext.Writer.Write(closeTag.ToString(TagRenderMode.Normal));

			// Write out the header
			if (!String.IsNullOrEmpty(dialogTitle))
			{
				TagBuilder h3Tag = new TagBuilder("h3");
				h3Tag.SetInnerText(dialogTitle);
				html.ViewContext.Writer.Write(h3Tag.ToString(TagRenderMode.Normal));
			}

			// Finish off the header tag
			html.ViewContext.Writer.Write(headerTag.ToString(TagRenderMode.EndTag));

			// Write out the body start
			TagBuilder bodyTag = new TagBuilder("div");
			bodyTag.Attributes.Add("class", "modal-body");

			// Add any extra attributes to the body
			bodyTag.AddAttributes(bodyHtmlAttributes);
			
			html.ViewContext.Writer.Write(bodyTag.ToString(TagRenderMode.StartTag));

			// Finish off when the BootstrapModal object is disposed

			return new BootstrapModal(html.ViewContext, buttons);
		}
	}

	public enum BootstrapModalButtons
	{
		None,
		OmitFooter,
		Cancel,
		Close,
		OK
	}
}
