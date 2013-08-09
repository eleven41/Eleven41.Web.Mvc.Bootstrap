using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web.Mvc
{
	public static class AlertExtensions
	{
		public static BootstrapAlert BeginSuccessBox(this HtmlHelper html)
		{
			return BeginSuccessBox(html, null);
		}

		public static BootstrapAlert BeginSuccessBox(this HtmlHelper html, string title)
		{
			return BeginAlertHelper(html, "alert-success", title, null, null);
		}

		public static BootstrapAlert BeginInfoBox(this HtmlHelper html)
		{
			return BeginInfoBox(html, null);
		}

		public static BootstrapAlert BeginInfoBox(this HtmlHelper html, string title)
		{
			return BeginAlertHelper(html, "alert-info", title, null, null);
		}

		public static BootstrapAlert BeginErrorBox(this HtmlHelper html)
		{
			return BeginErrorBox(html, null, null, null);
		}

		public static BootstrapAlert BeginErrorBox(this HtmlHelper html, string title)
		{
			return BeginErrorBox(html, title, null, null);
		}

		public static BootstrapAlert BeginErrorBox(this HtmlHelper html, string title, string id)
		{
			return BeginErrorBox(html, title, id, null);
		}

		public static BootstrapAlert BeginErrorBox(this HtmlHelper html, string title, string id, string className)
		{
			return BeginAlertHelper(html, "alert-error", title, id, className);
		}

		public static BootstrapAlert BeginWarningBox(this HtmlHelper html)
		{
			return BeginWarningBox(html, null);
		}

		public static BootstrapAlert BeginWarningBox(this HtmlHelper html, string title)
		{
			return BeginAlertHelper(html, null, title, null, null);
		}

		private static BootstrapAlert BeginAlertHelper(this HtmlHelper html, string alertClassName, string title, string id, string className)
		{
			TagBuilder tag = new TagBuilder("div");
			tag.AddOrMergeAttribute("class", "alert");

			if (!String.IsNullOrEmpty(alertClassName))
				tag.AddOrMergeAttribute("class", alertClassName);

			if (!String.IsNullOrEmpty(className))
				tag.AddOrMergeAttribute("class", className);

			if (!String.IsNullOrEmpty(id))
				tag.Attributes.Add("id", id);

			html.ViewContext.Writer.Write(tag.ToString(TagRenderMode.StartTag));

			if (!String.IsNullOrEmpty(title))
			{
				TagBuilder titleTag = new TagBuilder("h4");
				titleTag.MergeAttribute("class", "alert-heading");
				titleTag.SetInnerText(title);
				html.ViewContext.Writer.Write(titleTag.ToString(TagRenderMode.Normal));
			}

			BootstrapAlert theAlert = new BootstrapAlert(html.ViewContext);
			return theAlert;
		}
	}
}
